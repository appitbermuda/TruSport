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
using System.Diagnostics;
using TruSport.Views.Tennis;

namespace TruSport.ViewModels.Tennis
{
    public class FixtureDetailPageViewModel : BaseViewModel
    {
        #region Fields
        private int selectedIndex;
        private TennisFixture _fixtureItem;
        private ObservableCollection<TennisFixture> headToHeadFixtureCollection;
        private ObservableCollection<TennisFixture> fixturesCollection;

        private Ad _ad;
        private double subHeight;
        private bool _isHeadToHeadActivityIndicatorVisible;
        private bool _isTableActivityIndicatorVisible;
        private bool _isActivityIndicatorVisible;
        private bool isFavourite;
        private bool isFavouriteVisible;
        private bool noConnectivity;
        private bool cancelFixtureRefresh;
        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        FixtureService fixtureService;
        AdService adService;
        INavigation Navigation;

        #endregion

        #region Constructor

        public FixtureDetailPageViewModel(INavigation navigation, TennisFixture fixture)
        {
            Navigation = navigation;
            HeadToHeadFixtureCollection = new ObservableCollection<TennisFixture>();
            fixtureService = new FixtureService();
            adService = new AdService();

            SelectedIndex = 0;

            GenerateSource(fixture);

            FavouriteCommand = new Command(async () => await Favourite());
            AdTappedCommand = new Command(AdTapped);
        }

        #endregion

        #region Properties
        public Command AdTappedCommand { get; }
        public Command FavouriteCommand { get; }

        public TennisFixture FixtureItem
        {
            get { return _fixtureItem; }
            set { Set(ref _fixtureItem, value); }
        }

        public ObservableCollection<TennisFixture> HeadToHeadFixtureCollection
        {
            get { return headToHeadFixtureCollection; }
            set { Set(ref headToHeadFixtureCollection, value); }
        }

        public ObservableCollection<TennisFixture> FixturesCollection
        {
            get { return fixturesCollection; }
            set { Set(ref fixturesCollection, value); }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { Set(ref selectedIndex, value); }
        }

        public double SubHeight
        {
            get { return subHeight; }
            set { Set(ref subHeight, value); }
        }

        public bool IsHeadToHeadActivityIndicatorVisible
        {
            get { return _isHeadToHeadActivityIndicatorVisible; }
            set { Set(ref _isHeadToHeadActivityIndicatorVisible, value); }
        }

        public bool IsTableActivityIndicatorVisible
        {
            get { return _isTableActivityIndicatorVisible; }
            set { Set(ref _isTableActivityIndicatorVisible, value); }
        }

        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public bool IsFavourite
        {
            get { return isFavourite; }
            set { Set(ref isFavourite, value); }
        }

        public bool IsFavouriteVisible
        {
            get { return isFavouriteVisible; }
            set { Set(ref isFavouriteVisible, value); }
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

        internal async void GenerateSource(TennisFixture fixtureItem)
        {
            CancelFixtureRefresh = true;

            IsActivityIndicatorVisible = true;
            IsHeadToHeadActivityIndicatorVisible = true;
            IsTableActivityIndicatorVisible = true;

            try
            {
                FixtureItem = fixtureItem;

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

                if (fixtureItem.FixtureTime > DateTime.Now)
                    IsFavouriteVisible = true;
                else
                    IsFavouriteVisible = false;

                IsFavourite = await App.Database.IsTennisFixtureFavourite(fixtureItem.ID);

                var fixture = await fixtureService.GetTennisFixture(FixtureItem.ID);

                HeadToHeadFixtureCollection = new ObservableCollection<TennisFixture>(fixture.HeadToHead);
                IsHeadToHeadActivityIndicatorVisible = false;

                //TennisResultCollection = new ObservableCollection<TennisGameResult>(fixture.TennisGameResults);

                SubHeight = 5 * 40;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Fixture Detail");
            }

            IsActivityIndicatorVisible = false;
            IsTableActivityIndicatorVisible = false;
            IsHeadToHeadActivityIndicatorVisible = false;
        }

        internal async Task RefreshFixtures()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    NoConnectivity = false;

                    FixtureItem = await fixtureService.GetTennisFixture(FixtureItem.ID);
                }
                else
                    NoConnectivity = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TennisFixture Refresh");
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

        async Task Favourite()
        {
            try
            {
                if (IsFavourite)
                {
                    IsFavourite = false;

                    await App.Database.DeleteTennisFixtureFavourite(FixtureItem.ID);
                }
                else
                {
                    IsFavourite = true;

                    var favourite = new Favourite
                    {
                        TennisFixtureID = FixtureItem.ID,
                        Type = "Fixture",
                        Sport = "Tennis"
                    };

                    await App.Database.SaveTennisFavourite(favourite);
                }

            }
            catch (Exception ex)
            {
                IsFavourite = !IsFavourite;
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        #endregion
    }
}
