using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using System.Threading.Tasks;
using TruSport.Services;
using TruSport.Views.Admin;

namespace TruSport.ViewModels
{
    public class PlayerAdminPageViewModel : BaseViewModel
    {
        #region Fields
        private Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> selectedTeamChangedCommand;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onPlayerSelectedCommand;
        private Player tappedInfo;
        private PlayerSeason playerItem;
        private string setTitle;
        private string firstName;
        private string lastName;
        private string teamName;
        private Team team;
        private int? jerseyNumber;
        private int? goals;
        private int? yellowCards;
        private int? redCards;
        private int? gamesPlayed;
        private bool isActive;
        private ObservableCollection<Team> teamCollection;
        private ObservableCollection<string> fieldCollection;
        private ObservableCollection<string> syncTitleCollection;
        private ObservableCollection<PlayerSeason> playerCollection;
        private ObservableCollection<Player> premierPlayerCollection;
        private ObservableCollection<Player> firstDivisionPlayerCollection;
        private ObservableCollection<Player> coronaPlayerCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        PlayerService playerService;
        TeamService teamService;

        INavigation Navigation;


        #endregion

        #region Constructor

        public PlayerAdminPageViewModel()
        {
            PlayerCollection = new ObservableCollection<PlayerSeason>();

            SetTitle = "Add";

            playerService = new PlayerService();
            
            GenerateSource();

            PlayerSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(PlayerSelected);
        }

        public PlayerAdminPageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            PlayerCollection = new ObservableCollection<PlayerSeason>();

            SetTitle = "Add";

            playerService = new PlayerService();

            GenerateSource();

            PlayerSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(PlayerSelected);
        }

        public PlayerAdminPageViewModel(INavigation navigation, PlayerSeason player)
        {
            Navigation = navigation;

            TeamCollection = new ObservableCollection<Team>();
            teamService = new TeamService();
            playerService = new PlayerService();

            SetTitle = "Edit";

            
            GenerateSource(player);

            SelectedTeamChangedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(TeamSelectionChanged);
            SaveCommand = new Command(async () => await Save());
        }

        //public PlayerDirectoryPageViewModel()
        //{
        //    PlayerDirectoryCollection = new ObservableCollection<Player>();
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

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> PlayerSelectedCommand
        {
            get { return onPlayerSelectedCommand; }
            set { onPlayerSelectedCommand = value; }
        }

        public Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs> SelectedTeamChangedCommand
        {
            get { return selectedTeamChangedCommand; }
            set { selectedTeamChangedCommand = value; }
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

        public PlayerSeason PlayerItem
        {
            get { return playerItem; }
            set { Set(ref this.playerItem, value); }
        }

        public ObservableCollection<PlayerSeason> PlayerCollection
        {
            get { return playerCollection; }
            set { Set(ref this.playerCollection, value); }
        }

        public ObservableCollection<Player> FirstDivisionPlayerCollection
        {
            get { return firstDivisionPlayerCollection; }
            set { this.firstDivisionPlayerCollection = value; }
        }

        public ObservableCollection<Player> CoronaPlayerCollection
        {
            get { return coronaPlayerCollection; }
            set { this.coronaPlayerCollection = value; }
        }

        public ObservableCollection<string> SyncTitleCollection
        {
            get { return syncTitleCollection; }
            set { this.syncTitleCollection = value; }
        }

        public ObservableCollection<Team> TeamCollection
        {
            get { return teamCollection; }
            set { Set(ref teamCollection, value); }
        }

        public ObservableCollection<string> FieldCollection
        {
            get { return fieldCollection; }
            set { Set(ref fieldCollection, value); }
        }

        public string TeamName
        {
            get { return teamName; }
            set { Set(ref this.teamName, value); }
        }

        public Team Team
        {
            get { return team; }
            set { Set(ref this.team, value); }
        }

        public string SetTitle
        {
            get { return setTitle; }
            set { this.setTitle = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { Set(ref firstName, value); }
        }

        public string LastName
        {
            get { return lastName; }
            set { Set(ref lastName, value); }
        }

        public int? JerseyNumber
        {
            get { return jerseyNumber; }
            set { Set(ref jerseyNumber, value); }
        }

        public int? Goals
        {
            get { return goals; }
            set { Set(ref goals, value); }
        }

        public int? GamesPlayed
        {
            get { return gamesPlayed; }
            set { Set(ref gamesPlayed, value); }
        }

        public int? YellowCards
        {
            get { return yellowCards; }
            set { Set(ref yellowCards, value); }
        }

        public int? RedCards
        {
            get { return redCards; }
            set { Set(ref redCards, value); }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { Set(ref isActive, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource(PlayerSeason player)
        {
            IsActivityIndicatorVisible = true;

            var teamsList = await teamService.GetTeams();

            teamsList.ForEach(e => e.Team.League = e.League);
            teamsList.ForEach(e => e.Team.LeagueID = e.LeagueID);

            var teams = teamsList.Where(e => e.Season.IsCurrent).Select(e => e.Team).ToList();

            if (player != null)
            {
                PlayerItem = player;

                FirstName = player.Player.FirstName;
                LastName = player.Player.LastName;
                JerseyNumber = player.Player.JerseyNumber;
                Goals = player.Goals;
                GamesPlayed = player.GamesPlayed;
                YellowCards = player.YellowCards;
                RedCards = player.RedCards;
                Team = player.Team;
                TeamName = player.Team.Name;
                IsActive = player.IsActive;
            }

            TeamCollection = new ObservableCollection<Team>(teams.OrderBy(e => e.League.Name));

            IsActivityIndicatorVisible = false;
            //var player = new Player
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
            //for(var i = 0; i < SyncTitles.Length; i++)
            //{
            //    SyncTitleCollection.Add(SyncTitles[i]);
            //}
            IsActivityIndicatorVisible = true;

            var players = await playerService.GetPlayers();

            PlayerCollection = new ObservableCollection<PlayerSeason>(players.Where(e => e.Season.IsCurrent).OrderBy(e => e.Player.Name));

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
        //        PlayerItem.HomeFieldID = field.ID;
        //        PlayerItem.HomeFieldName = field.Name;
        //    });
        //}

        //private void LeagueSelectionChanged(Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        //{
        //    LeagueName = e.NewValue as string;

        //    Task.Run(async () =>
        //    {
        //        var league = await App.Database.GetLeagueByName(LeagueName);

        //        PlayerItem.LeagueID = league.ID;
        //        PlayerItem.LeagueName = league.Name;
        //    });
        //}

        async Task Save()
        {
            try
            {
                //await Application.Current.MainPage.DisplayAlert("Success", "The fixture was saved successfully!", "Okay");
                IsActivityIndicatorVisible = true;

                PlayerItem.Player.JerseyNumber = JerseyNumber;
                PlayerItem.Player.FirstName = FirstName;
                PlayerItem.Player.LastName = LastName;
                PlayerItem.GamesPlayed = GamesPlayed;
                PlayerItem.Goals = Goals;
                PlayerItem.YellowCards = YellowCards;
                PlayerItem.RedCards = RedCards;
                PlayerItem.IsActive = IsActive;

                await playerService.Update(PlayerItem);

                await Navigation.PopAsync();

                IsActivityIndicatorVisible = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Issue saving player, please try again.", "Okay");
                IsActivityIndicatorVisible = false;
            }
        }

        private void TeamSelectionChanged(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var _team = e.Value as Team;

            TeamName = _team.Name;
            Team = _team;
            //var team = TeamList.Where(x => x.Name == HomeTeamName || x.Alias == HomeTeamName).FirstOrDefault();
            PlayerItem.Team = _team;
            PlayerItem.TeamID = _team.ID;
        }

        private async void PlayerSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as PlayerSeason;

            if (item != null)
            {
                //CancelFixtureRefresh = false;

                await Navigation.PushAsync(new EditPlayerPage(item));
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
            tappedInfo = e.ItemData as Player;
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

        Player[] Player = new Player[]
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
