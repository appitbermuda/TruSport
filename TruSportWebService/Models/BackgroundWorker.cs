using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnTrackWebService.Data;
using OnTrackWebService.Models.CricHQ;
using OnTrackWebService.Repository;

namespace OnTrackWebService.Models
{
    public class BackgroundWorker
    {
        private readonly ILogger _logger;
        //OnTrackContext _context;
        private int _counter;
        public FixtureRepository fixtureRepository { get; }
        //private readonly IServiceScope _scope;
        private readonly IServiceScopeFactory scopeFactory;

        public BackgroundWorker(ILogger<BackgroundWorker> logger, IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            //_scope = services.CreateScope(); // CreateScope is in Microsoft.Extensions.DependencyInjection
            //_context = context;
            //_counter = 0;
            _logger = logger;
            //fixtureRepository = new FixtureRepository(_context);
        }

        public async void Execute()
        {
            try
            {
                
                    //var fixtures = await dbContext.Fixtures.ToListAsync();

                    await UpdateLiveCricketScores();
                

                //using (var context = _scope.ServiceProvider.GetRequiredService<OnTrackContext>())
                //{
                //    var fixtures = await context.Fixtures.ToListAsync();
                //    //return context..GetCollection<Foo>();
                //}
                //fixtureRepository.UpdateLiveCricketScores();
                _logger.LogDebug(_counter.ToString());
                //_counter++;
            }
            catch (Exception ex)
            {

            }
        }

        public async Task UpdateLiveCricketScores()
        {
            try
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<OnTrackContext>();
                    try
                    {
                        
                        var todaysFixtures = await dbContext.CricketFixtures.Where(e => e.Date == DateTime.Now.ToLocalTime().Date).ToListAsync();

                        if (todaysFixtures != null && todaysFixtures.Count > 0)
                        {
                            //Check crichq
                            HttpClient client = new HttpClient();
                            client.DefaultRequestHeaders.Add(Constants.CricHQApiKeyName, Constants.CricHQApiKey);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var response = await client.GetAsync($"{Constants.CricHQEndpoint}/{Constants.CricHQFixturesEndpoint}");

                            if (response.IsSuccessStatusCode)
                            {
                                var crichqresponse = await response.Content.ReadAsStringAsync();

                                if (!String.IsNullOrEmpty(crichqresponse))
                                {
                                    crichq crichq = JsonConvert.DeserializeObject<crichq>(crichqresponse);
                                    List<item> items = new List<item>();

                                    foreach (var match in crichq.items)
                                    {
                                        if (match.dates.Any(d => d.start_date == DateTime.Now.ToLocalTime().Date))
                                        {
                                            items.Add(match);
                                        }
                                    }

                                    if(items != null && items.Count > 0)
                                    {

                                    }
                                }


                                //_context.Database.BeginTransaction();

                                //await _context.SaveChangesAsync();

                                //_context.Database.CommitTransaction();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContext.Database.RollbackTransaction();
                        Debug.WriteLine(ex.Message, "Update Live Scores");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "UpdateLiveCricketScores");
            }
        }
    }
}
