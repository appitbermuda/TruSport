using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using TruSport.Services;
using Xamarin.Essentials;
using System.Diagnostics;

namespace TruSport.ViewModels
{
    public class LeagueStatPageViewModel : BaseViewModel
    {
        #region Fields
        private LeagueStat tappedInfo; 
        private ObservableCollection<LeagueStat> premierPlayerGoalsCollection;
        private ObservableCollection<LeagueStat> firstPlayerGoalsCollection;
        private ObservableCollection<LeagueStat> coronaPlayerGoalsCollection;
        private ObservableCollection<LeagueStat> premierGoalsConcededCollection;
        private ObservableCollection<LeagueStat> firstGoalsConcededCollection;
        private ObservableCollection<LeagueStat> coronaGoalsConcededCollection;
        private ObservableCollection<LeagueStat> premierGoalsScoredCollection;
        private ObservableCollection<LeagueStat> firstGoalsScoredCollection;
        private ObservableCollection<LeagueStat> coronaGoalsScoredCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        
        LeagueStatService leagueStatsService;

        #endregion

        #region Constructor

        public LeagueStatPageViewModel()
        {
            PremierPlayerGoalsCollection = new ObservableCollection<LeagueStat>();
            FirstPlayerGoalsCollection = new ObservableCollection<LeagueStat>();
            CoronaPlayerGoalsCollection = new ObservableCollection<LeagueStat>();
            
            leagueStatsService = new LeagueStatService();
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

        public ObservableCollection<LeagueStat> PremierPlayerGoalsCollection
        {
            get { return premierPlayerGoalsCollection; }
            set { Set(ref this.premierPlayerGoalsCollection, value); }
        }

        public ObservableCollection<LeagueStat> FirstPlayerGoalsCollection
        {
            get { return firstPlayerGoalsCollection; }
            set { Set(ref this.firstPlayerGoalsCollection, value); }
        }

        public ObservableCollection<LeagueStat> CoronaPlayerGoalsCollection
        {
            get { return coronaPlayerGoalsCollection; }
            set { Set(ref this.coronaPlayerGoalsCollection, value); }
        }

        public ObservableCollection<LeagueStat> PremierGoalsScoredCollection
        {
            get { return premierGoalsScoredCollection; }
            set { Set(ref this.premierGoalsScoredCollection, value); }
        }

        public ObservableCollection<LeagueStat> FirstGoalsScoredCollection
        {
            get { return firstGoalsScoredCollection; }
            set { Set(ref this.firstGoalsScoredCollection, value); }
        }

        public ObservableCollection<LeagueStat> CoronaGoalsScoredCollection
        {
            get { return coronaGoalsScoredCollection; }
            set { Set(ref this.coronaGoalsScoredCollection, value); }
        }

        public ObservableCollection<LeagueStat> PremierGoalsConcededCollection
        {
            get { return premierGoalsConcededCollection; }
            set { Set(ref this.premierGoalsConcededCollection, value); }
        }

        public ObservableCollection<LeagueStat> FirstGoalsConcededCollection
        {
            get { return firstGoalsConcededCollection; }
            set { Set(ref this.firstGoalsConcededCollection, value); }
        }

        public ObservableCollection<LeagueStat> CoronaGoalsConcededCollection
        {
            get { return coronaGoalsConcededCollection; }
            set { Set(ref this.coronaGoalsConcededCollection, value); }
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
            IsActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                NoConnectivity = false;

                try
                {
                    var goalsScoredByPlayerList = await leagueStatsService.GetGoalsScoredByPlayer();
                    var goalsScoredByTeamList = await leagueStatsService.GetGoalsScoredByTeam();
                    var goalsConcededByTeamList = await leagueStatsService.GetGoalsConcededByTeam();

                    //Top Scorers
                    var firstDivPlayers = goalsScoredByPlayerList.Where(e => e.LeagueName.Contains("First Division"));
                    if (firstDivPlayers != null)
                        FirstPlayerGoalsCollection = new ObservableCollection<LeagueStat>(firstDivPlayers.OrderByDescending(e => e.Goals));


                    var premDivPlayers = goalsScoredByPlayerList.Where(e => e.LeagueName.Contains("Premier Division"));
                    if (premDivPlayers != null)
                        PremierPlayerGoalsCollection = new ObservableCollection<LeagueStat>(premDivPlayers.OrderByDescending(e => e.Goals));


                    var coronaDivPlayers = goalsScoredByPlayerList.Where(e => e.LeagueName.Contains("Corona League"));
                    if (coronaDivPlayers != null)
                        CoronaPlayerGoalsCollection = new ObservableCollection<LeagueStat>(coronaDivPlayers.OrderByDescending(e => e.Goals));

                    //Most Scored
                    var firstDivScored = goalsScoredByTeamList.Where(e => e.LeagueName.Contains("First Division"));
                    if (firstDivScored != null)
                        FirstGoalsScoredCollection = new ObservableCollection<LeagueStat>(firstDivScored.OrderByDescending(e => e.Goals));


                    var premDivScored = goalsScoredByTeamList.Where(e => e.LeagueName.Contains("Premier Division"));
                    if (premDivScored != null)
                        PremierGoalsScoredCollection = new ObservableCollection<LeagueStat>(premDivScored.OrderByDescending(e => e.Goals));


                    var coronaDivScored = goalsScoredByTeamList.Where(e => e.LeagueName.Contains("Corona League"));
                    if (coronaDivScored != null)
                        CoronaGoalsScoredCollection = new ObservableCollection<LeagueStat>(coronaDivScored.OrderByDescending(e => e.Goals));


                    //Most Conceded
                    var firstDivConceded = goalsConcededByTeamList.Where(e => e.LeagueName.Contains("First Division"));
                    if (firstDivConceded != null)
                        FirstGoalsConcededCollection = new ObservableCollection<LeagueStat>(firstDivConceded.OrderByDescending(e => e.Goals));


                    var premDivConceded = goalsConcededByTeamList.Where(e => e.LeagueName.Contains("Premier Division"));
                    if (premDivConceded != null)
                        PremierGoalsConcededCollection = new ObservableCollection<LeagueStat>(premDivConceded.OrderByDescending(e => e.Goals));


                    var coronaDivConceded = goalsConcededByTeamList.Where(e => e.LeagueName.Contains("Corona League"));
                    if (coronaDivConceded != null)
                        CoronaGoalsConcededCollection = new ObservableCollection<LeagueStat>(coronaDivConceded.OrderByDescending(e => e.Goals));
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message, "League Stats");
                }
            }
            else
                NoConnectivity = true;
            IsActivityIndicatorVisible = false;
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

        #endregion

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            tappedInfo = e.ItemData as LeagueStat;
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
