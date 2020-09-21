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
    public class OrderService
    {
        public OrderService()
        {
        }

        public async Task<List<Order>> GetMatchDayOrder(string Email)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Order/MatchDay", Method.GET);
                    request.AddParameter("Email", Email);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(response.Content);

                        return orders;
                    }
                }

                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }
            return null;
        }

        public async Task<List<Order>> GetOrderHistory(string Email)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Order/History", Method.GET);
                    request.AddParameter("Email", Email);
                    //request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(response.Content);

                        return orders;
                    }
                }

                return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }
            return null;
        }
    }
}
