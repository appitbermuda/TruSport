using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using TruSport.Services;
using TruSport.Views.Football;
using Syncfusion.DataSource.Extensions;

namespace TruSport.ViewModels
{
    public class TeamProfilePageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Transfers> transferCollection;
        private TeamListView tappedInfo;
        private ObservableCollection<string> syncTitleCollection;
        private ObservableCollection<Fixture> fixtureCollection;
        private ObservableCollection<Fixture> formCollection;
        private ObservableCollection<Coach> coachCollection;
        private ObservableCollection<PlayerSeason> playerCollection;
        private ObservableCollection<LeagueTable> tableCollection;
        private LeagueTable tableItem;
        private Team teamItem;
        private Field field;
        private Command<object> leagueSelectedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool isFavourite;
        private bool noTransfers;

        TeamService teamService;
        FixtureService fixtureService;
        LeagueTableService leagueTableService;
        PlayerService playerService;
        INavigation Navigation;
        TransferService transferService;

        #endregion

        #region Constructor

        public TeamProfilePageViewModel()
        {
            FixtureCollection = new ObservableCollection<Fixture>();
            FormCollection = new ObservableCollection<Fixture>();
            PlayerCollection = new ObservableCollection<PlayerSeason>();
            TableCollection = new ObservableCollection<LeagueTable>();
            CoachCollection = new ObservableCollection<Coach>();
            TransferCollection = new ObservableCollection<Transfers>();
            TableItem = new LeagueTable();
            TeamItem = new Team();

            SyncTitleCollection = new ObservableCollection<string>();

            transferService = new TransferService();
            teamService = new TeamService();
            leagueTableService = new LeagueTableService();
            fixtureService = new FixtureService();
            playerService = new PlayerService();
            GenerateSource();
        }

        public TeamProfilePageViewModel(INavigation navigation, Team Team)
        {
            Navigation = navigation;
            FixtureCollection = new ObservableCollection<Fixture>();
            FormCollection = new ObservableCollection<Fixture>();
            PlayerCollection = new ObservableCollection<PlayerSeason>();
            TableCollection = new ObservableCollection<LeagueTable>();
            CoachCollection = new ObservableCollection<Coach>();
            TransferCollection = new ObservableCollection<Transfers>();
            TableItem = new LeagueTable();
            TeamItem = new Team();

            SyncTitleCollection = new ObservableCollection<string>();

            transferService = new TransferService();
            teamService = new TeamService();
            leagueTableService = new LeagueTableService();
            fixtureService = new FixtureService();
            playerService = new PlayerService();
            GenerateSource(Team);

            FavouriteCommand = new Command(async () => await Favourite());
            LeagueSelectedCommand = new Command<object>(SelectedLeague);
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
        public Command<object> LeagueSelectedCommand
        {
            get { return leagueSelectedCommand; }
            set { leagueSelectedCommand = value; }
        }
        public Command FavouriteCommand { get; }
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

        public ObservableCollection<Transfers> TransferCollection
        {
            get { return transferCollection; }
            set { Set(ref transferCollection, value); }
        }

        public ObservableCollection<Fixture> FixtureCollection
        {
            get { return fixtureCollection; }
            set { Set(ref fixtureCollection, value); }
        }

        public ObservableCollection<Fixture> FormCollection
        {
            get { return formCollection; }
            set { Set(ref formCollection, value); }
        }

        public ObservableCollection<PlayerSeason> PlayerCollection
        {
            get { return playerCollection; }
            set { Set(ref playerCollection, value); }
        }

        public ObservableCollection<LeagueTable> TableCollection
        {
            get { return tableCollection; }
            set { Set(ref tableCollection, value); }
        }

        public ObservableCollection<Coach> CoachCollection
        {
            get { return coachCollection; }
            set { Set(ref coachCollection, value); }
        }

        public LeagueTable TableItem
        {
            get { return tableItem; }
            set { Set(ref tableItem, value); }
        }

        public Team TeamItem
        {
            get { return teamItem; }
            set { Set(ref teamItem, value); }
        }

        public Field Field
        {
            get { return field; }
            set { Set(ref field, value); }
        }

        public ObservableCollection<string> SyncTitleCollection
        {
            get { return syncTitleCollection; }
            set { Set(ref syncTitleCollection, value); }
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

        public bool NoTransfers
        {
            get { return noTransfers; }
            set { Set(ref noTransfers, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            //var journeys = await databaseManager.GetPlayersByCategoryID(CategoryID);
            //foreach(var player in players)
            //{
            //    JourneyCollection.Add(player);
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

        internal async void GenerateSource(Team Team)
        {
            IsActivityIndicatorVisible = true;

            //if (Team.Coaches == null || Team.Players == null)
            var _team = await teamService.Get(Team.ID);
            Team = _team.Team;

            //var image = "http://ontrackimagestore.blob.core.windows.net/images/bfa.png";
            //Team.TeamLogo = image;
            TeamItem = Team;
            //TeamItem.TeamLogo = image;
            
            //Field = await App.Database.GetFieldByID(Team.HomeFieldID);
            Field = Team.Field;

            IsFavourite = await App.Database.IsTeamFavourite(Team.ID);

            //var coaches = await App.Database.GetCoachesByTeam(Team.ID);
            CoachCollection = new ObservableCollection<Coach>(Team.Coaches);
            //foreach (var coach in coaches)
            //{
            //    CoachCollection.Add(coach);
            //}

            //var players = await App.Database.GetPlayersByTeam(Team.ID);
            var players = await playerService.GetTeamPlayers(Team.ID);
            PlayerCollection = new ObservableCollection<PlayerSeason>(players.OrderBy(e => e.Player.LastName));
            //foreach (var player in players)
            //{
            //    PlayerCollection.Add(player);
            //}

            //var fixtures = await App.Database.GetFixturesByTeam(Team.ID);
            //var fixtures = await GetFixturesByTeam(Team.ID);
            var fixtures = await fixtureService.GetTeamFixtures(Team.ID);
            FixtureCollection = new ObservableCollection<Fixture>(fixtures.OrderBy(e => e.Date));

            var form = fixtures.Where(e => e.FixtureTime <= DateTime.Now && e.Match.HomeTeamScore.HasValue && e.Match.AwayTeamScore.HasValue).OrderByDescending(e => e.Date).Take(6).ToList();
            form.ForEach(e => e.SelectedTeamID = Team.ID);
            form.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == Team.ID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) || (e.AwayTeamID == Team.ID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == Team.ID && e.Match.AwayTeamPenalty > e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == Team.ID && e.Match.HomeTeamPenalty > e.Match.AwayTeamPenalty) ? "W" : (e.AwayTeamID == Team.ID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) || (e.HomeTeamID == Team.ID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == Team.ID && e.Match.AwayTeamPenalty < e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == Team.ID && e.Match.HomeTeamPenalty < e.Match.AwayTeamPenalty) ? "L" : (!e.Match.HomeTeamScore.HasValue || !e.Match.AwayTeamScore.HasValue) ? "" : "D");
            FormCollection = new ObservableCollection<Fixture>(form);

            //var table = await databaseManager.GetLeagueTableByTeam(Team.ID);
            //var table = await GetLeagueTable(Team.LeagueID);
            List<LeagueTable> tables = new List<LeagueTable>();

            if(_team.League.Name == "First Division")
                tables = await leagueTableService.GetFirstDivisionTables();
            else if (_team.League.Name == "Premier Division")
                tables = await leagueTableService.GetPremierLeagueTables();
            else if (_team.League.Name == "Corona League")
                tables = await leagueTableService.GetCoronaLeagueTables();

            if (tables.Count > 0)
            {
                //foreach(var table in tables)
                //{
                //    if(table.TeamID == Team.ID)
                //    {
                //        table.IsSelected = true;
                //        break;
                //    }

                //    tables
                //}

                tables.ForEach(x => x.IsSelectedTeam = (x.TeamID == Team.ID));

                TableCollection = new ObservableCollection<LeagueTable>(tables);

                TableItem = tables.FirstOrDefault(e => e.TeamID == Team.ID);
            }
            //else
            //IsTableVisible = true;

            var transfers = await transferService.GetTransfers();
            transfers = transfers.Where(e => e.NewTeam == Team.Name).ToList();

            if (transfers.Count > 0)
            {
                NoTransfers = false;
                var transfer = transfers.FirstOrDefault();

                TransferCollection = new ObservableCollection<Transfers>(transfers);
            }
            else
                NoTransfers = true;

            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        //public async Task<List<Fixture>> GetFixturesByTeam(string TeamID)
        //{
        //    CultureInfo MyCultureInfo = CultureInfo.InvariantCulture;
        //    List<Fixture> fixtureListView = new List<Fixture>();

        //    try
        //    {
        //        var matches = await App.Database.GetMatches();
        //        var fixtures = await App.Database.GetFixturesByTeam(TeamID);
        //        var teams = await App.Database.GetTeams();
        //        var leagues = await App.Database.GetLeagues();
        //        var fields = await App.Database.GetFields();
        //        var matchTypes = await App.Database.GetMatchTypes();



        //        foreach (var fixture in fixtures)
        //        {
        //            var match = matches.FirstOrDefault(e => e.FixtureID == fixture.ID);
        //            var homeTeam = teams.FirstOrDefault(e => e.ID == fixture.HomeTeamID);
        //            var awayTeam = teams.FirstOrDefault(e => e.ID == fixture.AwayTeamID);
        //            var thisField = fields.FirstOrDefault(e => e.ID == fixture.FieldID);
        //            var league = leagues.FirstOrDefault(e => e.ID == fixture.LeagueID);
        //            var matchType = matchTypes.FirstOrDefault(e => e.ID == fixture.MatchTypeID);

        //            string homeTeamName = "TBD";
        //            string awayTeamName = "TBD";

        //            if (homeTeam != null)
        //            {
        //                homeTeamName = homeTeam.Name;

        //                if (homeTeam.Alias != null && homeTeam.Alias != "")
        //                    homeTeamName = homeTeam.Alias;
        //            }

        //            if (awayTeam != null)
        //            {
        //                awayTeamName = awayTeam.Name;

        //                if (awayTeam.Alias != null && awayTeam.Alias != "")
        //                    awayTeamName = awayTeam.Alias;
        //            }

        //            DateTime matchTime = new DateTime();

        //            DateTime fixtureDate = new DateTime();
        //            fixtureDate = fixture.Date.ToUniversalTime();
        //            //DateTime.TryParse(fixture.Date, out fixtureDate);
        //            //DateTime.TryParse(fixture.Date, MyCultureInfo, DateTimeStyles.AssumeLocal, out fixtureDate);

        //            DateTime fixtureTime = new DateTime();
        //            //DateTime.TryParse(fixture.Time, out fixtureTime);
        //            fixtureTime = fixture.Time.ToUniversalTime();
        //            //var thisDateTime = TimeZoneInfo.ConvertTime(fixtureTime, TimeZoneInfo.Local);

        //            //fixtureTime = fixtureTime.ToUniversalTime();
        //            matchTime = fixtureDate.Add(fixtureTime.TimeOfDay);

        //            if (matchTime.AddMinutes(110) < DateTime.Now)
        //            {
        //                fixtureListView.Add(new Fixture
        //                {
        //                    ID = fixture.ID,
        //                    Date = fixtureDate,
        //                    Time = fixtureTime,
        //                    HomeTeamID = fixture.HomeTeamID,
        //                    AwayTeamID = fixture.AwayTeamID,
        //                    LeagueID = fixture.LeagueID,
        //                    FieldID = fixture.FieldID,
        //                    MatchTypeID = fixture.MatchTypeID,
        //                    HomeTeamName = homeTeamName,
        //                    AwayTeamName = awayTeamName,
        //                    FieldName = thisField != null ? thisField.Name : "TBD",
        //                    LeagueName = league != null ? league.Name : "",
        //                    MatchTypeName = matchType != null ? matchType.Name : "",
        //                    AwayTeamScore = match.AwayTeamScore,
        //                    HomeTeamScore = match.HomeTeamScore,
        //                    HomeTeamLogo = homeTeam.TeamLogo,
        //                    AwayTeamLogo = awayTeam.TeamLogo
        //                });
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    { }

        //    return fixtureListView.Where(e => e.HomeTeamName != "TBD" && e.AwayTeamName != "TBD").OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
        //}

        //public async Task<List<LeagueTable>> GetLeagueTable(string LeagueID)
        //{
        //    List<LeagueTable> tableListView = new List<LeagueTable>();
        //    LeagueTable thisTableListView = new LeagueTable();
        //    try
        //    {
        //        CultureInfo MyCultureInfo = CultureInfo.InvariantCulture;

        //        var league = await App.Database.GetLeagueByID(LeagueID);
        //        var teams = await App.Database.GetTeams();
        //        var matches = await App.Database.GetMatches();
        //        var fixtures = await App.Database.GetFixtures();

        //        //var allMatchTypes = await App.Database.GetMatchTypes();

        //        //var matchTypes = allMatchTypes.Where(e => e.Name == "League");
        //        List<LeagueTable> initialTableListView = new List<LeagueTable>();

        //        //var matchType = matchTypes.FirstOrDefault();

        //        var thisTeams = teams.Where(e => e.LeagueID == LeagueID);

        //        foreach (var team in thisTeams)
        //        {

        //            int totalWins = 0;
        //            int totalLoss = 0;
        //            int totalDraws = 0;
        //            int totalPoints = 0;
        //            int totalGoalsFor = 0;
        //            int totalGoalsAgainst = 0;
        //            int totalGamesPlayed = 0;

        //            //var league = leagues.FirstOrDefault(e => e.ID == team.LeagueID);

        //            //var thisFixtures = fixtures.Where(e => (e.HomeTeamID == team.ID || e.AwayTeamID == team.ID) && e.MatchTypeID == matchType.ID && e.LeagueID == league.ID);
        //            var thisFixtures = fixtures.Where(e => (e.HomeTeamID == team.ID || e.AwayTeamID == team.ID) && e.LeagueID == league.ID);


        //            foreach (var fixture in thisFixtures)
        //            {
        //                var match = matches.Where(e => e.FixtureID == fixture.ID).FirstOrDefault();
        //                DateTime matchTime = new DateTime();

        //                DateTime fixtureDate = new DateTime();
        //                fixtureDate = fixture.Date.ToUniversalTime();
        //                //DateTime.TryParse(fixture.Date, out fixtureDate);
        //                //DateTime.TryParse(fixture.Date, MyCultureInfo, DateTimeStyles.AssumeLocal, out fixtureDate);

        //                DateTime fixtureTime = new DateTime();
        //                //DateTime.TryParse(fixture.Time, out fixtureTime);
        //                fixtureTime = fixture.Time.ToUniversalTime();
        //                //var thisDateTime = TimeZoneInfo.ConvertTime(fixtureTime, TimeZoneInfo.Local);

        //                matchTime = fixtureDate.Add(fixtureTime.TimeOfDay);

        //                if (matchTime < DateTime.Now)
        //                {
        //                    if (fixture.HomeTeamID == team.ID)
        //                    {
        //                        if (match.HomeTeamScore > match.AwayTeamScore)
        //                        {
        //                            totalWins = totalWins + 1;
        //                            totalPoints = totalPoints + 3;
        //                            totalGamesPlayed = totalGamesPlayed + 1;
        //                        }
        //                        else if (match.HomeTeamScore == match.AwayTeamScore)
        //                        {
        //                            totalDraws = totalDraws + 1;
        //                            totalPoints = totalPoints + 1;
        //                            totalGamesPlayed = totalGamesPlayed + 1;
        //                        }
        //                        else if (match.HomeTeamScore < match.AwayTeamScore)
        //                        {
        //                            totalLoss = totalLoss + 1;
        //                            totalGamesPlayed = totalGamesPlayed + 1;

        //                        }
        //                        totalGoalsFor = totalGoalsFor + match.HomeTeamScore;
        //                        totalGoalsAgainst = totalGoalsAgainst + match.AwayTeamScore;
        //                    }
        //                    else if (fixture.AwayTeamID == team.ID)
        //                    {
        //                        if (match.AwayTeamScore > match.HomeTeamScore)
        //                        {
        //                            totalWins = totalWins + 1;
        //                            totalPoints = totalPoints + 3;
        //                            totalGamesPlayed = totalGamesPlayed + 1;
        //                        }
        //                        else if (match.AwayTeamScore == match.HomeTeamScore)
        //                        {
        //                            totalDraws = totalDraws + 1;
        //                            totalPoints = totalPoints + 1;
        //                            totalGamesPlayed = totalGamesPlayed + 1;
        //                        }
        //                        else if (match.AwayTeamScore < match.HomeTeamScore)
        //                        {
        //                            totalLoss = totalLoss + 1;
        //                            totalGamesPlayed = totalGamesPlayed + 1;
        //                        }
        //                        totalGoalsFor = totalGoalsFor + match.AwayTeamScore;
        //                        totalGoalsAgainst = totalGoalsAgainst + match.HomeTeamScore;
        //                    }
        //                }

        //            }

        //            bool isTeam = false;

        //            if (team.ID == TeamItem.ID)
        //                isTeam = true;

        //            initialTableListView.Add(new LeagueTable
        //            {
        //                ID = null,
        //                TeamID = team.ID,
        //                TeamName = team.Name,
        //                Position = 0,
        //                LeagueID = team.LeagueID,
        //                LeagueName = league.Name,
        //                Win = totalWins,
        //                Loss = totalLoss,
        //                Draw = totalDraws,
        //                Points = totalPoints,
        //                GoalsFor = totalGoalsFor,
        //                GoalsAgainst = totalGoalsAgainst,
        //                GoalDifference = totalGoalsFor - totalGoalsAgainst,
        //                GamesPlayed = totalGamesPlayed,
        //                IsSelectedTeam = isTeam
        //            });


        //        }

        //            var thisInitialTableListView = initialTableListView.OrderByDescending(e => e.Points);

        //            int position = 1;
        //            int tableID = 1;
        //            foreach (var initialTable in thisInitialTableListView)
        //            {

        //                tableListView.Add(new LeagueTable
        //                {
        //                    ID = initialTable.ID,
        //                    TeamID = initialTable.TeamID,
        //                    TeamName = initialTable.TeamName,
        //                    Position = position,
        //                    LeagueID = initialTable.LeagueID,
        //                    LeagueName = initialTable.LeagueName,
        //                    Win = initialTable.Win,
        //                    Loss = initialTable.Loss,
        //                    Draw = initialTable.Draw,
        //                    Points = initialTable.Points,
        //                    GoalsFor = initialTable.GoalsFor,
        //                    GoalsAgainst = initialTable.GoalsAgainst,
        //                    GoalDifference = initialTable.GoalDifference,
        //                    GamesPlayed = initialTable.GamesPlayed == 0 ? (initialTable.Win + initialTable.Loss + initialTable.Draw) : initialTable.GamesPlayed,
        //                    IsSelectedTeam = initialTable.IsSelectedTeam
        //                });

        //                position++;
        //                tableID++;
        //            }


        //        //return tableListView.Where(e => e.TeamID == TeamID).ToList();
        //        //}
        //    }
        //    catch (Exception ex)
        //    { }

        //    return tableListView.OrderBy(e => e.Position).ToList();

        //}

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

        private async void SelectedLeague(object obj)
        {
            var groupResult = obj as Syncfusion.DataSource.Extensions.GroupResult;

            var items = new List<Fixture>(groupResult.Items.ToList<Fixture>());
            var data = items[0];

            //var fixture = fixtures.FirstOrDefault();

            var league = data.League;

            await Navigation.PushAsync(new CompetitionDetailsPage(league));

        }

        async Task Favourite()
        {
            try
            {
                if (IsFavourite)
                {
                    IsFavourite = false;

                    await App.Database.DeleteTeamFavourite(TeamItem.ID);
                    //await App.Current.MainPage.DisplayAlert("Success", "Favourite deleted successfully.", "OK");
                }
                else
                {
                    IsFavourite = true;

                    var favourite = new Favourite
                    {
                        TeamID = TeamItem.ID,
                        Type = "Team"
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


        #endregion

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            tappedInfo = e.ItemData as TeamListView;
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

        TeamListView[] Player = new TeamListView[]
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
