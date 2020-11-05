using System;
using TruSport.Data;
using TruSport.Views;
using TruSport.Views.Admin;
using TruSport.Views.Football;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using System.IO;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using TruSport.Views.Cricket;

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
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTUxNzhAMzEzNjJlMzQyZTMwZkJVNlFpZWo2ajNrRCtTdllpYWpUbDlYRUdIZyswTUl1MWN6aHo2M3lUQT0=");
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjUwNTkxQDMxMzgyZTMxMmUzMFc0QlJSUWl1VnVJa3UrM0JRSFBvK0hwajdNb0JtM3NzN0c0ODNlRHI4UDQ9");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjc2MzQ3QDMxMzgyZTMxMmUzMEJqVi9DRlJZTVk3QThlVXVzTU5LRXVmeERPV2VqVjk5L1JUWk4yZjc1YTA9");

            InitializeComponent();


            var buttonStyle = new Style(typeof(Button))
            {
                Setters = {
                    new Setter {Property = Button.TextColorProperty, Value = Color.FromHex("0091EA") }
                }
            };

            Resources = new ResourceDictionary();
            Resources.Add("buttonStyle", buttonStyle);
            //Resources.Add("primaryGray", Color.FromHex("ff00b4"));
            //Resources.Add("primaryGray", Color.FromHex("37474F"));
            Resources.Add("primaryPink", Color.FromHex("848685"));
            Resources.Add("primaryAccentPink", Color.FromHex("ff77d7"));
            //Resources.Add("primaryPink", Color.FromHex("ff77d7"));
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


            //MainPage = new AdminMainPage();
            //MainPage = new FootballMainPage();
            //MainPage = new OnTrackPage();
            //MainPage = new NavigationPage(new OnTrackPage())
            //{
            //    BackgroundColor = (Color)App.Current.Resources["primaryDarkBlueTwo"],
            //    BarTextColor = Color.White,
            //};

            //MainPage = new FootballMasterDetailPage();


            MainPage = new NavigationPage(new MainPage());
            

            //MainPage = new NavigationPage(new AdminMainPage()
            //{
            //    BarBackgroundColor = (Color)App.Current.Resources["primaryLightGray"],
            //    BarTextColor = Color.White,

            //});

            //MainPage = new NavigationPage(new FootballMainPage()
            //{
            //    BarBackgroundColor = Color.FromHex("37474F"),
            //    BarTextColor = Color.White
            //});
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
            if (!AppCenter.Configured)
            {
                Microsoft.AppCenter.Push.Push.PushNotificationReceived += (sender, e) =>
                {

                    //// Add the notification message and title to the message
                    //var summary = $"Push notification received:" +
                    //                    $"\n\tNotification title: {e.Title}" +
                    //                    $"\n\tMessage: {e.Message}";

                    //// If there is custom data associated with the notification,
                    //// print the entries
                    //if (e.CustomData != null)
                    //{
                    //    summary += "\n\tCustom data:\n";
                    //    foreach (var key in e.CustomData.Keys)
                    //    {
                    //        summary += $"\t\t{key} : {e.CustomData[key]}\n";
                    //    }
                    //}

                    //// Send the notification summary to debug output
                    //System.Diagnostics.Debug.WriteLine(summary);
                    
                };
            }

            var userLoggedIn = await SecureStorage.GetAsync("UserLoggedIn");
            var token = await SecureStorage.GetAsync("Token");
            
            //if(userLoggedIn == null)

#if DEBUG
            //await SecureStorage.SetAsync("TeamID", "fb9e133c-f062-4bd2-947a-b3db229464ae");
            //AppCenter.Start("7e262408-f3ac-48de-90ea-44ae3d91643b", typeof(Push));
#else
            //AppCenter.Start("b77a4a09-aacb-4224-82cc-64a8d8e3e9d6", typeof(Push));
            
#endif

            AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start("ios=7e262408-f3ac-48de-90ea-44ae3d91643b;android=7396cb46-271a-4f33-88d5-b97ed582f5c9",
                  typeof(Analytics), typeof(Crashes), typeof(Push));

            var deviceID = await AppCenter.GetInstallIdAsync();

            //await Push.SetEnabledAsync(true);

            bool isEnabled = await Push.IsEnabledAsync();

            VersionTracking.Track();


            // Handle when your app starts
            if (App.Database != null)
            {
                var sport = await App.Database.GetDefaultSport();

                if (sport == null || String.IsNullOrEmpty(sport.Sport))
                    MainPage = new NavigationPage(new MainPage());
                else
                {
                    if (sport.Sport.ToLower() == "cricket")
                        MainPage = new CricketMasterDetailPage();
                    else
                        MainPage = new FootballMasterDetailPage();
                }
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }

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
