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

namespace TruSport.ViewModels.Cricket
{
    public class StatPageViewModel : BaseViewModel
    {
        #region Fields
        private CricketLeagueTable tappedInfo; 
        private ObservableCollection<CricketLeagueTable> premierTeamCollection;
        private ObservableCollection<CricketLeagueTable> firstDivisionTeamCollection;

        private ObservableCollection<LeagueStat> _premierPlayerMostRunsCollection;
        private ObservableCollection<LeagueStat> _premierPlayerMostWicketsCollection;

        private ObservableCollection<LeagueStat> _firstPlayerMostRunsCollection;
        private ObservableCollection<LeagueStat> _firstPlayerMostWicketsCollection;

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

        public StatPageViewModel(MatchType matchType)
        {
            PremierDivisionCollection = new ObservableCollection<CricketLeagueTable>();
            FirstDivisionCollection = new ObservableCollection<CricketLeagueTable>();

            PremierPlayerMostRunsCollection = new ObservableCollection<LeagueStat>();
            PremierPlayerMostWicketsCollection = new ObservableCollection<LeagueStat>();

            FirstPlayerMostRunsCollection = new ObservableCollection<LeagueStat>();
            FirstPlayerMostWicketsCollection = new ObservableCollection<LeagueStat>();

            leagueTableService = new LeagueTableService();
            leagueStatsService = new LeagueStatService();
            adService = new AdService();

            GenerateSource(matchType);

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

        public ObservableCollection<CricketLeagueTable> PremierDivisionCollection
        {
            get { return premierTeamCollection; }
            set { Set(ref this.premierTeamCollection, value); }
        }

        public ObservableCollection<CricketLeagueTable> FirstDivisionCollection
        {
            get { return firstDivisionTeamCollection; }
            set { Set(ref this.firstDivisionTeamCollection, value); }
        }

        public ObservableCollection<LeagueStat> PremierPlayerMostRunsCollection
        {
            get { return _premierPlayerMostRunsCollection; }
            set { Set(ref this._premierPlayerMostRunsCollection, value); }
        }

        public ObservableCollection<LeagueStat> PremierPlayerMostWicketsCollection
        {
            get { return _premierPlayerMostWicketsCollection; }
            set { Set(ref this._premierPlayerMostWicketsCollection, value); }
        }

        public ObservableCollection<LeagueStat> FirstPlayerMostRunsCollection
        {
            get { return _firstPlayerMostRunsCollection; }
            set { Set(ref this._firstPlayerMostRunsCollection, value); }
        }

        public ObservableCollection<LeagueStat> FirstPlayerMostWicketsCollection
        {
            get { return _firstPlayerMostWicketsCollection; }
            set { Set(ref this._firstPlayerMostWicketsCollection, value); }
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

        internal async void GenerateSource(MatchType matchType)
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

                    var premDivTeams = await leagueTableService.GetPremierLeagueCricketTables(matchType.ID);
                    if (premDivTeams != null)
                        PremierDivisionCollection = new ObservableCollection<CricketLeagueTable>(premDivTeams.OrderBy(e => e.Position));

                    var firstDivTeams = await leagueTableService.GetFirstDivisionCricketTables(matchType.ID);
                    if (firstDivTeams != null)
                        FirstDivisionCollection = new ObservableCollection<CricketLeagueTable>(firstDivTeams.OrderBy(e => e.Position));

                    var runsByPlayer = await leagueStatsService.GetMostRunsByPlayer(matchType.ID);
                    if (runsByPlayer != null)
                        PremierPlayerMostRunsCollection = new ObservableCollection<LeagueStat>(runsByPlayer.Where(e => e.LeagueName.Contains("Premier Division")));

                    if (runsByPlayer != null)
                        FirstPlayerMostRunsCollection = new ObservableCollection<LeagueStat>(runsByPlayer.Where(e => e.LeagueName.Contains("First Division")));

                    var wicketsByPlayer = await leagueStatsService.GetMostWicketsByPlayer(matchType.ID);
                    if (wicketsByPlayer != null)
                        PremierPlayerMostWicketsCollection = new ObservableCollection<LeagueStat>(wicketsByPlayer.Where(e => e.LeagueName.Contains("Premier Division")));

                    if (wicketsByPlayer != null)
                        FirstPlayerMostWicketsCollection = new ObservableCollection<LeagueStat>(wicketsByPlayer.Where(e => e.LeagueName.Contains("First Division")));

                }
                else
                    NoConnectivity = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Stat");
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
