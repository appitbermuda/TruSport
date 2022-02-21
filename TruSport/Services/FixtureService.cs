using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;
using Xamarin.Essentials;

namespace TruSport.Services
{
    public class FixtureService
    {
        public FixtureService()
        {
        }

        //[Deprecated]
        public async Task<List<Fixture>> GetFixtures()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/AllFixtures", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<Fixture>> GetSportFixtures(string Sport)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/Sport", Method.GET);
                request.AddParameter("Sport", Sport);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BasketballFixture>> GetBasketballFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/AllBasketball", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BasketballFixture> fixtures = JsonConvert.DeserializeObject<List<BasketballFixture>>(response.Content);

                    return fixtures;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<BowlingFixtureListView> GetBowlingFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/AllBowling", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    BowlingFixtureListView fixtures = JsonConvert.DeserializeObject<BowlingFixtureListView>(response.Content);

                    return fixtures;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<CricketFixture>> GetCricketFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/AllCricket", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<CricketFixture> fixtures = JsonConvert.DeserializeObject<List<CricketFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<TennisFixtureListView> GetTennisFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/AllTennis", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    TennisFixtureListView fixtures = JsonConvert.DeserializeObject<TennisFixtureListView>(response.Content);

                    return fixtures;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }


        public async Task<BasketballFixture> GetBasketballFixture(string id)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/Basketball", Method.GET);
                request.AddParameter("id", id);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    BasketballFixture fixture = JsonConvert.DeserializeObject<BasketballFixture>(response.Content);

                    return fixture;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }


        public async Task<BowlingFixture> GetBowlingFixture(string id)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/Bowling", Method.GET);
                request.AddParameter("id", id);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    BowlingFixture fixture = JsonConvert.DeserializeObject<BowlingFixture>(response.Content);

                    return fixture;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<CricketFixture> GetCricketFixture(string id)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/Cricket", Method.GET);
                request.AddParameter("id", id);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    CricketFixture fixture = JsonConvert.DeserializeObject<CricketFixture>(response.Content);

                    return fixture;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<TennisFixture> GetTennisFixture(string id)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/Tennis", Method.GET);
                request.AddParameter("id", id);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    TennisFixture fixture = JsonConvert.DeserializeObject<TennisFixture>(response.Content);

                    return fixture;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BasketballFixture>> GetPastBasketballFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/PastBasketball", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BasketballFixture> fixtures = JsonConvert.DeserializeObject<List<BasketballFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BowlingFixture>> GetPastBowlingFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/PastBowling", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BowlingFixture> fixtures = JsonConvert.DeserializeObject<List<BowlingFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<CricketFixture>> GetPastCricketFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/PastCricket", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<CricketFixture> fixtures = JsonConvert.DeserializeObject<List<CricketFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<TennisFixture>> GetPastTennisFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/PastTennis", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TennisFixture> fixtures = JsonConvert.DeserializeObject<List<TennisFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BasketballFixture>> GetUpcomingBasketballFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/UpcomingBasketball", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BasketballFixture> fixtures = JsonConvert.DeserializeObject<List<BasketballFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BowlingFixture>> GetUpcomingBowlingFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/UpcomingBowling", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BowlingFixture> fixtures = JsonConvert.DeserializeObject<List<BowlingFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<CricketFixture>> GetUpcomingCricketFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/UpcomingCricket", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<CricketFixture> fixtures = JsonConvert.DeserializeObject<List<CricketFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<TennisFixture>> GetUpcomingTennisFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/UpcomingTennis", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TennisFixture> fixtures = JsonConvert.DeserializeObject<List<TennisFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<Fixture>> GetFootballFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/AllFootball", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<Fixture> GetFootballFixture(string id)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/Football", Method.GET);
                request.AddParameter("id", id);
                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    Fixture fixture = JsonConvert.DeserializeObject<Fixture>(response.Content);

                    return fixture;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<Fixture>> GetPastFootballFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/PastFootball", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<Fixture>> GetUpcomingFootballFixtures()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/UpcomingFootball", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BasketballFixture>> GetBasketballHeadToHeadFixtures(string fixtureID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/BasketballHeadToHead", Method.GET);
                request.AddParameter("fixtureID", fixtureID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BasketballFixture> fixtures = JsonConvert.DeserializeObject<List<BasketballFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BowlingFixture>> GetBowlingHeadToHeadFixtures(string fixtureID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/BowlingHeadToHead", Method.GET);
                request.AddParameter("fixtureID", fixtureID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BowlingFixture> fixtures = JsonConvert.DeserializeObject<List<BowlingFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<Fixture>> GetFootballHeadToHeadFixtures(string fixtureID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/FootballHeadToHead", Method.GET);
                request.AddParameter("fixtureID", fixtureID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<CricketFixture>> GetCricketHeadToHeadFixtures(string fixtureID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/CricketHeadToHead", Method.GET);
                request.AddParameter("fixtureID", fixtureID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<CricketFixture> fixtures = JsonConvert.DeserializeObject<List<CricketFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<TennisFixture>> GetTennisHeadToHeadFixtures(string fixtureID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/TennisHeadToHead", Method.GET);
                request.AddParameter("fixtureID", fixtureID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TennisFixture> fixtures = JsonConvert.DeserializeObject<List<TennisFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<Fixture>> GetHomeTeamFixtures(string fixtureID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/GetHomeFixtures", Method.GET);
                request.AddParameter("fixtureID", fixtureID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<Fixture>> GetAwayTeamFixtures(string fixtureID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/GetAwayFixtures", Method.GET);
                request.AddParameter("fixtureID", fixtureID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<LiveBasketballFixture>> GetLiveBasketballFixtures()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/LiveBasketball", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LiveBasketballFixture> fixtures = JsonConvert.DeserializeObject<List<LiveBasketballFixture>>(response.Content);

                    if(fixtures != null)
                        return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return null;
        }

        public async Task<List<Fixture>> GetLiveFixture()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/Live", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<LiveFixture>> GetLiveCricketFixtures()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/LiveCricket", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LiveFixture> fixtures = JsonConvert.DeserializeObject<List<LiveFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<LiveFixture>> GetLiveFootballFixtures()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/LiveFootball", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LiveFixture> fixtures = JsonConvert.DeserializeObject<List<LiveFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        //Deprecated
        public async Task<List<LiveFixture>> GetLiveFixtures()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/LiveFixtures", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LiveFixture> fixtures = JsonConvert.DeserializeObject<List<LiveFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }
        

        public async Task<List<Fixture>> GetFixturesByTeam(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/GetTeamFixtures", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BasketballFixture>> GetBasketballTeamFixtures(string teamID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/BasketballTeam", Method.GET);
                request.AddParameter("teamID", teamID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BasketballFixture> fixtures = JsonConvert.DeserializeObject<List<BasketballFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BowlingFixture>> GetBowlingTeamFixtures(string teamID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/BowlingTeam", Method.GET);
                request.AddParameter("teamID", teamID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BowlingFixture> fixtures = JsonConvert.DeserializeObject<List<BowlingFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<CricketFixture>> GetCricketTeamFixtures(string teamID)
        {
            try
            {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Fixture/CricketTeam", Method.GET);
                    request.AddParameter("teamID", teamID);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<CricketFixture> fixtures = JsonConvert.DeserializeObject<List<CricketFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<TennisFixture>> GetTennisPlayerFixtures(string playerID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/TennisPlayer", Method.GET);
                request.AddParameter("playerID", playerID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TennisFixture> fixtures = JsonConvert.DeserializeObject<List<TennisFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BasketballFixture>> GetBasketballTeamForm(string teamID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/BasketballTeamForm", Method.GET);
                request.AddParameter("teamID", teamID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BasketballFixture> fixtures = JsonConvert.DeserializeObject<List<BasketballFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BowlingFixture>> GetBowlingTeamForm(string teamID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/BowlingTeamForm", Method.GET);
                request.AddParameter("teamID", teamID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BowlingFixture> fixtures = JsonConvert.DeserializeObject<List<BowlingFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<CricketFixture>> GetCricketTeamForm(string teamID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/CricketTeamForm", Method.GET);
                request.AddParameter("teamID", teamID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<CricketFixture> fixtures = JsonConvert.DeserializeObject<List<CricketFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<TennisFixture>> GetTennisPlayerForm(string playerID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/TennisPlayerForm", Method.GET);
                request.AddParameter("playerID", playerID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TennisFixture> fixtures = JsonConvert.DeserializeObject<List<TennisFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<Fixture>> GetFootballTeamFixtures(string teamID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/FootballTeam", Method.GET);
                request.AddParameter("teamID", teamID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<Fixture>> GetFootballTeamForm(string teamID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/FootballTeamForm", Method.GET);
                request.AddParameter("teamID", teamID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BasketballFixture>> GetBasketballLeagueFixtures(string leagueID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/BasketballLeague", Method.GET);
                request.AddParameter("leagueID", leagueID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BasketballFixture> fixtures = JsonConvert.DeserializeObject<List<BasketballFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<BowlingFixture>> GetBowlingLeagueFixtures(string leagueID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/BowlingLeague", Method.GET);
                request.AddParameter("leagueID", leagueID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BowlingFixture> fixtures = JsonConvert.DeserializeObject<List<BowlingFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<CricketFixture>> GetCricketLeagueFixtures(string leagueID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/CricketLeague", Method.GET);
                request.AddParameter("leagueID", leagueID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<CricketFixture> fixtures = JsonConvert.DeserializeObject<List<CricketFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<Fixture>> GetFootballLeagueFixtures(string leagueID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/FootballLeague", Method.GET);
                request.AddParameter("leagueID", leagueID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> fixtures = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<List<TennisFixture>> GetTennisTournamentFixtures(string tournamentID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Fixture/TennisTournament", Method.GET);
                request.AddParameter("tournamentID", tournamentID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TennisFixture> fixtures = JsonConvert.DeserializeObject<List<TennisFixture>>(response.Content);

                    return fixtures.OrderByDescending(e => e.Date).ThenBy(e => e.Time).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }
            return null;
        }

        public async Task<Fixture> Get(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Fixture/Get", Method.GET);
                    request.AddParameter("id", ID);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        Fixture fixture = JsonConvert.DeserializeObject<Fixture>(response.Content);

                        return fixture;
                    }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");

                return null;
            }
        }

        public async Task<bool> Insert(Fixture fixture)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Fixture/Insert", Method.POST);
                    request.AddJsonBody(fixture);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
                return false;
            }
        }

        public async Task<bool> Update(Fixture fixture)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Fixture/Update", Method.POST);
                    request.AddJsonBody(fixture);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
                return false;
            }
        }
    }
}
