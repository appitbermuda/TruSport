using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.LatestVersion;
using TruSport.Data;
using TruSport.Services;
using TruSport.ViewModel;
using TruSport.Views;
using TruSport.Views.Basketball;
using TruSport.Views.Bowling;
using TruSport.Views.Cricket;
using TruSport.Views.Football;
using TruSport.Views.Tennis;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport
{
    public partial class MainPage : ContentPage
    {
        readonly INotificationRegistrationService _notificationRegistrationService;
        FixtureService fixtureService;

        MainPageViewModel mainPageViewModel;

        public MainPage()
        {
            mainPageViewModel = new MainPageViewModel(Navigation);
            fixtureService = new FixtureService();


            InitializeComponent();

            this.BindingContext = mainPageViewModel;
            _notificationRegistrationService =
            PushServiceContainer.Resolve<INotificationRegistrationService>();
            //loader.Easing = Easing.Linear;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //await GetVersionInfo();

            //// Handle when your app starts
            //if (App.Database != null)
            //{
            //    var sport = await App.Database.GetDefaultSport();

            //    if (sport != null && !String.IsNullOrEmpty(sport.Sport))
            //    {
            //        if (sport.Sport.ToLower() == "cricket")
            //            App.Current.MainPage = new CricketMasterDetailPage();
            //        else if (sport.Sport.ToLower() == "bowling")
            //            App.Current.MainPage = new BowlingMasterDetailPage();
            //        else if (sport.Sport.ToLower() == "tennis")
            //            App.Current.MainPage = new TennisMasterDetailPage();
            //        else if (sport.Sport.ToLower() == "basketball")
            //            App.Current.MainPage = new BasketballMasterDetailPage();
            //        else
            //            App.Current.MainPage = new FootballMasterDetailPage();
            //    }
            //}
        }

        void ShowAlert(string message)
        => MainThread.BeginInvokeOnMainThread(()
        => DisplayAlert("Notification", message, "OK").ContinueWith((task)
            =>
        { if (task.IsFaulted) throw task.Exception; }));


        private async Task GetVersionInfo()
        {
            string result = string.Empty;
            try
            {
                var isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();

                if (!isLatest)
                {
                    await DisplayAlert("Update Required", "There is a new version of OnTrack available. Please update now.", "Update");

                    await CrossLatestVersion.Current.OpenAppInStore();
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}