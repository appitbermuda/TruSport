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
    public class FixtureProductService
    {
        public FixtureProductService()
        {
        }

        public async Task<List<FixtureProduct>> GetFixtureProducts()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("FixtureProduct/All", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    //request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<FixtureProduct> matchTickets = JsonConvert.DeserializeObject<List<FixtureProduct>>(response.Content);

                        return matchTickets;
                    }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }
            return null;
        }

        public async Task<List<Fixture>> GetProducts()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("FixtureProduct/Products", Method.GET);
                //request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Fixture> matchTickets = JsonConvert.DeserializeObject<List<Fixture>>(response.Content);

                    return matchTickets;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }
            return null;
        }

        public async Task<List<FixtureProduct>> GetFixtureFixtureProducts(string fixtureID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("FixtureProduct/Fixture", Method.GET);
                request.AddParameter("fixtureID", fixtureID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<FixtureProduct> matchTickets = JsonConvert.DeserializeObject<List<FixtureProduct>>(response.Content);

                    return matchTickets;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }
            return null;
        }

        public async Task<List<FixtureProduct>> GetTeamFixtureProducts(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("FixtureProduct/Team", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<FixtureProduct> matchTickets = JsonConvert.DeserializeObject<List<FixtureProduct>>(response.Content);

                    return matchTickets;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }
            return null;
        }

        public async Task<List<FixtureProduct>> GetTodayTeamFixtureProducts(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("FixtureProduct/TodayByTeam", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<FixtureProduct> matchTickets = JsonConvert.DeserializeObject<List<FixtureProduct>>(response.Content);

                    return matchTickets;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }
            return null;
        }

    }
}
