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

namespace TruSport.ViewModels.Tennis
{
    public class RankingPageViewModel : BaseViewModel
    {
        #region Fields
        private TennisRanking tappedInfo; 
        private ObservableCollection<TennisRanking> _mensCurrentRanking;
        private ObservableCollection<TennisRanking> _womensCurrentRanking;

        private Command<Syncfusion.ListView.XForms.ItemTappedEventArgs> itemtapCommand;
        private Command<object> favoriteTapCommand;
        private Command<object> resetTapCommand;
        private bool _isActivityIndicatorVisible;
        private bool noConnectivity;
        private Ad _ad;

        RankingService rankingsService;
        AdService adService;

        #endregion

        #region Constructor

        public RankingPageViewModel()
        {
            MensCurrentRanking = new ObservableCollection<TennisRanking>();
            WomensCurrentRanking = new ObservableCollection<TennisRanking>();

            rankingsService = new RankingService();
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

        public ObservableCollection<TennisRanking> MensCurrentRanking
        {
            get { return _mensCurrentRanking; }
            set { Set(ref this._mensCurrentRanking, value); }
        }

        public ObservableCollection<TennisRanking> WomensCurrentRanking
        {
            get { return _womensCurrentRanking; }
            set { Set(ref this._womensCurrentRanking, value); }
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
            //for(var i = 0; i < SyncTitles.Length; i++)
            //{
            //    SyncTitleCollection.Add(SyncTitles[i]);
            //}
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
                                Ad = ads.Any(e => e.Sport == Constants.Tennis) ? ads.FirstOrDefault(e => e.Sport == Constants.Tennis) : ads.FirstOrDefault(e => String.IsNullOrEmpty(e.Sport));
                            });
                        }
                    });

                    var menCurrentRanking = await rankingsService.GetMensCurrentRanking();

                    if(menCurrentRanking != null)
                        MensCurrentRanking = new ObservableCollection<TennisRanking>(menCurrentRanking);

                    var womenCurrentRanking = await rankingsService.GetWomensCurrentRanking();

                    if (womenCurrentRanking != null)
                        WomensCurrentRanking = new ObservableCollection<TennisRanking>(womenCurrentRanking);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Ranking");
                }
            }
            else
                NoConnectivity = true;

            IsActivityIndicatorVisible = false;
            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
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
