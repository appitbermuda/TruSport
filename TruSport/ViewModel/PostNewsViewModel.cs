using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TruSport.Data;
using TruSport.Model;
using Xamarin.Forms;

namespace TruSport.ViewModels
{
    public class PostNewsViewModel : BaseViewModel
    {
        //private const string BernewsFeedUri = "http://bernews.com/tag/bermuda-football/feed/";
        //private const string IStatsFeedUri = "http://www.islandstats.com/islandstats_rss.asp";
        //private const string RGFeedUri = "http://www.royalgazette.com/section/?template=RSS";

        private RssFeedItem feedItem;
        private string title;
        private string obo;
        private string description;

        //private ObservableCollection<RssFeedItem> berNewsFeed;
        //private ObservableCollection<RssFeedItem> iStatsFeed;
        //private ObservableCollection<RssFeedItem> rgFeed;

        private bool _isActivityIndicatorVisible;

        public PostNewsViewModel()
        {

            //BerNewsFeed = new ObservableCollection<RssFeedItem>();
            //IStatsFeed = new ObservableCollection<RssFeedItem>();
            //RGFeed = new ObservableCollection<RssFeedItem>();

            GenerateSource();
        }


        public bool IsActivityIndicatorVisible
        {
            get { return _isActivityIndicatorVisible; }
            set { Set(ref _isActivityIndicatorVisible, value); }
        }

        private ObservableCollection<RssFeedItem> _feed;

        public ObservableCollection<RssFeedItem> Feed
        {
            get { return _feed; }
            set { Set(ref _feed, value); }
        }

        public RssFeedItem FeedItem
        {
            get { return feedItem; }
            set { Set(ref feedItem, value); }
        }

        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        public string OBO
        {
            get { return obo; }
            set { Set(ref obo, value); }
        }

        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }

        //public ObservableCollection<RssFeedItem> RSSFeed
        //{
        //    get { return rssFeed; }
        //    set { Set(ref rssFeed, value); }
        //}

        //public ObservableCollection<RssFeedItem> BerNewsFeed
        //{
        //    get { return berNewsFeed; }
        //    set { Set(ref berNewsFeed, value); }
        //}

        //public ObservableCollection<RssFeedItem> IStatsFeed
        //{
        //    get { return iStatsFeed; }
        //    set { Set(ref iStatsFeed, value); }
        //}

        //public ObservableCollection<RssFeedItem> RGFeed
        //{
        //    get { return rgFeed; }
        //    set { Set(ref rgFeed, value); }
        //}

        private RssFeedItem _selectedItem;

        public RssFeedItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                OnItemSelected(value);
                Set(ref _selectedItem, value);
            }
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



            FeedItem = new RssFeedItem("", "", "", null, "", DateTime.Now);


            //BerNewsFeed = new ObservableCollection<RssFeedItem>(await RssClient.LoadBernews(new Uri(BernewsFeedUri)));
            //IStatsFeed = new ObservableCollection<RssFeedItem>(await RssClient.LoadIStats(new Uri(IStatsFeedUri)));
            //RGFeed = new ObservableCollection<RssFeedItem>(await RssClient.LoadRG(new Uri(RGFeedUri)));

            //var feedItem = new RssFeedItem("This is the Title", "This Is the description", null, "BerNews", DateTime.Now);

            //RSSFeed.Add(feedItem);

            //Feed = new ObservableCollection<RssFeedItem>(RSSFeed.OrderByDescending(e => e.Date));

            //foreach(var feed in BerNewsFeed)
            //{
            //    if (feed != null)
            //        if (feed.Date > DateTime.Now.AddDays(-7))
            //            RSSFeed.Add(feed);
            //}

            //foreach (var feed in IStatsFeed)
            //{
            //    if(feed != null)
            //        if (feed.Date > DateTime.Now.AddDays(-7))
            //            RSSFeed.Add(feed);
            //}

            //foreach (var feed in RGFeed)
            //{
            //    if(feed != null)
            //        if (feed.Date > DateTime.Now.AddDays(-7))
            //            RSSFeed.Add(feed);
            //}

            //Feed = new ObservableCollection<RssFeedItem>(RSSFeed.OrderByDescending(e => e.Date));


            IsActivityIndicatorVisible = false;

            //ItemTapCommand = new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(ItemTapped);
        }

        public async void Update()
        {
            IsActivityIndicatorVisible = true;

            

            //RSSFeed = new ObservableCollection<RssFeedItem>();
            //BerNewsFeed = new ObservableCollection<RssFeedItem>(await RssClient.LoadBernews(new Uri(BernewsFeedUri)));
            //IStatsFeed = new ObservableCollection<RssFeedItem>(await RssClient.LoadIStats(new Uri(IStatsFeedUri)));
            //RGFeed = new ObservableCollection<RssFeedItem>(await RssClient.LoadRG(new Uri(RGFeedUri)));

            //var feedItem = new RssFeedItem("This is the Title", "This Is the description", null, "BerNews", DateTime.Now);

            //RSSFeed.Add(feedItem);

            //foreach(var feed in BerNewsFeed)
            //{
            //    if (feed != null)
            //        if (feed.Date > DateTime.Now.AddDays(-7))
            //            RSSFeed.Add(feed);
            //}

            //foreach (var feed in IStatsFeed)
            //{
            //    if(feed != null)
            //        if (feed.Date > DateTime.Now.AddDays(-7))
            //            RSSFeed.Add(feed);
            //}

            //foreach (var feed in RGFeed)
            //{
            //    if(feed != null)
            //        if (feed.Date > DateTime.Now.AddDays(-7))
            //            RSSFeed.Add(feed);
            //}
            ////RSSFeed.Add(BerNewsFeed);
            //Feed = new ObservableCollection<RssFeedItem>(RSSFeed.OrderByDescending(e => e.Date));

            IsActivityIndicatorVisible = false;
        }
    }
}
