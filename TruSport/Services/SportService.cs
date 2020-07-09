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
    public class SportService
    {
        public SportService()
        {
        }

        public async Task<List<Sport>> GetSports()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Sport/AllSports", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<Sport> sports = JsonConvert.DeserializeObject<List<Sport>>(response.Content);
                    return sports;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport");
            }
            return null;
        }
    }
}
