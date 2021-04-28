using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;

namespace TruSport.Services
{
    public class TournamentService
    {
        public TournamentService()
        {
        }

        public async Task<List<TennisTournament>> GetTournaments()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Tournament/AllTournaments", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    //request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<TennisTournament> tournaments = JsonConvert.DeserializeObject<List<TennisTournament>>(response.Content);

                        return tournaments;
                    }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tournament");
            }
            return null;
        }

        public async Task<List<TennisTournament>> GetCricketTournaments()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Tournament/Cricket", Method.GET);
                //request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TennisTournament> tournaments = JsonConvert.DeserializeObject<List<TennisTournament>>(response.Content);

                    return tournaments;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tournament");
            }
            return null;
        }

        public async Task<List<TennisTournament>> GetFootballTournaments()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Tournament/Football", Method.GET);
                //request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TennisTournament> tournaments = JsonConvert.DeserializeObject<List<TennisTournament>>(response.Content);

                    return tournaments;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tournament");
            }
            return null;
        }

        public async Task<List<TennisTournament>> GetTournaments(string SportID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{

                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Tournament/GetTournaments", Method.GET);
                request.AddParameter("SportID", SportID);

                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TennisTournament> tournaments = JsonConvert.DeserializeObject<List<TennisTournament>>(response.Content);

                    return tournaments;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tournament");
            }
            return null;
        }

        public async Task<List<TennisTournament>> GetTournamentsBySport(string SportType)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{

                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Tournament/GetTournamentsBySport", Method.GET);
                request.AddParameter("SportType", SportType);

                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TennisTournament> tournaments = JsonConvert.DeserializeObject<List<TennisTournament>>(response.Content);

                    return tournaments;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tournament");
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
