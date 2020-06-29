using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;

namespace TruSport.Services
{
    public class SettingService
    {
        public SettingService()
        {
        }

        public async Task<SportFeature> GetHomeMobileFeatureImages()
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Setting/HomeMobileFeatureImages", Method.GET);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    SportFeature sportMenu = JsonConvert.DeserializeObject<SportFeature>(response.Content);

                    return sportMenu;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Setting");

                return null;
            }
        }

        public async Task<Setting> Get(string ID)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Setting/Get", Method.GET);
                request.AddParameter("id", ID);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                Setting setting = JsonConvert.DeserializeObject<Setting>(response.Content);

                return setting;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Setting");

                return null;
            }
        }
    }
}
