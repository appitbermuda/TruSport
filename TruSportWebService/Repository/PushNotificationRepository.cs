using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class PushNotificationRepository : IPushNotificationRepository<Push>
    {
        OnTrackContext _context;

        public PushNotificationRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Send(string name, string title, string body)
        {
            // let's assume you have a User object that contains 
            // * iOS Devices 
            // * Android Devices
            var push = new Push
            {
                Content = new Content
                {
                    Name = name,
                    Title = title,
                    Body = body
                },
                Target = null
            };

            try
            {

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(Constants.ApiKeyName, Constants.ApiKey);


                var json = JsonConvert.SerializeObject(push);
                HttpContent content = new StringContent(json);

                //content.Headers
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                //if (user.IOSDevices.Any())
                //{
                //push.Target.Devices = user.IOSDevices;
                await client.PostAsync($"{Constants.Url}{Constants.Organization}/{Constants.IOS}/{Constants.Apis.Notification}", content);
                //}

                //if (user.AndroidDevices.Any())
                //{
                //push.Target.Devices = user.AndroidDevices;
                await client.PostAsync($"{Constants.Url}{Constants.Organization}/{Constants.Android}/{Constants.Apis.Notification}", content);
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return "Notification sent successfully!";
        }
    }
}
