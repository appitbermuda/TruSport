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
using TruSport.Views.Basketball;
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

namespace TruSport.ViewModels.Basketball
{
    public class FixtureDetailPageViewModel : BaseViewModel
    {
        #region Fields
        private int selectedIndex;
        private BasketballFixture _fixtureItem;
        private RosterCoachListView rosterCoach;
        private ObservableCollection<BasketballLeagueStanding> tableCollection;
        private ObservableCollection<BasketballFixture> homeTeamFixtureCollection;
        private ObservableCollection<BasketballFixture> headToHeadFixtureCollection;
        private ObservableCollection<BasketballFixture> awayTeamFixtureCollection;
        private ObservableCollection<BasketballFixture> fixturesCollection;
        private ObservableCollection<BasketballRoster> rosterCollection;
        private ObservableCollection<RosterListView> subRosterCollection;
        private ObservableCollection<MatchRosterSummary> matchRosterSummaryCollection;

        private double subHeight;
        private bool _isActivityIndicatorVisible;
        private bool isFavourite;
        private bool isFavouriteVisible;
        private bool noConnectivity;
        private bool squadAvailable;
        private bool summaryAvailable;
        private bool rosterOrSquadAvailable;
        private bool cancelFixtureRefresh;
        private bool _hasScore;
        FixtureService fixtureService;
        RosterService rosterService;
        CoachService coachService;
        LeagueTableService leagueTableService;
        NotificationRegistrationService notificationRegistrationService;
        AdService adService;
        INavigation Navigation;

        #endregion

        #region Constructor

        public FixtureDetailPageViewModel(INavigation navigation, BasketballFixture fixture)
        {
            Navigation = navigation;
            HomeTeamFixtureCollection = new ObservableCollection<BasketballFixture>();
            AwayTeamFixtureCollection = new ObservableCollection<BasketballFixture>();
            HeadToHeadFixtureCollection = new ObservableCollection<BasketballFixture>();
            RosterCollection = new ObservableCollection<BasketballRoster>();
            TableCollection = new ObservableCollection<BasketballLeagueStanding>();
            rosterService = new RosterService();
            fixtureService = new FixtureService();
            coachService = new CoachService();
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

        public BasketballFixture FixtureItem
        {
            get { return _fixtureItem; }
            set { Set(ref _fixtureItem, value); }
        }

        public RosterCoachListView RosterCoach
        {
            get { return rosterCoach; }
            set { Set(ref rosterCoach, value); }
        }

        public ObservableCollection<BasketballLeagueStanding> TableCollection
        {
            get { return tableCollection; }
            set { Set(ref tableCollection, value); }
        }

        public ObservableCollection<BasketballFixture> HeadToHeadFixtureCollection
        {
            get { return headToHeadFixtureCollection; }
            set { Set(ref headToHeadFixtureCollection, value); }
        }

        public ObservableCollection<BasketballFixture> HomeTeamFixtureCollection
        {
            get { return homeTeamFixtureCollection; }
            set { Set(ref homeTeamFixtureCollection, value); }
        }

        public ObservableCollection<BasketballFixture> AwayTeamFixtureCollection
        {
            get { return awayTeamFixtureCollection; }
            set { Set(ref awayTeamFixtureCollection, value); }
        }

        public ObservableCollection<BasketballFixture> FixturesCollection
        {
            get { return fixturesCollection; }
            set { Set(ref fixturesCollection, value); }
        }

        public ObservableCollection<BasketballRoster> RosterCollection
        {
            get { return rosterCollection; }
            set { Set(ref rosterCollection, value); }
        }

        public ObservableCollection<MatchRosterSummary> MatchRosterSummaryCollection
        {
            get { return matchRosterSummaryCollection; }
            set { Set(ref matchRosterSummaryCollection, value); }
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

        public bool HasScore
        {
            get { return _hasScore; }
            set { Set(ref _hasScore, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource(BasketballFixture fixtureItem)
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
                            Ad = ads.Any(e => e.Sport == Constants.Basketball) ? ads.FirstOrDefault(e => e.Sport == Constants.Basketball) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                        });
                    }
                });

                if(FixtureItem.Match != null)
                    HasScore = FixtureItem.Match.HomeTeamScore.HasValue && !FixtureItem.Match.AwayTeamScore.HasValue;

                var fixture = await fixtureService.GetBasketballFixture(FixtureItem.ID);

                if (fixtureItem.FixtureTime > DateTime.Now)
                    IsFavouriteVisible = true;
                else
                    IsFavouriteVisible = false;

                IsFavourite = await App.Database.IsFixtureFavourite(fixtureItem.ID);

                TableCollection = new ObservableCollection<BasketballLeagueStanding>(fixture.BasketballLeagueStanding);

                HeadToHeadFixtureCollection = new ObservableCollection<BasketballFixture>(fixture.HeadToHead);

                //MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(fixture.MatchRosterSummary);

                RosterCollection = new ObservableCollection<BasketballRoster>(fixture.Rosters);

                //SubHeight = 7 * 40;

                //var matchRostersList = await rosterService.GetFixtureMatchRosters(fixture.ID);

                //if (matchRostersList == null || matchRostersList.Count == 0)
                //{
                //    SummaryAvailable = false;
                //    SquadAvailable = false;
                //    RosterOrSquadAvailable = true;
                //}
                //else
                //{
                //    List<MatchRoster> homeRoster = new List<MatchRoster>();
                //    List<MatchRoster> subHomeRoster = new List<MatchRoster>();
                //    List<MatchRoster> awayRoster = new List<MatchRoster>();
                //    List<MatchRoster> subAwayRoster = new List<MatchRoster>();
                //    List<RosterListView> rosterLists = new List<RosterListView>();
                //    List<RosterListView> subRosterLists = new List<RosterListView>();
                //    List<RosterCoachListView> coachRosterLists = new List<RosterCoachListView>();

                //    if (matchRostersList.Count > 11)
                //    {
                //        SummaryAvailable = false;
                //        SquadAvailable = true;
                //        RosterOrSquadAvailable = false;

                //        homeRoster = matchRostersList.Where(e => e.TeamID == fixture.HomeTeamID && e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                //        subHomeRoster = matchRostersList.Where(e => e.TeamID == fixture.HomeTeamID && !e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                //        awayRoster = matchRostersList.Where(e => e.TeamID == fixture.AwayTeamID && e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                //        subAwayRoster = matchRostersList.Where(e => e.TeamID == fixture.AwayTeamID && !e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                //    }
                //    else
                //    {
                //        SummaryAvailable = true;
                //        SquadAvailable = false;
                //        RosterOrSquadAvailable = false;

                //        homeRoster = matchRostersList.Where(e => e.TeamID == fixture.HomeTeamID).OrderBy(e => e.Player.LastName).ToList();
                //        awayRoster = matchRostersList.Where(e => e.TeamID == fixture.AwayTeamID).OrderBy(e => e.Player.LastName).ToList();
                //    }


                //    for (var i = 0; i < homeRoster.Count; i++)
                //    {
                //        rosterLists.Add(new RosterListView
                //        {
                //            FixtureID = fixture.ID,
                //            HomeTeamID = fixture.HomeTeamID,
                //            HomePlayerID = homeRoster[i].PlayerID,
                //            HomePlayerName = homeRoster[i].Player.Name,
                //            HomeJerseyNumber = homeRoster[i].JerseyNumber,
                //        });
                //    }

                //    for (var i = 0; i < awayRoster.Count; i++)
                //    {
                //        if(i < rosterLists.Count)
                //        {
                //            rosterLists[i].AwayTeamID = fixture.AwayTeamID;
                //            rosterLists[i].AwayPlayerID = awayRoster[i].PlayerID;
                //            rosterLists[i].AwayPlayerName = awayRoster[i].Player.Name;
                //            rosterLists[i].AwayJerseyNumber = awayRoster[i].JerseyNumber;
                //        }
                //        else
                //        {
                //            rosterLists.Add(new RosterListView
                //            {
                //                FixtureID = fixture.ID,
                //                AwayTeamID = fixture.AwayTeamID,
                //                AwayPlayerID = awayRoster[i].PlayerID,
                //                AwayPlayerName = awayRoster[i].Player.Name,
                //                AwayJerseyNumber = awayRoster[i].JerseyNumber
                //            });
                //        }

                //    }

                //    for (var i = 0; i < 7; i++)
                //    {

                //        RosterListView addRoster = new RosterListView();
                //        addRoster.FixtureID = fixture.ID;

                //        if (i < subHomeRoster.Count)
                //        {
                //            addRoster.HomeTeamID = fixture.HomeTeamID;
                //            addRoster.HomePlayerID = subHomeRoster[i].PlayerID;
                //            addRoster.HomePlayerName = subHomeRoster[i].Player.Name;
                //            addRoster.HomeJerseyNumber = subHomeRoster[i].JerseyNumber;
                //        }

                //        if (i < subAwayRoster.Count)
                //        {
                //            addRoster.AwayTeamID = fixture.AwayTeamID;
                //            addRoster.AwayPlayerID = subAwayRoster[i].PlayerID;
                //            addRoster.AwayPlayerName = subAwayRoster[i].Player.Name;
                //            addRoster.AwayJerseyNumber = subAwayRoster[i].JerseyNumber;
                //        }

                //        if (i < subHomeRoster.Count || i < subAwayRoster.Count)
                //            subRosterLists.Add(addRoster);
                //        else
                //            break;
                //    }



                //    RosterCollection = new ObservableCollection<RosterListView>(rosterLists);

                //    SubRosterCollection = new ObservableCollection<RosterListView>(subRosterLists);

                //    var homeTeamCoach = await coachService.GetTeamCoaches(fixture.HomeTeamID);
                //    var awayTeamCoach = await coachService.GetTeamCoaches(fixture.AwayTeamID);

                //    RosterCoach = new RosterCoachListView
                //    {
                //        HomeTeamID = fixture.HomeTeamID,
                //        HomeCoachID = homeTeamCoach != null ? homeTeamCoach.FirstOrDefault().ID : "",
                //        HomeCoachName = homeTeamCoach != null ? homeTeamCoach.FirstOrDefault().Name : "",
                //        AwayTeamID = fixture.AwayTeamID,
                //        AwayCoachID = awayTeamCoach != null ? awayTeamCoach.FirstOrDefault().ID : "",
                //        AwayCoachName = awayTeamCoach != null ? awayTeamCoach.FirstOrDefault().Name : "",
                //    };

                //    MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(fixture.MatchRosterSummary);

                //}

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
                Debug.WriteLine(ex.Message, "BasketballFixture Detail");
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

                    FixtureItem = await fixtureService.GetBasketballFixture(FixtureItem.ID);
                }
                else
                    NoConnectivity = true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "BasketballFixture Refresh");
            }
        }

        async Task Favourite()
        {
            try
            {
                if (IsFavourite)
                {
                    IsFavourite = false;

                    await App.Database.DeleteBasketballFixtureFavourite(FixtureItem.ID);
                }
                else
                {
                    IsFavourite = true;

                    var favourite = new Favourite
                    {
                        FixtureID = FixtureItem.ID,
                        Type = "BasketballFixture"
                    };

                    await App.Database.SaveBasketballFavourite(favourite);

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
