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
    public class AwardService
    {
        public AwardService()
        {
        }

        public async Task<List<Award>> GetAwards()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("Token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/GetAll", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetSportAwards(string SportID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("Token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/SportAwards", Method.GET);
                request.AddParameter("SportID", SportID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetAwardsBySportType(string SportType)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("Token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/AwardsBySport", Method.GET);
                request.AddParameter("SportType", SportType);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetBowlingPlayerOfTheWeek()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/BowlingPlayerOfTheWeek", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetCricketPlayerOfTheWeek()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/CricketPlayerOfTheWeek", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetTennisPlayerOfTheWeek()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/TennisPlayerOfTheWeek", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetFootballPlayerOfTheWeek()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/FootballPlayerOfTheWeek", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetCricketPlayerOfTheMonth()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/CricketPlayerOfTheMonth", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetTennisPlayerOfTheMonth()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/TennisPlayerOfTheMonth", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetFootballPlayerOfTheMonth()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/FootballPlayerOfTheMonth", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetCricketPlayerOfTheYear()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/CricketPlayerOfTheYear", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetTennisPlayerOfTheYear()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/TennisPlayerOfTheYear", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<List<Award>> GetFootballPlayerOfTheYear()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Award/FootballPlayerOfTheYear", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Award> playerAwards = JsonConvert.DeserializeObject<List<Award>>(response.Content);

                    return playerAwards;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }
            return null;
        }

        public async Task<Award> Get(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("Token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Award/Get", Method.GET);
                    request.AddParameter("id", ID);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        Award playerAward = JsonConvert.DeserializeObject<Award>(response.Content);

                        return playerAward;
                    }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");

                return null;
            }
        }

        public async Task<bool> Insert(Award playerAward)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Award/Insert", Method.POST);
                    request.AddJsonBody(playerAward);
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
                Debug.WriteLine(ex.Message, "Award");
                return false;
            }
        }

        public async Task<bool> Update(Award playerAward)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Award/Update", Method.POST);
                    request.AddJsonBody(playerAward);
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
                Debug.WriteLine(ex.Message, "Award");
                return false;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Award/Delete", Method.DELETE);
                    request.AddParameter("id", id);
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
                Debug.WriteLine(ex.Message, "Award");
                return false;
            }
        }
    }
}
