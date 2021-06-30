using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views;
using TruSport.Views.Bowling;
using TruSport.Views.Cricket;
using TruSport.Views.Tennis;
using TruSport.Views.Tickets;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        #region Fields
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

            GenerateSource();

            RefreshCommand = new Command(() => GenerateSource());

            SportTappedCommand = new Command(() => SportTapped());
            TicketTappedCommand = new Command(() => TicketTapped());
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
                Application.Current.MainPage = new NavigationPage(new SportsPage());
            }
            catch (Exception ex)
            {

            }
        }

        private async void TicketTapped()
        {
            try
            {
                Application.Current.MainPage = (new TicketFlyoutPage());                
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
