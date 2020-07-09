using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using TruSport.Views;
using TruSport.Views.Cricket;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Sport> _sports;
        private SportFeature _featureImages;
        private bool _isActivityIndicatorVisible;
        private bool _noConnectivity;
        INavigation Navigation;
        SettingService settingService;
        SportService sportService;

        #endregion

        #region Constructor

        public MainPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            //databaseManager = new DatabaseManager();
            settingService = new SettingService();
            sportService = new SportService();

            GenerateSource();

            RefreshCommand = new Command(() => GenerateSource());

            FootballTappedCommand = new Command(() => FootballTapped());
            CricketTappedCommand = new Command(() => CricketTapped());
            //TennisTappedCommand = new Command(() => TennisTapped());
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
                    var sports = await sportService.GetSports();

                    FeatureImages = await settingService.GetHomeMobileFeatureImages();

                    Sports = new ObservableCollection<Sport>(sports);

                    await App.Database.ImportIfNotExistsSports(sports);
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

        private async void FootballTapped()
        {
            try
            {
                //await SecureStorage.SetAsync("DefaultSport","Football");
                //await SecureStorage.SetAsync("Sport", "Football");

                Sport sport = new Sport();

                if (Sports == null)
                {
                    var sports = await sportService.GetSports();
                    sport = sports.FirstOrDefault(e => e.Name.ToLower() == "football");
                }
                else
                {
                    sport = Sports.FirstOrDefault(e => e.Name.ToLower() == "football");
                }

                await App.Database.SetDefaultSport(sport);

                //Navigation.InsertPageBefore(new FootballMasterDetailPage(), Navigation.NavigationStack.First());
                //await Navigation.PopToRootAsync();
                Application.Current.MainPage = new FootballMasterDetailPage();
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
                Sport sport = new Sport();

                if (Sports == null)
                {
                    var sports = await sportService.GetSports();
                    sport = sports.FirstOrDefault(e => e.Name.ToLower() == "cricket");
                }
                else
                {
                    sport = Sports.FirstOrDefault(e => e.Name.ToLower() == "cricket");
                }


                await App.Database.SetDefaultSport(sport);
                Application.Current.MainPage = new CricketMasterDetailPage();
                
                //Navigation.InsertPageBefore(new CricketMasterDetailPage(), Navigation.NavigationStack.First());
                //await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {

            }
        }

        //private async void TennisTapped()
        //{
        //    try
        //    {
        //        await Navigation.PushAsync(new DirectoryPage("Tennis"));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private async void TrackTapped()
        //{
        //    try
        //    {
        //        await Navigation.PushAsync(new DirectoryPage("Track & Field"));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private async void SwimmingTapped()
        //{
        //    try
        //    {
        //        await Navigation.PushAsync(new DirectoryPage("Swimming"));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private async void RugbyTapped()
        //{
        //    try
        //    {
        //        await Navigation.PushAsync(new DirectoryPage("Rugby"));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private async void BasketballTapped()
        //{
        //    try
        //    {
        //        await Navigation.PushAsync(new DirectoryPage("Basketball"));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private async void HockeyTapped()
        //{
        //    try
        //    {
        //        await Navigation.PushAsync(new DirectoryPage("Field Hockey"));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private async void GolfTapped()
        //{
        //    try
        //    {
        //        await Navigation.PushAsync(new DirectoryPage("Golf"));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private async void CyclingTapped()
        //{
        //    try
        //    {
        //        await Navigation.PushAsync(new DirectoryPage("Cycling"));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        #endregion
    }
}
