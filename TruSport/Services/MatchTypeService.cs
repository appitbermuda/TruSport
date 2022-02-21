using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;

namespace TruSport.Services
{
    public class MatchTypeService
    {
        public MatchTypeService()
        {
        }

        public async Task<List<MatchType>> GetMatchTypes()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("MatchType/AllMatchTypes", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    //request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<MatchType> matchTypes = JsonConvert.DeserializeObject<List<MatchType>>(response.Content);

                        return matchTypes;
                    }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchType");
            }
            return null;
        }
    }
}
