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
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;

namespace TruSport.ViewModels
{
    public class StatPageViewModel : BaseViewModel
    {
        #region Fields
        private LeagueTable tappedInfo; 
        private ObservableCollection<LeagueTable> premierTeamCollection;
        private ObservableCollection<LeagueTable> firstDivisionTeamCollection;
        private ObservableCollection<LeagueTable> coronaTeamCollection;
        private ObservableCollection<LeagueTable> womensTeamCollection;
        private ObservableCollection<LeagueStat> playerGoalsCollection;
        private ObservableCollection<LeagueStat> teamConcededCollection;
        private ObservableCollection<LeagueStat> teamScoredCollection;

        private ObservableCollection<LeagueStat> premierPlayerGoalsCollection;
        private ObservableCollection<LeagueStat> firstPlayerGoalsCollection;
        private ObservableCollection<LeagueStat> coronaPlayerGoalsCollection;
        private ObservableCollection<LeagueStat> womenPlayerGoalsCollection;
        private ObservableCollection<LeagueStat> premierGoalsConcededCollection;
        private ObservableCollection<LeagueStat> firstGoalsConcededCollection;
        private ObservableCollection<LeagueStat> coronaGoalsConcededCollection;
        private ObservableCollection<LeagueStat> womenGoalsConcededCollection;
        private ObservableCollection<LeagueStat> premierGoalsScoredCollection;
        private ObservableCollection<LeagueStat> firstGoalsScoredCollection;
        private ObservableCollection<LeagueStat> coronaGoalsScoredCollection;
        private ObservableCollection<LeagueStat> womenGoalsScoredCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;

        LeagueStatService leagueStatsService;
        LeagueTableService leagueTableService;
        AdService adService;

        #endregion

        #region Constructor

        public StatPageViewModel()
        {
            PremierDivisionCollection = new ObservableCollection<LeagueTable>();
            FirstDivisionCollection = new ObservableCollection<LeagueTable>();
            CoronaLeagueCollection = new ObservableCollection<LeagueTable>();
            WomensLeagueCollection = new ObservableCollection<LeagueTable>();

            PremierPlayerGoalsCollection = new ObservableCollection<LeagueStat>();
            FirstPlayerGoalsCollection = new ObservableCollection<LeagueStat>();
            CoronaPlayerGoalsCollection = new ObservableCollection<LeagueStat>();
            WomenPlayerGoalsCollection = new ObservableCollection<LeagueStat>();

            PremierTeamScoredCollection = new ObservableCollection<LeagueStat>();
            FirstTeamScoredCollection = new ObservableCollection<LeagueStat>();
            CoronaTeamScoredCollection = new ObservableCollection<LeagueStat>();
            WomenTeamScoredCollection = new ObservableCollection<LeagueStat>();

            PremierTeamConcededCollection = new ObservableCollection<LeagueStat>();
            FirstTeamConcededCollection = new ObservableCollection<LeagueStat>();
            CoronaTeamConcededCollection = new ObservableCollection<LeagueStat>();
            WomenTeamConcededCollection = new ObservableCollection<LeagueStat>();

            leagueTableService = new LeagueTableService();
            leagueStatsService = new LeagueStatService();
            adService = new AdService();

            GenerateSource();

            AdTappedCommand = new Command(AdTapped);
        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }
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

        public ObservableCollection<LeagueTable> PremierDivisionCollection
        {
            get { return premierTeamCollection; }
            set { Set(ref this.premierTeamCollection, value); }
        }

        public ObservableCollection<LeagueTable> FirstDivisionCollection
        {
            get { return firstDivisionTeamCollection; }
            set { Set(ref this.firstDivisionTeamCollection, value); }
        }

        public ObservableCollection<LeagueTable> CoronaLeagueCollection
        {
            get { return coronaTeamCollection; }
            set { Set(ref this.coronaTeamCollection, value); }
        }

        public ObservableCollection<LeagueTable> WomensLeagueCollection
        {
            get { return womensTeamCollection; }
            set { Set(ref this.womensTeamCollection, value); }
        }
        //public ObservableCollection<LeagueStat> PlayerGoalsCollection
        //{
        //    get { return playerGoalsCollection; }
        //    set { Set(ref this.playerGoalsCollection, value); }
        //}

        //public ObservableCollection<LeagueStat> TeamConcededCollection
        //{
        //    get { return teamConcededCollection; }
        //    set { Set(ref this.teamConcededCollection, value); }
        //}

        //public ObservableCollection<LeagueStat> TeamScoredCollection
        //{
        //    get { return teamScoredCollection; }
        //    set { Set(ref this.teamScoredCollection, value); }
        //}

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

        public ObservableCollection<LeagueStat> WomenPlayerGoalsCollection
        {
            get { return womenPlayerGoalsCollection; }
            set { Set(ref this.womenPlayerGoalsCollection, value); }
        }

        public ObservableCollection<LeagueStat> PremierTeamScoredCollection
        {
            get { return premierGoalsScoredCollection; }
            set { Set(ref this.premierGoalsScoredCollection, value); }
        }

        public ObservableCollection<LeagueStat> FirstTeamScoredCollection
        {
            get { return firstGoalsScoredCollection; }
            set { Set(ref this.firstGoalsScoredCollection, value); }
        }

        public ObservableCollection<LeagueStat> CoronaTeamScoredCollection
        {
            get { return coronaGoalsScoredCollection; }
            set { Set(ref this.coronaGoalsScoredCollection, value); }
        }

        public ObservableCollection<LeagueStat> WomenTeamScoredCollection
        {
            get { return womenGoalsScoredCollection; }
            set { Set(ref this.womenGoalsScoredCollection, value); }
        }

        public ObservableCollection<LeagueStat> PremierTeamConcededCollection
        {
            get { return premierGoalsConcededCollection; }
            set { Set(ref this.premierGoalsConcededCollection, value); }
        }

        public ObservableCollection<LeagueStat> FirstTeamConcededCollection
        {
            get { return firstGoalsConcededCollection; }
            set { Set(ref this.firstGoalsConcededCollection, value); }
        }

        public ObservableCollection<LeagueStat> CoronaTeamConcededCollection
        {
            get { return coronaGoalsConcededCollection; }
            set { Set(ref this.coronaGoalsConcededCollection, value); }
        }

        public ObservableCollection<LeagueStat> WomenTeamConcededCollection
        {
            get { return womenGoalsConcededCollection; }
            set { Set(ref this.womenGoalsConcededCollection, value); }
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

        private Ad _ad;
        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
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

                    await Task.Run(async () =>
                    {
                        var ads = await adService.GetAds();

                        if (ads != null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Ad = ads.Any(e => e.Sport == Constants.Football) ? ads.FirstOrDefault(e => e.Sport == Constants.Football) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                            });
                        }
                    });

                    var premDivTeams = await leagueTableService.GetPremierLeagueTables();
                    if (premDivTeams != null)
                        PremierDivisionCollection = new ObservableCollection<LeagueTable>(premDivTeams.OrderBy(e => e.Position));

                    var firstDivTeams = await leagueTableService.GetFirstDivisionTables();
                    if (firstDivTeams != null)
                        FirstDivisionCollection = new ObservableCollection<LeagueTable>(firstDivTeams.OrderBy(e => e.Position));


                    var coronaDivTeams = await leagueTableService.GetCoronaLeagueTables();
                    if (coronaDivTeams != null)
                        CoronaLeagueCollection = new ObservableCollection<LeagueTable>(coronaDivTeams.OrderBy(e => e.Position));

                    var womensDivTeams = await leagueTableService.GetWomensLeagueTables();
                    if (womensDivTeams != null)
                        WomensLeagueCollection = new ObservableCollection<LeagueTable>(womensDivTeams.OrderBy(e => e.Position));



                    //var goalsScoredByPlayerList = await leagueStatsService.GetGoalsScoredByPlayer();
                    //PlayerGoalsCollection = new ObservableCollection<LeagueStat>(goalsScoredByPlayerList);

                    //var goalsScoredByTeamList = await leagueStatsService.GetGoalsScoredByTeam();
                    //TeamScoredCollection = new ObservableCollection<LeagueStat>(goalsScoredByTeamList);

                    //var goalsConcededByTeamList = await leagueStatsService.GetGoalsConcededByTeam();
                    //TeamConcededCollection = new ObservableCollection<LeagueStat>(goalsConcededByTeamList);

                    var goalsScoredByPlayerList = await leagueStatsService.GetGoalsScoredByPlayer();

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

                    var womenDivPlayers = goalsScoredByPlayerList.Where(e => e.LeagueName.Contains("Women's League"));
                    if (womenDivPlayers != null)
                        WomenPlayerGoalsCollection = new ObservableCollection<LeagueStat>(womenDivPlayers.OrderByDescending(e => e.Goals));


                    var goalsScoredByTeamList = await leagueStatsService.GetGoalsScoredByTeam();

                    //Most Scored
                    var firstDivScored = goalsScoredByTeamList.Where(e => e.LeagueName.Contains("First Division"));
                    if (firstDivScored != null)
                        FirstTeamScoredCollection = new ObservableCollection<LeagueStat>(firstDivScored.OrderByDescending(e => e.Goals));

                    var premDivScored = goalsScoredByTeamList.Where(e => e.LeagueName.Contains("Premier Division"));
                    if (premDivScored != null)
                        PremierTeamScoredCollection = new ObservableCollection<LeagueStat>(premDivScored.OrderByDescending(e => e.Goals));

                    var coronaDivScored = goalsScoredByTeamList.Where(e => e.LeagueName.Contains("Corona League"));
                    if (coronaDivScored != null)
                        CoronaTeamScoredCollection = new ObservableCollection<LeagueStat>(coronaDivScored.OrderByDescending(e => e.Goals));

                    var womenDivScored = goalsScoredByTeamList.Where(e => e.LeagueName.Contains("Women's League"));
                    if (womenDivScored != null)
                        WomenTeamScoredCollection = new ObservableCollection<LeagueStat>(womenDivScored.OrderByDescending(e => e.Goals));


                    var goalsConcededByTeamList = await leagueStatsService.GetGoalsConcededByTeam();

                    //Most Conceded
                    var firstDivConceded = goalsConcededByTeamList.Where(e => e.LeagueName.Contains("First Division"));
                    if (firstDivConceded != null)
                        FirstTeamConcededCollection = new ObservableCollection<LeagueStat>(firstDivConceded.OrderByDescending(e => e.Goals));


                    var premDivConceded = goalsConcededByTeamList.Where(e => e.LeagueName.Contains("Premier Division"));
                    if (premDivConceded != null)
                        PremierTeamConcededCollection = new ObservableCollection<LeagueStat>(premDivConceded.OrderByDescending(e => e.Goals));


                    var coronaDivConceded = goalsConcededByTeamList.Where(e => e.LeagueName.Contains("Corona League"));
                    if (coronaDivConceded != null)
                        CoronaTeamConcededCollection = new ObservableCollection<LeagueStat>(coronaDivConceded.OrderByDescending(e => e.Goals));

                    var womenDivConceded = goalsConcededByTeamList.Where(e => e.LeagueName.Contains("Women's League"));
                    if (womenDivConceded != null)
                        WomenTeamConcededCollection = new ObservableCollection<LeagueStat>(womenDivConceded.OrderByDescending(e => e.Goals));
                }
                else
                    NoConnectivity = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Table");
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        private async void AdTapped()
        {
            try
            {
                await Task.Run(async () =>
                {
                    await adService.Impressions(Ad.ID);
                });

                await Launcher.OpenAsync(new Uri(Ad.URL));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Ad Tapped");
            }
        }

        #endregion
    }
}
