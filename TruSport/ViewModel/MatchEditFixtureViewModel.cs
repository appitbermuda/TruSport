using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Syncfusion.XForms.ComboBox;
using TruSport.Model;
using TruSport.Services;
using TruSport.ViewModels;
using Xamarin.Forms;

namespace TruSport.ViewModel
{
    public class MatchEditFixtureViewModel : BaseViewModel
    {
        #region Fields

        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedFieldChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedLeagueChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedMatchTypeChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedHomeTeamChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedAwayTeamChangedCommand;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedMatchDateChanged;
        private Command<Syncfusion.SfPicker.XForms.SelectionChangedEventArgs> selectedMatchTimeChanged;
        private Fixture fixtureItem;
        private DateTime matchDate;
        private DateTime matchTime;
        private string setTitle;
        private Field field;
        private string fieldName;
        private League league;
        private string leagueName;
        private MatchType matchType;
        private string matchTypeName;
        private Team homeTeam;
        private string homeTeamName;
        private string homeTeamID;
        private int homeTeamScore;
        private int homeTeamPenalty;
        private Team awayTeam;
        private string awayTeamName;
        private string awayTeamID;
        private int awayTeamScore;
        private int awayTeamPenalty;
        private bool isPenalties;
        private bool isPostponed;
        private List<League> leagueList;
        private List<MatchType> matchTypeList;
        private List<Field> fieldList;
        private List<Team> teamList;
        private ObservableCollection<object> matchDateCollection;
        private ObservableCollection<object> matchTimeCollection;
        private ObservableCollection<League> leagueCollection;
        private ObservableCollection<MatchType> matchTypeCollection;
        private ObservableCollection<Field> fieldCollection;
        private ObservableCollection<Team> teamCollection;
        private ObservableCollection<Fixture> fixturesCollection;
        private ObservableCollection<MatchRoster> homeRosterCollection;
        private ObservableCollection<MatchRoster> awayRosterCollection;
        private bool _homeTeamSelected;
        private bool _awayTeamSelected;
        private bool _isActivityIndicatorVisible;
        FixtureService fixtureService;
        LeagueService leagueService;
        MatchTypeService matchTypeService;
        FieldService fieldService;
        TeamService teamService;
        INavigation Navigation;

        #endregion

        public MatchEditFixtureViewModel(INavigation navigation)
        {
            Navigation = navigation;
            FixturesCollection = new ObservableCollection<Fixture>();
            fixtureService = new FixtureService();

            GenerateSource();
        }

        public MatchEditFixtureViewModel(INavigation navigation, Fixture fixture)
        {
            Navigation = navigation;
            fixtureService = new FixtureService();
            leagueService = new LeagueService();
            matchTypeService = new MatchTypeService();
            fieldService = new FieldService();
            teamService = new TeamService();
            MatchDateCollection = new ObservableCollection<object>();

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

        public Command SaveCommand { get; }

        public Fixture FixtureItem
        {
            get { return fixtureItem; }
            set { Set(ref fixtureItem, value); }
        }

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

        public List<League> LeagueList
        {
            get { return leagueList; }
            set { Set(ref leagueList, value); }
        }

        public List<Field> FieldList
        {
            get { return fieldList; }
            set { Set(ref fieldList, value); }
        }

        public List<MatchType> MatchTypeList
        {
            get { return matchTypeList; }
            set { Set(ref matchTypeList, value); }
        }

        public List<Team> TeamList
        {
            get { return teamList; }
            set { Set(ref teamList, value); }
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

        public ObservableCollection<Fixture> FixturesCollection
        {
            get { return fixturesCollection; }
            set { Set(ref fixturesCollection, value); }
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

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
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

        public string LeagueName
        {
            get { return leagueName; }
            set { Set(ref leagueName, value); }
        }

        public League League
        {
            get { return league; }
            set { Set(ref league, value); }
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

        public string HomeTeamID
        {
            get { return homeTeamID; }
            set { Set(ref this.homeTeamID, value); }
        }

        public int HomeTeamScore
        {
            get { return homeTeamScore; }
            set { Set(ref this.homeTeamScore, value); }
        }

        public int HomeTeamPenalty
        {
            get { return homeTeamPenalty; }
            set { Set(ref this.homeTeamPenalty, value); }
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

        public string AwayTeamID
        {
            get { return awayTeamID; }
            set { Set(ref this.awayTeamID, value); }
        }

        public int AwayTeamScore
        {
            get { return awayTeamScore; }
            set { Set(ref this.awayTeamScore, value); }
        }

        public int AwayTeamPenalty
        {
            get { return awayTeamPenalty; }
            set { Set(ref this.awayTeamPenalty, value); }
        }

        public bool AwayTeamSelected
        {
            get { return _awayTeamSelected; }
            set { Set(ref _awayTeamSelected, value); }
        }

        public bool HomeTeamSelected
        {
            get { return _homeTeamSelected; }
            set { Set(ref _homeTeamSelected, value); }
        }

        public bool IsPostponed
        {
            get { return isPostponed; }
            set { Set(ref isPostponed, value); }
        }

        public bool IsPenalties
        {
            get { return isPostponed; }
            set { Set(ref isPostponed, value); }
        }

        internal async void GenerateSource()
        {

            IsActivityIndicatorVisible = true;

            try
            {
                List<Fixture> fixtures = await fixtureService.GetFixtures();
                FixturesCollection = new ObservableCollection<Fixture>(fixtures.Where(e => e.FixtureTime <= DateTime.Now));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;

        }

        internal async void GenerateSource(Fixture fixture)
        {

            IsActivityIndicatorVisible = true;

            try
            { 
                fixture = await fixtureService.Get(fixture.ID);
                FixtureItem = fixture;

                League = fixture.League;
                LeagueName = fixture.League.Name;
                FieldName = fixture.Field.Name;
                MatchTypeName = fixture.MatchType.Name;
                HomeTeamName = fixture.HomeTeam.Name;
                AwayTeamName = fixture.AwayTeam.Name;
                HomeTeamScore = fixture.Match.HomeTeamScore ?? 0;
                AwayTeamScore = fixture.Match.AwayTeamScore ?? 0;
                IsPostponed = fixture.IsPostponed;
                IsPenalties = fixture.Match.IsPenalties ?? false;
                HomeTeamPenalty = fixture.Match.HomeTeamPenalty ?? 0;
                AwayTeamPenalty = fixture.Match.AwayTeamPenalty ?? 0;

                ObservableCollection<object> todayDatecollection = new ObservableCollection<object>();
                ObservableCollection<object> todayTimecollection = new ObservableCollection<object>();

                //Select today dates
                

                todayDatecollection.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(fixture.Date.Date.Month).Substring(0, 3));

                if (fixture.Date.Date.Day < 10)
                    todayDatecollection.Add("0" + fixture.Date.Date.Day);
                else
                    todayDatecollection.Add(fixture.Date.Date.Day.ToString());

                todayDatecollection.Add(fixture.Date.Year.ToString());

                var _matchTime = TimeSpan.Parse(fixture.Time);
                
                todayTimecollection.Add(_matchTime.Hours.ToString());

                if (_matchTime.Minutes < 10)
                    todayTimecollection.Add("0" + _matchTime.Minutes.ToString());
                else
                    todayTimecollection.Add(_matchTime.Minutes.ToString());


                MatchTimeCollection = todayTimecollection;
                MatchDateCollection = todayDatecollection;

                MatchDate = fixture.Date;
                MatchTime = DateTime.Parse(fixture.Time);

                LeagueList = await leagueService.GetLeagues();
                MatchTypeList = await matchTypeService.GetMatchTypes();
                FieldList = await fieldService.GetFields();
                var teams = await teamService.GetTeams();
                TeamList = teams.Where(e => e.Season.IsCurrent).Select(e => e.Team).ToList();
                //LeagueCollection = new ObservableCollection<string>(leagueList.Select(e => e.Name));
                LeagueCollection = new ObservableCollection<League>(LeagueList);
                MatchTypeCollection = new ObservableCollection<MatchType>(MatchTypeList);
                FieldCollection = new ObservableCollection<Field>(FieldList);
                TeamCollection = new ObservableCollection<Team>(TeamList);
                //MatchTypeCollection = new ObservableCollection<string>(matchTypeList.Select(e => e.Name));
                //FieldCollection = new ObservableCollection<string>(fieldList.Select(e => e.Name));
                //TeamCollection = new ObservableCollection<string>(teamList.Select(e => e.Name));
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
            }

            IsActivityIndicatorVisible = false;

        }

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

        async Task Save()
        {

            try
            {
                var save = await Application.Current.MainPage.DisplayAlert("Confirmation", "Are you sure you want to save this fixture? Please make sure everything is correct.", "Yes", "Cancel");

                if (save)
                {
                    IsActivityIndicatorVisible = true;

                    FixtureItem.Date = MatchDate;
                    FixtureItem.Time = MatchTime.TimeOfDay.ToString();
                    FixtureItem.Match.HomeTeamScore = HomeTeamScore;
                    FixtureItem.Match.AwayTeamScore = AwayTeamScore;
                    FixtureItem.IsPostponed = IsPostponed;
                    FixtureItem.Match.IsPenalties = IsPenalties;
                    FixtureItem.Match.HomeTeamPenalty = HomeTeamPenalty;
                    FixtureItem.Match.AwayTeamPenalty = AwayTeamPenalty;

                    if (await fixtureService.Update(FixtureItem))
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "The fixture was saved successfully!", "Okay");
                        MessagingCenter.Send<Fixture>(FixtureItem, "Update");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Issue saving fixture, if the problem persists please contact us at ontrackbda@gmail.com", "Okay");
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
            catch(Exception ex)
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
            var thisMatchTime = new DateTime(1990, 01, 01, hour, min, 0);

            MatchTime = thisMatchTime;
        }
    }

    public class SelectionChangedBehavior : Behavior<SfComboBox>
    {
        //public Command Display { get; private set; }
        //MatchEditFixtureViewModel matchEditFixtureViewModel;
        //protected override void OnAttachedTo(SfComboBox bindable)
        //{
        //    bindable.SelectionChanged += Bindable_SelectionChanged;
        //    base.OnAttachedTo(bindable);
        //}
        //protected override void OnDetachingFrom(SfComboBox bindable)
        //{
        //    bindable.SelectionChanged -= Bindable_SelectionChanged;
        //    base.OnDetachingFrom(bindable);
        //}

        //void Bindable_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        //{
        //    matchEditFixtureViewModel = new MatchEditFixtureViewModel();
        //    var selectedValue = e.Value as List<object>;
        //    var lastItem = selectedValue.Last();
        //    //_league.ID = new List<string>();
        //    foreach (var item in selectedValue)
        //    {
        //        if (item == lastItem)
        //        {
        //            var selId = item as League;
        //            matchEditFixtureViewModel.FixtureItem.LeagueID = selId.ID;

        //            Application.Current.MainPage.DisplayAlert("Selected Id", "The last selected ID is " + selId.ID, "Ok");
        //        }
        //        else
        //        {
        //            var selId = item as League;
        //            matchEditFixtureViewModel.FixtureItem.LeagueID = selId.ID;
        //        }

        //    }
        //}
    }
}
