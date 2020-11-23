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
using TruSport.Views.Bowling;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;

namespace TruSport.ViewModels.Bowling
{
    public class TablePageViewModel : BaseViewModel
    {
        #region Fields
        private BowlingLeagueStanding tappedInfo; 
        private ObservableCollection<BowlingLeagueStanding> somersbyLeagueCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        private Ad _ad;
        INavigation Navigation;
        LeagueTableService leagueTableService;
        AdService adService;

        #endregion

        #region Constructor

        public TablePageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            SomersbyLeagueCollection = new ObservableCollection<BowlingLeagueStanding>();
            
            leagueTableService = new LeagueTableService();
            adService = new AdService();
            GenerateSource();

            TableTappedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
            AdTappedCommand = new Command(AdTapped);
        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }
        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> TableTappedCommand
        {
            get { return itemtapCommand; }
            set { itemtapCommand = value; }
        }

        public ObservableCollection<BowlingLeagueStanding> SomersbyLeagueCollection
        {
            get { return somersbyLeagueCollection; }
            set { Set(ref this.somersbyLeagueCollection, value); }
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

                    var somersbyLeague = await leagueTableService.GetSomersbyLeagueBowlingTables();
                    if (somersbyLeague != null)
                        SomersbyLeagueCollection = new ObservableCollection<BowlingLeagueStanding>(somersbyLeague.OrderBy(e => e.Position));
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
            try
            {
                tappedInfo = e.ItemData as BowlingLeagueStanding;

                await Navigation.PushModalAsync(new TableDetailPage(tappedInfo));
            }
            catch(Exception ex)
            {

            }

        }
    }
}
