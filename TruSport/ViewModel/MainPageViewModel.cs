using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views;
using TruSport.Views.Basketball;
using TruSport.Views.Bowling;
using TruSport.Views.Cricket;
using TruSport.Views.Event;
using TruSport.Views.Golf;
using TruSport.Views.Tennis;
using TruSport.Views.Tickets;
using TruSport.Views.Triathlon;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        #region Fields

        NotificationRegistrationService notificationRegistrationService;
        private ObservableCollection<Sport> _sports;
        private LandingFeature _featureImages;
        private bool _isActivityIndicatorVisible;
        private bool _noConnectivity;
        INavigation Navigation;
        SettingService settingService;

        #endregion

        #region Constructor

        public MainPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            //databaseManager = new DatabaseManager();
            settingService = new SettingService();
            notificationRegistrationService = new NotificationRegistrationService();

            GenerateSource();

            RefreshCommand = new Command(() => GenerateSource());

            SportTappedCommand = new Command(() => SportTapped());
            TicketTappedCommand = new Command(() => TicketTapped());
            ShopTappedCommand = new Command(() => ShopTapped());
            EventTappedCommand = new Command(() => EventTapped());
            //TrackTappedCommand = new Command(() => TrackTapped());
            //SwimmingTappedCommand = new Command(() => SwimmingTapped());
            //RugbyTappedCommand = new Command(() => RugbyTapped());
            //BasketballTappedCommand = new Command(() => BasketballTapped());
            //HockeyTappedCommand = new Command(() => HockeyTapped());
            //GolfTappedCommand = new Command(() => GolfTapped());
            //CyclingTappedCommand = new Command(() => CyclingTapped());
        }

        #endregion

        #region Properties

        public Command RefreshCommand { get; }
        public Command SportTappedCommand { get; }
        public Command TicketTappedCommand { get; }
        public Command ShopTappedCommand { get; }
        public Command EventTappedCommand { get; }

        public LandingFeature FeatureImages
        {
            get { return _featureImages; }
            set { Set(ref _featureImages, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool NoConnectivity
        {
            get { return _noConnectivity; }
            set { Set(ref _noConnectivity, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    FeatureImages = await settingService.GetLandingMobileFeatureImages();
                }
                else
                {
                    NoConnectivity = true;
                }

                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {

            }

            IsActivityIndicatorVisible = false;
        }

        private async void SportTapped()
        {
            try
            {
                //Application.Current.MainPage = new NavigationPage(new SportsPage());

                if (App.Database != null)
                {
                    var sport = await App.Database.GetDefaultSport();

                    if (sport != null && !String.IsNullOrEmpty(sport.Sport))
                    {
                        if (sport.Sport == Constants.Cricket)
                            App.Current.MainPage = new CricketMasterDetailPage();
                        else if (sport.Sport == Constants.Bowling)
                            App.Current.MainPage = new BowlingMasterDetailPage();
                        else if (sport.Sport == Constants.Tennis)
                            App.Current.MainPage = new TennisMasterDetailPage();
                        else if (sport.Sport == Constants.Basketball)
                            App.Current.MainPage = new BasketballMasterDetailPage();
                        else if (sport.Sport == Constants.Triathlon)
                            App.Current.MainPage = new TriathlonFlyoutPage();
                        else if (sport.Sport == Constants.Golf)
                            App.Current.MainPage = new GolfFlyoutPage();
                        else
                            App.Current.MainPage = new FootballMasterDetailPage();
                    }
                    else
                        Application.Current.MainPage = new NavigationPage(new OnTrackPage());
                }
                else
                    Application.Current.MainPage = new NavigationPage(new OnTrackPage());
            }
            catch (Exception ex)
            {

            }
        }

        private async void TicketTapped()
        {
            try
            {
                string Email = await SecureStorage.GetAsync("Email");
                var Customer = await App.Database.GetCustomerByIDAsync(Email);
                if (Customer != null)
                    App.IsLoggedIn = true;
                else
                    App.IsLoggedIn = false;

                Application.Current.MainPage = (new TicketFlyoutPage());      
            }
            catch (Exception ex)
            {

            }
        }

        private async void ShopTapped()
        {
            try
            {
                Application.Current.MainPage = new NavigationPage(new ShopPage(true));
            }
            catch (Exception ex)
            {

            }
        }

        private async void EventTapped()
        {
            try
            {
                Application.Current.MainPage = (new EventFlyoutPage());
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
