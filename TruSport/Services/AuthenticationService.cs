using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using RestSharp;
using TruSport.Model;
using Xamarin.Essentials;

namespace TruSport.Services
{
    public class AuthenticationService
    {
        public async Task<Customer> SignIn(CustomerAuthentication customerAuthentication)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Customer/SignIn", Method.POST);
                request.AddJsonBody(customerAuthentication);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    Customer Customer = JsonConvert.DeserializeObject<Customer>(response.Content);

                    return Customer;
                }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");

                return null;
            }
        }

        public async Task<Customer> SignUp(CustomerRequest customerRequest)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Customer/SignUp", Method.POST);
                request.AddJsonBody(customerRequest);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    Customer Customer = JsonConvert.DeserializeObject<Customer>(response.Content);

                    return Customer;
                }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");

                return null;
            }
        }

        public async Task<bool> CustomerExists(string email)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Customer/CustomerExists", Method.GET);
                request.AddParameter("email", email);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    bool customerExists = JsonConvert.DeserializeObject<bool>(response.Content);

                    return customerExists;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return false;
        }

        public async Task<bool> Update(Customer customer)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("Customer/Update", Method.POST);
                    request.AddJsonBody(customer);
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
                Debug.WriteLine(ex.Message, "Customer");
                return false;
            }
        }

        public async Task<bool> HasTemporaryPassword(string email)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Customer/HasTemporaryPassword", Method.GET);
                request.AddParameter("email", email);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    bool userExist = JsonConvert.DeserializeObject<bool>(response.Content);

                    return userExist;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");

                return false;
            }
        }

        public async Task<bool> ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Customer/ForgotPassword", Method.POST);
                request.AddJsonBody(forgotPassword);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
                return false;
            }
        }

        public async Task<string> ResetPassword(PasswordReset passwordReset)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("Customer/ResetPassword", Method.POST);
                request.AddJsonBody(passwordReset);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    string resetPassword = JsonConvert.DeserializeObject<string>(response.Content);

                    return resetPassword;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
                return null;
            }
        }
    }
}
