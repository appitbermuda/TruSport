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
    public class FlyerService
    {
        public FlyerService()
        {
        }

        public async Task<List<Flyer>> GetFlyers()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                
                var request = new RestRequest("Flyer/AllFlyers", Method.GET);

                IRestResponse response = await client.ExecuteTaskAsync(request);

                
                if (response.IsSuccessful)
                {
                    List<Flyer> flyers = JsonConvert.DeserializeObject<List<Flyer>>(response.Content);

                    return flyers;
                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message, "Flyer");
            }
            return null;
        }

        public async Task<Flyer> Get(string ID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Flyer/Get", Method.GET);
                request.AddParameter("id", ID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    Flyer flyer = JsonConvert.DeserializeObject<Flyer>(response.Content);

                    return flyer;
                }
                
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Flyer");

                return null;
            }
        }
    }
}
