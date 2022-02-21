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
    public class CustomerTicketService
    {
        public CustomerTicketService()
        {
        }

        public async Task<List<CustomerTicket>> GetCustomerTickets()
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("CustomerTicket/Tickets", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<CustomerTicket> customerTickets = JsonConvert.DeserializeObject<List<CustomerTicket>>(response.Content);

                        return customerTickets;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
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
                    var request = new RestRequest("CustomerTicket/TransferRequests", Method.GET);
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
                Debug.WriteLine(ex.Message, "CustomerTicket");
            }
            return null;
        }

        public async Task<List<CustomerTicket>> GetTodayCustomerTickets(string teamID)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("CustomerTicket/Today", Method.GET);
                    request.AddParameter("teamID", teamID);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<CustomerTicket> customerTickets = JsonConvert.DeserializeObject<List<CustomerTicket>>(response.Content);

                        return customerTickets;
                    }
                }

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
            }
            return null;
        }

        public async Task<List<CustomerTicket>> GetEventCustomerTickets(string eventID)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("CustomerTicket/Event", Method.GET);
                    request.AddParameter("eventID", eventID);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<CustomerTicket> customerTickets = JsonConvert.DeserializeObject<List<CustomerTicket>>(response.Content);

                        return customerTickets;
                    }
                }

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
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

                    var request = new RestRequest("CustomerTicket/PurchaseTicket", Method.POST);

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

        public async Task<PaymentResponse> ZeroPurchase(PaymentAuthorize payment)
        {
            PaymentResponse paymentResponse = new PaymentResponse();
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);

                    var request = new RestRequest("CustomerTicket/ZeroPurchase", Method.POST);

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

        public async Task<PaymentResponse> PurchaseTest(PaymentAuthorize payment)
        {
            PaymentResponse paymentResponse = new PaymentResponse();
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);

                    var request = new RestRequest("CustomerTicket/PurchaseTest", Method.POST);

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

        public async Task<PaymentResponse> ZeroPurchaseTest(PaymentAuthorize payment)
        {
            PaymentResponse paymentResponse = new PaymentResponse();
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);

                    var request = new RestRequest("CustomerTicket/ZeroPurchaseTest", Method.POST);

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
                    var request = new RestRequest("CustomerTicket/Transfer", Method.POST);
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
                    var request = new RestRequest("CustomerTicket/AcceptTransfer", Method.POST);
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
