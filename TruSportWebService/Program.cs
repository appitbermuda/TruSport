using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnTrackWebService.Models;

namespace OnTrackWebService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost =  CreateWebHostBuilder(args).Build();

            //// We create a dedicate background thread that will be running alongside the web server.
            //Thread counterBackgroundWorkerThread = new Thread(CounterHandlerAsync)
            //{
            //    IsBackground = true
            //};

            //// We start the background thread, providing it with the webHost.Service so that we can benefit from dependency injection.
            //counterBackgroundWorkerThread.Start(webHost.Services);

            webHost.Run(); // At this point, we're running the server as normal.
        }

        private static void CounterHandlerAsync(object obj)
        {
            // Here we check that the provided parameter is, in fact, an IServiceProvider
            IServiceProvider provider = obj as IServiceProvider
            ?? throw new ArgumentException($"Passed in thread parameter was not of type {nameof(IServiceProvider)}", nameof(obj));

            // Using an infinite loop for this demonstration but it all depends on the work you want to do.
            while (true)
            {
                // Here we create a new scope for the IServiceProvider so that we can get already built objects from the Inversion Of Control Container.
                using (IServiceScope scope = provider.CreateScope())
                {
                    // Here we retrieve the singleton instance of the BackgroundWorker.
                    BackgroundWorker backgroundWorker = scope.ServiceProvider.GetRequiredService<BackgroundWorker>();

                    // And we execute it, which will log out a number to the console
                    backgroundWorker.Execute();
                }

                // This is only placed here so that the console doesn't get spammed with too many log lines
                Thread.Sleep(TimeSpan.FromSeconds(60));
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
                .UseStartup<Startup>();
        
    }
}
