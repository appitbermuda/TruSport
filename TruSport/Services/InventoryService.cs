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
    public class InventoryService
    {
        public InventoryService()
        {
        }

        public async Task<List<Inventory>> GetInventorys()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Inventory/AllInventorys", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    //request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<Inventory> inventorys = JsonConvert.DeserializeObject<List<Inventory>>(response.Content);

                        return inventorys;
                    }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Inventory");
            }
            return null;
        }

        public async Task<bool> CheckInventory(string FixtureID)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Inventory/Check", Method.GET);
                    //request.AddHeader("authorization", "Bearer " + accessToken);
                    request.AddParameter("FixtureID", FixtureID);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        bool hasStock = JsonConvert.DeserializeObject<bool>(response.Content);

                        return hasStock;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");

            }


            return false;
        }

        public async Task<int> TicketInventory(string FixtureID)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Inventory/Ticket", Method.GET);
                    //request.AddHeader("authorization", "Bearer " + accessToken);
                    request.AddParameter("FixtureID", FixtureID);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        int stock = JsonConvert.DeserializeObject<int>(response.Content);

                        return stock;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");

            }


            return 0;
        }
    }
}
