using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using TruSport.Services;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace TruSport.ViewModels
{
    public class TeamPageViewModel : BaseViewModel
    {
        #region Fields
        private TeamListView tappedInfo;
        private ObservableCollection<string> syncTitleCollection;
        private ObservableCollection<Team> teamCollection;
        private ObservableCollection<Team> premierTeamCollection;
        private ObservableCollection<Team> firstDivisionTeamCollection;
        private ObservableCollection<Team> coronaTeamCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private string imageTest;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        
        TeamService teamService;

        #endregion

        #region Constructor

        public TeamPageViewModel()
        {
            TeamCollection = new ObservableCollection<Team>();
            PremierTeamCollection = new ObservableCollection<Team>();
            FirstDivisionTeamCollection = new ObservableCollection<Team>();
            CoronaTeamCollection = new ObservableCollection<Team>();
            SyncTitleCollection = new ObservableCollection<string>();
            
            teamService = new TeamService();

            GenerateSource();
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

        public ObservableCollection<Team> TeamCollection
        {
            get { return teamCollection; }
            set { Set(ref teamCollection, value); }
        }

        public ObservableCollection<Team> PremierTeamCollection
        {
            get { return premierTeamCollection; }
            set { Set(ref premierTeamCollection, value); }
        }

        public ObservableCollection<Team> FirstDivisionTeamCollection
        {
            get { return firstDivisionTeamCollection; }
            set { Set(ref firstDivisionTeamCollection, value); }
        }

        public ObservableCollection<Team> CoronaTeamCollection
        {
            get { return coronaTeamCollection; }
            set { this.coronaTeamCollection = value; }
        }

        public ObservableCollection<string> SyncTitleCollection
        {
            get { return syncTitleCollection; }
            set { this.syncTitleCollection = value; }
        }

        public string ImageTest
        {
            get { return imageTest; }
            set { Set(ref imageTest, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource(string CategoryID)
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

        internal async void GenerateSource()
        {
            //for(var i = 0; i < SyncTitles.Length; i++)
            //{
            //    SyncTitleCollection.Add(SyncTitles[i]);
            //}

            try
            {


            IsActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                NoConnectivity = false;

                    var image = "http://ontrackimagestore.blob.core.windows.net/images/bfa.png";
                    ImageTest = image;
                    //var teams = await App.Database.GetTeams();
                    var teams = await teamService.GetTeams();


                teams.ForEach(e => e.Team.League = e.League);
                teams.ForEach(e => e.Team.LeagueID = e.LeagueID);

                TeamCollection = new ObservableCollection<Team>(teams.Where(e => e.Season.IsCurrent).Select(e => e.Team).OrderBy(e => e.Name));


                var firstDivisionTeams = teams.Select(e => e.Team).Where(e => e.League.Name == "First Division");
                FirstDivisionTeamCollection = new ObservableCollection<Team>(firstDivisionTeams);


                var premierDivisionTeams = teams.Select(e => e.Team).Where(e => e.League.Name == "Premier Division");
                PremierTeamCollection = new ObservableCollection<Team>(premierDivisionTeams);

                var coronaDivisionTeams = teams.Select(e => e.Team).Where(e => e.League.Name == "Corona League");
                CoronaTeamCollection = new ObservableCollection<Team>(coronaDivisionTeams);

                    //var coronaDivTeams = teams.Where(e => e.LeagueName == "Corona League");
                    //foreach (var coronaDivTeam in coronaDivTeams)
                    //{
                    //    CoronaTeamCollection.Add(coronaDivTeam);
                    //}
                }
                else
                    NoConnectivity = true;

            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Hmmmm","Looks like something went wrong, please check you are connected to a wifi or cellular connection.","Okay");
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
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

        public async Task ServerImage(string image)
        {


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
