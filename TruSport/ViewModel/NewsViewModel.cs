using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public NewsViewModel()
        {
            Feed = new ObservableCollection<RssFeedItem>();

            newsService = new NewsService();

            GenerateSource();
        }

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

                var feed = await newsService.GetFeed();

                Feed = new ObservableCollection<RssFeedItem>(feed.OrderByDescending(e => e.Date));
            }
            else
                NoConnectivity = true;

            IsActivityIndicatorVisible = false;
        }
    }
}
