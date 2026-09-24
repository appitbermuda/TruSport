using System;
using TruSport.Data;
using TruSport.Views;
using TruSport.Views.Football;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Microsoft.AppCenter;
//using Microsoft.AppCenter.Push;
using System.IO;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using TruSport.Views.Cricket;
using TruSport.Views.Bowling;
using TruSport.Views.Tickets;
using TruSport.Views.Tennis;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TruSport
{
    public partial class App : Application
    {
        public static bool IsLoggedIn { get; set; }
        public static string UserFullName;
        public static string UserFirstName;
        public static string UserLastName;
        public static double ScreenHeight;
        public static double ScreenWidth;
        public static string UserID;
        public static string UserType;
        public static string DeviceID;

        
        static OnTrackDatabase database;

        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjQ5NjIxQDMyMzAyZTMxMmUzMFoyNG92dytaQndwT1R0Q1dUcXhqYzRmTkE4TlY5cHVuVWJjL2NZTE5GSHc9");

            InitializeComponent();


            var buttonStyle = new Style(typeof(Button))
            {
                Setters = {
                    new Setter {Property = Button.TextColorProperty, Value = Color.FromHex("0091EA") }
                }
            };

            Resources = new ResourceDictionary();
            Resources.Add("buttonStyle", buttonStyle);
            Resources.Add("primaryPink", Color.FromHex("848685"));
            Resources.Add("primaryAccentPink", Color.FromHex("ff77d7"));
            Resources.Add("primaryLightGray", Color.FromHex("556167"));
            Resources.Add("primaryBarBlue", Color.FromHex("0e1550"));
            Resources.Add("primaryDarkBlue", Color.FromHex("2a348d"));
            Resources.Add("primaryDarkBlueOne", Color.FromHex("29338a"));
            Resources.Add("primaryDarkBlueTwo", Color.FromHex("1d2462"));
            Resources.Add("primaryLightBlue", Color.FromHex("3d4ac6"));
            Resources.Add("primaryBlue", Color.FromHex("4a81c2"));
            Resources.Add("primaryButtonBlue", Color.FromHex("0091EA"));
            Resources.Add("primaryCell", Color.FromHex("5d7785"));
            Resources["fontAwesomeSolidFamily"] = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS ? "FontAwesome5ProSolid" : "fa-solid-900.ttf#Font Awesome 5 Pro Solid";
            Resources["fontAwesomeLightFamily"] = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS ? "FontAwesome5ProLight" : "fa-light-300.ttf#Font Awesome 5 Pro Light";
            Resources["fontAwesomeRegularFamily"] = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS ? "FontAwesome5ProRegular" : "fa-regular-400.ttf#Font Awesome 5 Pro Regular";
            Resources["fontAwesomeBrandFamily"] = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS ? "FontAwesome5ProBrand" : "fa-brand-400.ttf#Font Awesome 5 Pro Brand";
            Resources["fontAwesomeDuotoneFamily"] = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS ? "FontAwesome5ProDuoTone" : "fa-duotone-900.ttf#Font Awesome 5 Pro DuoTone";
            
            Resources["fontFamily"] = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS ? "icomoon" : "icomoon.ttf#icomoon";

            PushServiceContainer.Resolve<IPushNotificationActionService>()
            .ActionTriggered += NotificationActionTriggered;

            MainPage = new NavigationPage(new MainPage());
            
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            var query = uri.PathAndQuery.Trim(new[] { '/' });

            if (query.EndsWith("ticket", StringComparison.OrdinalIgnoreCase))
            {
                App.Current.MainPage = new TicketFlyoutPage();
            }
        }

        void NotificationActionTriggered(object sender, Model.PushAction e)
    => ShowActionAlert(e);

        void ShowActionAlert(Model.PushAction action)
            => MainThread.BeginInvokeOnMainThread(()
                => MainPage?.DisplayAlert("Notification", $"{action} action received", "OK")
                    .ContinueWith((task) => { if (task.IsFaulted) throw task.Exception; }));

        public static OnTrackDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new OnTrackDatabase(
                      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OnTrackDB.db3"));
                }
                return database;
            }
        }

        protected override async void OnStart()
        {
            

#if DEBUG
            //await SecureStorage.SetAsync("TeamID", "fb9e133c-f062-4bd2-947a-b3db229464ae");
            //AppCenter.Start("7e262408-f3ac-48de-90ea-44ae3d91643b", typeof(Push));
#else
            //AppCenter.Start("b77a4a09-aacb-4224-82cc-64a8d8e3e9d6", typeof(Push));
            
#endif

            AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start("ios=7e262408-f3ac-48de-90ea-44ae3d91643b;android=7396cb46-271a-4f33-88d5-b97ed582f5c9",
                  typeof(Analytics), typeof(Crashes));

            VersionTracking.Track();

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override async void OnResume()
        {
            MessagingCenter.Send<string>("Fixtures", "RefreshFixtures");
            //var userLoggedIn = await App.Database.IsUserLoggedIn();

            //if (!userLoggedIn)
            //    MainPage = new NavigationPage(new AuthenticationPage())
            //    {
            //        BarBackgroundColor = (Color)App.Current.Resources["primaryDarkBlueTwo"],
            //        BarTextColor = Color.White
            //    };

            //App.Current.MainPage = new FootballMainPage();  
            // Handle when your app resumes


            //var sport = await SecureStorage.GetAsync("Sport");

            //if (String.IsNullOrEmpty(sport))
            //    await SecureStorage.SetAsync("Sport", "Football");
        }

        public static Color LookupColor(string key)
        {
            try
            {
                Application.Current.Resources.TryGetValue(key, out var newColor);
                return (Color)newColor;
            }
            catch
            {
                return Color.White;
            }
        }

        public static string AppTheme
        {
            get; set;
        }
    }
}
