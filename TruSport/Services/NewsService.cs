using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;
using Xamarin.Essentials;

namespace TruSport.Services
{
    public class NewsService
    {
        public NewsService()
        {
        }

        public async Task<List<RssFeedItem>> GetFeed()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("News/Feed", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<RssFeedItem> feed = JsonConvert.DeserializeObject<List<RssFeedItem>>(response.Content);

                    return feed;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }
            return null;
        }

        public async Task<List<RssFeedItem>> GetBowlingFeed()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("News/BowlingFeed", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<RssFeedItem> feed = JsonConvert.DeserializeObject<List<RssFeedItem>>(response.Content);

                    return feed;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }
            return null;
        }

        public async Task<List<RssFeedItem>> GetCricketFeed()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("News/CricketFeed", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<RssFeedItem> feed = JsonConvert.DeserializeObject<List<RssFeedItem>>(response.Content);

                    return feed;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }
            return null;
        }

        public async Task<List<RssFeedItem>> GetFootballFeed()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("News/FootballFeed", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<RssFeedItem> feed = JsonConvert.DeserializeObject<List<RssFeedItem>>(response.Content);

                    return feed;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }
            return null;
        }

        public async Task<List<RssFeedItem>> GetTennisFeed()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("News/TennisFeed", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<RssFeedItem> feed = JsonConvert.DeserializeObject<List<RssFeedItem>>(response.Content);

                    return feed;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }
            return null;
        }
    }
}
