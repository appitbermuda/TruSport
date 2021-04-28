using System;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using TruSport.Services;
using System.Collections.Generic;
using TruSport.Views.Tennis;
using Syncfusion.DataSource.Extensions;
using TruSport.Extensions;
using Xamarin.Essentials;
using NodaTime;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Device = Xamarin.Forms.Device;
using System.Windows.Input;
using Syncfusion.SfCalendar.XForms;
using Newtonsoft.Json;
using TruSport.Views;
using System.Diagnostics;

namespace TruSport.ViewModels.Tennis
{
    public class TournamentDetailPageViewModel : BaseViewModel
    {
        #region Fields
        
        public TennisTournament _tennisTournament;
        private int selectedIndex;
        private ObservableCollection<TennisFixture> fixtureCollection;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> onFixtureSelectedCommand;
        private Command<object> refreshFixturesCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        private bool cancelFixtureRefresh;
        private Ad _ad;

        FixtureService fixtureService;
        AdService adService;

        INavigation Navigation;

        #endregion

        #region Constructor

        public TournamentDetailPageViewModel(INavigation navigation, TennisTournament tennisTournament)
        {
            Navigation = navigation;

            FixtureCollection = new ObservableCollection<TennisFixture>();

            fixtureService = new FixtureService();
            adService = new AdService();

            SelectedIndex = 0;

            GenerateSource(tennisTournament);

            RefreshFixturesCommand = new Command<object>(async (obj) => await RefreshFixtures());

            OnFixtureSelectedCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(FixtureSelected);
            AdTappedCommand = new Command(AdTapped);

            MessagingCenter.Subscribe<string>("Fixtures", "RefreshFixtures", async (sender) =>
            {
                MessagingCenter.Unsubscribe<string>("Fixtures", "RefreshFixtures");
                await RefreshFixtures();

            });
        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }
        public Command<object> RefreshFixturesCommand
        {
            get { return refreshFixturesCommand; }
            set { refreshFixturesCommand = value; }
        }

        public Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> OnFixtureSelectedCommand
        {
            get { return onFixtureSelectedCommand; }
            set { onFixtureSelectedCommand = value; }
        }

        public ObservableCollection<TennisFixture> FixtureCollection
        {
            get { return fixtureCollection; }
            set { Set(ref fixtureCollection, value); }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { Set(ref selectedIndex, value); }
        }

        public TennisTournament TennisTournament
        {
            get { return _tennisTournament; }
            set { Set(ref _tennisTournament, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool NoConnectivity
        {
            get { return noConnectivity; }
            set { Set(ref noConnectivity, value); }
        }

        public bool CancelFixtureRefresh
        {
            get { return cancelFixtureRefresh; }
            set { Set(ref cancelFixtureRefresh, value); }
        }

        public Ad Ad
        {
            get { return _ad; }
            set { Set(ref _ad, value); }
        }

        #endregion

        #region Generate Source

        internal async void GenerateSource(TennisTournament tennisTournament)
        {
            TennisTournament = tennisTournament;
            CancelFixtureRefresh = true;
            IsActivityIndicatorVisible = true;

            try
            {
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

                await RefreshFixtures();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                NoConnectivity = true;
            }

            IsActivityIndicatorVisible = false;
        }

        internal async Task RefreshFixtures()
        {
            try
            { 
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;

                    var fixtures = await fixtureService.GetTennisTournamentFixtures(TennisTournament.ID);

                    if (fixtures != null)
                    {
                        if (fixtures != null)
                        {
                            FixtureCollection = new ObservableCollection<TennisFixture>(fixtures.OrderByDescending(e => e.FixtureTime));
                        }
                    }
                }
                else
                    NoConnectivity = true;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "RefreshFixtures");
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

        private async void FixtureSelected(Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var item = e.ItemData as TennisFixture;

            if (item != null)
                await Navigation.PushAsync(new FixtureDetailsPage(item));
        }

        #endregion

    }
}
