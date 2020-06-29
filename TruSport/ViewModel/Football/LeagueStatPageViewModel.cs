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
        private ObservableCollection<LeagueStat> playerGoalsCollection;
        private ObservableCollection<LeagueStat> teamConcededCollection;
        private ObservableCollection<LeagueStat> teamScoredCollection;

        private ObservableCollection<LeagueStat> premierPlayerGoalsCollection;
        private ObservableCollection<LeagueStat> firstPlayerGoalsCollection;
        private ObservableCollection<LeagueStat> coronaPlayerGoalsCollection;
        private ObservableCollection<LeagueStat> premierGoalsConcededCollection;
        private ObservableCollection<LeagueStat> firstGoalsConcededCollection;
        private ObservableCollection<LeagueStat> coronaGoalsConcededCollection;
        private ObservableCollection<LeagueStat> premierGoalsScoredCollection;
        private ObservableCollection<LeagueStat> firstGoalsScoredCollection;
        private ObservableCollection<LeagueStat> coronaGoalsScoredCollection;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        
        LeagueStatService leagueStatsService;

        #endregion

        #region Constructor

        public LeagueStatPageViewModel()
        {
            PlayerGoalsCollection = new ObservableCollection<LeagueStat>();
            TeamScoredCollection = new ObservableCollection<LeagueStat>();
            TeamConcededCollection = new ObservableCollection<LeagueStat>();
            
            leagueStatsService = new LeagueStatService();

            GenerateSource();
        }

        #endregion

        #region Properties


        public ObservableCollection<LeagueStat> PlayerGoalsCollection
        {
            get { return playerGoalsCollection; }
            set { Set(ref this.playerGoalsCollection, value); }
        }

        public ObservableCollection<LeagueStat> TeamConcededCollection
        {
            get { return teamConcededCollection; }
            set { Set(ref this.teamConcededCollection, value); }
        }

        public ObservableCollection<LeagueStat> TeamScoredCollection
        {
            get { return teamScoredCollection; }
            set { Set(ref this.teamScoredCollection, value); }
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

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                NoConnectivity = false;

                try
                {
                    var goalsScoredByPlayerList = await leagueStatsService.GetGoalsScoredByPlayer();
                    PlayerGoalsCollection = new ObservableCollection<LeagueStat>(goalsScoredByPlayerList);

                    var goalsScoredByTeamList = await leagueStatsService.GetGoalsScoredByTeam();
                    TeamScoredCollection = new ObservableCollection<LeagueStat>(goalsScoredByTeamList);

                    var goalsConcededByTeamList = await leagueStatsService.GetGoalsConcededByTeam();
                    TeamConcededCollection = new ObservableCollection<LeagueStat>(goalsConcededByTeamList);


                    ////Top Scorers
                    //var firstDivPlayers = goalsScoredByPlayerList.Where(e => e.LeagueName.Contains("First Division"));
                    //if (firstDivPlayers != null)
                    //    FirstPlayerGoalsCollection = new ObservableCollection<LeagueStat>(firstDivPlayers.OrderByDescending(e => e.Goals));


                    //var premDivPlayers = goalsScoredByPlayerList.Where(e => e.LeagueName.Contains("Premier Division"));
                    //if (premDivPlayers != null)
                    //    PremierPlayerGoalsCollection = new ObservableCollection<LeagueStat>(premDivPlayers.OrderByDescending(e => e.Goals));


                    //var coronaDivPlayers = goalsScoredByPlayerList.Where(e => e.LeagueName.Contains("Corona League"));
                    //if (coronaDivPlayers != null)
                    //    CoronaPlayerGoalsCollection = new ObservableCollection<LeagueStat>(coronaDivPlayers.OrderByDescending(e => e.Goals));


                    

                    ////Most Scored
                    //var firstDivScored = goalsScoredByTeamList.Where(e => e.LeagueName.Contains("First Division"));
                    //if (firstDivScored != null)
                    //    FirstGoalsScoredCollection = new ObservableCollection<LeagueStat>(firstDivScored.OrderByDescending(e => e.Goals));


                    //var premDivScored = goalsScoredByTeamList.Where(e => e.LeagueName.Contains("Premier Division"));
                    //if (premDivScored != null)
                    //    PremierGoalsScoredCollection = new ObservableCollection<LeagueStat>(premDivScored.OrderByDescending(e => e.Goals));


                    //var coronaDivScored = goalsScoredByTeamList.Where(e => e.LeagueName.Contains("Corona League"));
                    //if (coronaDivScored != null)
                    //    CoronaGoalsScoredCollection = new ObservableCollection<LeagueStat>(coronaDivScored.OrderByDescending(e => e.Goals));


                    //var goalsConcededByTeamList = await leagueStatsService.GetGoalsConcededByTeam();

                    ////Most Conceded
                    //var firstDivConceded = goalsConcededByTeamList.Where(e => e.LeagueName.Contains("First Division"));
                    //if (firstDivConceded != null)
                    //    FirstGoalsConcededCollection = new ObservableCollection<LeagueStat>(firstDivConceded.OrderByDescending(e => e.Goals));


                    //var premDivConceded = goalsConcededByTeamList.Where(e => e.LeagueName.Contains("Premier Division"));
                    //if (premDivConceded != null)
                    //    PremierGoalsConcededCollection = new ObservableCollection<LeagueStat>(premDivConceded.OrderByDescending(e => e.Goals));


                    //var coronaDivConceded = goalsConcededByTeamList.Where(e => e.LeagueName.Contains("Corona League"));
                    //if (coronaDivConceded != null)
                    //    CoronaGoalsConcededCollection = new ObservableCollection<LeagueStat>(coronaDivConceded.OrderByDescending(e => e.Goals));
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message, "League Stats");
                }
            }
            else
                NoConnectivity = true;
            IsActivityIndicatorVisible = false;
        }

        #endregion
    }
}
