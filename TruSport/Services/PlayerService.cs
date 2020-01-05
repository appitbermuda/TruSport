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
    public class PlayerService
    {
        public PlayerService()
        {
        }

        //public async Task<List<Player>> GetPlayers()
        //{
        //    try
        //    {
        //        //string accessToken = await SecureStorage.GetAsync("Token");

        //        //if (accessToken != null)
        //        //{
        //        var client = new RestClient(Constants.APIEndpoint);
        //        var request = new RestRequest("Player/AllPlayers", Method.GET);
        //        //request.AddHeader("authorization", "Bearer " + accessToken);

        //        // We execute the request and capture the response
        //        // in a variable called `response`
        //        IRestResponse response = await client.ExecuteTaskAsync(request);

        //        if (response.IsSuccessful)
        //        {
        //            List<Player> coaches = JsonConvert.DeserializeObject<List<Player>>(response.Content);

        //            return coaches;
        //        }
        //        //}

        //        //return null;

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "Player");
        //    }
        //    return null;
        //}

        //public async Task<List<Player>> GetTeamPlayers(string TeamID)
        //{
        //    try
        //    {
        //        //string accessToken = await SecureStorage.GetAsync("Token");

        //        //if (accessToken != null)
        //        //{
        //        var client = new RestClient(Constants.APIEndpoint);
        //        var request = new RestRequest("Player/TeamPlayers", Method.GET);
        //        request.AddParameter("teamID", TeamID);
        //        //request.AddHeader("authorization", "Bearer " + accessToken);

        //        // We execute the request and capture the response
        //        // in a variable called `response`
        //        IRestResponse response = await client.ExecuteTaskAsync(request);

        //        if (response.IsSuccessful)
        //        {
        //            List<Player> players = JsonConvert.DeserializeObject<List<Player>>(response.Content);

        //            return players;
        //        }
        //        //}

        //        //return null;

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "Player");
        //    }
        //    return null;
        //}

        public async Task<List<PlayerSeason>> GetPlayers()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("Token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Player/AllPlayers", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<PlayerSeason> players = JsonConvert.DeserializeObject<List<PlayerSeason>>(response.Content);

                    return players;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }
            return null;
        }

        public async Task<List<PlayerSeason>> GetTeamPlayers(string TeamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("Token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Player/TeamPlayers", Method.GET);
                request.AddParameter("teamID", TeamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<PlayerSeason> players = JsonConvert.DeserializeObject<List<PlayerSeason>>(response.Content);

                    return players;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }
            return null;
        }

        public async Task<PlayerSeason> Get(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("Token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Player/GetPlayer", Method.GET);
                    request.AddParameter("id", ID);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        PlayerSeason player = JsonConvert.DeserializeObject<PlayerSeason>(response.Content);

                        return player;
                    }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");

                return null;
            }
        }

        public async Task<bool> Update(PlayerSeason player)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Player/Update", Method.POST);
                    request.AddJsonBody(player);
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
                Debug.WriteLine(ex.Message, "Player");
                return false;
            }
        }

        public async Task<bool> RemovePlayer(PlayerSeason player)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Player/RemovePlayer", Method.POST);
                    request.AddJsonBody(player);
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
                Debug.WriteLine(ex.Message, "Player");
                return false;
            }
        }
    }
}
