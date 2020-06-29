using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using System.Threading.Tasks;
using TruSport.Services;
using System.Diagnostics;
using Microsoft.AppCenter.Crashes;

namespace TruSport.ViewModels
{
    public class FavouritePageViewModel : BaseViewModel
    {
        #region Fields
        private Favourite tappedInfo;
        private ObservableCollection<Team> favouriteTeamCollection;
        private ObservableCollection<Fixture> favouriteFixturesCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool _isTeamActivityIndicatorVisible;
        private bool _isFixtureActivityIndicatorVisible;
        private bool noTeamFavourites;
        private bool noFixtureFavourites;
        private bool noConnectivity;
        TeamService teamService;
        
        #endregion

        #region Constructor

        public FavouritePageViewModel()
        {
            teamService = new TeamService();
            FavouriteTeamCollection = new ObservableCollection<Team>();
            FavouriteFixturesCollection = new ObservableCollection<Fixture>();
            GenerateSource();

            DeleteTeamFavouriteCommand = new Command<object>(DeleteTeamFavourite);
            DeleteFixtureFavouriteCommand = new Command<object>(DeleteFixtureFavourite);
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
        public Command<object> DeleteTeamFavouriteCommand { get; }
        public Command<object> DeleteFixtureFavouriteCommand { get; }

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

        public ObservableCollection<Team> FavouriteTeamCollection
        {
            get { return favouriteTeamCollection; }
            set { Set(ref favouriteTeamCollection, value); }
        }

        public ObservableCollection<Fixture> FavouriteFixturesCollection
        {
            get { return favouriteFixturesCollection; }
            set { Set(ref favouriteFixturesCollection, value); }
        }

        public bool NoTeamFavourites
        {
            get { return noTeamFavourites; }
            set { Set(ref noTeamFavourites, value); }
        }

        public bool NoFixtureFavourites
        {
            get { return noFixtureFavourites; }
            set { Set(ref noFixtureFavourites, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool IsTeamActivityIndicatorVisible
        {
            get { return _isTeamActivityIndicatorVisible; }
            set { Set(ref _isTeamActivityIndicatorVisible, value); }
        }

        public bool IsFixtureActivityIndicatorVisible
        {
            get { return _isFixtureActivityIndicatorVisible; }
            set { Set(ref _isFixtureActivityIndicatorVisible, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;
                    IsTeamActivityIndicatorVisible = true;
                    IsFixtureActivityIndicatorVisible = true;

                    var favouriteTeams = await App.Database.GetFootballTeamFavourites();
                    if (favouriteTeams != null && favouriteTeams.Count > 0)
                    {
                        favouriteTeams.ForEach(e => e.Team.League = e.League);
                        NoTeamFavourites = false;
                        FavouriteTeamCollection = new ObservableCollection<Team>(favouriteTeams.Select(e => e.Team));
                    }
                    else
                        NoTeamFavourites = true;

                    IsTeamActivityIndicatorVisible = false;
                    IsFixtureActivityIndicatorVisible = true;

                    var favouriteFixtures = await App.Database.GetFootballFixtureFavourites();

                    if (favouriteFixtures != null && favouriteFixtures.Count > 0)
                    {
                        NoFixtureFavourites = false;
                        FavouriteFixturesCollection = new ObservableCollection<Fixture>(favouriteFixtures.Select(e => e.Fixture));
                    }
                    else
                        NoFixtureFavourites = true;

                    IsFixtureActivityIndicatorVisible = false;

                }
                else
                    NoConnectivity = true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Favourite");
            }

            IsTeamActivityIndicatorVisible = false;
            IsFixtureActivityIndicatorVisible = false;
            IsActivityIndicatorVisible = false;
        }

        public async Task RefreshTeamFavourites()
        {
            try
            {
                var favouriteTeams = await App.Database.GetFootballTeamFavourites();
                if (favouriteTeams != null && favouriteTeams.Count > 0)
                {
                    var teams = await teamService.GetTeams();
                    teams.ForEach(e => e.Team.League = e.League);
                    teams.ForEach(e => e.Team.LeagueID = e.LeagueID);
                    NoTeamFavourites = false;
                    FavouriteTeamCollection = new ObservableCollection<Team>(teams.Where(e => favouriteTeams.Select(x => x.TeamID).Contains(e.TeamID) && e.Season.IsCurrent).Select(e => e.Team).ToList());
                }
                else
                    NoTeamFavourites = true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "RefreshTeamFavourites");
            }
        }

        public async Task RefreshFixtureFavourites()
        {
            try
            {
                var favouriteFixtures = await App.Database.GetFootballFixtureFavourites();

                if (favouriteFixtures != null && favouriteFixtures.Count > 0)
                {
                    NoFixtureFavourites = false;
                    FavouriteFixturesCollection = new ObservableCollection<Fixture>(favouriteFixtures.Select(e => e.Fixture));
                }
                else
                    NoFixtureFavourites = true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "RefreshFixtureFavourites");
            }
        }

        public async void DeleteTeamFavourite(object obj)
        {
            try
            {
                var team = obj as Team;

                await App.Database.DeleteTeamFavourite(team.ID);

                await RefreshTeamFavourites();

                //DisplayDataDeletedPromt();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "DeleteTeamFavourite");
            }
        }

        public async void DeleteFixtureFavourite(object obj)
        {
            try
            {
                var fixture = obj as Fixture;

                await App.Database.DeleteFootballFixtureFavourite(fixture.ID);

                await RefreshFixtureFavourites();

                //DisplayDataDeletedPromt();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "DeleteFixtureFavourite");
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
            tappedInfo = e.ItemData as Favourite;
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
