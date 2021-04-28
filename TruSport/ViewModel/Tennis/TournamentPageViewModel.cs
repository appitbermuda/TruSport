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
using TruSport.Views.Tennis;

namespace TruSport.ViewModels.Tennis
{
    public class TournamentPageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<TennisTournament> tennisTournamentCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onTournamentSelectedCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        FixtureService fixtureService;
        TransferService transferService;
        TournamentService tennisTournamentService;
        INavigation Navigation;
        AdService adService;

        #endregion

        #region Constructor

        public TournamentPageViewModel(INavigation navigation)
        {
            Navigation = navigation;

            tennisTournamentCollection = new ObservableCollection<TennisTournament>();
            fixtureService = new FixtureService();
            tennisTournamentService = new TournamentService();
            adService = new AdService();

            GenerateSource();

            AdTappedCommand = new Command(AdTapped);
            OnTournamentSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(TournamentSelected);

        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }


        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnTournamentSelectedCommand
        {
            get { return onTournamentSelectedCommand; }
            set { onTournamentSelectedCommand = value; }
        }

        public ObservableCollection<TennisTournament> TournamentCollection
        {
            get { return tennisTournamentCollection; }
            set { Set(ref tennisTournamentCollection, value); }
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
                                Ad = ads.Any(e => e.Sport == Constants.Tennis) ? ads.FirstOrDefault(e => e.Sport == Constants.Tennis) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                            });
                        }
                    });

                    var tennisTournaments = await tennisTournamentService.GetTournaments();

                    if (tennisTournaments != null)
                    {
                        TournamentCollection = new ObservableCollection<TennisTournament>(tennisTournaments.OrderByDescending(e => e.Start));
                    }
                }
                else
                    NoConnectivity = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Competitions");
            }
            finally
            {
                IsActivityIndicatorVisible = false;
            }
        }

        private async void TournamentSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as TennisTournament;

            if (item != null)
                await Navigation.PushAsync(new TournamentDetailPage(item));
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
