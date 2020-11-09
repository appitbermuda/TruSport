using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class PushNotificationRepository : IPushNotificationRepository<Push>
    {
        OnTrackContext _context;
        readonly NotificationHubClient _hub;
        readonly Dictionary<string, NotificationPlatform> _installationPlatform;
        readonly ILogger<PushNotificationRepository> _logger;

        public PushNotificationRepository(OnTrackContext context, IOptions<NotificationHubOptions> options, ILogger<PushNotificationRepository> logger)
        {
            _context = context;
            _logger = logger;
            _hub = NotificationHubClient.CreateClientFromConnectionString(
                options.Value.ConnectionString,
                options.Value.Name);

            _installationPlatform = new Dictionary<string, NotificationPlatform>
            {
                { nameof(NotificationPlatform.Apns).ToLower(), NotificationPlatform.Apns },
                { nameof(NotificationPlatform.Fcm).ToLower(), NotificationPlatform.Fcm }
            };
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallation deviceInstallation, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(deviceInstallation?.InstallationId) ||
                string.IsNullOrWhiteSpace(deviceInstallation?.Platform) ||
                string.IsNullOrWhiteSpace(deviceInstallation?.PushChannel))
                return false;

            var installation = new Installation()
            {
                InstallationId = deviceInstallation.InstallationId,
                PushChannel = deviceInstallation.PushChannel,
                Tags = deviceInstallation.Tags
            };

            if (_installationPlatform.TryGetValue(deviceInstallation.Platform, out var platform))
                installation.Platform = platform;
            else
                return false;

            try
            {
                await _hub.CreateOrUpdateInstallationAsync(installation, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(installationId))
                return false;

            try
            {
                await _hub.DeleteInstallationAsync(installationId, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest, CancellationToken token)
        {
            if ((notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                (!notificationRequest.Silent &&
                (string.IsNullOrWhiteSpace(notificationRequest?.Text)) ||
                string.IsNullOrWhiteSpace(notificationRequest?.Action)))
                return false;

            var androidPushTemplate = notificationRequest.Silent ?
                PushTemplates.Silent.Android :
                PushTemplates.Generic.Android;

            var iOSPushTemplate = notificationRequest.Silent ?
                PushTemplates.Silent.iOS :
                PushTemplates.Generic.iOS;

            var androidPayload = PrepareNotificationPayload(
                androidPushTemplate,
                notificationRequest.Text,
                notificationRequest.Action);

            var iOSPayload = PrepareNotificationPayload(
                iOSPushTemplate,
                notificationRequest.Text,
                notificationRequest.Action);

            try
            {
                if (notificationRequest.Tags.Length == 0)
                {
                    // This will broadcast to all users registered in the notification hub
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, token);
                }
                else if (notificationRequest.Tags.Length <= 20)
                {
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, notificationRequest.Tags, token);
                }
                else
                {
                    var notificationTasks = notificationRequest.Tags
                        .Select((value, index) => (value, index))
                        .GroupBy(g => g.index / 20, i => i.value)
                        .Select(tags => SendPlatformNotificationsAsync(androidPayload, iOSPayload, tags, token));

                    await Task.WhenAll(notificationTasks);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error sending notification");
                return false;
            }
        }

        string PrepareNotificationPayload(string template, string text, string action) => template
            .Replace("$(alertMessage)", text, StringComparison.InvariantCulture)
            .Replace("$(alertAction)", action, StringComparison.InvariantCulture);

        Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, CancellationToken token)
        {
            var sendTasks = new Task[]
            {
                _hub.SendFcmNativeNotificationAsync(androidPayload, token),
                _hub.SendAppleNativeNotificationAsync(iOSPayload, token)
            };

            return Task.WhenAll(sendTasks);
        }

        Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, IEnumerable<string> tags, CancellationToken token)
        {
            var sendTasks = new Task[]
            {
                _hub.SendFcmNativeNotificationAsync(androidPayload, tags, token),
                _hub.SendAppleNativeNotificationAsync(iOSPayload, tags, token)
            };

            return Task.WhenAll(sendTasks);
        }

        public async Task<string> SendTest(string message)
        {
            // let's assume you have a User object that contains 
            // * iOS Devices 
            // * Android Devices
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(Constants.FullAccessConnectionString, Constants.NotificationHubName);
            Dictionary<string, string> templateParameters = new Dictionary<string, string>();

            templateParameters["messageParam"] = message;

            try
            {
                await hub.SendTemplateNotificationAsync(templateParameters, "default");
                Console.WriteLine($"Sent message to default subscribers.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send template notification: {ex.Message}");
            }

            return "Notification sent successfully!";
        }

        public async Task<string> SendTagNotification(string message, string tag)
        {
            // let's assume you have a User object that contains 
            // * iOS Devices 
            // * Android Devices
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(Constants.FullAccessConnectionString, Constants.NotificationHubName);
            Dictionary<string, string> templateParameters = new Dictionary<string, string>();

            templateParameters["messageParam"] = message;

            try
            {
                await hub.SendTemplateNotificationAsync(templateParameters, tag);
                Console.WriteLine($"Sent message to {tag} subscribers.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send template notification: {ex.Message}");
            }

            return "Notification sent successfully!";
        }

        public async Task<string> SendNotification(string message)
        {
            // let's assume you have a User object that contains 
            // * iOS Devices 
            // * Android Devices
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(Constants.FullAccessConnectionString, Constants.NotificationHubName);
            Dictionary<string, string> templateParameters = new Dictionary<string, string>();

            templateParameters["messageParam"] = message;

            try
            {
                await hub.SendTemplateNotificationAsync(templateParameters, "default");
                Console.WriteLine($"Sent message to default subscribers.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send template notification: {ex.Message}");
            }

            return "Notification sent successfully!";
        }

        public Task<string> Send(string name, string title, string body)
        {
            //DeviceInstallation
            throw new NotImplementedException();
        }
    }
}
