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

namespace TruSport.ViewModels.Cricket
{
    public class FixtureDetailPageViewModel : BaseViewModel
    {
        #region Fields
        private int selectedIndex;
        private CricketFixture _fixtureItem;
        private RosterCoachListView rosterCoach;
        private ObservableCollection<CricketLeagueTable> tableCollection;
        private ObservableCollection<CricketFixture> homeTeamFixtureCollection;
        private ObservableCollection<CricketFixture> headToHeadFixtureCollection;
        private ObservableCollection<CricketFixture> awayTeamFixtureCollection;
        private ObservableCollection<CricketFixture> fixturesCollection;
        private ObservableCollection<RosterListView> rosterCollection;
        private ObservableCollection<RosterListView> subRosterCollection;
        private ObservableCollection<MatchInning> cricketScoreCollection;

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
        FixtureService fixtureService;
        RosterService rosterService;
        CoachService coachService;
        LeagueTableService leagueTableService;
        AdService adService;
        INavigation Navigation;

        #endregion

        #region Constructor

        public FixtureDetailPageViewModel(INavigation navigation, CricketFixture fixture)
        {
            Navigation = navigation;
            HomeTeamFixtureCollection = new ObservableCollection<CricketFixture>();
            AwayTeamFixtureCollection = new ObservableCollection<CricketFixture>();
            HeadToHeadFixtureCollection = new ObservableCollection<CricketFixture>();
            RosterCollection = new ObservableCollection<RosterListView>();
            SubRosterCollection = new ObservableCollection<RosterListView>();
            TableCollection = new ObservableCollection<CricketLeagueTable>();
            rosterService = new RosterService();
            fixtureService = new FixtureService();
            coachService = new CoachService();
            leagueTableService = new LeagueTableService();
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

        public CricketFixture FixtureItem
        {
            get { return _fixtureItem; }
            set { Set(ref _fixtureItem, value); }
        }

        public RosterCoachListView RosterCoach
        {
            get { return rosterCoach; }
            set { Set(ref rosterCoach, value); }
        }

        public ObservableCollection<CricketLeagueTable> TableCollection
        {
            get { return tableCollection; }
            set { Set(ref tableCollection, value); }
        }

        public ObservableCollection<CricketFixture> HeadToHeadFixtureCollection
        {
            get { return headToHeadFixtureCollection; }
            set { Set(ref headToHeadFixtureCollection, value); }
        }

        public ObservableCollection<CricketFixture> HomeTeamFixtureCollection
        {
            get { return homeTeamFixtureCollection; }
            set { Set(ref homeTeamFixtureCollection, value); }
        }

        public ObservableCollection<CricketFixture> AwayTeamFixtureCollection
        {
            get { return awayTeamFixtureCollection; }
            set { Set(ref awayTeamFixtureCollection, value); }
        }

        public ObservableCollection<CricketFixture> FixturesCollection
        {
            get { return fixturesCollection; }
            set { Set(ref fixturesCollection, value); }
        }

        public ObservableCollection<RosterListView> RosterCollection
        {
            get { return rosterCollection; }
            set { Set(ref rosterCollection, value); }
        }

        public ObservableCollection<RosterListView> SubRosterCollection
        {
            get { return subRosterCollection; }
            set { Set(ref subRosterCollection, value); }
        }

        public ObservableCollection<MatchInning> MatchInningCollection
        {
            get { return cricketScoreCollection; }
            set { Set(ref cricketScoreCollection, value); }
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

        #endregion

        #region Generate Source

        internal async void GenerateSource(CricketFixture fixtureItem)
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

                IsFavourite = await App.Database.IsFixtureFavourite(fixtureItem.ID);

                var fixture = await fixtureService.GetCricketFixture(FixtureItem.ID);

                

                TableCollection = new ObservableCollection<CricketLeagueTable>(fixture.LeagueTable);
                IsTableActivityIndicatorVisible = false;

                HeadToHeadFixtureCollection = new ObservableCollection<CricketFixture>(fixture.HeadToHead);
                IsHeadToHeadActivityIndicatorVisible = false;

                MatchInningCollection = new ObservableCollection<MatchInning>(fixture.MatchInnings);

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
                //        if (i < rosterLists.Count)
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

                SubHeight = 5 * 40;

                //RosterCollection = new ObservableCollection<RosterListView>(rosterLists);

                //SubRosterCollection = new ObservableCollection<RosterListView>(subRosterLists);

                //var homeTeamCoach = await coachService.GetTeamCoaches(fixture.HomeTeamID);
                //var awayTeamCoach = await coachService.GetTeamCoaches(fixture.AwayTeamID);

                //RosterCoach = new RosterCoachListView
                //{
                //    HomeTeamID = fixture.HomeTeamID,
                //    HomeCoachID = homeTeamCoach != null ? homeTeamCoach.FirstOrDefault().ID : "",
                //    HomeCoachName = homeTeamCoach != null ? homeTeamCoach.FirstOrDefault().Name : "",
                //    AwayTeamID = fixture.AwayTeamID,
                //    AwayCoachID = awayTeamCoach != null ? awayTeamCoach.FirstOrDefault().ID : "",
                //    AwayCoachName = awayTeamCoach != null ? awayTeamCoach.FirstOrDefault().Name : "",
                //};


                //List<MatchInning> matchRosterSummaries = new List<MatchInning>();
                //bool IsHomeTeam = false;

                //var matchRosters = homeRoster.Union(awayRoster).ToList();
                //foreach (var roster in matchRosters)
                //{
                //    if (roster.TeamID == fixture.HomeTeamID)
                //        IsHomeTeam = true;
                //    else
                //        IsHomeTeam = false;

                //    if (roster.MatchStats.Count > 0)
                //    {
                //        for (var i = 0; i < roster.MatchStats.Count; i++)
                //        {
                //            if (roster.MatchStats[i].Goal > 0)
                //            {
                //                matchRosterSummaries.Add(new MatchInning
                //                {
                //                    FixtureID = roster.FixtureID,
                //                    TeamID = roster.TeamID,
                //                    PlayerID = roster.PlayerID,
                //                    PlayerName = roster.Player.Name,
                //                    AssistPlayerID = roster.MatchStats[i].AssistPlayerID,
                //                    AssistPlayerName = roster.MatchStats[i].AssistPlayer != null ? roster.MatchStats[i].AssistPlayer.Name : null,
                //                    Minute = roster.MatchStats[i].GoalTime ?? 0,
                //                    Goal = 1,
                //                    IsHomeTeam = IsHomeTeam
                //                });
                //            }

                //            if (roster.MatchStats[i].YellowCard > 0)
                //            {
                //                matchRosterSummaries.Add(new MatchInning
                //                {
                //                    FixtureID = roster.FixtureID,
                //                    TeamID = roster.TeamID,
                //                    PlayerID = roster.PlayerID,
                //                    PlayerName = roster.Player.Name,
                //                    Minute = roster.MatchStats[i].YellowCardTime ?? 0,
                //                    YellowCard = 1,
                //                    IsHomeTeam = IsHomeTeam
                //                });
                //            }

                //            if (roster.MatchStats[i].RedCard > 0)
                //            {
                //                matchRosterSummaries.Add(new MatchInning
                //                {
                //                    FixtureID = roster.FixtureID,
                //                    TeamID = roster.TeamID,
                //                    PlayerID = roster.PlayerID,
                //                    PlayerName = roster.Player.Name,
                //                    Minute = roster.MatchStats[i].RedCardTime ?? 0,
                //                    RedCard = 1,
                //                    IsHomeTeam = IsHomeTeam
                //                });
                //            }
                //        }
                //    }

                //    if (roster.SubstitutePlayerID != null && roster.IsStarter)
                //    {
                //        matchRosterSummaries.Add(new MatchInning
                //        {
                //            FixtureID = roster.FixtureID,
                //            TeamID = roster.TeamID,
                //            PlayerID = roster.PlayerID,
                //            PlayerName = roster.Player.Name,
                //            SubstitutePlayerID = roster.SubstitutePlayerID,
                //            SubstitutePlayerName = roster.SubstitutePlayer.Name,
                //            Minute = roster.SubstituteTime ?? 0,
                //            IsSub = true,
                //            IsHomeTeam = IsHomeTeam
                //        });

                //    }
                //}

                //if (matchRosterSummaries.Any(e => e.Minute == -1))
                //    MatchInningCollection = new ObservableCollection<MatchInning>(matchRosterSummaries);
                //else
                //    MatchInningCollection = new ObservableCollection<MatchInning>(matchRosterSummaries.OrderBy(e => e.Minute));

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

                    FixtureItem = await fixtureService.GetCricketFixture(FixtureItem.ID);
                }
                else
                    NoConnectivity = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CricketFixture Refresh");
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

                    await App.Database.DeleteCricketFixtureFavourite(FixtureItem.ID);
                }
                else
                {
                    IsFavourite = true;

                    var favourite = new Favourite
                    {
                        CricketFixtureID = FixtureItem.ID,
                        Type = "Fixture"
                    };

                    await App.Database.SaveCricketFavourite(favourite);
                }

            }
            catch (Exception ex)
            {
                IsFavourite = !IsFavourite;
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        #endregion
    }
}
