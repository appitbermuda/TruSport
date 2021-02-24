using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ImageCircle.Forms.Plugin.Droid;
using Microsoft.WindowsAzure.MobileServices;
using Android.Content;

namespace TruSport.Droid
{
    [Activity(Label = "OnTrack", Icon = "@mipmap/ontracklogo", Theme = "@style/SplashScreen", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, NoHistory = true)]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        //{
        //    TabLayoutResource = Resource.Layout.Tabbar;
        //    ToolbarResource = Resource.Layout.Toolbar;

        //    //base.Window.RequestFeature(WindowFeatures.ActionBar);
        //    //// Name of the MainActivity theme you had there before.
        //    //// Or you can use global::Android.Resource.Style.ThemeHoloLight
        //    //base.SetTheme(Resource.Style.MainTheme);

        //    base.OnCreate(savedInstanceState, persistentState);

        //    CurrentPlatform.Init();
        //    CrossCurrentActivity.Current.Init(this, savedInstanceState);
        //    Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
        //    global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
        //    ImageCircleRenderer.Init();
        //    LoadApplication(new App());
        //}

        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }

        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);
        //    InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
        //}

        public override void OnBackPressed() { }
    }
}