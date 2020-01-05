using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;

namespace TruSport.Services
{
    public class UserTypeService
    {
        public UserTypeService()
        {
        }

        public async Task<List<UserType>> GetUserTypes()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("UserType/AllUserTypes", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    //request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<UserType> UserTypes = JsonConvert.DeserializeObject<List<UserType>>(response.Content);

                        return UserTypes;
                    }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "UserType");
            }
            return null;
        }

        //public async Task<Player> Get(string ID)
        //{
        //    try
        //    {
        //        string accessToken = await SecureStorage.GetAsync("oauth_token");

        //        if (accessToken != null)
        //        {
        //            var client = new RestClient(Constants.APIEndpoint);
        //            var request = new RestRequest("Player/Get", Method.GET);
        //            request.AddHeader("authorization", "Bearer " + accessToken);
        //            request.AddParameter("id", ID);

        //            // We execute the request and capture the response
        //            // in a variable called `response`
        //            IRestResponse response = await client.ExecuteTaskAsync(request);

        //            if (response.IsSuccessful)
        //            {
        //                Player stop = JsonConvert.DeserializeObject<Player>(response.Content);

        //                return stop;
        //            }
        //        }

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "Player");

        //        return null;
        //    }
        //}
    }
}
