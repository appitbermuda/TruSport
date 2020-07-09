using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;

namespace TruSport.Services
{
    public class LeagueService
    {
        public LeagueService()
        {
        }

        public async Task<List<League>> GetLeagues()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("League/AllLeagues", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    //request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<League> leagues = JsonConvert.DeserializeObject<List<League>>(response.Content);

                        return leagues;
                    }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }
            return null;
        }

        public async Task<List<League>> GetAllLeagues()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("League/GetLeagues", Method.GET);
                //request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<League> leagues = JsonConvert.DeserializeObject<List<League>>(response.Content);

                    return leagues;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }
            return null;
        }

        public async Task<List<League>> GetCricketLeagues()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("League/Cricket", Method.GET);
                //request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<League> leagues = JsonConvert.DeserializeObject<List<League>>(response.Content);

                    return leagues;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }
            return null;
        }

        public async Task<List<League>> GetFootballLeagues()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("League/Football", Method.GET);
                //request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<League> leagues = JsonConvert.DeserializeObject<List<League>>(response.Content);

                    return leagues;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }
            return null;
        }

        public async Task<List<League>> GetLeagues(string SportID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{

                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("League/GetLeagues", Method.GET);
                request.AddParameter("SportID", SportID);

                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<League> leagues = JsonConvert.DeserializeObject<List<League>>(response.Content);

                    return leagues;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }
            return null;
        }

        public async Task<List<League>> GetLeaguesBySport(string SportType)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{

                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("League/GetLeaguesBySport", Method.GET);
                request.AddParameter("SportType", SportType);

                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<League> leagues = JsonConvert.DeserializeObject<List<League>>(response.Content);

                    return leagues;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }
            return null;
        }

        //public async Task<Player> Get(string ID)
        //{
        //    try
        //    {
        //        string accessToken = await SecureStorage.GetAsync("oauth_token");

        //        if (accessToken != null)
        //        {
        //            var client = new RestClient(Constants.APIEndpoint);
        //            var request = new RestRequest("Player/Get", Method.GET);
        //            request.AddHeader("authorization", "Bearer " + accessToken);
        //            request.AddParameter("id", ID);

        //            // We execute the request and capture the response
        //            // in a variable called `response`
        //            IRestResponse response = await client.ExecuteTaskAsync(request);

        //            if (response.IsSuccessful)
        //            {
        //                Player stop = JsonConvert.DeserializeObject<Player>(response.Content);

        //                return stop;
        //            }
        //        }

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "Player");

        //        return null;
        //    }
        //}
    }
}
