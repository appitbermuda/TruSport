using System;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Ads;
using Microsoft.WindowsAzure.MobileServices;
using ImageCircle.Forms.Plugin.Droid;
using Android.Gms.Common;
using Android.Content;
using Xamarin.Forms;
using TruSport.Styles;
using Android.Content.Res;
using Android.Support.V7.App;
using Plugin.Permissions;

namespace TruSport.Droid
{
    [Activity(Label = "OnTrack", Icon = "@mipmap/ontrack_launcher", Theme = "@style/SplashScreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly string TAG = "MainActivity";
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        protected override void OnCreate(Bundle bundle)
        {
            MobileAds.Initialize(ApplicationContext, "ca-app-pub-1338169805120312~2954350726");
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.Window.RequestFeature(WindowFeatures.ActionBar);
            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(bundle);

            //SetContentView(Resource.Layout.Main);
            Forms.SetFlags("CarouselView_Experimental");

            CurrentPlatform.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            Xamarin.Essentials.Platform.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            ImageCircleRenderer.Init();

            LoadApplication(new App());

            SetAppTheme();
            IsPlayServicesAvailable(); //You can use this method to check if play services are available.
            CreateNotificationChannel();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (intent.Extras != null)
            {
                var message = intent.GetStringExtra("message");
                //(App.Current.MainPage as MainPage)?.AddMessage(message);
            }

            base.OnNewIntent(intent);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(Constants.DebugTag, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(Constants.DebugTag, "This device is not supported");
                }
                return false;
            }
            return true;
        }

        private void OnModeChanged(Page arg1, string theme)
        {
            if (theme == "light")
            {
                Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
            }
            else
            {
                Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);
            }
            SetTheme(theme);
        }

        void CreateNotificationChannel()
        {
            // Notification channels are new as of "Oreo".
            // There is no need to create a notification channel on older versions of Android.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName = Constants.NotificationChannelName;
                var channelDescription = String.Empty;
                var channel = new NotificationChannel(channelName, channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }

        void SetAppTheme()
        {
            if (Resources.Configuration.UiMode.HasFlag(UiMode.NightYes))
                SetTheme("dark");
            else
                SetTheme("light");
        }

        void SetTheme(string mode)
        {
            if (mode == "dark")
            {
                //if (App.AppTheme != null && App.AppTheme == "dark")
                //    return;
                App.Current.Resources = new DarkTheme();
            }
            else
            {
                //if (App.AppTheme != null && App.AppTheme != "dark")
                //    return;
                App.Current.Resources = new LightTheme();
            }

            App.AppTheme = mode;
        }
    }
}

