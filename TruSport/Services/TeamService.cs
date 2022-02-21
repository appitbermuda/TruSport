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

        public async Task<List<TeamSeason>> GetSportTeams(string SportID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/SportTeams", Method.GET);
                request.AddParameter("SportID", SportID);
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

        public async Task<List<Team>> GetBasketballTeams()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/AllBasketball", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(response.Content);

                    return teams;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }
            return null;
        }

        public async Task<List<Team>> GetBowlingTeams()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/AllBowling", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(response.Content);

                    return teams;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }
            return null;
        }

        public async Task<List<Team>> GetCricketTeams()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/AllCricket", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(response.Content);

                    return teams;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }
            return null;
        }

        public async Task<List<Team>> GetFootballTeams()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/AllFootball", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(response.Content);

                    return teams;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }
            return null;
        }

        public async Task<List<TeamSeason>> GetTeamsBySportType(string SportType)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/GetTeamsBySport", Method.GET);
                request.AddParameter("SportType", SportType);
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

        public async Task<TeamSeason> GetFootballTeam(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/Football", Method.GET);
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

        public async Task<TeamSeason> GetCricketTeam(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/Cricket", Method.GET);
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

        public async Task<TeamSeason> GetBasketballTeam(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/Basketball", Method.GET);
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

        public async Task<TeamSeason> GetBowlingTeam(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/Bowling", Method.GET);
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

        public async Task<Team> GetBasketballProfile(string ID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/BasketballProfile", Method.GET);
                request.AddParameter("id", ID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    Team team = JsonConvert.DeserializeObject<Team>(response.Content);

                    return team;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");

                return null;
            }
        }



        public async Task<Team> GetBowlingProfile(string ID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/BowlingProfile", Method.GET);
                request.AddParameter("id", ID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    Team team = JsonConvert.DeserializeObject<Team>(response.Content);

                    return team;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");

                return null;
            }
        }

        public async Task<Team> GetCricketProfile(string ID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/CricketProfile", Method.GET);
                request.AddParameter("id", ID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    Team team = JsonConvert.DeserializeObject<Team>(response.Content);

                    return team;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");

                return null;
            }
        }

        public async Task<Team> GetFootballProfile(string ID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Team/FootballProfile", Method.GET);
                request.AddParameter("id", ID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    Team team = JsonConvert.DeserializeObject<Team>(response.Content);

                    return team;
                }

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
