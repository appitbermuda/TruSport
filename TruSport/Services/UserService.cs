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
    public class UserService
    {
        public UserService()
        {
        }

        public async Task<List<User>> GetAll()
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("User/AllUsers", Method.GET);
                    request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<User> users = JsonConvert.DeserializeObject<List<User>>(response.Content);

                        return users;
                    }
                }

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return null;
        }

        public async Task<bool> UserExists(string email)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("User/UserExists", Method.GET);
                request.AddParameter("email", email);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    bool userExists = JsonConvert.DeserializeObject<bool>(response.Content);

                    return userExists;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return false;
        }

        public async Task<User> SignIn(UserAuthentication userAuthentication)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("User/SignIn", Method.POST);
                request.AddJsonBody(userAuthentication);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    User User = JsonConvert.DeserializeObject<User>(response.Content);

                    return User;
                }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");

                return null;
            }
        }

        public async Task<User> SignUp(UserRequest userRequest)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("oauth_token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("User/SignUp", Method.POST);
                request.AddJsonBody(userRequest);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    User User = JsonConvert.DeserializeObject<User>(response.Content);

                    return User;
                }
                //}

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");

                return null;
            }
        }

        public async Task<bool> Update(User user)
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("User/Update", Method.POST);
                    request.AddJsonBody(user);
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
                Debug.WriteLine(ex.Message, "User");
                return false;
            }
        }

        public async Task<bool> ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("User/ForgotPassword", Method.POST);
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
                Debug.WriteLine(ex.Message, "User");
                return false;
            }
        }

        public async Task<string> ResetPassword(PasswordReset passwordReset)
        {
            try
            {
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("User/ResetPassword", Method.POST);
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
                Debug.WriteLine(ex.Message, "User");
                return null;
            }
        }
    }
}
