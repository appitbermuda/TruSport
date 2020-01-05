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
    public class TeamService
    {
        public TeamService()
        {
        }

        //public async Task<List<Team>> GetTeams()
        //{
        //    try
        //    {
        //        //string accessToken = await SecureStorage.GetAsync("oauth_token");

        //        //if (accessToken != null)
        //        //{
        //        var client = new RestClient(Constants.APIEndpoint);
        //        var request = new RestRequest("Team/AllTeams", Method.GET);
        //        //request.AddHeader("authorization", "Bearer " + accessToken);

        //        // We execute the request and capture the response
        //        // in a variable called `response`
        //        IRestResponse response = await client.ExecuteTaskAsync(request);

        //        if (response.IsSuccessful)
        //        {
        //            List<Team> coaches = JsonConvert.DeserializeObject<List<Team>>(response.Content);

        //            return coaches;
        //        }
        //        //}

        //        //return null;

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "Team");
        //    }
        //    return null;
        //}

        //public async Task<List<Team>> GetLeagueTeams(string leagueID)
        //{
        //    try
        //    {
        //        //string accessToken = await SecureStorage.GetAsync("oauth_token");

        //        //if (accessToken != null)
        //        //{
        //        var client = new RestClient(Constants.APIEndpoint);
        //        var request = new RestRequest("Team/LeagueTeams", Method.GET);
        //        request.AddParameter("leagueID", leagueID);
        //        //request.AddHeader("authorization", "Bearer " + accessToken);

        //        // We execute the request and capture the response
        //        // in a variable called `response`
        //        IRestResponse response = await client.ExecuteTaskAsync(request);

        //        if (response.IsSuccessful)
        //        {
        //            List<Team> coaches = JsonConvert.DeserializeObject<List<Team>>(response.Content);

        //            return coaches;
        //        }
        //        //}

        //        //return null;

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "Team");
        //    }
        //    return null;
        //}

        public async Task<List<TeamSeason>> GetTeams()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/AllTeams", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TeamSeason> teams = JsonConvert.DeserializeObject<List<TeamSeason>>(response.Content);

                    return teams;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }
            return null;
        }

        public async Task<List<TeamSeason>> GetLeagueTeams(string leagueID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/LeagueTeams", Method.GET);
                request.AddParameter("leagueID", leagueID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<TeamSeason> teams = JsonConvert.DeserializeObject<List<TeamSeason>>(response.Content);

                    return teams;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }
            return null;
        }

        public async Task<TeamSeason> Get(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Team/Get", Method.GET);
                    request.AddParameter("id", ID);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        TeamSeason team = JsonConvert.DeserializeObject<TeamSeason>(response.Content);

                        return team;
                    }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");

                return null;
            }
        }

        public async Task<bool> Update(Team team)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Team/Update", Method.POST);
                    request.AddJsonBody(team);
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
                Debug.WriteLine(ex.Message, "Team");
                return false;
            }
        }
    }
}
