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
    public class TablePageViewModel : BaseViewModel
    {
        #region Fields
        private BasketballLeagueStanding tappedInfo; 
        private ObservableCollection<BasketballLeagueStanding> islandLeagueFallCollection;
        //private ObservableCollection<BasketballLeagueStanding> firstDivisionTeamCollection;
        //private ObservableCollection<BasketballLeagueStanding> coronaTeamCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        
        LeagueTableService leagueTableService;
        AdService adService;

        #endregion

        #region Constructor

        public TablePageViewModel()
        {
            IslandLeagueFallCollection = new ObservableCollection<BasketballLeagueStanding>();
            //FirstDivisionTeamCollection = new ObservableCollection<BasketballLeagueStanding>();
            //CoronaTeamCollection = new ObservableCollection<BasketballLeagueStanding>();
            
            leagueTableService = new LeagueTableService();
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

        public ObservableCollection<BasketballLeagueStanding> IslandLeagueFallCollection
        {
            get { return islandLeagueFallCollection; }
            set { Set(ref this.islandLeagueFallCollection, value); }
        }

        //public ObservableCollection<BasketballLeagueStanding> FirstDivisionTeamCollection
        //{
        //    get { return firstDivisionTeamCollection; }
        //    set { Set(ref this.firstDivisionTeamCollection, value); }
        //}

        //public ObservableCollection<BasketballLeagueStanding> CoronaTeamCollection
        //{
        //    get { return coronaTeamCollection; }
        //    set { Set(ref this.coronaTeamCollection, value); }
        //}

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
                                Ad = ads.Any(e => e.Sport == Constants.Basketball) ? ads.FirstOrDefault(e => e.Sport == Constants.Basketball) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                            });
                        }
                    });

                    var islandFallLeague = await leagueTableService.GetIslandFallBasketballLeagueStandings();
                    if (islandFallLeague != null)
                        IslandLeagueFallCollection = new ObservableCollection<BasketballLeagueStanding>(islandFallLeague.OrderBy(e => e.Position));

                    //var firstDivTeams = await leagueTableService.GetFirstDivisionTables();
                    //if (firstDivTeams != null)
                    //    FirstDivisionTeamCollection = new ObservableCollection<BasketballLeagueStanding>(firstDivTeams.OrderBy(e => e.Position));


                    //var coronaDivTeams = await leagueTableService.GetCoronaBasketballLeagueStandings();
                    //if (coronaDivTeams != null)
                    //    CoronaTeamCollection = new ObservableCollection<BasketballLeagueStanding>(coronaDivTeams.OrderBy(e => e.Position));

                }
                else
                    NoConnectivity = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Table");
            }

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
