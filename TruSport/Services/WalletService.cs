using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;
using TruSport.Model.Ticket;
using Xamarin.Essentials;

namespace TruSport.Services
{
    public class WalletService
    {
        public WalletService()
        {
        }

        public async Task<List<Wallet>> GetWallet()
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {

                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Wallet/All", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<Wallet> cards = JsonConvert.DeserializeObject<List<Wallet>>(response.Content);

                        return cards;
                    }
                }

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
            }
            return null;
        }

        public async Task<Wallet> GetCard(string ID)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {

                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Wallet/Get", Method.GET);
                    request.AddParameter("ID", ID);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        Wallet card = JsonConvert.DeserializeObject<Wallet>(response.Content);

                        return card;
                    }
                }

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
            }
            return null;
        }

        public async Task<bool> Insert(Wallet wallet)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Wallet/Insert", Method.POST);
                    request.AddJsonBody(wallet);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
                return false;
            }
        }

        public async Task<bool> Update(Wallet wallet)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Wallet/Update", Method.POST);
                    request.AddJsonBody(wallet);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
                return false;
            }
        }

        public async Task<bool> Delete(string ID)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Wallet/Delete", Method.DELETE);
                    request.AddParameter("ID", ID);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
                return false;
            }
        }
    }
}
