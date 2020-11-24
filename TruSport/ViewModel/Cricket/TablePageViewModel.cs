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
    public class TablePageViewModel : BaseViewModel
    {
        #region Fields
        private CricketLeagueTable tappedInfo; 
        private ObservableCollection<CricketLeagueTable> premierTeamCollection;
        private ObservableCollection<CricketLeagueTable> firstDivisionTeamCollection;
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
            PremierTeamCollection = new ObservableCollection<CricketLeagueTable>();
            FirstDivisionTeamCollection = new ObservableCollection<CricketLeagueTable>();
            
            leagueTableService = new LeagueTableService();
            adService = new AdService();
            GenerateSource();

            AdTappedCommand = new Command(AdTapped);
        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }
        public ObservableCollection<CricketLeagueTable> PremierTeamCollection
        {
            get { return premierTeamCollection; }
            set { Set(ref this.premierTeamCollection, value); }
        }

        public ObservableCollection<CricketLeagueTable> FirstDivisionTeamCollection
        {
            get { return firstDivisionTeamCollection; }
            set { Set(ref this.firstDivisionTeamCollection, value); }
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
                                Ad = ads.Any(e => e.Sport == Constants.Bowling) ? ads.FirstOrDefault(e => e.Sport == Constants.Bowling) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                            });
                        }
                    });

                    var premDivTeams = await leagueTableService.GetPremierLeagueCricketTables();
                    if (premDivTeams != null)
                        PremierTeamCollection = new ObservableCollection<CricketLeagueTable>(premDivTeams.OrderBy(e => e.Position));

                    var firstDivTeams = await leagueTableService.GetFirstDivisionCricketTables();
                    if (firstDivTeams != null)
                        FirstDivisionTeamCollection = new ObservableCollection<CricketLeagueTable>(firstDivTeams.OrderBy(e => e.Position));
                }
                else
                    NoConnectivity = true;
            }
            catch(Exception ex)
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

        private async void ItemTapped(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            tappedInfo = e.ItemData as CricketLeagueTable;
        }
    }
}
