using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using TruSport.Data;
using TruSport.Model;
using TruSport.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TruSport.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        private ObservableCollection<RssFeedItem> _feed;
        private RssFeedItem _selectedItem;
        private bool noConnectivity;
        private bool _isActivityIndicatorVisible;

        NewsService newsService;
        AdService adService;

        public NewsViewModel()
        {
            Feed = new ObservableCollection<RssFeedItem>();

            newsService = new NewsService();
            adService = new AdService();

            GenerateSource();

            AdTappedCommand = new Command(AdTapped);
        }

        public Command AdTappedCommand { get; }
        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        public ObservableCollection<RssFeedItem> Feed
        {
            get { return _feed; }
            set { Set(ref _feed, value); }
        }

        public RssFeedItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                OnItemSelected(value);
                Set(ref _selectedItem, value);
            }
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

        private void OnItemSelected(RssFeedItem item)
        {
            if (item != null)
            {
                Device.OpenUri(item.Link);
            }
        }

        internal async void GenerateSource()
        {
            IsActivityIndicatorVisible = true;

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

                var feed = await newsService.GetFeed();

                Feed = new ObservableCollection<RssFeedItem>(feed.OrderByDescending(e => e.Date));
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
    }
}
