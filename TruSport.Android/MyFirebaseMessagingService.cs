using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using WindowsAzure.Messaging;

namespace TruSport.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            string messageBody = string.Empty;

            if (message.GetNotification() != null)
            {
                messageBody = message.GetNotification().Body;
            }

            // NOTE: test messages sent via the Azure portal will be received here
            else
            {
                messageBody = message.Data.Values.First();
            }

            // convert the incoming message to a local notification
            SendLocalNotification(messageBody);

            // send the incoming message directly to the MainPage
            //SendMessageToMainPage(messageBody);
        }

        public async override void OnNewToken(string token)
        {
            // TODO: save token instance locally, or log if desired
            await App.Database.SaveToken(token);
            SendRegistrationToServer(token);
        }

        void SendLocalNotification(string body)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("message", body);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, Constants.NotificationChannelName)
                .SetContentTitle("OnTrack")
                .SetSmallIcon(Resource.Drawable.ic_launcher)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId(Constants.NotificationChannelName);
            }

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

        void SendMessageToMainPage(string body)
        {
            //(App.Current.MainPage as MainPage)?.AddMessage(body);
        }

        async void SendRegistrationToServer(string token)
        {
            try
            {
                NotificationHub hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString, this);

                var tags = Constants.SubscriptionTags;
                if (App.Database != null)
                {
                    var userTags = await App.Database.GetTags();

                    if (userTags != null)
                    {
                        tags = userTags;
                    }
                }

                // register device with Azure Notification Hub using the token from FCM
                Registration registration = hub.Register(token, tags);

                // subscribe to the SubscriptionTags list with a simple template.
                string pnsHandle = registration.PNSHandle;
                TemplateRegistration templateReg = hub.RegisterTemplate(pnsHandle, "defaultTemplate", Constants.FCMTemplateBody, tags);
            }
            catch (Exception e)
            {
                Log.Error(Constants.DebugTag, $"Error registering device: {e.Message}");
            }
        }


    }
}
