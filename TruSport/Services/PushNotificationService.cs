using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
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

        public async Task<List<PushNotification>> GetPushNotifications()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);

                var request = new RestRequest("PushNotification/AllNotifications", Method.GET);

                IRestResponse response = await client.ExecuteTaskAsync(request);


                if (response.IsSuccessful)
                {
                    List<PushNotification> pushNotifications = JsonConvert.DeserializeObject<List<PushNotification>>(response.Content);

                    return pushNotifications;
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "PushNotifications");
            }
            return null;
        }

        public async Task<PushNotification> Get(string ID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("PushNotification/Get", Method.GET);
                request.AddParameter("id", ID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    PushNotification pushNotification = JsonConvert.DeserializeObject<PushNotification>(response.Content);

                    return pushNotification;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "PushNotification");

                return null;
            }
        }
    }
}
