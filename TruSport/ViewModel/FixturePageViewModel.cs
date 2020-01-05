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

namespace TruSport.ViewModels
{
    public class FixturePageViewModel : BaseViewModel
    {
        #region Fields
        private Fixture tappedInfo;
        public CalendarEventCollection calendarInlineEvents;
        private ObservableCollection<DateTime> noMatchDates;
        private ObservableCollection<string> syncTitleCollection;
        private int selectedIndex;
        private Fixture fixtureItem;
        private RosterCoachListView rosterCoach;
        private ObservableCollection<Fixture> homeTeamFixtureCollection;
        private ObservableCollection<Fixture> headToHeadFixtureCollection;
        private ObservableCollection<Fixture> awayTeamFixtureCollection;
        private ObservableCollection<Fixture> fixturesCollection;
        private ObservableCollection<Fixture> pastCollection;
        private ObservableCollection<Fixture> upcomingCollection;
        private ObservableCollection<LiveFixture> liveCollection;
        private ObservableCollection<RosterListView> rosterCollection;
        private ObservableCollection<RosterListView> subRosterCollection;
        private ObservableCollection<Coach> coachCollection;
        private ObservableCollection<MatchRoster> homeRosterCollection;
        private ObservableCollection<MatchRoster> awayRosterCollection;
        private ObservableCollection<MatchRosterSummary> matchRosterSummaryCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onFixtureSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onLiveFixtureSelectedCommand;
        private Command<object> leagueSelectedCommand;
        private Command calendarVisibilityClickedCommand;
        private Command<object> refreshPastFixturesCommand;
        private Command<object> refreshUpcomingFixturesCommand;
        private Command<object> refreshLiveFixturesCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private double subHeight;
        private bool refreshActive;
        private bool noUpcomingFixtures;
        private bool noLiveFixtures;
        private bool _isActivityIndicatorVisible;
        private bool isFavourite;
        private bool isFavouriteVisible;
        private bool noConnectivity;
        private bool squadAvailable;
        private bool summaryAvailable;
        private bool rosterOrSquadAvailable;
        private bool cancelFixtureRefresh;
        private bool cancelLiveRefresh;
        private bool cancelPastRefresh;
        private bool isUpcomingCalendarVisible;
        private DateTime _selectedDate;
        private DateTime _minDate;
        FixtureService fixtureService;
        MatchRosterService matchRosterService;
        CoachService coachService;
        INavigation Navigation;

        #endregion

        #region Constructor

        public FixturePageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            FixturesCollection = new ObservableCollection<Fixture>();
            PastCollection = new ObservableCollection<Fixture>();
            UpcomingCollection = new ObservableCollection<Fixture>();
            LiveCollection = new ObservableCollection<LiveFixture>();
            SyncTitleCollection = new ObservableCollection<string>();
            CalendarInlineEvents = new CalendarEventCollection();

            fixtureService = new FixtureService();

            SelectedIndex = 0;

            GenerateSource();

            CalendarCellTapped = new Command<CalendarTappedEventArgs>(CellTapped);

            RefreshPastFixturesCommand = new Command<object>(async (obj) => await RefreshPastFixtures());
            RefreshUpcomingFixturesCommand = new Command<object>(async (obj) => await RefreshUpcomingFixtures());
            RefreshLiveFixturesCommand = new Command<object>(async (obj) => await RefreshLiveFixtures());

            OnFixtureSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(FixtureSelected);
            OnLiveFixtureSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(LiveFixtureSelected);
            LeagueSelectedCommand = new Command<object>(SelectedLeague);
            CalendarVisibilityClickedCommand = new Command(CalendarVisibilityClicked);
            //SelectPurchaseCommand = new Command<object>(PurchaseSubscription);

            
            
            MessagingCenter.Subscribe<string>("Fixtures", "RefreshFixtures", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("Fixtures", "RefreshFixtures");
                RefreshFixturesTimer();

            });

        }

        public FixturePageViewModel(INavigation navigation, Fixture fixture)
        {
            Navigation = navigation;
            HomeTeamFixtureCollection = new ObservableCollection<Fixture>();
            AwayTeamFixtureCollection = new ObservableCollection<Fixture>();
            HeadToHeadFixtureCollection = new ObservableCollection<Fixture>();
            RosterCollection = new ObservableCollection<RosterListView>();
            SubRosterCollection = new ObservableCollection<RosterListView>();
            matchRosterService = new MatchRosterService();
            fixtureService = new FixtureService();
            coachService = new CoachService();

            SelectedIndex = 0;

            GenerateSource(fixture);

            FavouriteCommand = new Command(async () => await Favourite());
        }

        public FixturePageViewModel(INavigation navigation, LiveFixture fixture)
        {
            Navigation = navigation;
            HomeTeamFixtureCollection = new ObservableCollection<Fixture>();
            AwayTeamFixtureCollection = new ObservableCollection<Fixture>();
            HeadToHeadFixtureCollection = new ObservableCollection<Fixture>();
            RosterCollection = new ObservableCollection<RosterListView>();
            SubRosterCollection = new ObservableCollection<RosterListView>();
            matchRosterService = new MatchRosterService();
            fixtureService = new FixtureService();
            coachService = new CoachService();

            SelectedIndex = 0;

            GenerateSource(fixture);

            FavouriteCommand = new Command(async () => await Favourite());
        }


        //public PlayerDirectoryPageViewModel()
        //{
        //    PlayerDirectoryCollection = new ObservableCollection<PlayerListView>();
        //    Category = "U21";
        //    
        //    GenerateSource();
        //}

        #endregion

        #region Properties
        internal SfListView PlayerCategoryList
        {
            get;
            set;
        }
        internal SfListView AgentCategoryList
        {
            get;
            set;
        }

        public CalendarEventCollection CalendarInlineEvents
        {
            get { return calendarInlineEvents; }
            set { Set(ref calendarInlineEvents, value); }
        }

    public ICommand CalendarCellTapped { get; set; }
        public Command<object> LeagueSelectedCommand
        {
            get { return leagueSelectedCommand; }
            set { leagueSelectedCommand = value; }
        }

        public Command CalendarVisibilityClickedCommand
        {
            get { return calendarVisibilityClickedCommand; }
            set { calendarVisibilityClickedCommand = value; }
        }

        public Command FavouriteCommand { get; }
        public Command<object> RefreshPastFixturesCommand
        {
            get { return refreshPastFixturesCommand; }
            set { refreshPastFixturesCommand = value; }
        }

        public Command<object> RefreshUpcomingFixturesCommand
        {
            get { return refreshUpcomingFixturesCommand; }
            set { refreshUpcomingFixturesCommand = value; }
        }

        public Command<object> RefreshLiveFixturesCommand
        {
            get { return refreshLiveFixturesCommand; }
            set { refreshLiveFixturesCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnFixtureSelectedCommand
        {
            get { return onFixtureSelectedCommand; }
            set { onFixtureSelectedCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnLiveFixtureSelectedCommand
        {
            get { return onLiveFixtureSelectedCommand; }
            set { onLiveFixtureSelectedCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> ItemTapCommand
        {
            get { return itemtapCommand; }
            set { itemtapCommand = value; }
        }
        public Command<object> FavoriteTapCommand
        {
            get { return favoriteTapCommand; }
            set { favoriteTapCommand = value; }
        }
        public Command<object> ResetTapCommand
        {
            get { return resetTapCommand; }
            set { resetTapCommand = value; }
        }

        public Fixture FixtureItem
        {
            get { return fixtureItem; }
            set { Set(ref fixtureItem, value); }
        }

        public RosterCoachListView RosterCoach
        {
            get { return rosterCoach; }
            set { Set(ref rosterCoach, value); }
        }

        public ObservableCollection<DateTime> NoMatchDates
        {
            get { return noMatchDates; }
            set { Set(ref noMatchDates, value); }
        }

        public ObservableCollection<Fixture> HeadToHeadFixtureCollection
        {
            get { return headToHeadFixtureCollection; }
            set { Set(ref headToHeadFixtureCollection, value); }
        }

        public ObservableCollection<Fixture> HomeTeamFixtureCollection
        {
            get { return homeTeamFixtureCollection; }
            set { Set(ref homeTeamFixtureCollection, value); }
        }

        public ObservableCollection<Fixture> AwayTeamFixtureCollection
        {
            get { return awayTeamFixtureCollection; }
            set { Set(ref awayTeamFixtureCollection, value); }
        }

        public ObservableCollection<Fixture> FixturesCollection
        {
            get { return fixturesCollection; }
            set { Set(ref fixturesCollection, value); }
        }

        public ObservableCollection<Fixture> PastCollection
        {
            get { return pastCollection; }
            set { Set(ref pastCollection, value); }
        }

        public ObservableCollection<Fixture> UpcomingCollection
        {
            get { return upcomingCollection; }
            set { Set(ref upcomingCollection, value); }
        }

        public ObservableCollection<LiveFixture> LiveCollection
        {
            get { return liveCollection; }
            set { Set(ref liveCollection, value); }
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

        public ObservableCollection<Coach> CoachCollection
        {
            get { return coachCollection; }
            set { Set(ref coachCollection, value); }
        }

        public ObservableCollection<MatchRoster> HomeRosterCollection
        {
            get { return homeRosterCollection; }
            set { Set(ref homeRosterCollection, value); }
        }

        public ObservableCollection<MatchRoster> AwayRosterCollection
        {
            get { return awayRosterCollection; }
            set { Set(ref awayRosterCollection, value); }
        }

        public ObservableCollection<MatchRosterSummary> MatchRosterSummaryCollection
        {
            get { return matchRosterSummaryCollection; }
            set { Set(ref matchRosterSummaryCollection, value); }
        }

        public ObservableCollection<string> SyncTitleCollection
        {
            get { return syncTitleCollection; }
            set { Set(ref syncTitleCollection, value); }
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

        public bool RefreshActive
        {
            get { return refreshActive; }
            set { Set(ref refreshActive, value); }
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

        public bool NoUpcomingFixtures
        {
            get { return noUpcomingFixtures; }
            set { Set(ref noUpcomingFixtures, value); }
        }

        public bool NoLiveFixtures
        {
            get { return noLiveFixtures; }
            set { Set(ref noLiveFixtures, value); }
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

        public bool IsUpcomingCalendarVisible
        {
            get { return isUpcomingCalendarVisible; }
            set { Set(ref isUpcomingCalendarVisible, value); }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { Set(ref _selectedDate, value); }
        }

        public DateTime MinDate
        {
            get { return _minDate; }
            set { Set(ref _minDate, value); }
        }

        //public bool CancelLiveRefresh
        //{
        //    get { return cancelLiveRefresh; }
        //    set { Set(ref cancelLiveRefresh, value); }
        //}

        //public bool CancelPastRefresh
        //{
        //    get { return cancelPastRefresh; }
        //    set { Set(ref cancelPastRefresh, value); }
        //}

        #endregion

        #region Generate Source

        internal async void GenerateSource(string CategoryID)
        {
            IsActivityIndicatorVisible = true;

            //var fixtures = await databaseManager.GetFixtures();
            //var oFixturesList = new ObservableCollection<Fixture>(fixtures);
            //var fixturesList = oFixturesList.OrderBy(e => e.Date);

            //foreach (var fixture in fixturesList)
            //{
            //    FixturesCollection.Add(fixture);
            //}

            IsActivityIndicatorVisible = false;
            //var player = new PlayerListView
            //{
            //    FirstName = "Kacy Milan",
            //    LastName = "Butterfield",
            //    Height = 77,
            //    Weight = 168,
            //    CategoryID = CategoryID,
            //    Positions = new PositionListView[] { new PositionListView { PositionTypeID = "2AC71725-DC75-4953-90AC-09A3031F3795",PlayerID = "08424B47-F31F-485B-BEBE-3DB3D5108E77"  }, new PositionListView { PositionTypeID = "16E702FA-F705-4A63-BBD3-4FF07D7AEF3C",PlayerID = "08424B47-F31F-485B-BEBE-3DB3D5108E77" } }
            //};
            //PlayerDirectoryCollection.Add(player);
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        internal async void GenerateAdminSource()
        {
            IsActivityIndicatorVisible = true;

            //var fixtures = await databaseManager.GetFixtures();
            //var oFixturesList = new ObservableCollection<Fixture>(fixtures);
            //var fixturesList = oFixturesList.OrderBy(e => e.Date);

            //foreach (var fixture in fixturesList)
            //{
            //    FixturesCollection.Add(fixture);
            //}

            IsActivityIndicatorVisible = false;
            //var player = new PlayerListView
            //{
            //    FirstName = "Kacy Milan",
            //    LastName = "Butterfield",
            //    Height = 77,
            //    Weight = 168,
            //    CategoryID = CategoryID,
            //    Positions = new PositionListView[] { new PositionListView { PositionTypeID = "2AC71725-DC75-4953-90AC-09A3031F3795",PlayerID = "08424B47-F31F-485B-BEBE-3DB3D5108E77"  }, new PositionListView { PositionTypeID = "16E702FA-F705-4A63-BBD3-4FF07D7AEF3C",PlayerID = "08424B47-F31F-485B-BEBE-3DB3D5108E77" } }
            //};
            //PlayerDirectoryCollection.Add(player);
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        internal async void GenerateSource(Fixture fixture)
        {
            CancelFixtureRefresh = true;

            IsActivityIndicatorVisible = true;

            try
            {
                if(fixture.FixtureTime > DateTime.Now)
                {
                    IsFavouriteVisible = true;
                }
                else
                {
                    IsFavouriteVisible = false;
                }

                IsFavourite = await App.Database.IsFixtureFavourite(fixture.ID);

                List<Fixture> homeTeamfixtures = await fixtureService.GetFixtures();
                List<Fixture> awayTeamfixtures = await fixtureService.GetFixtures();

                //List<Fixture> awayTeamfixtures = await fixtureService.GetFixtures();
                //var fixtures = await App.Database.GetFixtures();
                FixtureItem = fixture;

                var matchRostersList = await matchRosterService.GetFixtureMatchRosters(fixture.ID);

                if (matchRostersList == null || matchRostersList.Count == 0)
                {
                    SummaryAvailable = false;
                    SquadAvailable = false;
                    RosterOrSquadAvailable = true;
                }
                else
                {
                    List<MatchRoster> homeRoster = new List<MatchRoster>();
                    List<MatchRoster> subHomeRoster = new List<MatchRoster>();
                    List<MatchRoster> awayRoster = new List<MatchRoster>();
                    List<MatchRoster> subAwayRoster = new List<MatchRoster>();
                    List<RosterListView> rosterLists = new List<RosterListView>();
                    List<RosterListView> subRosterLists = new List<RosterListView>();
                    List<RosterCoachListView> coachRosterLists = new List<RosterCoachListView>();

                    if (matchRostersList.Count > 11)
                    {
                        SummaryAvailable = false;
                        SquadAvailable = true;
                        RosterOrSquadAvailable = false;

                        homeRoster = matchRostersList.Where(e => e.TeamID == fixture.HomeTeamID && e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                        subHomeRoster = matchRostersList.Where(e => e.TeamID == fixture.HomeTeamID && !e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                        awayRoster = matchRostersList.Where(e => e.TeamID == fixture.AwayTeamID && e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                        subAwayRoster = matchRostersList.Where(e => e.TeamID == fixture.AwayTeamID && !e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                    }
                    else
                    {
                        SummaryAvailable = true;
                        SquadAvailable = false;
                        RosterOrSquadAvailable = false;

                        homeRoster = matchRostersList.Where(e => e.TeamID == fixture.HomeTeamID).OrderBy(e => e.Player.LastName).ToList();
                        awayRoster = matchRostersList.Where(e => e.TeamID == fixture.AwayTeamID).OrderBy(e => e.Player.LastName).ToList();
                    }

                    
                    for (var i = 0; i < homeRoster.Count; i++)
                    {
                        rosterLists.Add(new RosterListView
                        {
                            FixtureID = fixture.ID,
                            HomeTeamID = fixture.HomeTeamID,
                            HomePlayerID = homeRoster[i].PlayerID,
                            HomePlayerName = homeRoster[i].Player.Name,
                            HomeJerseyNumber = homeRoster[i].JerseyNumber,
                            //AwayTeamID = fixture.AwayTeamID,
                            //AwayPlayerID = awayRoster[i].PlayerID,
                            //AwayPlayerName = awayRoster[i].Player.Name,
                            //AwayJerseyNumber = awayRoster[i].JerseyNumber
                        });
                    }

                    for (var i = 0; i < awayRoster.Count; i++)
                    {
                        if(i < rosterLists.Count)
                        {
                            rosterLists[i].AwayTeamID = fixture.AwayTeamID;
                            rosterLists[i].AwayPlayerID = awayRoster[i].PlayerID;
                            rosterLists[i].AwayPlayerName = awayRoster[i].Player.Name;
                            rosterLists[i].AwayJerseyNumber = awayRoster[i].JerseyNumber;
                        }
                        else
                        {
                            rosterLists.Add(new RosterListView
                            {
                                FixtureID = fixture.ID,
                                AwayTeamID = fixture.AwayTeamID,
                                AwayPlayerID = awayRoster[i].PlayerID,
                                AwayPlayerName = awayRoster[i].Player.Name,
                                AwayJerseyNumber = awayRoster[i].JerseyNumber
                            });
                        }
                        
                    }

                    for (var i = 0; i < 7; i++)
                    {

                        RosterListView addRoster = new RosterListView();
                        addRoster.FixtureID = fixture.ID;

                        if (i < subHomeRoster.Count)
                        {
                            addRoster.HomeTeamID = fixture.HomeTeamID;
                            addRoster.HomePlayerID = subHomeRoster[i].PlayerID;
                            addRoster.HomePlayerName = subHomeRoster[i].Player.Name;
                            addRoster.HomeJerseyNumber = subHomeRoster[i].JerseyNumber;
                        }

                        if (i < subAwayRoster.Count)
                        {
                            addRoster.AwayTeamID = fixture.AwayTeamID;
                            addRoster.AwayPlayerID = subAwayRoster[i].PlayerID;
                            addRoster.AwayPlayerName = subAwayRoster[i].Player.Name;
                            addRoster.AwayJerseyNumber = subAwayRoster[i].JerseyNumber;
                        }

                        if (i < subHomeRoster.Count || i < subAwayRoster.Count)
                            subRosterLists.Add(addRoster);
                        else
                            break;
                    }

                    SubHeight = subRosterLists.Count() * 40;

                    RosterCollection = new ObservableCollection<RosterListView>(rosterLists);

                    SubRosterCollection = new ObservableCollection<RosterListView>(subRosterLists);

                    //CoachCollection = new ObservableCollection<RosterCoachListView>();
                    var homeTeamCoach = await coachService.GetTeamCoaches(fixture.HomeTeamID);
                    var awayTeamCoach = await coachService.GetTeamCoaches(fixture.AwayTeamID);

                    RosterCoach = new RosterCoachListView
                    {
                        HomeTeamID = fixture.HomeTeamID,
                        HomeCoachID = homeTeamCoach != null ? homeTeamCoach.FirstOrDefault().ID : "",
                        HomeCoachName = homeTeamCoach != null ? homeTeamCoach.FirstOrDefault().Name : "",
                        AwayTeamID = fixture.AwayTeamID,
                        AwayCoachID = awayTeamCoach != null ? awayTeamCoach.FirstOrDefault().ID : "",
                        AwayCoachName = awayTeamCoach != null ? awayTeamCoach.FirstOrDefault().Name : "",
                    };


                    List<MatchRosterSummary> matchRosterSummaries = new List<MatchRosterSummary>();
                    bool IsHomeTeam = false;

                    var matchRosters = homeRoster.Union(awayRoster).ToList();
                    foreach (var roster in matchRosters)
                    {
                        if (roster.TeamID == fixture.HomeTeamID)
                            IsHomeTeam = true;
                        else
                            IsHomeTeam = false;

                        if (roster.MatchStats.Count > 0)
                        {
                            for (var i = 0; i < roster.MatchStats.Count; i++)
                            {
                                if (roster.MatchStats[i].Goal > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = roster.FixtureID,
                                        TeamID = roster.TeamID,
                                        PlayerID = roster.PlayerID,
                                        PlayerName = roster.Player.Name,
                                        AssistPlayerID = roster.MatchStats[i].AssistPlayerID,
                                        AssistPlayerName = roster.MatchStats[i].AssistPlayer != null ? roster.MatchStats[i].AssistPlayer.Name : null,
                                        Minute = roster.MatchStats[i].GoalTime ?? 0,
                                        Goal = 1,
                                        IsHomeTeam = IsHomeTeam
                                    });
                                }

                                if (roster.MatchStats[i].YellowCard > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = roster.FixtureID,
                                        TeamID = roster.TeamID,
                                        PlayerID = roster.PlayerID,
                                        PlayerName = roster.Player.Name,
                                        Minute = roster.MatchStats[i].YellowCardTime ?? 0,
                                        YellowCard = 1,
                                        IsHomeTeam = IsHomeTeam
                                    });
                                }

                                if (roster.MatchStats[i].RedCard > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = roster.FixtureID,
                                        TeamID = roster.TeamID,
                                        PlayerID = roster.PlayerID,
                                        PlayerName = roster.Player.Name,
                                        Minute = roster.MatchStats[i].RedCardTime ?? 0,
                                        RedCard = 1,
                                        IsHomeTeam = IsHomeTeam
                                    });
                                }
                            }
                        }

                        if (roster.SubstitutePlayerID != null && roster.IsStarter)
                        {
                            matchRosterSummaries.Add(new MatchRosterSummary
                            {
                                FixtureID = roster.FixtureID,
                                TeamID = roster.TeamID,
                                PlayerID = roster.PlayerID,
                                PlayerName = roster.Player.Name,
                                SubstitutePlayerID = roster.SubstitutePlayerID,
                                SubstitutePlayerName = roster.SubstitutePlayer.Name,
                                Minute = roster.SubstituteTime ?? 0,
                                IsSub = true,
                                IsHomeTeam = IsHomeTeam
                            });

                        }
                    }

                    if (matchRosterSummaries.Any(e => e.Minute == -1))
                        MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(matchRosterSummaries);
                    else
                        MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(matchRosterSummaries.OrderBy(e => e.Minute));

                }

                

                var homeTeamFixtures = homeTeamfixtures.Where(e => (e.HomeTeamID == fixture.HomeTeamID || e.AwayTeamID == fixture.HomeTeamID) && e.ID != fixture.ID && e.FixtureTime < DateTime.Now.AddMinutes(-90)).ToList();
                var awayTeamFixtures = awayTeamfixtures.Where(e => (e.HomeTeamID == fixture.AwayTeamID || e.AwayTeamID == fixture.AwayTeamID) && e.ID != fixture.ID && e.FixtureTime < DateTime.Now.AddMinutes(-90)).ToList();
                homeTeamFixtures.ForEach(e => e.SelectedTeamID = fixture.HomeTeamID);
                homeTeamFixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == fixture.HomeTeamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) ? "W" : (e.HomeTeamID == fixture.HomeTeamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) ? "L" : (e.AwayTeamID == fixture.HomeTeamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) ? "W" : (e.AwayTeamID == fixture.HomeTeamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) ? "L" : "D");
                HomeTeamFixtureCollection = new ObservableCollection<Fixture>(homeTeamFixtures.Where(e => (e.Match.HomeTeamScore.HasValue && e.Match.AwayTeamScore.HasValue) && !e.IsPostponed));
                //HomeTeamFixtureCollection.ForEach(e => e.SelectedTeamID = fixture.HomeTeamID);
                //HomeTeamFixtureCollection.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == fixture.HomeTeamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) ? "W" : (e.HomeTeamID == fixture.HomeTeamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) ? "L" : (e.AwayTeamID == fixture.HomeTeamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) ? "W" : (e.AwayTeamID == fixture.HomeTeamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) ? "L" : "D");

                var headToHeadFixtures = homeTeamfixtures.Where(e => ((e.HomeTeamID == fixture.HomeTeamID && e.AwayTeamID == fixture.AwayTeamID) || (e.HomeTeamID == fixture.AwayTeamID && e.AwayTeamID == fixture.HomeTeamID)) && e.ID != fixture.ID && e.FixtureTime < DateTime.Now.AddMinutes(-90));

                //var headToHeadFixtures = fixtures.Where(e => ((e.HomeTeamID == fixture.HomeTeamID && e.AwayTeamID == fixture.AwayTeamID) || (e.HomeTeamID == fixture.AwayTeamID && e.AwayTeamID == fixture.HomeTeamID)));
                HeadToHeadFixtureCollection = new ObservableCollection<Fixture>(headToHeadFixtures.Where(e => (e.Match.HomeTeamScore.HasValue && e.Match.AwayTeamScore.HasValue) && !e.IsPostponed));

                //var awayTeamFixtures = fixtures.Where(e => (e.HomeTeamID == fixture.AwayTeamID || e.AwayTeamID == fixture.AwayTeamID) && e.ID != fixture.ID);
                awayTeamFixtures.ForEach(e => e.SelectedTeamID = fixture.AwayTeamID);
                awayTeamFixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == fixture.AwayTeamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) ? "W" : (e.HomeTeamID == fixture.AwayTeamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) ? "L" : (e.AwayTeamID == fixture.AwayTeamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) ? "W" : (e.AwayTeamID == fixture.AwayTeamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) ? "L" : "D");
                AwayTeamFixtureCollection = new ObservableCollection<Fixture>(awayTeamFixtures.Where(e => (e.Match.HomeTeamScore.HasValue && e.Match.AwayTeamScore.HasValue) && !e.IsPostponed));

                //if (fixture.Date.Add(TimeSpan.Parse(fixture.Time)) == DateTime.Now.Date && (fixture.Date.Add(TimeSpan.Parse(fixture.Time)).AddMinutes(105) > DateTime.Now && fixture.Date.Add(TimeSpan.Parse(fixture.Time)) < DateTime.Now))
                //{
                //    fixture.GameTime = (105 - (TimeSpan.Parse(fixture.Time).Add(TimeSpan.FromMinutes(105)) - DateTime.Now.TimeOfDay).TotalMinutes).ToString("##");

                //    Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(60), () =>
                //    {
                //        if (Convert.ToInt32(fixture.GameTime) > 0)
                //        {
                //            fixture.GameTime = (105 - (TimeSpan.Parse(fixture.Time).Add(TimeSpan.FromMinutes(105)) - DateTime.Now.TimeOfDay).TotalMinutes).ToString("##");
                            
                //        }
                //        else
                //            return false;

                //        return true;
                //    });
                //}

            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        internal async void GenerateSource(LiveFixture fixture)
        {
            CancelFixtureRefresh = true;

            IsActivityIndicatorVisible = true;

            try
            {
                IsFavouriteVisible = false;
                IsFavourite = await App.Database.IsFixtureFavourite(fixture.ID);

                //if(fixt)
                var thisFixture = await fixtureService.Get(fixture.ID);
                List<Fixture> homeTeamfixtures = await fixtureService.GetFixtures();
                List<Fixture> awayTeamfixtures = await fixtureService.GetFixtures();

                //List<Fixture> awayTeamfixtures = await fixtureService.GetFixtures();
                //var fixtures = await App.Database.GetFixtures();
                FixtureItem = thisFixture;

                var matchRostersList = await matchRosterService.GetFixtureMatchRosters(fixture.ID);

                if (matchRostersList == null || matchRostersList.Count == 0)
                {
                    SummaryAvailable = false;
                }
                else
                {
                    SummaryAvailable = true;

                    List<RosterListView> rosterLists = new List<RosterListView>();
                    List<RosterListView> subRosterLists = new List<RosterListView>();
                    List<RosterCoachListView> coachRosterLists = new List<RosterCoachListView>();
                    var homeRoster = matchRostersList.Where(e => e.TeamID == fixture.HomeTeamID && e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                    var subHomeRoster = matchRostersList.Where(e => e.TeamID == fixture.HomeTeamID && !e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                    var awayRoster = matchRostersList.Where(e => e.TeamID == fixture.AwayTeamID && e.IsStarter).OrderBy(e => e.Player.LastName).ToList();
                    var subAwayRoster = matchRostersList.Where(e => e.TeamID == fixture.AwayTeamID && !e.IsStarter).OrderBy(e => e.Player.LastName).ToList();

                    for (var i = 0; i < homeRoster.Count; i++)
                    {
                        rosterLists.Add(new RosterListView
                        {
                            FixtureID = fixture.ID,
                            HomeTeamID = fixture.HomeTeamID,
                            HomePlayerID = homeRoster[i].PlayerID,
                            HomePlayerName = homeRoster[i].Player.Name,
                            HomeJerseyNumber = homeRoster[i].JerseyNumber,
                            //AwayTeamID = fixture.AwayTeamID,
                            //AwayPlayerID = awayRoster[i].PlayerID,
                            //AwayPlayerName = awayRoster[i].Player.Name,
                            //AwayJerseyNumber = awayRoster[i].JerseyNumber
                        });
                    }

                    for (var i = 0; i < awayRoster.Count; i++)
                    {
                        if (i < rosterLists.Count)
                        {
                            rosterLists[i].AwayTeamID = fixture.AwayTeamID;
                            rosterLists[i].AwayPlayerID = awayRoster[i].PlayerID;
                            rosterLists[i].AwayPlayerName = awayRoster[i].Player.Name;
                            rosterLists[i].AwayJerseyNumber = awayRoster[i].JerseyNumber;
                        }
                        else
                        {
                            rosterLists.Add(new RosterListView
                            {
                                FixtureID = fixture.ID,
                                AwayTeamID = fixture.AwayTeamID,
                                AwayPlayerID = awayRoster[i].PlayerID,
                                AwayPlayerName = awayRoster[i].Player.Name,
                                AwayJerseyNumber = awayRoster[i].JerseyNumber
                            });
                        }

                    }

                    for (var i = 0; i < 7; i++)
                    {

                        RosterListView addRoster = new RosterListView();
                        addRoster.FixtureID = fixture.ID;

                        if (i < subHomeRoster.Count)
                        {
                            addRoster.HomeTeamID = fixture.HomeTeamID;
                            addRoster.HomePlayerID = subHomeRoster[i].PlayerID;
                            addRoster.HomePlayerName = subHomeRoster[i].Player.Name;
                            addRoster.HomeJerseyNumber = subHomeRoster[i].JerseyNumber;
                        }

                        if (i < subAwayRoster.Count)
                        {
                            addRoster.AwayTeamID = fixture.AwayTeamID;
                            addRoster.AwayPlayerID = subAwayRoster[i].PlayerID;
                            addRoster.AwayPlayerName = subAwayRoster[i].Player.Name;
                            addRoster.AwayJerseyNumber = subAwayRoster[i].JerseyNumber;
                        }

                        if (i < subHomeRoster.Count || i < subAwayRoster.Count)
                            subRosterLists.Add(addRoster);
                        else
                            break;
                    }

                    SubHeight = subRosterLists.Count() * 40;

                    RosterCollection = new ObservableCollection<RosterListView>(rosterLists);

                    SubRosterCollection = new ObservableCollection<RosterListView>(subRosterLists);

                    //CoachCollection = new ObservableCollection<RosterCoachListView>();
                    var homeTeamCoach = await coachService.GetTeamCoaches(fixture.HomeTeamID);
                    var awayTeamCoach = await coachService.GetTeamCoaches(fixture.AwayTeamID);

                    RosterCoach = new RosterCoachListView
                    {
                        HomeTeamID = fixture.HomeTeamID,
                        HomeCoachID = homeTeamCoach != null ? homeTeamCoach.FirstOrDefault().ID : "",
                        HomeCoachName = homeTeamCoach != null ? homeTeamCoach.FirstOrDefault().Name : "",
                        AwayTeamID = fixture.AwayTeamID,
                        AwayCoachID = awayTeamCoach != null ? awayTeamCoach.FirstOrDefault().ID : "",
                        AwayCoachName = awayTeamCoach != null ? awayTeamCoach.FirstOrDefault().Name : "",
                    };


                    List<MatchRosterSummary> matchRosterSummaries = new List<MatchRosterSummary>();
                    bool IsHomeTeam = false;

                    var matchRosters = homeRoster.Union(awayRoster).ToList();
                    foreach (var roster in matchRosters)
                    {
                        if (roster.TeamID == fixture.HomeTeamID)
                            IsHomeTeam = true;
                        else
                            IsHomeTeam = false;

                        if (roster.MatchStats.Count > 0)
                        {
                            for (var i = 0; i < roster.MatchStats.Count; i++)
                            {
                                if (roster.MatchStats[i].Goal > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = roster.FixtureID,
                                        TeamID = roster.TeamID,
                                        PlayerID = roster.PlayerID,
                                        PlayerName = roster.Player.Name,
                                        AssistPlayerID = roster.MatchStats[i].AssistPlayerID,
                                        AssistPlayerName = roster.MatchStats[i].AssistPlayer != null ? roster.MatchStats[i].AssistPlayer.Name : null,
                                        Minute = roster.MatchStats[i].GoalTime ?? 0,
                                        Goal = 1,
                                        IsHomeTeam = IsHomeTeam
                                    });
                                }

                                if (roster.MatchStats[i].YellowCard > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = roster.FixtureID,
                                        TeamID = roster.TeamID,
                                        PlayerID = roster.PlayerID,
                                        PlayerName = roster.Player.Name,
                                        Minute = roster.MatchStats[i].YellowCardTime ?? 0,
                                        YellowCard = 1,
                                        IsHomeTeam = IsHomeTeam
                                    });
                                }

                                if (roster.MatchStats[i].RedCard > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = roster.FixtureID,
                                        TeamID = roster.TeamID,
                                        PlayerID = roster.PlayerID,
                                        PlayerName = roster.Player.Name,
                                        Minute = roster.MatchStats[i].RedCardTime ?? 0,
                                        RedCard = 1,
                                        IsHomeTeam = IsHomeTeam
                                    });
                                }
                            }
                        }

                        if (roster.SubstitutePlayerID != null && roster.IsStarter)
                        {
                            matchRosterSummaries.Add(new MatchRosterSummary
                            {
                                FixtureID = roster.FixtureID,
                                TeamID = roster.TeamID,
                                PlayerID = roster.PlayerID,
                                PlayerName = roster.Player.Name,
                                SubstitutePlayerID = roster.SubstitutePlayerID,
                                SubstitutePlayerName = roster.SubstitutePlayer.Name,
                                Minute = roster.SubstituteTime ?? 0,
                                IsSub = true,
                                IsHomeTeam = IsHomeTeam
                            });

                        }
                    }

                    if (matchRosterSummaries.Any(e => e.Minute == -1))
                        MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(matchRosterSummaries);
                    else
                        MatchRosterSummaryCollection = new ObservableCollection<MatchRosterSummary>(matchRosterSummaries.OrderBy(e => e.Minute));

                }



                var homeTeamFixtures = homeTeamfixtures.Where(e => (e.HomeTeamID == fixture.HomeTeamID || e.AwayTeamID == fixture.HomeTeamID) && e.ID != fixture.ID && e.FixtureTime < DateTime.Now.AddMinutes(-90)).ToList();
                var awayTeamFixtures = awayTeamfixtures.Where(e => (e.HomeTeamID == fixture.AwayTeamID || e.AwayTeamID == fixture.AwayTeamID) && e.ID != fixture.ID && e.FixtureTime < DateTime.Now.AddMinutes(-90)).ToList();
                homeTeamFixtures.ForEach(e => e.SelectedTeamID = fixture.HomeTeamID);
                homeTeamFixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == fixture.HomeTeamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) ? "W" : (e.HomeTeamID == fixture.HomeTeamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) ? "L" : (e.AwayTeamID == fixture.HomeTeamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) ? "W" : (e.AwayTeamID == fixture.HomeTeamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) ? "L" : "D");
                HomeTeamFixtureCollection = new ObservableCollection<Fixture>(homeTeamFixtures);
                //HomeTeamFixtureCollection.ForEach(e => e.SelectedTeamID = fixture.HomeTeamID);
                //HomeTeamFixtureCollection.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == fixture.HomeTeamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) ? "W" : (e.HomeTeamID == fixture.HomeTeamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) ? "L" : (e.AwayTeamID == fixture.HomeTeamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) ? "W" : (e.AwayTeamID == fixture.HomeTeamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) ? "L" : "D");

                var headToHeadFixtures = homeTeamfixtures.Where(e => ((e.HomeTeamID == fixture.HomeTeamID && e.AwayTeamID == fixture.AwayTeamID) || (e.HomeTeamID == fixture.AwayTeamID && e.AwayTeamID == fixture.HomeTeamID)) && e.ID != fixture.ID && e.FixtureTime < DateTime.Now.AddMinutes(-90));

                //var headToHeadFixtures = fixtures.Where(e => ((e.HomeTeamID == fixture.HomeTeamID && e.AwayTeamID == fixture.AwayTeamID) || (e.HomeTeamID == fixture.AwayTeamID && e.AwayTeamID == fixture.HomeTeamID)));
                HeadToHeadFixtureCollection = new ObservableCollection<Fixture>(headToHeadFixtures);

                //var awayTeamFixtures = fixtures.Where(e => (e.HomeTeamID == fixture.AwayTeamID || e.AwayTeamID == fixture.AwayTeamID) && e.ID != fixture.ID);
                awayTeamFixtures.ForEach(e => e.SelectedTeamID = fixture.AwayTeamID);
                awayTeamFixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == fixture.AwayTeamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) ? "W" : (e.HomeTeamID == fixture.AwayTeamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) ? "L" : (e.AwayTeamID == fixture.AwayTeamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) ? "W" : (e.AwayTeamID == fixture.AwayTeamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) ? "L" : "D");
                AwayTeamFixtureCollection = new ObservableCollection<Fixture>(awayTeamFixtures);

                //if (fixture.Date.Add(TimeSpan.Parse(fixture.Time)) == DateTime.Now.Date && (fixture.Date.Add(TimeSpan.Parse(fixture.Time)).AddMinutes(105) > DateTime.Now && fixture.Date.Add(TimeSpan.Parse(fixture.Time)) < DateTime.Now))
                //{
                //    fixture.GameTime = (105 - (TimeSpan.Parse(fixture.Time).Add(TimeSpan.FromMinutes(105)) - DateTime.Now.TimeOfDay).TotalMinutes).ToString("##");


                //    Xamarin.Forms.Device.StartTimer(TimeSpan.FromMinutes(1), () =>
                //    {
                //        if (Convert.ToInt32(fixture.GameTime) < 0)
                //        {
                //            fixture.GameTime = (105 - (TimeSpan.Parse(fixture.Time).Add(TimeSpan.FromMinutes(105)) - DateTime.Now.TimeOfDay).TotalMinutes).ToString("##");
                //            return true;
                //        }
                //        else
                //        {
                //            return false;
                //        }
                //    });
                //}

                //Device.StartTimer(TimeSpan.FromSeconds(60), () =>
                //{
                //    fixture.GameTime = (105 - (TimeSpan.Parse(fixture.Time).Add(TimeSpan.FromMinutes(105)) - DateTime.Now.TimeOfDay).TotalMinutes).ToString("##");

                //    Device.BeginInvokeOnMainThread(async () => await RefreshFixture(fixture.ID));
                //    //Device.BeginInvokeOnMainThread(async () => await RefreshPastFixtures());

                //    if (Convert.ToInt32(fixture.GameTime) > 0)
                //    {
                //        Device.BeginInvokeOnMainThread(() => { 
                //            fixture.GameTime = (105 - (TimeSpan.Parse(fixture.Time).Add(TimeSpan.FromMinutes(105)) - DateTime.Now.TimeOfDay).TotalMinutes).ToString("##");
                //            });
                //    }
                //    else
                //        return false;

                //    return true;

                //});

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        internal async void GenerateSource()
        {
            CancelFixtureRefresh = true;

            IsActivityIndicatorVisible = true;

            try
            {

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;
                    var fixtures = await fixtureService.GetFixtures();
                    //var fixtures = await App.Database.GetFixtures();

                    IsUpcomingCalendarVisible = false;

                    MinDate = DateTime.Now.Date;

                    //NoMatchDates = Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                    //    .Select(day => new DateTime(year, month, day)) // Map each day to a date
                    //    .ToList(); // Load dates into a list

                    if (fixtures != null)
                    {
                        var pastFixtures = fixtures.Where(e => e.FixtureTime < DateTime.Now);

                        PastCollection = new ObservableCollection<Fixture>(pastFixtures.OrderByDescending(e => e.FixtureTime));

                        var _fixture = fixtures.FirstOrDefault();

                        var upcomingFixtures = fixtures.Where(e => e.FixtureTime >= DateTime.Now);

                        UpcomingCollection = new ObservableCollection<Fixture>(upcomingFixtures.OrderBy(e => e.FixtureTime));

                        if (upcomingFixtures.Count() > 0)
                        {
                            NoUpcomingFixtures = false;
                            SelectedIndex = 1;

                            foreach (var fixture in UpcomingCollection)
                            {
                                CalendarInlineEvent event1 = new CalendarInlineEvent();
                                event1.StartTime = fixture.FixtureTime;
                                event1.EndTime = event1.StartTime.AddHours(2);
                                event1.Subject = string.Format("{0} v {1}", fixture.HomeTeam.Name, fixture.AwayTeam.Name);
                                event1.Color = (Color)App.Current.Resources["primaryDarkBlueTwo"];

                                CalendarInlineEvents.Add(event1);
                            }
                        }
                        else
                            NoUpcomingFixtures = true;

                        //var liveFixturesList = await fixtureService.GetLiveFixtures();

                        var liveFixtures = fixtures.Where(e => e.FixtureTime <= DateTime.Now &&
                            e.FixtureTime.AddMinutes(110) >= DateTime.Now && !e.IsPostponed);


                        if (liveFixtures.Count() > 0)
                        {
                            RefreshActive = true;
                            NoLiveFixtures = false;

                            List<LiveFixture> liveFixturesList = new List<LiveFixture>();

                            foreach (var fixture in liveFixtures)
                            {
                                liveFixturesList.Add(new LiveFixture
                                {
                                    ID = fixture.ID,
                                    FieldID = fixture.FieldID,
                                    FieldName = fixture.Field.Name,
                                    MatchTypeID = fixture.MatchTypeID,
                                    MatchTypeName = fixture.MatchType.Name,
                                    LeagueID = fixture.LeagueID,
                                    LeagueName = fixture.League.Name,
                                    SeasonID = fixture.SeasonID,
                                    SeasonDate = fixture.Season.Date,
                                    HomeTeamID = fixture.HomeTeamID,
                                    HomeTeamName = fixture.HomeTeam.Name,
                                    HomeTeamLogo = fixture.HomeTeam.TeamLogo,
                                    HomeTeamAlias = fixture.HomeTeam.Alias,
                                    HomeTeamPenalty = fixture.Match.HomeTeamPenalty,
                                    HomeTeamScore = fixture.Match.HomeTeamScore,
                                    AwayTeamID = fixture.AwayTeamID,
                                    AwayTeamName = fixture.AwayTeam.Name,
                                    AwayTeamLogo = fixture.AwayTeam.TeamLogo,
                                    AwayTeamAlias = fixture.AwayTeam.Alias,
                                    AwayTeamPenalty = fixture.Match.AwayTeamPenalty,
                                    AwayTeamScore = fixture.Match.AwayTeamScore,
                                    Date = fixture.Date,
                                    //Time = new DateTime().Add(TimeSpan.Parse(fixture.Time)).ToLocalTime().ToString("hh:mm"),
                                    Time = fixture.Time,
                                    IsPenalties = fixture.Match.IsPenalties ?? false
                                });
                            }

                            //RefreshFixturesTimer();
                            //var fixtures = await fixtureService.GetFixtures();

                            Device.StartTimer(TimeSpan.FromSeconds(60), () =>
                            {
                                //var liveFixtures = fixtures.Where(e => e.FixtureTime <= DateTime.Now &&
                                //    e.FixtureTime.AddMinutes(110) >= DateTime.Now && !e.IsPostponed);

                                if (liveFixtures.Count() > 0)
                                {
                                    RefreshActive = true;
                                    Device.BeginInvokeOnMainThread(async () => await RefreshFixtures());
                                }
                                else
                                {
                                    RefreshActive = false;
                                    NoLiveFixtures = true;
                                    return false;
                                }

                                return true;
                            });

                            LiveCollection = new ObservableCollection<LiveFixture>(liveFixturesList);
                        }
                        else
                        {
                            RefreshActive = false;

                            NoLiveFixtures = true;
                        }
                        //if (liveFixturesList.Count() > 0)
                        //{
                        //    NoLiveFixtures = false;

                        //    Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                        //    {
                        //        if (liveFixturesList.Count() > 0)
                        //        {
                        //            Device.BeginInvokeOnMainThread(async () => await RefreshLiveFixtures());
                        //            Device.BeginInvokeOnMainThread(async () => await RefreshPastFixtures());
                        //            return true;
                        //        }
                        //        else
                        //        {
                        //            NoLiveFixtures = true;
                        //            return false;
                        //        }
                        //    });
                        //}
                        //else
                        //    NoLiveFixtures = true;

                        //LiveCollection = new ObservableCollection<LiveFixture>(liveFixturesList);
                    }
                }
                else
                    NoConnectivity = true;

            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        internal async Task RefreshFixture(string fixtureID)
        {

            //IsActivityIndicatorVisible = true;

            //var pastFixtures = await databaseManager.GetPastFixtures();
            //PastCollection = new ObservableCollection<Fixture>(pastFixtures);
            var fixture = await fixtureService.Get(fixtureID);
            //var fixtures = await App.Database.GetFixtures();

            if (fixture != null)
            {
                FixtureItem = fixture;
            }
        }

        internal async Task RefreshFixtures()
        {

            //IsActivityIndicatorVisible = true;

            //var pastFixtures = await databaseManager.GetPastFixtures();
            //PastCollection = new ObservableCollection<Fixture>(pastFixtures);
            var fixtures = await fixtureService.GetFixtures();
            //var fixtures = await App.Database.GetFixtures();

            if (fixtures != null)
            {
                var pastFixtures = fixtures.Where(e => e.FixtureTime <= DateTime.Now);

                PastCollection = new ObservableCollection<Fixture>(pastFixtures);

                var upcomingFixtures = fixtures.Where(e => e.FixtureTime >= DateTime.Now);

                UpcomingCollection = new ObservableCollection<Fixture>(upcomingFixtures.OrderBy(e => e.FixtureTime));

                var liveFixtures = fixtures.Where(e => e.FixtureTime <= DateTime.Now &&
                        e.FixtureTime.AddMinutes(110) >= DateTime.Now && !e.IsPostponed);

                List<LiveFixture> liveFixturesList = new List<LiveFixture>();

                foreach (var fixture in liveFixtures)
                {
                    liveFixturesList.Add(new LiveFixture
                    {
                        ID = fixture.ID,
                        FieldID = fixture.FieldID,
                        FieldName = fixture.Field.Name,
                        MatchTypeID = fixture.MatchTypeID,
                        MatchTypeName = fixture.MatchType.Name,
                        LeagueID = fixture.LeagueID,
                        LeagueName = fixture.League.Name,
                        SeasonID = fixture.SeasonID,
                        SeasonDate = fixture.Season.Date,
                        HomeTeamID = fixture.HomeTeamID,
                        HomeTeamName = fixture.HomeTeam.Name,
                        HomeTeamLogo = fixture.HomeTeam.TeamLogo,
                        HomeTeamAlias = fixture.HomeTeam.Alias,
                        HomeTeamPenalty = fixture.Match.HomeTeamPenalty,
                        HomeTeamScore = fixture.Match.HomeTeamScore,
                        AwayTeamID = fixture.AwayTeamID,
                        AwayTeamName = fixture.AwayTeam.Name,
                        AwayTeamLogo = fixture.AwayTeam.TeamLogo,
                        AwayTeamAlias = fixture.AwayTeam.Alias,
                        AwayTeamPenalty = fixture.Match.AwayTeamPenalty,
                        AwayTeamScore = fixture.Match.AwayTeamScore,
                        Date = fixture.Date,
                        //Time = new DateTime().Add(TimeSpan.Parse(fixture.Time)).ToLocalTime().ToString("hh:mm"),
                        Time = fixture.Time,
                        IsPenalties = fixture.Match.IsPenalties ?? false
                    });
                }

                LiveCollection = new ObservableCollection<LiveFixture>(liveFixturesList);
            }
        }


        internal async Task RefreshPastFixtures()
        {

            //IsActivityIndicatorVisible = true;

            //var pastFixtures = await databaseManager.GetPastFixtures();
            //PastCollection = new ObservableCollection<Fixture>(pastFixtures);
            var fixtures = await fixtureService.GetFixtures();
            //var fixtures = await App.Database.GetFixtures();

            if (fixtures != null)
            {
                var pastFixtures = fixtures.Where(e => e.FixtureTime <= DateTime.Now.AddMinutes(110));
                //var pastFixtures = await databaseManager.GetPastFixtures();
                PastCollection = new ObservableCollection<Fixture>(pastFixtures);

                //IsActivityIndicatorVisible = false;
            }
        }

        internal async Task RefreshUpcomingFixtures()
        {
            var fixtures = await fixtureService.GetFixtures();
            var upcomingFixtures = fixtures.Where(e => e.FixtureTime >= DateTime.Now);
            //var upcomingFixtures = await databaseManager.GetAllUpcomingFixtures();
            //foreach (var upcomingFixture in upcomingFixtures)
            //{
            UpcomingCollection = new ObservableCollection<Fixture>(upcomingFixtures.OrderBy(e => e.FixtureTime));
            //}

            if (upcomingFixtures.Count() > 0)
            {
                NoUpcomingFixtures = false;
                SelectedIndex = 1;
            }
            else
                NoUpcomingFixtures = true;
            //var upcomingFixtures = await databaseManager.GetAllUpcomingFixtures();
            //UpcomingCollection = new ObservableCollection<Fixture>(upcomingFixtures);

        }

        private async void CalendarVisibilityClicked()
        {
            IsUpcomingCalendarVisible = !IsUpcomingCalendarVisible;
        }

        private async void SelectedLeague(object obj)
        {
            var groupResult = obj as Syncfusion.DataSource.Extensions.GroupResult;

            var items = new List<Fixture>(groupResult.Items.ToList<Fixture>());
            var data = items[0];

            //var fixture = fixtures.FirstOrDefault();

            var league = data.League;

            await Navigation.PushAsync(new CompetitionDetailsPage(league));
            
        }

        internal async Task RefreshFixturesTimer()
        {
            if (!RefreshActive)
            {
                var fixtures = await fixtureService.GetFixtures();

                Device.StartTimer(TimeSpan.FromSeconds(60), () =>
                {
                    var liveFixtures = fixtures.Where(e => e.FixtureTime <= DateTime.Now &&
                        e.FixtureTime.AddMinutes(110) >= DateTime.Now);

                    if (liveFixtures.Count() > 0)
                    {
                        RefreshActive = true;
                        Device.BeginInvokeOnMainThread(async () => await RefreshFixtures());
                    }
                    else
                    {
                        RefreshActive = false;
                        NoLiveFixtures = true;
                        return false;
                    }

                    return true;
                });
            }
        }

        internal async Task RefreshLiveFixtures()
        {
            var liveFixturesList = await fixtureService.GetLiveFixtures();

            if (liveFixturesList.Count > 0)
            {
                NoLiveFixtures = false;

            }
            else
                NoLiveFixtures = true;

            //LiveCollection.Clear();
            LiveCollection = new ObservableCollection<LiveFixture>(liveFixturesList);
        }

        async Task Favourite()
        {
            try
            {
                if (IsFavourite)
                {
                    IsFavourite = false;

                    await App.Database.DeleteFixtureFavourite(FixtureItem.ID);
                    //await App.Current.MainPage.DisplayAlert("Success", "Favourite deleted successfully.", "OK");
                }
                else
                {
                    IsFavourite = true;

                    var favourite = new Favourite
                    {
                        FixtureID = FixtureItem.ID,
                        Type = "Fixture"
                    };

                    await App.Database.SaveFavourite(favourite);
                    //await App.Current.MainPage.DisplayAlert("Success", "Favourite saved successfully.", "OK");
                }

                //await DisplayAlert("Error", "There was an issue saving this transfer, please try again or contact us.", "OK");


            }
            catch (Exception ex)
            {
                IsFavourite = !IsFavourite;
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {

                //IsSavingFavourite = false;
                //EnableFavourite = true;
            }
        }

        //private void ResetTapped(object obj)
        //{
        //    secondLV.DataSource.Filter = null;
        //    secondLV.DataSource.RefreshFilter();
        //    firstLV.AllowSwiping = true;
        //}

        //private void FavoriteTapped(object obj)
        //{
        //    var departureInfo = obj as DepartureInfo;
        //    var pinnedInfo = FirstLVCollection.Any(o => o.Name == departureInfo.Name) ? FirstLVCollection.First(o => o.Name == departureInfo.Name) : null;
        //    if (pinnedInfo == null)
        //    {
        //        FirstLVCollection.Add(new PinnedInfo() { Name = departureInfo.Name, RouteName = departureInfo.Name, Icon = departureInfo.Icon, IsFavorite = true });
        //    }
        //}

        private async void FixtureSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as Fixture;

            //CancelFixtureRefresh = false;

            await Navigation.PushAsync(new FixtureDetailsPage(item));
        }

        private async void LiveFixtureSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as LiveFixture;

            //CancelFixtureRefresh = false;

            await Navigation.PushAsync(new FixtureDetailsPage(item));
        }

        private void CellTapped(CalendarTappedEventArgs obj)
        {
            var text = obj.DateTime.ToString("dd/MM/yyyy") + " " + obj.SelectedAppointment.ToString();
            IsUpcomingCalendarVisible = false;
        }

        #endregion

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            tappedInfo = e.ItemData as Fixture;
            //if (tappedInfo.IsFavorite)
            //{
            //    secondLV.DataSource.Filter = FilterDepartures;
            //    tappedInfo.IsFavorite = false;
            //}
            //else
            //{
            //    secondLV.DataSource.Filter = null;
            //    tappedInfo.IsFavorite = true;
            //}
            //secondLV.DataSource.RefreshFilter();
        }

        //private bool FilterDepartures(object obj)
        //{
        //    var departureInfo = obj as DepartureInfo;
        //    if (tappedInfo == null)
        //        return true;

        //    if (departureInfo.Name.ToLower().Contains(tappedInfo.Name.ToLower())
        //         || departureInfo.RouteName.ToLower().Contains(tappedInfo.RouteName.ToLower()))
        //        return true;
        //    else
        //        return false;
        //}

        #region Player Info

        Fixture[] Player = new Fixture[]
         {
            
         };

        string[] Agents = new string[]
        {
            "LOCAL",
            "INTERNATIONAL"
        };

        string[] SyncTitles = new string[]
        {
            "First Division",
            "Premier Division",
            "Corona League"
        };

        #endregion
    }
}
