using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using TruSport.Services;
using Xamarin.Essentials;

namespace TruSport.ViewModels
{
    public class CompetitionsPageViewModel : BaseViewModel
    {
        #region Fields
        private LeagueTableListView tappedInfo; 
        private ObservableCollection<League> leagueCollection;
        private ObservableCollection<Fixture> fixtureCollection;
        private ObservableCollection<Fixture> fixtureFloatCollection;
        private ObservableCollection<LeagueTable> tableCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool _isActivityVisible;
        private bool isLoadMoreVisible;
        private int totalCount;
        private int tabCount;
        private bool isTableExist;
        private bool noConnectivity;
        FixtureService fixtureService;
        LeagueTableService leagueTableService;
        TransferService transferService;
        LeagueService leagueService;
        INavigation Navigation;

        #endregion

        #region Constructor

        public CompetitionsPageViewModel()
        {
            leagueCollection = new ObservableCollection<League>();
            fixtureService = new FixtureService();
            leagueTableService = new LeagueTableService();
            leagueService = new LeagueService();

            GenerateSource();
        }

        public CompetitionsPageViewModel(INavigation navigation, string LeagueID)
        {
            Navigation = navigation;
            leagueCollection = new ObservableCollection<League>();
            fixtureCollection = new ObservableCollection<Fixture>();
            tableCollection = new ObservableCollection<LeagueTable>();
            fixtureService = new FixtureService();
            leagueTableService = new LeagueTableService();
            transferService = new TransferService();

            GenerateSource(LeagueID);

            LoadMoreItemsCommand = new Command<object>(LoadMoreItems, CanLoadMoreItems);
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
        public Command<object> LoadMoreItemsCommand { get; set; }
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

        public ObservableCollection<League> LeagueCollection
        {
            get { return leagueCollection; }
            set { Set(ref leagueCollection, value); }
        }

        public ObservableCollection<Fixture> FixtureCollection
        {
            get { return fixtureCollection; }
            set { Set(ref fixtureCollection, value); }
        }

        public ObservableCollection<Fixture> FixtureFloatCollection
        {
            get { return fixtureFloatCollection; }
            set { Set(ref fixtureFloatCollection, value); }
        }

        public ObservableCollection<LeagueTable> TableCollection
        {
            get { return tableCollection; }
            set { Set(ref tableCollection, value); }
        }

        public bool IsTableExist
        {
            get { return isTableExist; }
            set { Set(ref isTableExist, value); }
        }

        public int TabCount
        {
            get { return tabCount; }
            set { Set(ref tabCount, value); }
        }

        public int TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }

        public bool IsLoadMoreVisible
        {
            get { return isLoadMoreVisible; }
            set { Set(ref isLoadMoreVisible, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        public bool IsActivityVisible
        {
            get { return _isActivityVisible; }
            set { Set(ref _isActivityVisible, value); }
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

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                NoConnectivity = false;

                //var leagues = await App.Database.GetLeagues();
                var leagues = await leagueService.GetLeagues();
            leagues = leagues.OrderBy(e => e.Name).ToList();
            LeagueCollection = new ObservableCollection<League>(leagues.OrderBy(e => e.Name).ToList());
            }
            else
                NoConnectivity = true;

            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        internal async void GenerateSource(string LeagueID)
        {
            IsActivityIndicatorVisible = true;
            IsLoadMoreVisible = false;

            var table = await leagueTableService.GetLeagueTableByLeague(LeagueID);
            var transfers = await transferService.GetLeagueTransfers(LeagueID);
            
            //var table = await GetLeagueTable(LeagueID);
            if (table != null && table.Count > 1)
            {
                table = table.Where(e => e.Season.IsCurrent).ToList();
                TableCollection = new ObservableCollection<LeagueTable>(table);
                IsTableExist = true;

            }
            else
            {
                IsTableExist = false;
                TabCount = 1;
            }

            var fixtures = await fixtureService.GetLeagueFixtures(LeagueID);
            //var fixtures = await GetLeagueFixtures(LeagueID);

            if (fixtures != null)
            {
                FixtureFloatCollection = new ObservableCollection<Fixture>(fixtures.Where(e => e.Season.IsCurrent).OrderBy(e => e.Date).ThenBy(e => TimeSpan.Parse(e.Time)));
                TotalCount = FixtureFloatCollection.Count;

                if(TotalCount > 0)
                {
                    var startIndex = FixtureFloatCollection.OrderBy(e => e.Date).IndexOf(e => e.Date > DateTime.Now);

                    if (startIndex >= 0)
                    {
                        if (startIndex != 0 && TotalCount > 50)
                        {
                            IsLoadMoreVisible = true;

                            AddFixtures(startIndex, 50);
                        }
                        else
                            AddFixtures(0, TotalCount);

                    }
                    else
                        AddFixtures(0, TotalCount);
                }
            }

            //MessagingCenter.Send<string>("FixturesLoaded", "Refresh");

            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        private bool CanLoadMoreItems(object obj)
        {
            if (FixtureCollection.Count >= TotalCount)
                return false;
            return true;
        }

        private async void LoadMoreItems(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.SfListView;
            listview.IsBusy = true;

            if (FixtureFloatCollection.Count > 0)
            {
                var index = FixtureCollection.Count;
                var count = index + 50 >= TotalCount ? TotalCount - index : 50;
                //count = count >= TotalCount ? TotalCount - index : 50;
                AddFixtures(index, count);
            }

            listview.IsBusy = false;
        }

        private void AddFixtures(int index, int count)
        {
            count = count >= TotalCount ? TotalCount - index : 50;
            for (int i = index; i < index + count; i++)
            {
                if (i < TotalCount)
                {
                    var fixture = FixtureFloatCollection[i];

                    FixtureCollection.Add(fixture);
                }
                else
                    break;
            }

            //IsUpNextIndicatorVisible = false;
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

        #endregion

        public async Task<List<Fixture>> GetLeagueFixtures(string LeagueID)
        {

            List<Fixture> fixtures = new List<Fixture>();

            fixtures = await fixtureService.GetLeagueFixtures(LeagueID);
            //CultureInfo MyCultureInfo = CultureInfo.InvariantCulture;
            //List<Fixture> fixtureListView = new List<Fixture>();

            //try
            //{
            //    var matches = await App.Database.GetMatches();
            //    var fixtures = await App.Database.GetFixtures();
            //    var teams = await App.Database.GetTeams();
            //    var leagues = await App.Database.GetLeagues();
            //    var fields = await App.Database.GetFields();
            //    var matchTypes = await App.Database.GetMatchTypes();

            //    var thisFixtures = fixtures.Where(e => e.LeagueID == LeagueID);

            //    foreach (var fixture in thisFixtures)
            //    {
            //        var match = matches.FirstOrDefault(e => e.FixtureID == fixture.ID);
            //        var homeTeam = teams.FirstOrDefault(e => e.ID == fixture.HomeTeamID);
            //        var awayTeam = teams.FirstOrDefault(e => e.ID == fixture.AwayTeamID);
            //        var thisField = fields.FirstOrDefault(e => e.ID == fixture.FieldID);
            //        var league = leagues.FirstOrDefault(e => e.ID == fixture.LeagueID);
            //        var matchType = matchTypes.FirstOrDefault(e => e.ID == fixture.MatchTypeID);

            //        string homeTeamName = "TBD";
            //        string awayTeamName = "TBD";

            //        if (homeTeam != null)
            //        {
            //            homeTeamName = homeTeam.Name;

            //            if (homeTeam.Alias != null && homeTeam.Alias != "")
            //                homeTeamName = homeTeam.Alias;
            //        }

            //        if (awayTeam != null)
            //        {
            //            awayTeamName = awayTeam.Name;

            //            if (awayTeam.Alias != null && awayTeam.Alias != "")
            //                awayTeamName = awayTeam.Alias;
            //        }

            //        DateTime matchTime = new DateTime();

            //        DateTime fixtureDate = new DateTime();
            //        fixtureDate = fixture.Date.ToUniversalTime();
            //        //DateTime.TryParse(fixture.Date, out fixtureDate);
            //        //DateTime.TryParse(fixture.Date, MyCultureInfo, DateTimeStyles.AssumeLocal, out fixtureDate);

            //        DateTime fixtureTime = new DateTime();
            //        //DateTime.TryParse(fixture.Time, out fixtureTime);
            //        fixtureTime = fixture.Time.ToUniversalTime();
            //        //var thisDateTime = TimeZoneInfo.ConvertTime(fixtureTime, TimeZoneInfo.Local);

            //        //fixtureTime = fixtureTime.ToUniversalTime();
            //        matchTime = fixtureDate.Add(fixtureTime.TimeOfDay);

            //        if (matchTime.AddMinutes(110) < DateTime.Now)
            //        {
            //            fixtureListView.Add(new Fixture
            //            {
            //                ID = fixture.ID,
            //                Date = fixtureDate,
            //                Time = fixtureTime,
            //                HomeTeamID = fixture.HomeTeamID,
            //                AwayTeamID = fixture.AwayTeamID,
            //                LeagueID = fixture.LeagueID,
            //                FieldID = fixture.FieldID,
            //                MatchTypeID = fixture.MatchTypeID,
            //                HomeTeamName = homeTeamName,
            //                AwayTeamName = awayTeamName,
            //                FieldName = thisField != null ? thisField.Name : "TBD",
            //                LeagueName = league != null ? league.Name : "",
            //                MatchTypeName = matchType != null ? matchType.Name : "",
            //                AwayTeamScore = match.AwayTeamScore,
            //                HomeTeamScore = match.HomeTeamScore,
            //                HomeTeamLogo = homeTeam.TeamLogo,
            //                AwayTeamLogo = awayTeam.TeamLogo
            //            });
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{ }

            return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
        }

        public async Task<List<LeagueTableListView>> GetLeagueTable(string LeagueID)
        {
            //List<LeagueTableListView> tableListView = new List<LeagueTableListView>();
            //LeagueTableListView thisTableListView = new LeagueTableListView();
            //try
            //{
            //    CultureInfo MyCultureInfo = CultureInfo.InvariantCulture;

            //    var league = await App.Database.GetLeagueByID(LeagueID);
            //    var teams = await App.Database.GetTeams();
            //    var matches = await App.Database.GetMatches();
            //    var fixtures = await App.Database.GetFixtures();

            //    var allMatchTypes = await App.Database.GetMatchTypes();

            //    var matchTypes = allMatchTypes.Where(e => e.IsTable);
            //    List<LeagueTableListView> initialTableListView = new List<LeagueTableListView>();

            //    //var matchType = matchTypes.FirstOrDefault();

            //    var thisTeams = teams.Where(e => e.LeagueID == LeagueID);
            //    //var awayTeamsList1 = fixtures.Where(e => e.LeagueID == league.ID && e.MatchTypeID == matc);
            //    //var homeTeamsList1 = fixtures.Where(e => e.LeagueID == league.ID);
            //    var awayTeamsList = fixtures.Where(e => e.LeagueID == league.ID).Select(e => e.AwayTeamID).Distinct();
            //    var homeTeamsList = fixtures.Where(e => e.LeagueID == league.ID).Select(e => e.HomeTeamID).Distinct();

            //    List<String> teamsList = new List<string>(homeTeamsList);
            //    foreach(var team in awayTeamsList)
            //    {
            //        var teamID = teamsList.Where(e => e == team);
            //        if(teamID == null)
            //            teamsList.Add(team);
            //    }



            //    foreach (var teamID in teamsList)
            //    {

            //        int totalWins = 0;
            //        int totalLoss = 0;
            //        int totalDraws = 0;
            //        int totalPoints = 0;
            //        int totalGoalsFor = 0;
            //        int totalGoalsAgainst = 0;
            //        int totalGamesPlayed = 0;
            //        MatchType matchType = new MatchType();
            //        //var league = leagues.FirstOrDefault(e => e.ID == team.LeagueID);

            //        //var thisFixtures = fixtures.Where(e => (e.HomeTeamID == team.ID || e.AwayTeamID == team.ID) && e.MatchTypeID == matchType.ID && e.LeagueID == league.ID);
            //        var fixturesList = fixtures.Where(e => (e.HomeTeamID == teamID || e.AwayTeamID == teamID) && e.LeagueID == league.ID);

            //        var thisFixtures = (from thisFixture in fixturesList
            //                            from thisMatchType in matchTypes
            //                            where thisMatchType.ID == thisFixture.MatchTypeID && (thisFixture.AwayTeamID == teamID || thisFixture.HomeTeamID == teamID)
            //                            select thisFixture).ToList();

            //        var team = teams.Where(e => e.ID == teamID).FirstOrDefault();

            //        foreach (var fixture in thisFixtures)
            //        {
            //            var match = matches.Where(e => e.FixtureID == fixture.ID).FirstOrDefault();
            //            matchType = matchTypes.Where(e => e.ID == fixture.MatchTypeID).FirstOrDefault();

            //            DateTime matchTime = new DateTime();

            //            DateTime fixtureDate = new DateTime();
            //            fixtureDate = fixture.Date.ToUniversalTime();
            //            //DateTime.TryParse(fixture.Date, out fixtureDate);
            //            //DateTime.TryParse(fixture.Date, MyCultureInfo, DateTimeStyles.AssumeLocal, out fixtureDate);

            //            DateTime fixtureTime = new DateTime();
            //            //DateTime.TryParse(fixture.Time, out fixtureTime);
            //            fixtureTime = fixture.Time.ToUniversalTime();
            //            //var thisDateTime = TimeZoneInfo.ConvertTime(fixtureTime, TimeZoneInfo.Local);

            //            matchTime = fixtureDate.Add(fixtureTime.TimeOfDay);

            //            if (matchTime < DateTime.Now)
            //            {
            //                if (fixture.HomeTeamID == team.ID)
            //                {
            //                    if (match.HomeTeamScore > match.AwayTeamScore)
            //                    {
            //                        totalWins = totalWins + 1;
            //                        totalPoints = totalPoints + 3;
            //                        totalGamesPlayed = totalGamesPlayed + 1;
            //                    }
            //                    else if (match.HomeTeamScore == match.AwayTeamScore)
            //                    {
            //                        totalDraws = totalDraws + 1;
            //                        totalPoints = totalPoints + 1;
            //                        totalGamesPlayed = totalGamesPlayed + 1;
            //                    }
            //                    else if (match.HomeTeamScore < match.AwayTeamScore)
            //                    {
            //                        totalLoss = totalLoss + 1;
            //                        totalGamesPlayed = totalGamesPlayed + 1;

            //                    }
            //                    totalGoalsFor = totalGoalsFor + match.HomeTeamScore;
            //                    totalGoalsAgainst = totalGoalsAgainst + match.AwayTeamScore;
            //                }
            //                else if (fixture.AwayTeamID == team.ID)
            //                {
            //                    if (match.AwayTeamScore > match.HomeTeamScore)
            //                    {
            //                        totalWins = totalWins + 1;
            //                        totalPoints = totalPoints + 3;
            //                        totalGamesPlayed = totalGamesPlayed + 1;
            //                    }
            //                    else if (match.AwayTeamScore == match.HomeTeamScore)
            //                    {
            //                        totalDraws = totalDraws + 1;
            //                        totalPoints = totalPoints + 1;
            //                        totalGamesPlayed = totalGamesPlayed + 1;
            //                    }
            //                    else if (match.AwayTeamScore < match.HomeTeamScore)
            //                    {
            //                        totalLoss = totalLoss + 1;
            //                        totalGamesPlayed = totalGamesPlayed + 1;
            //                    }
            //                    totalGoalsFor = totalGoalsFor + match.AwayTeamScore;
            //                    totalGoalsAgainst = totalGoalsAgainst + match.HomeTeamScore;
            //                }
            //            }

            //        }

            //        initialTableListView.Add(new LeagueTableListView
            //        {
            //            ID = null,
            //            TeamID = team.ID,
            //            TeamName = team.Name,
            //            Position = 0,
            //            LeagueID = team.LeagueID,
            //            LeagueName = league.Name,
            //            Win = totalWins,
            //            Loss = totalLoss,
            //            Draw = totalDraws,
            //            Points = totalPoints,
            //            GoalsFor = totalGoalsFor,
            //            GoalsAgainst = totalGoalsAgainst,
            //            GoalDifference = totalGoalsFor - totalGoalsAgainst,
            //            GamesPlayed = totalGamesPlayed,
            //            MatchTypeName = matchType.Name
            //        });


            //    }

            //    var thisInitialTableListView = initialTableListView.OrderByDescending(e => e.MatchTypeName).ThenByDescending(e => e.Points).ToList();

            //    int position = 1;
            //    int tableID = 1;
            //    string matchTypeSort = "";
            //    for(var i = 0; i < thisInitialTableListView.Count; i++)
            //    {
            //        matchTypeSort = thisInitialTableListView[i].MatchTypeName;

            //        tableListView.Add(new LeagueTableListView
            //        {
            //            ID = thisInitialTableListView[i].ID,
            //            TeamID = thisInitialTableListView[i].TeamID,
            //            TeamName = thisInitialTableListView[i].TeamName,
            //            Position = position,
            //            LeagueID = thisInitialTableListView[i].LeagueID,
            //            LeagueName = thisInitialTableListView[i].LeagueName,
            //            Win = thisInitialTableListView[i].Win,
            //            Loss = thisInitialTableListView[i].Loss,
            //            Draw = thisInitialTableListView[i].Draw,
            //            Points = thisInitialTableListView[i].Points,
            //            GoalsFor = thisInitialTableListView[i].GoalsFor,
            //            GoalsAgainst = thisInitialTableListView[i].GoalsAgainst,
            //            GoalDifference = thisInitialTableListView[i].GoalDifference,
            //            GamesPlayed = thisInitialTableListView[i].GamesPlayed == 0 ? (thisInitialTableListView[i].Win + thisInitialTableListView[i].Loss + thisInitialTableListView[i].Draw) : thisInitialTableListView[i].GamesPlayed,
            //            MatchTypeName = thisInitialTableListView[i].MatchTypeName
            //        });

            //        if (i < thisInitialTableListView.Count - 1)
            //        {

            //            if (matchTypeSort == thisInitialTableListView[i + 1].MatchTypeName)
            //            {
            //                position++;
            //            }
            //            else
            //            {
            //                position = 1;
            //            }
            //        }
            //        //tableID++;
            //    }
            //    return tableListView.OrderBy(e => e.Position).ToList();
            //    //return tableListView.Where(e => e.TeamID == TeamID).ToList();
            //    //}
            //}
            //catch (Exception ex)
            //{ }

            return null;

        }

        #region Player Info

        Fixture[] Player = new Fixture[]
         {
            
         };

        #endregion
    }
}
