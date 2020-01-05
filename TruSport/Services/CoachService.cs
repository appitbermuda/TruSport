using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;

namespace TruSport.Services
{
    public class CoachService
    {
        public CoachService()
        {
        }

        public async Task<List<Coach>> GetCoaches()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{

                Analytics.TrackEvent("getting coaches");
                var client = new RestClient(Constants.APIEndpoint);
                Analytics.TrackEvent("coaches client");
                var request = new RestRequest("Coach/AllCoaches", Method.GET);
                Analytics.TrackEvent("coaches request");
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                Analytics.TrackEvent("coaches response");
                if (response.IsSuccessful)
                    {
                    Analytics.TrackEvent("coaches successful");
                    List<Coach> coaches = JsonConvert.DeserializeObject<List<Coach>>(response.Content);

                        return coaches;
                    }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Coach");
            }
            return null;
        }

        public async Task<List<Coach>> GetTeamCoaches(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Coach/TeamCoaches", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Coach> coaches = JsonConvert.DeserializeObject<List<Coach>>(response.Content);

                    return coaches;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Coach");
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
