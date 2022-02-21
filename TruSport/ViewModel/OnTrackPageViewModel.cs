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
using TruSport.Views.Tennis;
using TruSport.Views.Triathlon;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel.Football
{
    public class OnTrackPageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Sport> _sports;
        private SportFeature _featureImages;
        private bool _isActivityIndicatorVisible;
        INavigation Navigation;
        SettingService settingService;
        SportService sportService;
        NotificationRegistrationService notificationRegistrationService;

        #endregion

        #region Constructor

        public OnTrackPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            //databaseManager = new DatabaseManager();
            settingService = new SettingService();
            sportService = new SportService();
            notificationRegistrationService = new NotificationRegistrationService();

            GenerateSource();

            BowlingTappedCommand = new Command(() => BowlingTapped());
            FootballTappedCommand = new Command(() => FootballTapped());
            CricketTappedCommand = new Command(() => CricketTapped());
            TennisTappedCommand = new Command(() => TennisTapped());
            TrackTappedCommand = new Command(() => TrackTapped());
            SwimmingTappedCommand = new Command(() => SwimmingTapped());
            RugbyTappedCommand = new Command(() => RugbyTapped());
            BasketballTappedCommand = new Command(() => BasketballTapped());
            TriathlonTappedCommand = new Command(() => TriathlonTapped());
            HockeyTappedCommand = new Command(() => HockeyTapped());
            GolfTappedCommand = new Command(() => GolfTapped());
            CyclingTappedCommand = new Command(() => CyclingTapped());
        }

        #endregion

        #region Properties

        public Command BowlingTappedCommand { get; }
        public Command FootballTappedCommand { get; }
        public Command CricketTappedCommand { get; }
        public Command TennisTappedCommand { get; }
        public Command TrackTappedCommand { get; }
        public Command SwimmingTappedCommand { get; }
        public Command RugbyTappedCommand { get; }
        public Command BasketballTappedCommand { get; }
        public Command HockeyTappedCommand { get; }
        public Command GolfTappedCommand { get; }
        public Command CyclingTappedCommand { get; }
        public Command TriathlonTappedCommand { get; }

        public ObservableCollection<Sport> Sports
        {
            get { return _sports; }
            set { Set(ref _sports, value); }
        }

        public SportFeature FeatureImages
        {
            get { return _featureImages; }
            set { Set(ref _featureImages, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                //FeatureImages = await settingService.GetHomeMobileFeatureImages();

                var sports = await sportService.GetSports();
                await App.Database.ImportIfNotExistsSports(sports);
                Sports = new ObservableCollection<Sport>(sports);

                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {

            }

            IsActivityIndicatorVisible = false;
        }

        private async void FootballTapped()
        {
            try
            {
                //await SecureStorage.SetAsync("DefaultSport","Football");
                //await SecureStorage.SetAsync("Sport", "Football");

                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Football);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Football,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("FootballAlert", alert.IsAlert.ToString());

                //Navigation.InsertPageBefore(new FootballMasterDetailPage(), Navigation.NavigationStack.First());
                //await Navigation.PopToRootAsync();
                Application.Current.MainPage = new FootballMasterDetailPage();
            }
            catch (Exception ex)
            {

            }
        }

        private async void BowlingTapped()
        {
            try
            {
                //await SecureStorage.SetAsync("DefaultSport", "Cricket");
                //await SecureStorage.SetAsync("Sport", "Cricket");

                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Bowling);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Bowling,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("BowlingAlert", alert.IsAlert.ToString());

                Application.Current.MainPage = new BowlingMasterDetailPage();
                //Navigation.InsertPageBefore(new CricketMasterDetailPage(), Navigation.NavigationStack.First());
                //await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async void CricketTapped()
        {
            try
            {
                //await SecureStorage.SetAsync("DefaultSport", "Cricket");
                //await SecureStorage.SetAsync("Sport", "Cricket");

                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Cricket);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Cricket,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("CricketAlert", alert.IsAlert.ToString());

                Application.Current.MainPage = new CricketMasterDetailPage();
                //Navigation.InsertPageBefore(new CricketMasterDetailPage(), Navigation.NavigationStack.First());
                //await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async void TennisTapped()
        {
            try
            {
                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Tennis);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Tennis,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("TennisAlert", alert.IsAlert.ToString());

                Application.Current.MainPage = new TennisMasterDetailPage();
            }
            catch (Exception ex)
            {

            }
        }

        private async void TrackTapped()
        {
            try
            {
                var sport = Sports.FirstOrDefault(e => e.Name == Constants.TrackAndField);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.TrackAndField,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("TrackAndFieldAlert", alert.IsAlert.ToString());

                //Application.Current.MainPage = new TrackMasterDetailPage();
            }
            catch (Exception ex)
            {

            }
        }

        private async void SwimmingTapped()
        {
            try
            {
                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Swimming);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Swimming,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("SwimmingAlert", alert.IsAlert.ToString());

                //Application.Current.MainPage = new SwimmingMasterDetailPage();
            }
            catch (Exception ex)
            {

            }
        }

        private async void RugbyTapped()
        {
            try
            {
                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Rugby);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Rugby,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("RugbyAlert", alert.IsAlert.ToString());

                //Application.Current.MainPage = new RugbyMasterDetailPage();
            }
            catch (Exception ex)
            {

            }
        }

        private async void BasketballTapped()
        {
            try
            {
                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Basketball);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Basketball,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("BasketballAlert", alert.IsAlert.ToString());

                Application.Current.MainPage = new BasketballMasterDetailPage();
            }
            catch (Exception ex)
            {

            }
        }

        private async void HockeyTapped()
        {
            try
            {
                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Hockey);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Hockey,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("HockeyAlert", alert.IsAlert.ToString());

                //Application.Current.MainPage = new HockeyMasterDetailPage();
            }
            catch (Exception ex)
            {

            }
        }

        private async void GolfTapped()
        {
            try
            {
                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Golf);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Golf,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("GolfAlert", alert.IsAlert.ToString());

                //Application.Current.MainPage = new GolfMasterDetailPage();
            }
            catch (Exception ex)
            {

            }
        }

        private async void CyclingTapped()
        {
            try
            {
                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Cycling);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Cycling,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("CyclingAlert", alert.IsAlert.ToString());

                //Application.Current.MainPage = new CyclingMasterDetailPage();
            }
            catch (Exception ex)
            {

            }
        }

        private async void TriathlonTapped()
        {
            try
            {
                var sport = Sports.FirstOrDefault(e => e.Name == Constants.Triathlon);

                await App.Database.SetDefaultSport(sport);

                NotiAlert alert = new NotiAlert
                {
                    Sport = Constants.Triathlon,
                    IsAlert = true
                };

                await App.Database.SaveAlertSetting(alert);

                var tags = await App.Database.GetTags();
                try
                {
                    await notificationRegistrationService.RegisterDeviceAsync(tags);
                }
                catch (Exception ex)
                { }

                await SecureStorage.SetAsync("TriathlonAlert", alert.IsAlert.ToString());

                Application.Current.MainPage = new TriathlonFlyoutPage();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
