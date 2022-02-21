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

namespace TruSport.ViewModels.Basketball
{
    public class LeagueStatPageViewModel : BaseViewModel
    {
        #region Fields
        private BasketballLeagueStat tappedInfo;
        private ObservableCollection<BasketballLeagueStat> playerStatsCollection;
        private ObservableCollection<BasketballLeagueStat> teamForCollection;
        private ObservableCollection<BasketballLeagueStat> teamAgainstCollection;

        private ObservableCollection<BasketballLeagueStat> premierPlayerGoalsCollection;
        private ObservableCollection<BasketballLeagueStat> firstPlayerGoalsCollection;
        private ObservableCollection<BasketballLeagueStat> coronaPlayerGoalsCollection;
        private ObservableCollection<BasketballLeagueStat> premierGoalsConcededCollection;
        private ObservableCollection<BasketballLeagueStat> firstGoalsConcededCollection;
        private ObservableCollection<BasketballLeagueStat> coronaGoalsConcededCollection;
        private ObservableCollection<BasketballLeagueStat> premierGoalsScoredCollection;
        private ObservableCollection<BasketballLeagueStat> firstGoalsScoredCollection;
        private ObservableCollection<BasketballLeagueStat> coronaGoalsScoredCollection;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        
        LeagueStatService leagueStatsService;
        AdService adService;

        #endregion

        #region Constructor

        public LeagueStatPageViewModel()
        {
            PlayerStatsCollection = new ObservableCollection<BasketballLeagueStat>();
            TeamForCollection = new ObservableCollection<BasketballLeagueStat>();
            TeamAgainstCollection = new ObservableCollection<BasketballLeagueStat>();
            
            leagueStatsService = new LeagueStatService();
            adService = new AdService();

            GenerateSource();

            AdTappedCommand = new Command(AdTapped);
        }

        #endregion

        #region Properties

        public Command AdTappedCommand { get; }
        public ObservableCollection<BasketballLeagueStat> PlayerStatsCollection
        {
            get { return playerStatsCollection; }
            set { Set(ref this.playerStatsCollection, value); }
        }

        public ObservableCollection<BasketballLeagueStat> TeamForCollection
        {
            get { return teamForCollection; }
            set { Set(ref this.teamForCollection, value); }
        }

        public ObservableCollection<BasketballLeagueStat> TeamAgainstCollection
        {
            get { return teamAgainstCollection; }
            set { Set(ref this.teamAgainstCollection, value); }
        }

        private Ad _ad;
        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
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

                    await Task.Run(async () =>
                    {
                        var ads = await adService.GetAds();

                        if (ads != null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Ad = ads.Any(e => e.Sport == Constants.Basketball) ? ads.FirstOrDefault(e => e.Sport == Constants.Basketball) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                            });
                        }
                    });

                    var basketballStatsByPlayer = await leagueStatsService.GetBasketballStatsByPlayer();
                    PlayerStatsCollection = new ObservableCollection<BasketballLeagueStat>(basketballStatsByPlayer);

                    var basketballForStatsByTeam = await leagueStatsService.GetBasketballForStatsByTeam();
                    TeamForCollection = new ObservableCollection<BasketballLeagueStat>(basketballForStatsByTeam);

                    var basketballAgainstStatsByTeam = await leagueStatsService.GetBasketballAgainstStatsByTeam();
                    TeamAgainstCollection = new ObservableCollection<BasketballLeagueStat>(basketballAgainstStatsByTeam);
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
