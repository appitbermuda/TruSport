using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using TruSport.Services;
using System.Collections.Generic;
using TruSport.Views.Football;
using Syncfusion.DataSource.Extensions;
using TruSport.Extensions;
using Xamarin.Essentials;
using NodaTime;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Device = Xamarin.Forms.Device;
using System.Windows.Input;
using Syncfusion.SfCalendar.XForms;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TruSport.ViewModels.Golf
{
    public class FixtureDetailPageViewModel : BaseViewModel
    {
        #region Fields
        private int selectedIndex;
        private GolfFixture _fixtureItem;
        private RosterCoachListView rosterCoach;
        private ObservableCollection<GolfTourStanding> tableCollection;
        private ObservableCollection<GolfFixture> tourFixtureCollection;
        private ObservableCollection<GolfFixture> fixturesCollection;
        private ObservableCollection<GolfScore> scoreCollection;

        private double subHeight;
        private bool _isActivityIndicatorVisible;
        private bool isFavourite;
        private bool isFavouriteVisible;
        private bool noConnectivity;
        private bool cancelFixtureRefresh;
        private bool _hasScore;
        FixtureService fixtureService;
        LeagueTableService leagueTableService;
        NotificationRegistrationService notificationRegistrationService;
        AdService adService;
        INavigation Navigation;

        #endregion

        #region Constructor

        public FixtureDetailPageViewModel(INavigation navigation, GolfFixture fixture)
        {
            Navigation = navigation;
            TourFixtureCollection = new ObservableCollection<GolfFixture>();
            ScoreCollection = new ObservableCollection<GolfScore>();
            TableCollection = new ObservableCollection<GolfTourStanding>();
            fixtureService = new FixtureService();
            leagueTableService = new LeagueTableService();
            notificationRegistrationService = new NotificationRegistrationService();
            adService = new AdService();

            SelectedIndex = 0;

            GenerateSource(fixture);

            AdTappedCommand = new Command(AdTapped);
            FavouriteCommand = new Command(async () => await Favourite());
        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }
        public Command FavouriteCommand { get; }

        public GolfFixture FixtureItem
        {
            get { return _fixtureItem; }
            set { Set(ref _fixtureItem, value); }
        }

        public RosterCoachListView RosterCoach
        {
            get { return rosterCoach; }
            set { Set(ref rosterCoach, value); }
        }

        public ObservableCollection<GolfTourStanding> TableCollection
        {
            get { return tableCollection; }
            set { Set(ref tableCollection, value); }
        }

        public ObservableCollection<GolfFixture> TourFixtureCollection
        {
            get { return tourFixtureCollection; }
            set { Set(ref tourFixtureCollection, value); }
        }

        public ObservableCollection<GolfFixture> FixturesCollection
        {
            get { return fixturesCollection; }
            set { Set(ref fixturesCollection, value); }
        }

        public ObservableCollection<GolfScore> ScoreCollection
        {
            get { return scoreCollection; }
            set { Set(ref scoreCollection, value); }
        }

        private Ad _ad;
        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { Set(ref selectedIndex, value); }
        }

        public double SubHeight
        {
            get { return subHeight; }
            set { Set(ref subHeight, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool IsFavourite
        {
            get { return isFavourite; }
            set { Set(ref isFavourite, value); }
        }

        public bool IsFavouriteVisible
        {
            get { return isFavouriteVisible; }
            set { Set(ref isFavouriteVisible, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        public bool CancelFixtureRefresh
        {
            get { return cancelFixtureRefresh; }
            set { Set(ref cancelFixtureRefresh, value); }
        }

        public bool HasScore
        {
            get { return _hasScore; }
            set { Set(ref _hasScore, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource(GolfFixture fixtureItem)
        {
            CancelFixtureRefresh = true;

            IsActivityIndicatorVisible = true;

            try
            {
                FixtureItem = fixtureItem;

                await Task.Run(async () =>
                {
                    var ads = await adService.GetAds();

                    if (ads != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Ad = ads.Any(e => e.Sport == Constants.Golf) ? ads.FirstOrDefault(e => e.Sport == Constants.Golf) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                        });
                    }
                });

                var fixture = await fixtureService.GetGolfFixture(FixtureItem.ID);

                if (fixtureItem.FixtureTime > DateTime.Now)
                    IsFavouriteVisible = true;
                else
                    IsFavouriteVisible = false;

                IsFavourite = await App.Database.IsFixtureFavourite(fixtureItem.ID);

                if(fixture.TourStandings != null)
                    TableCollection = new ObservableCollection<GolfTourStanding>(fixture.TourStandings);

                if(fixture.Scores != null)
                    ScoreCollection = new ObservableCollection<GolfScore>(fixture.Scores);
                
                Device.StartTimer(TimeSpan.FromSeconds(60), () =>
                {
                    if (FixtureItem.FixtureTime <= DateTime.Now && FixtureItem.FixtureTime.AddMinutes(110) >= DateTime.Now && !FixtureItem.IsPostponed)
                    {
                        Device.BeginInvokeOnMainThread(async () => await RefreshFixtures());
                    }
                    else
                        return false;

                    return true;
                });
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "GolfFixture Detail");
            }

            IsActivityIndicatorVisible = false;
        }

        internal async Task RefreshFixtures()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;

                    FixtureItem = await fixtureService.GetGolfFixture(FixtureItem.ID);
                }
                else
                    NoConnectivity = true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "GolfFixture Refresh");
            }
        }

        async Task Favourite()
        {
            try
            {
                if (IsFavourite)
                {
                    IsFavourite = false;

                    await App.Database.DeleteFootballFixtureFavourite(FixtureItem.ID);
                }
                else
                {
                    IsFavourite = true;

                    var favourite = new Favourite
                    {
                        FixtureID = FixtureItem.ID,
                        Type = "GolfFixture"
                    };

                    await App.Database.SaveFootballFavourite(favourite);

                    await App.Database.SaveAlertSetting(new NotiAlert
                    {
                        Sport = favourite.FixtureID,
                        IsAlert = true
                    });

                    var tags = await App.Database.GetTags();
                    try
                    {
                        await notificationRegistrationService.RegisterDeviceAsync(tags);
                    }
                    catch (Exception ex)
                    { }
                }

            }
            catch (Exception ex)
            {
                IsFavourite = !IsFavourite;
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void AdTapped()
        {
            try
            {
                await Task.Run(async () =>
                {
                    await adService.Impressions(Ad.ID);
                });

                await Launcher.OpenAsync(new Uri(Ad.URL));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Ad Tapped");
            }
        }

        #endregion
    }
}
