using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;

namespace TruSport.Services
{
    public class LeagueTableService
    {
        public LeagueTableService()
        {
        }

        public async Task<List<LeagueTable>> GetPremierLeagueTables()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/PremierLeagueTable", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueTable> table = JsonConvert.DeserializeObject<List<LeagueTable>>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<LeagueTable>> GetFirstDivisionTables()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/FirstDivisionTable", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueTable> table = JsonConvert.DeserializeObject<List<LeagueTable>>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<BasketballLeagueStanding>> GetIslandFallBasketballLeagueStandings()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/IslandFallBasketballLeague", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BasketballLeagueStanding> table = JsonConvert.DeserializeObject<List<BasketballLeagueStanding>>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<BowlingLeagueStanding>> GetSomersbyLeagueBowlingTables()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/BowlingLeagueStanding", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BowlingLeagueStanding> table = JsonConvert.DeserializeObject<List<BowlingLeagueStanding>>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }


        public async Task<List<CricketLeagueTable>> GetPremierLeagueCricketTables()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/CricketPremierDivision", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<CricketLeagueTable> table = JsonConvert.DeserializeObject<List<CricketLeagueTable>>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<CricketLeagueTable>> GetFirstDivisionCricketTables()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/CricketFirstDivision", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<CricketLeagueTable> table = JsonConvert.DeserializeObject<List<CricketLeagueTable>>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<LeagueTable>> GetCoronaLeagueTables()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/CoronaLeagueTable", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueTable> table = JsonConvert.DeserializeObject<List<LeagueTable>>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<LeagueTable> GetPremierLeagueTableByTeam(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/PremierLeagueTableByTeam", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    LeagueTable table = JsonConvert.DeserializeObject<LeagueTable>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<LeagueTable> GetFirstDivisionTableByTeam(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/FirstDivisionTableByTeam", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    LeagueTable table = JsonConvert.DeserializeObject<LeagueTable>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<CricketLeagueTable> GetCricketPremierLeagueTableByTeam(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/CricketPremierLeagueTableByTeam", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    CricketLeagueTable table = JsonConvert.DeserializeObject<CricketLeagueTable>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<CricketLeagueTable> GetCricketFirstDivisionTableByTeam(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/CricketFirstDivisionTableByTeam", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    CricketLeagueTable table = JsonConvert.DeserializeObject<CricketLeagueTable>(response.Content);

                    return table;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<BasketballLeagueStanding>> GetBasketballTeamLeagueTable(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/BasketballTeam", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BasketballLeagueStanding> coaches = JsonConvert.DeserializeObject<List<BasketballLeagueStanding>>(response.Content);

                    return coaches;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<LeagueTable>> GetFootballTeamLeagueTable(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("LeagueTable/FootballTeam", Method.GET);
                    request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<LeagueTable> coaches = JsonConvert.DeserializeObject<List<LeagueTable>>(response.Content);

                        return coaches;
                    }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<CricketLeagueTable>> GetCricketTeamLeagueTable(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/CricketTeam", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<CricketLeagueTable> coaches = JsonConvert.DeserializeObject<List<CricketLeagueTable>>(response.Content);

                    return coaches;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<BowlingLeagueStanding>> GetBowlingTeamLeagueTable(string teamID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/BowlingTeam", Method.GET);
                request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BowlingLeagueStanding> tables = JsonConvert.DeserializeObject<List<BowlingLeagueStanding>>(response.Content);

                    return tables;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        //Deprecated
        public async Task<List<LeagueTable>> GetTeamLeagueTables(string teamID, string leagueID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/TeamTable", Method.GET);
                request.AddParameter("teamID", teamID);
                request.AddParameter("leagueID", leagueID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueTable> coaches = JsonConvert.DeserializeObject<List<LeagueTable>>(response.Content);

                    return coaches;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<LeagueTable>> GetLeagueTableByLeague(string leagueID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/LeagueTableByLeague", Method.GET);
                request.AddParameter("leagueID", leagueID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<LeagueTable> coaches = JsonConvert.DeserializeObject<List<LeagueTable>>(response.Content);

                    return coaches;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<BasketballLeagueStanding>> GetBasketballLeagueTableByLeague(string leagueID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/BasketballLeagueTableByLeague", Method.GET);
                request.AddParameter("leagueID", leagueID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<BasketballLeagueStanding> coaches = JsonConvert.DeserializeObject<List<BasketballLeagueStanding>>(response.Content);

                    return coaches;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<List<CricketLeagueTable>> GetCricketLeagueTableByLeague(string leagueID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("LeagueTable/CricketLeagueTableByLeague", Method.GET);
                request.AddParameter("leagueID", leagueID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<CricketLeagueTable> coaches = JsonConvert.DeserializeObject<List<CricketLeagueTable>>(response.Content);

                    return coaches;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }
            return null;
        }

        public async Task<LeagueTable> Get(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("LeagueTable/Get", Method.GET);
                    request.AddParameter("id", ID);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        LeagueTable fixture = JsonConvert.DeserializeObject<LeagueTable>(response.Content);

                        return fixture;
                    }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");

                return null;
            }
        }
    }
}
