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
using TruSport.Views.Admin;
using System.Globalization;
using System.Collections;

namespace TruSport.ViewModels
{
    public class FixtureAdminPageViewModel : BaseViewModel
    {
        #region Fields
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedFieldChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedLeagueChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedMatchTypeChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedHomeTeamChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedAwayTeamChangedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onFixtureSelectedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedMatchDateChanged;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedMatchTimeChanged;
        private FixtureListView tappedInfo;
        private ObservableCollection<string> syncTitleCollection;
        private Fixture fixtureItem; 
        private DateTime matchDate;
        private DateTime matchTime;
        private int? homeTeamScore;
        private int? homeTeamPenalty;
        private int? awayTeamScore;
        private int? awayTeamPenalty;
        private bool isPenalties;
        private bool isPostponed;
        private string setTitle;
        private string fieldName;
        private Field field;
        private League league;
        private string leagueName;
        private string matchTypeName;
        private MatchType matchType;
        private string homeTeamName;
        private Team homeTeam;
        private string awayTeamName;
        private Team awayTeam;
        private ObservableCollection<object> matchDateCollection;
        private ObservableCollection<object> matchTimeCollection;
        private ObservableCollection<League> leagueCollection;
        private ObservableCollection<MatchType> matchTypeCollection;
        private ObservableCollection<Field> fieldCollection;
        private ObservableCollection<Team> teamCollection;
        private ObservableCollection<Fixture> fixturesCollection;
        private ObservableCollection<FixtureListView> pastCollection;
        private ObservableCollection<FixtureListView> upcomingCollection;
        private ObservableCollection<LiveFixturesList> liveCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;

        FixtureService fixtureService;
        LeagueService leagueService;
        FieldService fieldService;
        MatchTypeService matchTypeService;
        TeamService teamService;

        //SeasonService seasonService;
        
        INavigation Navigation;

        #endregion

        #region Constructor

        //public FixtureAdminPageViewModel(INavigation navigation)
        //{
        //    FixturesCollection = new ObservableCollection<Fixture>();

        //    fixtureService = new FixtureService();

        //    SetTitle = "Add";

        //    GenerateSource();

        //    AddClickedCommand = new Command(async () => await AddClicked());
        //    OnFixtureSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(FixtureSelected);
        //}

        public FixtureAdminPageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            FixturesCollection = new ObservableCollection<Fixture>();

            fixtureService = new FixtureService();
            teamService = new TeamService();
            matchTypeService = new MatchTypeService();
            fieldService = new FieldService();
            leagueService = new LeagueService();

            SetTitle = "Add";

            GenerateSource();

            //SelectedFieldChangedCommand = new Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs>(FieldSelectionChanged);
            //SelectedLeagueChangedCommand = new Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs>(LeagueSelectionChanged);
            //SelectedMatchTypeChangedCommand = new Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs>(MatchTypeSelectionChanged);
            //SelectedAwayTeamChangedCommand = new Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs>(AwayTeamSelectionChanged);
            //SelectedHomeTeamChangedCommand = new Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs>(HomeTeamSelectionChanged);

            //SaveCommand = new Command(async () => await Save());
            AddClickedCommand = new Command(async () => await AddClicked());
            OnFixtureSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(FixtureSelected);
        }

        public FixtureAdminPageViewModel(INavigation navigation, Fixture fixture = null)
        {
            Navigation = navigation;
            FixturesCollection = new ObservableCollection<Fixture>();
            teamService = new TeamService();
            matchTypeService = new MatchTypeService();
            fieldService = new FieldService();
            leagueService = new LeagueService();
            fixtureService = new FixtureService();


            FixtureItem = fixture;

            SetTitle = "Edit";

            SyncTitleCollection = new ObservableCollection<string>();

            GenerateSource(fixture);

            SelectedFieldChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(FieldSelectionChanged);
            SelectedLeagueChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(LeagueSelectionChanged);
            SelectedMatchTypeChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(MatchTypeSelectionChanged);
            SelectedAwayTeamChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(AwayTeamSelectionChanged);
            SelectedHomeTeamChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(HomeTeamSelectionChanged);
            SelectedMatchDateChangedCommand = new Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs>(MatchDateChanged);
            SelectedMatchTimeChangedCommand = new Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs>(MatchTimeChanged);

            SaveCommand = new Command(async () => await Save());
            
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

        public Command AddClickedCommand { get; }
        public Command SaveCommand { get; }

        public Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> SelectedMatchDateChangedCommand
        {
            get { return selectedMatchDateChanged; }
            set { selectedMatchDateChanged = value; }
        }

        public Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> SelectedMatchTimeChangedCommand
        {
            get { return selectedMatchTimeChanged; }
            set { selectedMatchTimeChanged = value; }
        }

        public Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> SelectedFieldChangedCommand
        {
            get { return selectedFieldChangedCommand; }
            set { selectedFieldChangedCommand = value; }
        }
        public Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> SelectedLeagueChangedCommand
        {
            get { return selectedLeagueChangedCommand; }
            set { selectedLeagueChangedCommand = value; }
        }
        public Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> SelectedMatchTypeChangedCommand
        {
            get { return selectedMatchTypeChangedCommand; }
            set { selectedMatchTypeChangedCommand = value; }
        }
        public Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> SelectedHomeTeamChangedCommand
        {
            get { return selectedHomeTeamChangedCommand; }
            set { selectedHomeTeamChangedCommand = value; }
        }
        public Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> SelectedAwayTeamChangedCommand
        {
            get { return selectedAwayTeamChangedCommand; }
            set { selectedAwayTeamChangedCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> ItemTapCommand
        {
            get { return itemtapCommand; }
            set { itemtapCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnFixtureSelectedCommand
        {
            get { return onFixtureSelectedCommand; }
            set { onFixtureSelectedCommand = value; }
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

        public ObservableCollection<Fixture> FixturesCollection
        {
            get { return fixturesCollection; }
            set { Set(ref this.fixturesCollection, value); }
        }

        public ObservableCollection<FixtureListView> PastCollection
        {
            get { return pastCollection; }
            set { this.pastCollection = value; }
        }

        public ObservableCollection<FixtureListView> UpcomingCollection
        {
            get { return upcomingCollection; }
            set { this.upcomingCollection = value; }
        }

        public ObservableCollection<LiveFixturesList> LiveCollection
        {
            get { return liveCollection; }
            set { this.liveCollection = value; }
        }

        public ObservableCollection<string> SyncTitleCollection
        {
            get { return syncTitleCollection; }
            set { this.syncTitleCollection = value; }
        }

        public ObservableCollection<object> MatchDateCollection
        {
            get { return matchDateCollection; }
            set { Set(ref matchDateCollection, value); }
        }

        public ObservableCollection<object> MatchTimeCollection
        {
            get { return matchTimeCollection; }
            set { Set(ref matchTimeCollection, value); }
        }

        public ObservableCollection<League> LeagueCollection
        {
            get { return leagueCollection; }
            set { Set(ref leagueCollection, value); }
        }

        public ObservableCollection<Field> FieldCollection
        {
            get { return fieldCollection; }
            set { Set(ref fieldCollection, value); }
        }

        public ObservableCollection<MatchType> MatchTypeCollection
        {
            get { return matchTypeCollection; }
            set { Set(ref matchTypeCollection, value); }
        }

        public ObservableCollection<Team> TeamCollection
        {
            get { return teamCollection; }
            set { Set(ref teamCollection, value); }
        }

        public string SetTitle
        {
            get { return setTitle; }
            set { this.setTitle = value; }
        }

        public DateTime MatchDate
        {
            get { return matchDate; }
            set { Set(ref this.matchDate, value); }
        }

        public DateTime MatchTime
        {
            get { return matchTime; }
            set { Set(ref this.matchTime, value); }
        }

        public League League
        {
            get { return league; }
            set { Set(ref this.league, value); }
        }

        public string LeagueName
        {
            get { return leagueName; }
            set { Set(ref this.leagueName, value); }
        }

        public string FieldName
        {
            get { return fieldName; }
            set { Set(ref this.fieldName, value); }
        }

        public Field Field
        {
            get { return field; }
            set { Set(ref this.field, value); }
        }

        public string MatchTypeName
        {
            get { return matchTypeName; }
            set { Set(ref this.matchTypeName, value); }
        }

        public MatchType MatchType
        {
            get { return matchType; }
            set { Set(ref this.matchType, value); }
        }

        public string HomeTeamName
        {
            get { return homeTeamName; }
            set { Set(ref this.homeTeamName, value); }
        }

        public Team HomeTeam
        {
            get { return homeTeam; }
            set { Set(ref this.homeTeam, value); }
        }

        public string AwayTeamName
        {
            get { return awayTeamName; }
            set { Set(ref this.awayTeamName, value); }
        }

        public Team AwayTeam
        {
            get { return awayTeam; }
            set { Set(ref this.awayTeam, value); }
        }

        public int? HomeTeamScore
        {
            get { return homeTeamScore; }
            set { Set(ref this.homeTeamScore, value); }
        }

        public int? HomeTeamPenalty
        {
            get { return homeTeamPenalty; }
            set { Set(ref this.homeTeamPenalty, value); }
        }

        public int? AwayTeamScore
        {
            get { return awayTeamScore; }
            set { Set(ref this.awayTeamScore, value); }
        }

        public int? AwayTeamPenalty
        {
            get { return awayTeamPenalty; }
            set { Set(ref this.awayTeamPenalty, value); }
        }

        public bool IsPostponed
        {
            get { return isPostponed; }
            set { Set(ref isPostponed, value); }
        }

        public bool IsPenalties
        {
            get { return isPenalties; }
            set { Set(ref isPenalties, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource(string CategoryID)
        {
            IsActivityIndicatorVisible = true;

            //var fixtures = await databaseManager.GetFixtures();
            //var oFixturesList = new ObservableCollection<FixtureListView>(fixtures);
            //var fixturesList = oFixturesList.OrderByDescending(e => e.Date);

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

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                var fixtures = await fixtureService.GetFixtures();
                FixturesCollection = new ObservableCollection<Fixture>(fixtures);
                ////var fixtures = await App.Database.GetFixtures();
                //var fixtures = await databaseManager.GetFixtures();

                //var oFixturesList = new ObservableCollection<FixtureListView>(fixtures);
                //var fixturesList = oFixturesList.OrderByDescending(e => e.Date);

                //foreach (var fixture in fixturesList)
                //{
                //    FixturesCollection.Add(fixture);
                //} 

            }
            catch(Exception ex)
            { }
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
            //for(var i = 0; i < SyncTitles.Length; i++)
            //{
            //    SyncTitleCollection.Add(SyncTitles[i]);
            //}
            IsActivityIndicatorVisible = true;

            var leagues = await leagueService.GetLeagues();
            var teamsList = await teamService.GetTeams();

            teamsList.ForEach(e => e.Team.League = e.League);
            teamsList.ForEach(e => e.Team.LeagueID = e.LeagueID);

            var teams = teamsList.Where(e => e.Season.IsCurrent).Select(e => e.Team).ToList();
            var matchTypes = await matchTypeService.GetMatchTypes();
            var fields = await fieldService.GetFields();

            if (fixture != null)
            {
                var awayTeams = teams.Where(e => e.Alias == fixture.AwayTeam.Name);
                var homeTeams = teams.Where(e => e.Alias == fixture.HomeTeam.Name);
                FixtureItem = fixture;

                HomeTeamName = teams.Where(e => e.Alias == fixture.HomeTeam.Name || e.Name == fixture.HomeTeam.Name).FirstOrDefault().Name;
                AwayTeamName = teams.Where(e => e.Alias == fixture.AwayTeam.Name || e.Name == fixture.AwayTeam.Name).FirstOrDefault().Name;

                League = fixture.League;
                LeagueName = fixture.League.Name;
                FieldName = fixture.Field.Name;
                Field = fixture.Field;
                MatchTypeName = fixture.MatchType.Name;
                MatchType = fixture.MatchType;
                HomeTeam = fixture.HomeTeam;
                HomeTeamName = fixture.HomeTeam.Name;
                AwayTeam = fixture.AwayTeam;
                AwayTeamName = fixture.AwayTeam.Name;
                HomeTeamScore = fixture.Match.HomeTeamScore;
                AwayTeamScore = fixture.Match.AwayTeamScore;
                IsPostponed = fixture.IsPostponed;
                IsPenalties = fixture.Match.IsPenalties ?? false;
                HomeTeamPenalty = fixture.Match.HomeTeamPenalty;
                AwayTeamPenalty = fixture.Match.AwayTeamPenalty;

                ObservableCollection<object> todayDatecollection = new ObservableCollection<object>();
                ObservableCollection<object> todayTimecollection = new ObservableCollection<object>();

                //Select today dates]
                todayDatecollection.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fixture.Date.Date.Month).Substring(0, 3));

                if (fixture.Date.Date.Day < 10)
                    todayDatecollection.Add("0" + fixture.Date.Date.Day);
                else
                    todayDatecollection.Add(fixture.Date.Date.Day.ToString());

                todayDatecollection.Add(fixture.Date.Year.ToString());

                var _matchTime = new DateTime().AddDays(5).Add(TimeSpan.Parse(fixture.Time)).ToLocalTime().TimeOfDay;

                todayTimecollection.Add(_matchTime.Hours.ToString());

                if (_matchTime.Minutes < 10)
                    todayTimecollection.Add("0" + _matchTime.Minutes.ToString());
                else
                    todayTimecollection.Add(_matchTime.Minutes.ToString());


                MatchTimeCollection = todayTimecollection;
                MatchDateCollection = todayDatecollection;

                MatchDate = fixture.Date;
                MatchTime = DateTime.Parse(fixture.Time).ToLocalTime();

                //var TeamList = teams.Where(e => e.Season.IsCurrent).Select(e => e.Team).ToList();
                //LeagueCollection = new ObservableCollection<string>(leagueList.Select(e => e.Name));
                LeagueCollection = new ObservableCollection<League>(leagues);
                MatchTypeCollection = new ObservableCollection<MatchType>(matchTypes);
                FieldCollection = new ObservableCollection<Field>(fields);

                TeamCollection = new ObservableCollection<Team>(teams);

            }

            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        //private void FieldSelectionChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    FieldName = e.NewValue as string;

        //    Task.Run(async () =>
        //    {
        //        var fields = await App.Database.GetFields();

        //        var field = fields.Where(x => x.Name == FieldName).FirstOrDefault();
        //        FixtureItem.FieldID = field.ID;
        //        FixtureItem.Field.Name = field.Name;
        //    });
        //}

        //private void LeagueSelectionChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    LeagueName = e.NewValue as string;

        //    Task.Run(async () =>
        //    {
        //        var league = await App.Database.GetLeagueByName(LeagueName);

        //        FixtureItem.LeagueID = league.ID;
        //        FixtureItem.League.Name = league.Name;
        //    });
        //}

        //private void MatchTypeSelectionChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    MatchTypeName = e.NewValue as string;

        //    Task.Run(async () =>
        //    {
        //        var matchTypes = await App.Database.GetMatchTypes();

        //        var matchType = matchTypes.Where(x => x.Name == MatchTypeName).FirstOrDefault();
        //        FixtureItem.MatchTypeID = matchType.ID;
        //        FixtureItem.MatchType.Name = matchType.Name;
        //    });
        //}

        //private void HomeTeamSelectionChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    HomeTeamName = e.NewValue as string;

        //    Task.Run(async () =>
        //    {
        //        var teams = await App.Database.GetTeams();

        //        var team = teams.Where(x => x.Name == HomeTeamName || x.Alias == HomeTeamName).FirstOrDefault();
        //        FixtureItem.HomeTeamID = team.ID;
        //        FixtureItem.HomeTeam.Name = team.Alias ?? team.Name;
        //    });
        //}

        //private void AwayTeamSelectionChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    AwayTeamName = e.NewValue as string;

        //    Task.Run(async () =>
        //    {
        //        var teams = await App.Database.GetTeams();

        //        var team = teams.Where(x => x.Name == AwayTeamName || x.Alias == AwayTeamName).FirstOrDefault();
        //        FixtureItem.AwayTeamID = team.ID;
        //        FixtureItem.AwayTeam.Name = team.Alias ?? team.Name;
        //    });
        //}

        private void FieldSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _field = e.Value as Field;

            FieldName = _field.Name;
            Field = _field;
            //var field = FieldList.Where(x => x.Name == FieldName).FirstOrDefault();
            FixtureItem.Field = _field;
            FixtureItem.FieldID = _field.ID;
        }

        //private void LeagueSelectionChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    LeagueName = e.NewValue as string;

        //    var league = LeagueList.Where(x => x.Name == LeagueName).FirstOrDefault();
        //    FixtureItem.LeagueID = league.ID;
        //}

        private void LeagueSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _league = e.Value as League;

            LeagueName = _league.Name;
            League = _league;
            //var league = LeagueList.Where(x => x.Name == LeagueName).FirstOrDefault();
            FixtureItem.League = _league;
            FixtureItem.LeagueID = _league.ID;
        }

        private void MatchTypeSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _matchType = e.Value as MatchType;

            MatchTypeName = _matchType.Name;
            MatchType = _matchType;
            //var matchType = MatchTypeList.Where(x => x.Name == MatchTypeName).FirstOrDefault();
            FixtureItem.MatchType = _matchType;
            FixtureItem.MatchTypeID = _matchType.ID;
        }

        private void HomeTeamSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _team = e.Value as Team;

            HomeTeamName = _team.Name;
            HomeTeam = _team;
            //var team = TeamList.Where(x => x.Name == HomeTeamName || x.Alias == HomeTeamName).FirstOrDefault();
            FixtureItem.HomeTeam = _team;
            FixtureItem.HomeTeamID = _team.ID;
        }

        private void AwayTeamSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _team = e.Value as Team;

            AwayTeamName = _team.Name;
            AwayTeam = _team;
            //var team = TeamList.Where(x => x.Name == AwayTeamName || x.Alias == AwayTeamName).FirstOrDefault();
            FixtureItem.AwayTeam = _team;
            FixtureItem.AwayTeamID = _team.ID;
        }

        async Task AddClicked()
        {
            try
            {
                await Navigation.PushAsync(new EditFixturesPage(null));
            }
            catch (Exception ex)
            {
                //await Application.Current.MainPage.DisplayAlert("Error", "Issue saving fixture", "Okay");
            }
        }

        //async Task Save()
        //{
        //    try
        //    {
        //        //FixtureItem.Date = MatchDate;
        //        //FixtureItem.Time = new DateTime().Add(MatchTime);

        //        await Application.Current.MainPage.DisplayAlert("Success", "The fixture was saved successfully!", "Okay");
        //    }
        //    catch(Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", "Issue saving fixture", "Okay");
        //    }
        //}

        async Task Save()
        {

            try
            {
                if (LeagueName != null && MatchTypeName != null && FieldName != null && HomeTeamName != null && AwayTeamName != null)
                {
                    var save = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to save this fixture? Please make sure everything is correct.", "Yes", "Cancel");

                    if (save)
                    {
                        IsActivityIndicatorVisible = true;

                        if (FixtureItem != null)
                        {

                            FixtureItem.Date = MatchDate;
                            FixtureItem.Time = MatchTime.ToUniversalTime().TimeOfDay.ToString();
                            FixtureItem.LeagueID = League.ID;
                            FixtureItem.MatchTypeID = MatchType.ID;
                            FixtureItem.FieldID = Field.ID;
                            FixtureItem.HomeTeamID = HomeTeam.ID;
                            FixtureItem.AwayTeamID = AwayTeam.ID;
                            FixtureItem.Match.HomeTeamScore = HomeTeamScore;
                            FixtureItem.Match.AwayTeamScore = AwayTeamScore;
                            FixtureItem.IsPostponed = IsPostponed;
                            FixtureItem.Match.IsPenalties = IsPenalties;
                            FixtureItem.Match.HomeTeamPenalty = HomeTeamPenalty;
                            FixtureItem.Match.AwayTeamPenalty = AwayTeamPenalty;

                            if (await fixtureService.Update(FixtureItem))
                            {
                                //await Application.Current.MainPage.DisplayAlert("Success", "The fixture was saved successfully!", "Okay");
                                MessagingCenter.Send<Fixture>(FixtureItem, "Update");
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", "Issue saving fixture, if the problem persists please contact us at ontrackbda@gmail.com", "Okay");
                            }
                        }
                        else
                        {
                            FixtureItem.Date = MatchDate;
                            FixtureItem.Time = MatchTime.ToUniversalTime().TimeOfDay.ToString();
                            FixtureItem.LeagueID = League.ID;
                            FixtureItem.MatchTypeID = MatchType.ID;
                            FixtureItem.FieldID = Field.ID;
                            FixtureItem.HomeTeamID = HomeTeam.ID;
                            FixtureItem.AwayTeamID = AwayTeam.ID;
                            FixtureItem.Match.HomeTeamScore = HomeTeamScore;
                            FixtureItem.Match.AwayTeamScore = AwayTeamScore;
                            FixtureItem.IsPostponed = IsPostponed;
                            FixtureItem.Match.IsPenalties = IsPenalties;
                            FixtureItem.Match.HomeTeamPenalty = HomeTeamPenalty;
                            FixtureItem.Match.AwayTeamPenalty = AwayTeamPenalty;

                            if (await fixtureService.Insert(FixtureItem))
                            {
                                //await Application.Current.MainPage.DisplayAlert("Success", "The fixture was saved successfully!", "Okay");
                                MessagingCenter.Send<Fixture>(FixtureItem, "Update");
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", "Issue saving fixture, if the problem persists please contact us at ontrackbda@gmail.com", "Okay");
                            }
                        }

                        //if (await fixtureService.Update(FixtureItem))
                        //{
                        //    await Application.Current.MainPage.DisplayAlert("Success", "The fixture was saved successfully!", "Okay");
                        //    MessagingCenter.Send<Fixture>(FixtureItem, "Update");
                        //    await Navigation.PopAsync();
                        //}
                        //else
                        //{
                        //    await Application.Current.MainPage.DisplayAlert("Error", "Issue saving fixture, if the problem persists please contact us at ontrackbda@gmail.com", "Okay");
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Issue saving fixture", "Okay");
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        private async void FixtureSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as Fixture;

            if (item != null)
            {
                //CancelFixtureRefresh = false;

                await Navigation.PushAsync(new EditFixturesPage(item));
            }
        }

        private void MatchDateChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            try
            {
                var selectedDate = e.NewValue as IList;
                DateTime thisMatchDate = new DateTime();

                if (selectedDate.Count == 3)
                {
                    var month = selectedDate[0];
                    var day = Convert.ToInt32(selectedDate[1]);
                    var year = Convert.ToInt32(selectedDate[2]);

                    int monthIndex;
                    string[] MonthNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
                    monthIndex = Array.IndexOf(MonthNames, month) + 1;

                    thisMatchDate = new DateTime(year, monthIndex, day, 0, 0, 0);
                }
                else
                {
                    var month = selectedDate[0];
                    var year = Convert.ToInt32(selectedDate[1]);

                    int monthIndex;
                    string[] MonthNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
                    monthIndex = Array.IndexOf(MonthNames, month) + 1;

                    thisMatchDate = new DateTime(year, monthIndex, MatchDate.Day, 0, 0, 0);
                }

                MatchDate = thisMatchDate;
            }
            catch (Exception ex)
            {

            }
        }

        private void MatchTimeChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            var selectedTime = e.NewValue as IList;


            var hour = Convert.ToInt32(selectedTime[0]);
            var min = Convert.ToInt32(selectedTime[1]);
            //var timeOfDay = selectedTime[2].ToString();

            //if (timeOfDay == "PM")
            //    hour = hour + 12;

            //int monthIndex;
            //string[] MonthNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
            //monthIndex = Array.IndexOf(MonthNames, month) + 1;

            //var thisMatchTime = new DateTime(hour, min, 0);
            //var thisPreBookDateTime = new DateTime(year, monthIndex, day, hour, min, 0);
            var thisMatchTime = new DateTime(1990, 01, 05, hour, min, 0);

            MatchTime = thisMatchTime;
        }

        #endregion

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            tappedInfo = e.ItemData as FixtureListView;
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

        FixtureListView[] Player = new FixtureListView[]
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
