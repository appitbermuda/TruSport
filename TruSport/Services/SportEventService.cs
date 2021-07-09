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
    public class SportEventService
    {
        public SportEventService()
        {
        }

        public async Task<List<SportEvent>> GetSportEvents()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("Token");

                //if (accessToken != null)
                //{
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("SportEvent/All", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    //request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<SportEvent> matchTickets = JsonConvert.DeserializeObject<List<SportEvent>>(response.Content);

                        return matchTickets;
                    }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }
            return null;
        }

        public async Task<List<SportEvent>> GetTickets()
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("Token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("SportEvent/Tickets", Method.GET);
                //request.AddParameter("teamID", teamID);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<SportEvent> sportEvents = JsonConvert.DeserializeObject<List<SportEvent>>(response.Content);

                    return sportEvents;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }
            return null;
        }

        public async Task<List<EventTicket>> GetSportEventTicket(string eventID, string email)
        {
            try
            {
                //string accessToken = await SecureStorage.GetAsync("Token");

                //if (accessToken != null)
                //{
                var client = new RestClient(Constants.APIEndpoint);
                var request = new RestRequest("SportEvent/Ticket", Method.GET);
                request.AddParameter("eventID", eventID);
                request.AddParameter("email", email);
                //request.AddHeader("authorization", "Bearer " + accessToken);

                // We execute the request and capture the response
                // in a variable called `response`
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.IsSuccessful)
                {
                    List<EventTicket> eventTickets = JsonConvert.DeserializeObject<List<EventTicket>>(response.Content);

                    return eventTickets;
                }
                //}

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }
            return null;
        }

        public async Task<List<EventTicket>> GetTeamSportEvents()
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("SportEvent/Team", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    //request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<EventTicket> eventTickets = JsonConvert.DeserializeObject<List<EventTicket>>(response.Content);

                        return eventTickets;
                    }
                }

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }
            return null;
        }

        public async Task<List<EventTicket>> GetTodayTeamSportEvents()
        {
            try
            {
                string accessToken = await SecureStorage.GetAsync("Token");

                if (accessToken != null)
                {
                    var client = new RestClient(Constants.APIEndpoint);
                    var request = new RestRequest("SportEvent/TodayByTeam", Method.GET);
                    //request.AddParameter("teamID", teamID);
                    //request.AddHeader("authorization", "Bearer " + accessToken);

                    // We execute the request and capture the response
                    // in a variable called `response`
                    IRestResponse response = await client.ExecuteTaskAsync(request);

                    if (response.IsSuccessful)
                    {
                        List<EventTicket> matchTickets = JsonConvert.DeserializeObject<List<EventTicket>>(response.Content);

                        return matchTickets;
                    }
                }

                //return null;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }
            return null;
        }

    }
}
