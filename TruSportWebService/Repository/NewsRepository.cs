using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class NewsRepository : INewsRepository<RssFeedItem>
    {
        OnTrackContext _context;

        public NewsRepository(OnTrackContext context)
        {
            _context = context;
        }

        public async Task<List<RssFeedItem>> Feed()
        {
            const string BernewsFeedUri = "http://bernews.com/category/sports/feed/";
            const string IStatsFeedUri = "http://www.islandstats.com/islandstats_rss.asp";
            const string RGFeedUri = "http://www.royalgazette.com/section/?template=RSS";

            List<RssFeedItem> RSSFeed = new List<RssFeedItem>();

            try
            {
                var BerNewsFeed = await RssClient.LoadBernews(new Uri(BernewsFeedUri));
                var IStatsFeed = await RssClient.LoadIStats(new Uri(IStatsFeedUri));
                var RGFeed = await RssClient.LoadRG(new Uri(RGFeedUri));

                foreach (var feed in BerNewsFeed)
                {
                    if (feed != null)
                        if (feed.Date > DateTime.Now.AddDays(-7))
                            RSSFeed.Add(feed);
                }

                foreach (var feed in IStatsFeed)
                {
                    if (feed != null)
                        if (feed.Date > DateTime.Now.AddDays(-7))
                            RSSFeed.Add(feed);
                }

                foreach (var feed in RGFeed)
                {
                    if (feed != null)
                        if (feed.Date > DateTime.Now.AddDays(-7))
                            RSSFeed.Add(feed);
                }

                return RSSFeed.OrderByDescending(e => e.Date).ToList();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }

            return null;
        }

        public async Task<List<RssFeedItem>> CricketFeed()
        {
            const string BernewsFeedUri = "http://bernews.com/tag/bermuda-cricket/feed/";
            const string IStatsFeedUri = "http://www.islandstats.com/islandstats_rss.asp";
            const string RGFeedUri = "http://www.royalgazette.com/section/?template=RSS";

            List<RssFeedItem> RSSFeed = new List<RssFeedItem>();

            try
            {
                var BerNewsFeed = await RssClient.LoadBernews(new Uri(BernewsFeedUri));
                var IStatsFeed = await RssClient.LoadCricketIStats(new Uri(IStatsFeedUri));
                var RGFeed = await RssClient.LoadCricketRG(new Uri(RGFeedUri));

                foreach (var feed in BerNewsFeed)
                {
                    if (feed != null)
                        if (feed.Date > DateTime.Now.AddDays(-7))
                            RSSFeed.Add(feed);
                }

                foreach (var feed in IStatsFeed)
                {
                    if (feed != null)
                        if (feed.Date > DateTime.Now.AddDays(-7))
                            RSSFeed.Add(feed);
                }

                foreach (var feed in RGFeed)
                {
                    if (feed != null)
                        if (feed.Date > DateTime.Now.AddDays(-7))
                            RSSFeed.Add(feed);
                }

                return RSSFeed.OrderByDescending(e => e.Date).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }

            return null;
        }

        public async Task<List<RssFeedItem>> FootballFeed()
        {
            const string BernewsFeedUri = "http://bernews.com/tag/bermuda-football/feed/";
            const string IStatsFeedUri = "http://www.islandstats.com/islandstats_rss.asp";
            const string RGFeedUri = "http://www.royalgazette.com/section/?template=RSS";

            List<RssFeedItem> RSSFeed = new List<RssFeedItem>();

            try
            {
                var BerNewsFeed = await RssClient.LoadBernews(new Uri(BernewsFeedUri));
                var IStatsFeed = await RssClient.LoadFootballIStats(new Uri(IStatsFeedUri));
                var RGFeed = await RssClient.LoadFootballRG(new Uri(RGFeedUri));

                foreach (var feed in BerNewsFeed)
                {
                    if (feed != null)
                        if (feed.Date > DateTime.Now.AddDays(-7))
                            RSSFeed.Add(feed);
                }

                foreach (var feed in IStatsFeed)
                {
                    if (feed != null)
                        if (feed.Date > DateTime.Now.AddDays(-7))
                            RSSFeed.Add(feed);
                }

                foreach (var feed in RGFeed)
                {
                    if (feed != null)
                        if (feed.Date > DateTime.Now.AddDays(-7))
                            RSSFeed.Add(feed);
                }

                return RSSFeed.OrderByDescending(e => e.Date).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }

            return null;
        }
    }
}
