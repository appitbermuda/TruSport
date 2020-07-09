using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;

namespace TruSport.Services
{
    public class LeagueStatService
    {
        public LeagueStatService()
        {
        }

        public async Task<List<LeagueStat>> GetGoalsConcededByTeam()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueStat/GoalsConcededByTeam", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueStat> stats = JsonConvert.DeserializeObject<List<LeagueStat>>(response.Content);

                    return stats;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }
            return null;
        }

        public async Task<List<LeagueStat>> GetGoalsScoredByTeam()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueStat/GoalsScoredByTeam", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueStat> stats = JsonConvert.DeserializeObject<List<LeagueStat>>(response.Content);

                    return stats;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }
            return null;
        }

        public async Task<List<LeagueStat>> GetGoalsScoredByPlayer()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueStat/GoalsScoredByPlayer", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueStat> stats = JsonConvert.DeserializeObject<List<LeagueStat>>(response.Content);

                    return stats;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }
            return null;
        }

        public async Task<List<LeagueStat>> GetMostRunsByPlayer()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueStat/RunsByPlayer", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueStat> stats = JsonConvert.DeserializeObject<List<LeagueStat>>(response.Content);

                    return stats;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }
            return null;
        }

        public async Task<List<LeagueStat>> GetMostWicketsByPlayer()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueStat/WicketsByPlayer", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueStat> stats = JsonConvert.DeserializeObject<List<LeagueStat>>(response.Content);

                    return stats;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }
            return null;
        }

        public async Task<List<LeagueStat>> GetGoalsScoredByPlayerByTeam(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueStat/GoalsScoredByPlayerByTeam", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueStat> stats = JsonConvert.DeserializeObject<List<LeagueStat>>(response.Content);

                    return stats;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }
            return null;
        }

        public async Task<List<LeagueStat>> GetGoalsConcededByTeamByLeague(string leagueID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueStat/GoalsConcededByTeamByLeague", Method.GET);
                request.AddParameter("leagueID", leagueID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueStat> stats = JsonConvert.DeserializeObject<List<LeagueStat>>(response.Content);

                    return stats;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }
            return null;
        }

        public async Task<List<LeagueStat>> GetGoalsScoredByTeamByLeague(string leagueID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("LeagueStat/GoalsScoredByTeamByLeague", Method.GET);
                    request.AddParameter("leagueID", leagueID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<LeagueStat> stats = JsonConvert.DeserializeObject<List<LeagueStat>>(response.Content);

                        return stats;
                    }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }
            return null;
        }

        public async Task<LeagueStat> Get(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("LeagueStat/Get", Method.GET);
                    request.AddParameter("id", ID);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        LeagueStat fixture = JsonConvert.DeserializeObject<LeagueStat>(response.Content);

                        return fixture;
                    }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");

                return null;
            }
        }
    }
}
