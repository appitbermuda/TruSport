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
using TruSport.Views.Bowling;

namespace TruSport.ViewModels.Bowling
{
    public class FixtureDetailPageViewModel : BaseViewModel
    {
        #region Fields
        private BowlingLeagueStanding tappedInfo;
        private int selectedIndex;
        private BowlingFixture _fixtureItem;
        private RosterCoachListView rosterCoach;
        private ObservableCollection<BowlingLeagueStanding> tableCollection;
        private ObservableCollection<BowlingFixture> homeTeamFixtureCollection;
        private ObservableCollection<BowlingFixture> headToHeadFixtureCollection;
        private ObservableCollection<BowlingFixture> awayTeamFixtureCollection;
        private ObservableCollection<BowlingFixture> fixturesCollection;
        private ObservableCollection<BowlingRosterListView> rosterCollection;
        private ObservableCollection<BowlingGameResult> bowlingScoreCollection;

        private Ad _ad;
        private double subHeight;
        private bool _isHeadToHeadActivityIndicatorVisible;
        private bool _isTableActivityIndicatorVisible;
        private bool _isActivityIndicatorVisible;
        private bool isFavourite;
        private bool isFavouriteVisible;
        private bool noConnectivity;
        private bool squadAvailable;
        private bool summaryAvailable;
        private bool rosterOrSquadAvailable;
        private bool cancelFixtureRefresh;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        FixtureService fixtureService;
        RosterService rosterService;
        CoachService coachService;
        LeagueTableService leagueTableService;
        AdService adService;
        INavigation Navigation;

        #endregion

        #region Constructor

        public FixtureDetailPageViewModel(INavigation navigation, BowlingFixture fixture)
        {
            Navigation = navigation;
            HomeTeamFixtureCollection = new ObservableCollection<BowlingFixture>();
            AwayTeamFixtureCollection = new ObservableCollection<BowlingFixture>();
            HeadToHeadFixtureCollection = new ObservableCollection<BowlingFixture>();
            RosterCollection = new ObservableCollection<BowlingRosterListView>();
            TableCollection = new ObservableCollection<BowlingLeagueStanding>();
            rosterService = new RosterService();
            fixtureService = new FixtureService();
            coachService = new CoachService();
            leagueTableService = new LeagueTableService();
            adService = new AdService();

            SelectedIndex = 0;

            GenerateSource(fixture);

            FavouriteCommand = new Command(async () => await Favourite());
            TableTappedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
            AdTappedCommand = new Command(AdTapped);
        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }
        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> TableTappedCommand
        {
            get { return itemtapCommand; }
            set { itemtapCommand = value; }
        }
        public Command FavouriteCommand { get; }

        public BowlingFixture FixtureItem
        {
            get { return _fixtureItem; }
            set { Set(ref _fixtureItem, value); }
        }

        public RosterCoachListView RosterCoach
        {
            get { return rosterCoach; }
            set { Set(ref rosterCoach, value); }
        }

        public ObservableCollection<BowlingLeagueStanding> TableCollection
        {
            get { return tableCollection; }
            set { Set(ref tableCollection, value); }
        }

        public ObservableCollection<BowlingFixture> HeadToHeadFixtureCollection
        {
            get { return headToHeadFixtureCollection; }
            set { Set(ref headToHeadFixtureCollection, value); }
        }

        public ObservableCollection<BowlingFixture> HomeTeamFixtureCollection
        {
            get { return homeTeamFixtureCollection; }
            set { Set(ref homeTeamFixtureCollection, value); }
        }

        public ObservableCollection<BowlingFixture> AwayTeamFixtureCollection
        {
            get { return awayTeamFixtureCollection; }
            set { Set(ref awayTeamFixtureCollection, value); }
        }

        public ObservableCollection<BowlingFixture> FixturesCollection
        {
            get { return fixturesCollection; }
            set { Set(ref fixturesCollection, value); }
        }

        public ObservableCollection<BowlingRosterListView> RosterCollection
        {
            get { return rosterCollection; }
            set { Set(ref rosterCollection, value); }
        }

        public ObservableCollection<BowlingGameResult> BowlingResultCollection
        {
            get { return bowlingScoreCollection; }
            set { Set(ref bowlingScoreCollection, value); }
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

        public bool IsHeadToHeadActivityIndicatorVisible
        {
            get { return _isHeadToHeadActivityIndicatorVisible; }
            set { Set(ref _isHeadToHeadActivityIndicatorVisible, value); }
        }

        public bool IsTableActivityIndicatorVisible
        {
            get { return _isTableActivityIndicatorVisible; }
            set { Set(ref _isTableActivityIndicatorVisible, value); }
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

        public bool SquadAvailable
        {
            get { return squadAvailable; }
            set { Set(ref squadAvailable, value); }
        }

        public bool RosterOrSquadAvailable
        {
            get { return rosterOrSquadAvailable; }
            set { Set(ref rosterOrSquadAvailable, value); }
        }

        public bool SummaryAvailable
        {
            get { return summaryAvailable; }
            set { Set(ref summaryAvailable, value); }
        }

        public bool CancelFixtureRefresh
        {
            get { return cancelFixtureRefresh; }
            set { Set(ref cancelFixtureRefresh, value); }
        }

        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource(BowlingFixture fixtureItem)
        {
            CancelFixtureRefresh = true;

            IsActivityIndicatorVisible = true;
            IsHeadToHeadActivityIndicatorVisible = true;
            IsTableActivityIndicatorVisible = true;

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
                            Ad = ads.Any(e => e.Sport == Constants.Bowling) ? ads.FirstOrDefault(e => e.Sport == Constants.Bowling) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                        });
                    }
                });

                if (fixtureItem.FixtureTime > DateTime.Now)
                    IsFavouriteVisible = true;
                else
                    IsFavouriteVisible = false;

                IsFavourite = await App.Database.IsBowlingFixtureFavourite(fixtureItem.ID);

                var fixture = await fixtureService.GetBowlingFixture(FixtureItem.ID);

                TableCollection = new ObservableCollection<BowlingLeagueStanding>(fixture.LeagueTable);
                IsTableActivityIndicatorVisible = false;

                HeadToHeadFixtureCollection = new ObservableCollection<BowlingFixture>(fixture.HeadToHead);
                IsHeadToHeadActivityIndicatorVisible = false;

                RosterCollection = new ObservableCollection<BowlingRosterListView>(fixture.BowlingRosterList);

                BowlingResultCollection = new ObservableCollection<BowlingGameResult>(fixture.BowlingGameResults);

                SubHeight = 5 * 40;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Fixture Detail");
            }

            IsActivityIndicatorVisible = false;
            IsTableActivityIndicatorVisible = false;
            IsHeadToHeadActivityIndicatorVisible = false;
        }

        internal async Task RefreshFixtures()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;

                    FixtureItem = await fixtureService.GetBowlingFixture(FixtureItem.ID);
                }
                else
                    NoConnectivity = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingFixture Refresh");
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

        async Task Favourite()
        {
            try
            {
                if (IsFavourite)
                {
                    IsFavourite = false;

                    await App.Database.DeleteBowlingFixtureFavourite(FixtureItem.ID);
                }
                else
                {
                    IsFavourite = true;

                    var favourite = new Favourite
                    {
                        BowlingFixtureID = FixtureItem.ID,
                        Type = "Fixture",
                        Sport = "Bowling"
                    };

                    await App.Database.SaveBowlingFavourite(favourite);
                }

            }
            catch (Exception ex)
            {
                IsFavourite = !IsFavourite;
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            try
            {
                tappedInfo = e.ItemData as BowlingLeagueStanding;

                await Navigation.PushModalAsync(new TableDetailPage(tappedInfo));
            }
            catch (Exception ex)
            {

            }

        }

        #endregion
    }
}
