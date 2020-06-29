using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Foundation;
using Google.MobileAds;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
//using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using Syncfusion.SfCalendar.XForms.iOS;
using Syncfusion.SfNumericUpDown.XForms.iOS;
using Syncfusion.SfPicker.XForms.iOS;
using Syncfusion.SfPullToRefresh.XForms.iOS;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.MaskedEdit;
using Syncfusion.XForms.iOS.TextInputLayout;
using UIKit;
using UserNotifications;
using WindowsAzure.Messaging;
using Xamarin.Forms;

namespace TruSport.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        private SBNotificationHub Hub { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            MobileAds.SharedInstance.Start(CompletionHandler);

            Forms.SetFlags("CarouselView_Experimental");

            global::Xamarin.Forms.Forms.Init();

            App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;
            App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;

            //UITabBar.Appearance.TintColor = UIColor.Red;

            // Initialize Azure Mobile Apps
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            //UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBarWindow")).ValueForKey(new NSString("statusBar")) as UIView;
            //statusBar.TintColor = UIColor.White;
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfSwitchRenderer.Init();
            SfListViewRenderer.Init();
            SfCalendarRenderer.Init();
            SfPickerRenderer.Init();
            new SfBusyIndicatorRenderer();
            new SfNumericUpDownRenderer();
            SfMaskedEditRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            SfSegmentedControlRenderer.Init();
            SfPullToRefreshRenderer.Init();
            new Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            LoadApplication(new App());

            base.FinishedLaunching(app, options);

            RegisterForRemoteNotifications();

            return true;
        }

        private void CompletionHandler(InitializationStatus status)
        {
        }

        void RegisterForRemoteNotifications()
        {
            // register for remote notifications based on system version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                    UNAuthorizationOptions.Sound |
                    UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        if (granted)
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Hub = new SBNotificationHub(Constants.ListenConnectionString, Constants.NotificationHubName);

            // update registration with Azure Notification Hub
            Hub.UnregisterAll(deviceToken, async (error) =>
            {
                if (error != null)
                {
                    Debug.WriteLine($"Unable to call unregister {error}");
                    return;
                }

                var tags = new NSSet(Constants.SubscriptionTags.ToArray());

                if (App.Database != null)
                {
                    var userTags = await App.Database.GetTags();

                    if (userTags != null)
                    {
                        tags = new NSSet(userTags);
                    }
                }

                Hub.RegisterNative(deviceToken, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        Debug.WriteLine($"RegisterNativeAsync error: {errorCallback}");
                    }
                });

                var templateExpiration = DateTime.Now.AddDays(120).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                Hub.RegisterTemplate(deviceToken, "defaultTemplate", Constants.APNTemplateBody, templateExpiration, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        if (errorCallback != null)
                        {
                            Debug.WriteLine($"RegisterTemplateAsync error: {errorCallback}");
                        }
                    }
                });
            });
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // make sure we have a payload
            if (options != null && options.ContainsKey(new NSString("aps")))
            {
                // get the APS dictionary and extract message payload. Message JSON will be converted
                // into a NSDictionary so more complex payloads may require more processing
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                string payload = string.Empty;
                NSString payloadKey = new NSString("alert");
                if (aps.ContainsKey(payloadKey))
                {
                    payload = aps[payloadKey].ToString();
                }

                if (!string.IsNullOrWhiteSpace(payload))
                {
                    //(App.Current.MainPage as MainPage)?.AddMessage(payload);
                }

            }
            else
            {
                Debug.WriteLine($"Received request to process notification but there was no payload.");
            }
        }
    }
}
