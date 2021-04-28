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
    public class MatchTicketService
    {
        public MatchTicketService()
        {
        }

        public async Task<List<MatchTicket>> GetMatchTickets()
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("MatchTicket/Customer", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<MatchTicket> matchTickets = JsonConvert.DeserializeObject<List<MatchTicket>>(response.Content);

                        return matchTickets;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }
            return null;
        }

        public async Task<List<AcceptTransfer>> GetTransferRequests()
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("MatchTicket/TransferRequests", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<AcceptTransfer> transfers = JsonConvert.DeserializeObject<List<AcceptTransfer>>(response.Content);

                        return transfers;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }
            return null;
        }

        public async Task<List<MatchTicket>> GetTodayMatchTickets(string teamID)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("MatchTicket/Today", Method.GET);
                    request.AddParameter("teamID", teamID);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<MatchTicket> matchTickets = JsonConvert.DeserializeObject<List<MatchTicket>>(response.Content);

                        return matchTickets;
                    }
                }

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }
            return null;
        }

        public async Task<List<MatchTicket>> GetFixtureMatchTickets(string fixtureID)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("MatchTicket/Fixture", Method.GET);
                    request.AddParameter("fixtureID", fixtureID);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<MatchTicket> matchTickets = JsonConvert.DeserializeObject<List<MatchTicket>>(response.Content);

                        return matchTickets;
                    }
                }

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }
            return null;
        }

        public async Task<PaymentResponse> Purchase(PaymentAuthorize payment)
        {
            PaymentResponse paymentResponse = new PaymentResponse();
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);

//#if RELEASE
//                    var request = new RestRequest("MatchTicket/Purchase", Method.POST);
//#else
                    var request = new RestRequest("MatchTicket/PurchaseTest", Method.POST);
//#endif

                    request.AddJsonBody(payment);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(response.Content);

                        return paymentResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";
            paymentResponse.IsApproved = false;
            return paymentResponse;
        }

        public async Task<string> Transfer(TransferRequest transferRequest)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("MatchTicket/Transfer", Method.POST);
                    request.AddJsonBody(transferRequest);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        string transferred = JsonConvert.DeserializeObject<string>(response.Content);

                        return transferred;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }

            return "There was an error processing your request.";
        }

        public async Task<string> AcceptTransfer(AcceptTransfer accept)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("MatchTicket/AcceptTransfer", Method.POST);
                    request.AddJsonBody(accept);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        string accepted = JsonConvert.DeserializeObject<string>(response.Content);

                        return accepted;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }

            return "There was an issue accepting the transfer request.";
        }
    }
}
