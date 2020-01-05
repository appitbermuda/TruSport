using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using System.Threading.Tasks;
using TruSport.Services;
using System.Diagnostics;
using TruSport.Views.Admin;

namespace TruSport.ViewModels
{
    public class TeamAdminPageViewModel : BaseViewModel
    {
        #region Fields
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedFieldChangedCommand;
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedLeagueChangedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onTeamSelectedCommand;
        private TeamListView tappedInfo;
        private Team teamItem;
        private string setTitle;
        private string leagueName;
        private string fieldName;
        private League league;
        private Field field;
        private ObservableCollection<League> leagueCollection;
        private ObservableCollection<Field> fieldCollection;
        private ObservableCollection<string> syncTitleCollection;
        private ObservableCollection<Team> teamCollection;
        private ObservableCollection<TeamListView> premierTeamCollection;
        private ObservableCollection<TeamListView> firstDivisionTeamCollection;
        private ObservableCollection<TeamListView> coronaTeamCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        TeamService teamService;
        LeagueService leagueService;
        FieldService fieldService;

        INavigation Navigation;


        #endregion

        #region Constructor

        public TeamAdminPageViewModel()
        {
            TeamCollection = new ObservableCollection<Team>();

            SetTitle = "Add";

            teamService = new TeamService();
            
            GenerateSource();

            TeamSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(TeamSelected);
        }

        public TeamAdminPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            TeamCollection = new ObservableCollection<Team>();

            SetTitle = "Add";

            teamService = new TeamService();

            GenerateSource();

            TeamSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(TeamSelected);
        }

        public TeamAdminPageViewModel(INavigation navigation,  Team team)
        {
            Navigation = navigation;
            TeamCollection = new ObservableCollection<Team>();
            SetTitle = "Edit";
            teamService = new TeamService();
            fieldService = new FieldService();
            leagueService = new LeagueService();
            
            GenerateSource(team);

            SelectedFieldChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(FieldSelectionChanged);
            SelectedLeagueChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(LeagueSelectionChanged);

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

        public Command SaveCommand { get; }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> TeamSelectedCommand
        {
            get { return onTeamSelectedCommand; }
            set { onTeamSelectedCommand = value; }
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

        public Team TeamItem
        {
            get { return teamItem; }
            set { Set(ref this.teamItem, value); }
        }

        public ObservableCollection<Team> TeamCollection
        {
            get { return teamCollection; }
            set { Set(ref this.teamCollection, value); }
        }

        public ObservableCollection<TeamListView> FirstDivisionTeamCollection
        {
            get { return firstDivisionTeamCollection; }
            set { this.firstDivisionTeamCollection = value; }
        }

        public ObservableCollection<TeamListView> CoronaTeamCollection
        {
            get { return coronaTeamCollection; }
            set { this.coronaTeamCollection = value; }
        }

        public ObservableCollection<string> SyncTitleCollection
        {
            get { return syncTitleCollection; }
            set { this.syncTitleCollection = value; }
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

        public string SetTitle
        {
            get { return setTitle; }
            set { this.setTitle = value; }
        }

        public string LeagueName
        {
            get { return leagueName; }
            set { Set(ref leagueName, value); }
        }


        public League League
        {
            get { return league; }
            set { Set(ref this.league, value); }
        }

        public Field Field
        {
            get { return field; }
            set { Set(ref this.field, value); }
        }

        public string FieldName
        {
            get { return fieldName; }
            set { Set(ref fieldName, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource(Team team)
        {
            IsActivityIndicatorVisible = true;

            try
            {

                LeagueName = team.League.Name;
                FieldName = team.Field.Name;

                TeamItem = team;

                var leagues = await leagueService.GetLeagues();
                var fields = await fieldService.GetFields();
                LeagueCollection = new ObservableCollection<League>(leagues);
                FieldCollection = new ObservableCollection<Field>(fields);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team Admin");
            }

            IsActivityIndicatorVisible = false;
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                var teams = await teamService.GetTeams();


                teams.ForEach(e => e.Team.League = e.League);
                teams.ForEach(e => e.Team.LeagueID = e.LeagueID);

                TeamCollection = new ObservableCollection<Team>(teams.Where(e => e.Season.IsCurrent).Select(e => e.Team).OrderBy(e => e.Name));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team Admin");
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
        //        TeamItem.HomeFieldID = field.ID;
        //        TeamItem.HomeFieldName = field.Name;
        //    });
        //}

        //private void LeagueSelectionChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    LeagueName = e.NewValue as string;

        //    Task.Run(async () =>
        //    {
        //        var league = await App.Database.GetLeagueByName(LeagueName);

        //        TeamItem.LeagueID = league.ID;
        //        TeamItem.LeagueName = league.Name;
        //    });
        //}

        private void FieldSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _field = e.Value as Field;

            FieldName = _field.Name;
            Field = _field;
            //var field = FieldList.Where(x => x.Name == FieldName).FirstOrDefault();
            TeamItem.Field = _field;
            TeamItem.HomeFieldID = _field.ID;
        }

        private void LeagueSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _league = e.Value as League;

            LeagueName = _league.Name;
            League = _league;
            //var league = LeagueList.Where(x => x.Name == LeagueName).FirstOrDefault();
            TeamItem.League = _league;
            TeamItem.LeagueID = _league.ID;
        }

        async Task Save()
        {
            try
            {
                IsActivityIndicatorVisible = true;

                await teamService.Update(TeamItem);

                await Navigation.PopAsync();

                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Issue saving team, please try again.", "Okay");
            }
        }

        private async void TeamSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as Team;

            if (item != null)
            {
                //CancelFixtureRefresh = false;

                await Navigation.PushAsync(new EditTeamPage(item));
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
