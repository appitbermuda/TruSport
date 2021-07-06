using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;

namespace TruSport.Services
{
    public class AdService
    {
        public AdService()
        {
        }

        public async Task<List<Ad>> GetAds()
        {
            try
            {
                
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Ad/Retrieve", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Ad> ads = JsonConvert.DeserializeObject<List<Ad>>(response.Content);

                    return ads;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Ad");
            }

            return null;
        }

        public async Task Impressions(string AdID)
        {
            try
            {

                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Ad/Impression", Method.GET);
                request.AddParameter("id", AdID);
                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Ad");
            }
        }
    }
}
