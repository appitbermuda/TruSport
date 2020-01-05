using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
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
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Google.MobileAds.MobileAds.Configure("ca-app-pub-1338169805120312~6077936967");

            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();

            App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;
            App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;

            //UITabBar.Appearance.TintColor = UIColor.Red;

            // Initialize Azure Mobile Apps
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            new FreshEssentials.iOS.AdvancedFrameRendereriOS();

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


            return base.FinishedLaunching(app, options);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, System.Action<UIBackgroundFetchResult> completionHandler)
        {
            var result = Microsoft.AppCenter.Push.Push.DidReceiveRemoteNotification(userInfo);
            if (result)
            {
                completionHandler?.Invoke(UIBackgroundFetchResult.NewData);
            }
            else
            {
                completionHandler?.Invoke(UIBackgroundFetchResult.NoData);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Push.RegisteredForRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Push.FailedToRegisterForRemoteNotifications(error);
        }

        //public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        //{

        //    //...

        //    // Pass the notification payload to MSPush.
        //    Push.DidReceiveRemoteNotification(notification.Request.Content.UserInfo);

        //    // Complete handling the notification.
        //    completionHandler(UNNotificationPresentationOptions.None);
        //}

        //public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        //{

        //    //...

        //    // Pass the notification payload to MSPush.
        //    Push.DidReceiveRemoteNotification(response.Notification.Request.Content.UserInfo);

        //    // Complete handling the notification.
        //    completionHandler();
        //}
    }
}
