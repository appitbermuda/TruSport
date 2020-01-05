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
    public class PushNotificationService
    {
        public PushNotificationService()
        {
        }


        public async Task<string> Send(string name, string title, string body)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("oauth_token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("PushNotification/Send", Method.GET);
                    request.AddHeader("authorization", "Bearer " + accessToken);
                    request.AddParameter("name", name);
                    request.AddParameter("title", title);
                    request.AddParameter("body", body);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        string push = JsonConvert.DeserializeObject<string>(response.Content);

                        return push;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Push");

                return null;
            }
        }
    }
}
