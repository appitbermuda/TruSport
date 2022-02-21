using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using TruSport.Services;
using Xamarin.Essentials;
using System.Diagnostics;
using Microsoft.AppCenter.Crashes;

namespace TruSport.ViewModels.Triathlon
{
    public class CompetitionsPageViewModel : BaseViewModel
    {
        #region Fields
        private LeagueTableListView tappedInfo; 
        private ObservableCollection<League> leagueCollection;
        private bool _isActivityIndicatorVisible;
        private bool _isActivityVisible;
        private bool isLoadMoreVisible;
        private int totalCount;
        private int tabCount;
        private bool isTableExist;
        private bool noConnectivity;
        FixtureService fixtureService;
        LeagueTableService leagueTableService;
        TransferService transferService;
        LeagueService leagueService;
        AdService adService;
        INavigation Navigation;

        #endregion

        #region Constructor

        public CompetitionsPageViewModel()
        {
            LeagueCollection = new ObservableCollection<League>();
            fixtureService = new FixtureService();
            leagueTableService = new LeagueTableService();
            leagueService = new LeagueService();
            adService = new AdService();

            GenerateSource();

            AdTappedCommand = new Command(AdTapped);
        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }
        public ObservableCollection<League> LeagueCollection
        {
            get { return leagueCollection; }
            set { Set(ref leagueCollection, value); }
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

                    var leagues = await leagueService.GetBasketballLeagues();
                    LeagueCollection = new ObservableCollection<League>(leagues.OrderBy(e => e.Name).ToList());

                }
                else
                    NoConnectivity = true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Competitions");
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


        #region Player Info

        Fixture[] Player = new Fixture[]
         {
            
         };

        #endregion
    }
}
