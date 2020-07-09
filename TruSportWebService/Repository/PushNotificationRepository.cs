using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class PushNotificationRepository : IPushNotificationRepository<Push>
    {
        OnTrackContext _context;

        public PushNotificationRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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
