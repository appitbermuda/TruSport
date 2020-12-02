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


        public async Task<string> Send(Content notification)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("PushNotification/Send", Method.POST);
                    request.AddJsonBody(notification);
                    request.AddHeader("authorization", "Bearer " + accessToken);

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

        public async Task Send(NotificationRequest notificationRequest)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("PushNotification/requests", Method.POST);
                    request.AddJsonBody(notificationRequest);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        string push = JsonConvert.DeserializeObject<string>(response.Content);

                        
                    }
                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Push");                
            }
        }
    }
}
