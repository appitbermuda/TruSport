using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;

namespace TruSport.Services
{
    public class TransferService
    {
        public TransferService()
        {
        }

        public async Task<List<Transfer>> GetTransfers()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Transfer/AllTransfers", Method.GET);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Transfer> transfers = JsonConvert.DeserializeObject<List<Transfer>>(response.Content);

                    return transfers;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }
            return null;
        }

        public async Task<List<Transfer>> GetLeagueTransfers(string leagueID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Transfer/LeagueTransfers", Method.GET);
                request.AddParameter("leagueID", leagueID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Transfer> transfers = JsonConvert.DeserializeObject<List<Transfer>>(response.Content);

                    return transfers;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }
            return null;
        }

        public async Task<Transfer> Get(string ID)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Transfer/Get", Method.GET);
                    request.AddParameter("id", ID);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        Transfer transfer = JsonConvert.DeserializeObject<Transfer>(response.Content);

                        return transfer;
                    }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");

                return null;
            }
        }
    }
}
