using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.CricHQ;
using OnTrackWebService.Models.Imports;
using MatchType = OnTrackWebService.Models.MatchType;

namespace OnTrackWebService.Repository
{
    public class FixtureRepository : IOnTrackRepository<Fixture>
    {
        OnTrackContext _context;
        LeagueTableRepository leagueTableRepository;
        TeamRepository teamRepository;
        LeagueRepository leagueRepository;
        SeasonRepository seasonRepository;
        SettingRepository settingRepository;

        public FixtureRepository(OnTrackContext context)
        {
            _context = context;
            leagueTableRepository = new LeagueTableRepository(context);
            teamRepository = new TeamRepository(context);
            leagueRepository = new LeagueRepository(context);
            seasonRepository = new SeasonRepository(context);
            settingRepository = new SettingRepository(context);
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Fixture> Get(string id)
        {
            try
            {
                Fixture fixture = new Fixture();

                fixture = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Include(e => e.Sport).FirstOrDefaultAsync(e => e.ID == id);

                List<Coach> coaches = await _context.Coaches
                        .ToListAsync();

                fixture.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == fixture.HomeTeamID).ToList();
                fixture.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == fixture.AwayTeamID).ToList();


                return fixture;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetFixture");
            }
            return null;
        }


        public async Task<IEnumerable<Fixture>> Get()
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();

                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                
                .Include(e => e.Season)
                .Include(e => e.Sport).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                        .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                return fixtures;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetFixtures");
            }

            return null;
        }

        public async Task<IEnumerable<Fixture>> GetBySport(string sport)
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();
                Sport selectedSport = await _context.Sports.FirstOrDefaultAsync(e => e.Name == sport);

                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                
                .Include(e => e.Season)
                .Include(e => e.Sport).Where(e => e.SportID == selectedSport.ID).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                        .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                //if(selectedSport.Name == "Cricket")
                //{
                //    fixtures.ForEach(e => e.CricketMatch = new List<CricketMatch>
                //    {
                //        new CricketMatch
                //        {
                //            TeamID = e.HomeTeamID,
                //            FixtureID = e.ID,
                //            Runs = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Runs),
                //            Wickets = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Wickets),
                //            Overs = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Overs),
                //            Bye = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Bye),
                //            Extras = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Extras),
                //            LegBye = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.LegBye),
                //            NoBall = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.NoBall),
                //            Wide = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Wide)

                //        },
                //        new CricketMatch
                //        {
                //            TeamID = e.AwayTeamID,
                //            FixtureID = e.ID,
                //            Runs = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Runs),
                //            Wickets = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Wickets),
                //            Overs = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Overs),
                //            Bye = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Bye),
                //            Extras = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Extras),
                //            LegBye = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.LegBye),
                //            NoBall = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.NoBall),
                //            Wide = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Wide)
                //        }
                //    });
                //}


                return fixtures;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBySport");
            }

            return null;
        }

        public async Task<BowlingFixtureListView> GetBowlingFixtures()
        {
            try
            {
                var pastFixtures = await GetPastBowlingFixtures();
                var upcomingFixtures = await GetUpcomingBowlingFixtures();

                return new BowlingFixtureListView
                {
                    PastFixtures = pastFixtures.ToList(),
                    UpcomingFixtures = upcomingFixtures.ToList()
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBowlingFixtures");
            }

            return null;
        }

        public async Task<IEnumerable<BowlingFixture>> GetBowlingResults()
        {
            try
            {
                List<BowlingRosterListView> bowlingRosterLists = new List<BowlingRosterListView>();

                List<BowlingFixture> fixtures = new List<BowlingFixture>();
                fixtures = await _context.BowlingFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingGames)
                .Include(e => e.BowlingScore)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.Date.Date < DateTime.Now.Date).ToListAsync();

                List<BowlingGameResult> bowlingGameResults = new List<BowlingGameResult>();

                Parallel.ForEach(fixtures, fixture =>
                {
                    foreach (var homeFixtureRoster in fixture.BowlingRosters.Where(e => e.TeamID == fixture.HomeTeamID))
                    {
                        var awayFixtureRoster = fixture.BowlingRosters.FirstOrDefault(e => e.Position == homeFixtureRoster.Position && e.TeamID == fixture.AwayTeamID);

                        bowlingRosterLists.Add(new BowlingRosterListView
                        {
                            FixtureID = homeFixtureRoster.BowlingFixtureID,
                            HomeTeamID = homeFixtureRoster.TeamID,
                            AwayTeamID = awayFixtureRoster.TeamID,
                            HomePlayerName = homeFixtureRoster?.BowlingPlayerSeason?.Player?.Name,
                            AwayPlayerName = awayFixtureRoster?.BowlingPlayerSeason?.Player?.Name,
                            Position = homeFixtureRoster.Position.Value
                        });

                        if (homeFixtureRoster.BowlingGames != null && homeFixtureRoster.BowlingGames.Count > 0 && awayFixtureRoster.BowlingGames != null && awayFixtureRoster.BowlingGames.Count > 0)
                        {
                            foreach (var homeGame in homeFixtureRoster.BowlingGames)
                            {
                                var awayGame = awayFixtureRoster.BowlingGames.FirstOrDefault(e => e.Game == homeGame.Game);
                                bowlingGameResults.Add(new BowlingGameResult
                                {
                                    BowlingRosterID1 = homeGame.BowlingRosterID,
                                    BowlingRoster1 = homeGame.BowlingRoster,
                                    BowlingRosterID2 = awayGame.BowlingRosterID,
                                    BowlingRoster2 = awayGame.BowlingRoster,
                                    Game = homeGame.Game,
                                    Position = homeFixtureRoster.Position,
                                    Score1 = homeGame.Score,
                                    Score2 = awayGame.Score,
                                    Winner = homeGame.Score > awayGame.Score ? homeGame.BowlingRosterID : awayGame.Score > homeGame.Score ? awayGame.BowlingRosterID : null
                                });

                                homeGame.Win = homeGame.Score > awayGame.Score;
                                awayGame.Win = awayGame.Score > homeGame.Score;
                            }

                            fixture.BowlingRosters.ForEach(e => e.BowlingGameResults = bowlingGameResults.Where(d => d.BowlingRosterID1 == e.ID || d.BowlingRosterID2 == e.ID).ToList());

                        }
                    }

                    fixture.BowlingGameResults = bowlingGameResults;

                    //foreach (var homeRoster in fixture.BowlingRosters.Where(e => e.TeamID == fixture.HomeTeamID))
                    //{
                    //    var awayRoster = fixture.BowlingRosters.FirstOrDefault(e => e.Position == homeRoster.Position && e.TeamID == fixture.AwayTeamID);

                    //    bowlingRosterLists.Add(new BowlingRosterListView
                    //    {
                    //        FixtureID = homeRoster.BowlingFixtureID,
                    //        HomeTeamID = homeRoster.TeamID,
                    //        AwayTeamID = awayRoster.TeamID,
                    //        HomePlayerName = homeRoster?.BowlingPlayerSeason?.Player?.Name,
                    //        AwayPlayerName = awayRoster?.BowlingPlayerSeason?.Player?.Name
                    //    });
                    //}

                    fixture.BowlingRosterList = bowlingRosterLists;


                    //fixture.BowlingScore.HomeTeamTotalPoints = fixture.BowlingScore.HomeTeamMatchPoints + fixture.BowlingScore.HomeTeamPoints;
                    //fixture.BowlingScore.AwayTeamTotalPoints = fixture.BowlingScore.AwayTeamMatchPoints + fixture.BowlingScore.AwayTeamPoints;

                    //fixture.HomeTeam.Name = !String.IsNullOrEmpty(fixture.HomeTeam.Alias) ? fixture.HomeTeam.Alias : fixture.HomeTeam.Name;
                    //fixture.AwayTeam.Name = !String.IsNullOrEmpty(fixture.AwayTeam.Alias) ? fixture.AwayTeam.Alias : fixture.AwayTeam.Name;
                });

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);



                return fixtures.OrderByDescending(e => e.FixtureTime).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Past Bowling Fixture");
            }

            return null;
        }

        public async Task<IEnumerable<BowlingFixture>> GetUpcomingBowlingFixtures()
        {
            try
            {
                List<BowlingFixture> fixtures = new List<BowlingFixture>();
                fixtures = await _context.BowlingFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingGames)
                .Include(e => e.BowlingScore)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.Date.Date >= DateTime.Now.Date).ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);
                
                return fixtures.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Upcoming Bowling Fixture");
            }

            return null;
        }

        public async Task<BowlingFixture> GetBowlingFixture(string id)
        {
            try
            {
                List<BowlingRosterListView> bowlingRosterLists = new List<BowlingRosterListView>();

                BowlingFixture fixture = new BowlingFixture();
                fixture = await _context.BowlingFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingGames)
                .Include(e => e.BowlingScore)
                .Include(e => e.MatchType)
                .Include(e => e.Season).FirstOrDefaultAsync(e => e.ID == id);

                //if (fixture.BowlingScore != null)
                //{
                //    fixture.BowlingScore.HomeTeamTotalPoints = fixture.BowlingScore.HomeTeamMatchPoints + fixture.BowlingScore.HomeTeamPoints;
                //    fixture.BowlingScore.AwayTeamTotalPoints = fixture.BowlingScore.AwayTeamMatchPoints + fixture.BowlingScore.AwayTeamPoints;
                //}

                fixture.HomeTeam.Name = !String.IsNullOrEmpty(fixture.HomeTeam.Alias) ? fixture.HomeTeam.Alias : fixture.HomeTeam.Name;
                fixture.AwayTeam.Name = !String.IsNullOrEmpty(fixture.AwayTeam.Alias) ? fixture.AwayTeam.Alias : fixture.AwayTeam.Name;

                var table = await leagueTableRepository.GetBowlingLeagueStandings(fixture.LeagueID);
                if (table != null)
                {
                    table.ForEach(e => e.IsSelectedTeam = (e.TeamID == fixture.HomeTeamID || e.TeamID == fixture.AwayTeamID));

                    fixture.LeagueTable = table.ToList();
                }

                fixture.HeadToHead = await GetBowlingHeadToHead(fixture.ID);

                List<BowlingGameResult> bowlingGameResults = new List<BowlingGameResult>();

                foreach (var homeFixtureRoster in fixture.BowlingRosters.Where(e => e.TeamID == fixture.HomeTeamID))
                {
                    var awayFixtureRoster = fixture.BowlingRosters.FirstOrDefault(e => e.Position == homeFixtureRoster.Position && e.TeamID == fixture.AwayTeamID);

                    bowlingRosterLists.Add(new BowlingRosterListView
                    {
                        FixtureID = homeFixtureRoster.BowlingFixtureID,
                        HomeTeamID = homeFixtureRoster.TeamID,
                        AwayTeamID = awayFixtureRoster.TeamID,
                        HomePlayerName = homeFixtureRoster?.BowlingPlayerSeason?.Player?.Name,
                        AwayPlayerName = awayFixtureRoster?.BowlingPlayerSeason?.Player?.Name,
                        Position = homeFixtureRoster.Position.Value
                    });

                    if (homeFixtureRoster.BowlingGames != null && homeFixtureRoster.BowlingGames.Count > 0 && awayFixtureRoster.BowlingGames != null && awayFixtureRoster.BowlingGames.Count > 0)
                    {
                        foreach (var homeGame in homeFixtureRoster.BowlingGames)
                        {
                            var awayGame = awayFixtureRoster.BowlingGames.FirstOrDefault(e => e.Game == homeGame.Game);
                                    bowlingGameResults.Add(new BowlingGameResult
                                    {
                                        BowlingRosterID1 = homeGame.BowlingRosterID,
                                        BowlingRoster1 = homeGame.BowlingRoster,
                                        BowlingRosterID2 = awayGame.BowlingRosterID,
                                        BowlingRoster2 = awayGame.BowlingRoster,
                                        Game = homeGame.Game,
                                        Position = homeFixtureRoster.Position,
                                        Score1 = homeGame.Score,
                                        Score2 = awayGame.Score,
                                        Winner = homeGame.Score > awayGame.Score ? homeGame.BowlingRosterID : awayGame.Score > homeGame.Score ? awayGame.BowlingRosterID : null
                                    });

                                    homeGame.Win = homeGame.Score > awayGame.Score;
                                    awayGame.Win = awayGame.Score > homeGame.Score;
                        }

                        fixture.BowlingRosters.ForEach(e => e.BowlingGameResults = bowlingGameResults.Where(d => d.BowlingRosterID1 == e.ID || d.BowlingRosterID2 == e.ID).ToList());
                        
                    }
                }

                fixture.BowlingGameResults = bowlingGameResults;

                //foreach(var homeRoster in fixture.BowlingRosters.Where(e => e.TeamID == fixture.HomeTeamID))
                //{
                //    var awayRoster = fixture.BowlingRosters.FirstOrDefault(e => e.Position == homeRoster.Position && e.TeamID == fixture.AwayTeamID);

                //    bowlingRosterLists.Add(new BowlingRosterListView
                //    {
                //        FixtureID = homeRoster.BowlingFixtureID,
                //        HomeTeamID = homeRoster.TeamID,
                //        AwayTeamID = awayRoster.TeamID,
                //        HomePlayerName = homeRoster?.BowlingPlayerSeason?.Player?.Name,
                //        AwayPlayerName = awayRoster?.BowlingPlayerSeason?.Player?.Name
                //    });
                //}

                fixture.BowlingRosterList = bowlingRosterLists;

                

                return fixture;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<BowlingFixture>> GetPastBowlingFixtures()
        {
            try
            {
                List<BowlingRosterListView> bowlingRosterLists = new List<BowlingRosterListView>();

                List<BowlingFixture> fixtures = new List<BowlingFixture>();
                fixtures = await _context.BowlingFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingGames)
                .Include(e => e.BowlingScore)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.Date.Date < DateTime.Now.Date && e.HomeTeam.Name != "TBD" && e.AwayTeam.Name != "TBD").ToListAsync();

                List<BowlingGameResult> bowlingGameResults = new List<BowlingGameResult>();

                //Parallel.ForEach(fixtures, fixture =>
                //{
                //    foreach (var homeFixtureRoster in fixture.BowlingRosters.Where(e => e.TeamID == fixture.HomeTeamID))
                //    {
                //        var awayFixtureRoster = fixture.BowlingRosters.FirstOrDefault(e => e.Position == homeFixtureRoster.Position && e.TeamID == fixture.AwayTeamID);

                //        bowlingRosterLists.Add(new BowlingRosterListView
                //        {
                //            FixtureID = homeFixtureRoster.BowlingFixtureID,
                //            HomeTeamID = homeFixtureRoster.TeamID,
                //            AwayTeamID = awayFixtureRoster.TeamID,
                //            HomePlayerName = homeFixtureRoster?.BowlingPlayerSeason?.Player?.Name,
                //            AwayPlayerName = awayFixtureRoster?.BowlingPlayerSeason?.Player?.Name
                //        });

                //        if (homeFixtureRoster.BowlingGames != null && homeFixtureRoster.BowlingGames.Count > 0 && awayFixtureRoster.BowlingGames != null && awayFixtureRoster.BowlingGames.Count > 0)
                //        {
                //            foreach (var homeGame in homeFixtureRoster.BowlingGames)
                //            {
                //                var awayGame = awayFixtureRoster.BowlingGames.FirstOrDefault(e => e.Game == homeGame.Game);
                //                bowlingGameResults.Add(new BowlingGameResult
                //                {
                //                    BowlingRosterID1 = homeGame.BowlingRosterID,
                //                    BowlingRoster1 = homeGame.BowlingRoster,
                //                    BowlingRosterID2 = awayGame.BowlingRosterID,
                //                    BowlingRoster2 = awayGame.BowlingRoster,
                //                    Game = homeGame.Game,
                //                    Position = homeFixtureRoster.Position,
                //                    Score1 = homeGame.Score,
                //                    Score2 = awayGame.Score,
                //                    Winner = homeGame.Score > awayGame.Score ? homeGame.BowlingRosterID : awayGame.Score > homeGame.Score ? awayGame.BowlingRosterID : null
                //                });

                //                homeGame.Win = homeGame.Score > awayGame.Score;
                //                awayGame.Win = awayGame.Score > homeGame.Score;
                //            }

                //            fixture.BowlingRosters.ForEach(e => e.BowlingGameResults = bowlingGameResults.Where(d => d.BowlingRosterID1 == e.ID || d.BowlingRosterID2 == e.ID).ToList());

                //        }
                //    }

                //    fixture.BowlingGameResults = bowlingGameResults;

                //    fixture.BowlingRosterList = bowlingRosterLists;

                //    fixture.HomeTeam.Name = !String.IsNullOrEmpty(fixture.HomeTeam.Alias) ? fixture.HomeTeam.Alias : fixture.HomeTeam.Name;
                //    fixture.AwayTeam.Name = !String.IsNullOrEmpty(fixture.AwayTeam.Alias) ? fixture.AwayTeam.Alias : fixture.AwayTeam.Name;
                //});

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);

                return fixtures.OrderByDescending(e => e.FixtureTime).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Past Cricket Fixture");
            }

            return null;
        }

        public async Task<List<BowlingFixture>> GetBowlingHeadToHead(string fixtureID)
        {
            try
            {
                List<BowlingRosterListView> bowlingRosterLists = new List<BowlingRosterListView>();

                List<BowlingFixture> fixtures = new List<BowlingFixture>();
                BowlingFixture fixture = await _context.BowlingFixtures.FirstOrDefaultAsync(e => e.ID == fixtureID);

                fixtures = await _context.BowlingFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingGames)
                .Include(e => e.BowlingScore)
                .Include(e => e.MatchType)
                .Where(e => ((e.HomeTeamID == fixture.HomeTeamID && e.AwayTeamID == fixture.AwayTeamID) || (e.HomeTeamID == fixture.AwayTeamID && e.AwayTeamID == fixture.HomeTeamID)) && e.ID != fixture.ID && e.Date.Date < DateTime.Now.Date && !e.IsPostponed).ToListAsync();

                List<BowlingGameResult> bowlingGameResults = new List<BowlingGameResult>();

                foreach (var thisFixture in fixtures)
                {
                    foreach (var homeFixtureRoster in fixture.BowlingRosters.Where(e => e.TeamID == fixture.HomeTeamID))
                    {
                        var awayFixtureRoster = fixture.BowlingRosters.FirstOrDefault(e => e.Position == homeFixtureRoster.Position && e.TeamID == fixture.AwayTeamID);

                        bowlingRosterLists.Add(new BowlingRosterListView
                        {
                            FixtureID = homeFixtureRoster.BowlingFixtureID,
                            HomeTeamID = homeFixtureRoster.TeamID,
                            AwayTeamID = awayFixtureRoster.TeamID,
                            HomePlayerName = homeFixtureRoster?.BowlingPlayerSeason?.Player?.Name,
                            AwayPlayerName = awayFixtureRoster?.BowlingPlayerSeason?.Player?.Name,
                            Position = homeFixtureRoster.Position.Value
                        });

                        if (homeFixtureRoster.BowlingGames != null && homeFixtureRoster.BowlingGames.Count > 0 && awayFixtureRoster.BowlingGames != null && awayFixtureRoster.BowlingGames.Count > 0)
                        {
                            foreach (var homeGame in homeFixtureRoster.BowlingGames)
                            {
                                var awayGame = awayFixtureRoster.BowlingGames.FirstOrDefault(e => e.Game == homeGame.Game);
                                bowlingGameResults.Add(new BowlingGameResult
                                {
                                    BowlingRosterID1 = homeGame.BowlingRosterID,
                                    BowlingRoster1 = homeGame.BowlingRoster,
                                    BowlingRosterID2 = awayGame.BowlingRosterID,
                                    BowlingRoster2 = awayGame.BowlingRoster,
                                    Game = homeGame.Game,
                                    Position = homeFixtureRoster.Position,
                                    Score1 = homeGame.Score,
                                    Score2 = awayGame.Score,
                                    Winner = homeGame.Score > awayGame.Score ? homeGame.BowlingRosterID : awayGame.Score > homeGame.Score ? awayGame.BowlingRosterID : null
                                });

                                homeGame.Win = homeGame.Score > awayGame.Score;
                                awayGame.Win = awayGame.Score > homeGame.Score;
                            }

                            fixture.BowlingRosters.ForEach(e => e.BowlingGameResults = bowlingGameResults.Where(d => d.BowlingRosterID1 == e.ID || d.BowlingRosterID2 == e.ID).ToList());
                            
                        }
                    }

                    fixture.BowlingGameResults = bowlingGameResults;

                    //foreach (var homeRoster in fixture.BowlingRosters.Where(e => e.TeamID == fixture.HomeTeamID))
                    //{
                    //    var awayRoster = fixture.BowlingRosters.FirstOrDefault(e => e.Position == homeRoster.Position && e.TeamID == fixture.AwayTeamID);

                    //    bowlingRosterLists.Add(new BowlingRosterListView
                    //    {
                    //        FixtureID = homeRoster.BowlingFixtureID,
                    //        HomeTeamID = homeRoster.TeamID,
                    //        AwayTeamID = awayRoster.TeamID,
                    //        HomePlayerName = homeRoster?.BowlingPlayerSeason?.Player?.Name,
                    //        AwayPlayerName = awayRoster?.BowlingPlayerSeason?.Player?.Name
                    //    });
                    //}

                    fixture.BowlingRosterList = bowlingRosterLists;

                    //fixture.BowlingScore.HomeTeamTotalPoints = fixture.BowlingScore.HomeTeamMatchPoints + fixture.BowlingScore.HomeTeamPoints;
                    //fixture.BowlingScore.AwayTeamTotalPoints = fixture.BowlingScore.AwayTeamMatchPoints + fixture.BowlingScore.AwayTeamPoints;

                    fixture.HomeTeam.Name = !String.IsNullOrEmpty(fixture.HomeTeam.Alias) ? fixture.HomeTeam.Alias : fixture.HomeTeam.Name;
                    fixture.AwayTeam.Name = !String.IsNullOrEmpty(fixture.AwayTeam.Alias) ? fixture.AwayTeam.Alias : fixture.AwayTeam.Name;
                }

                //fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                //fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);

                return fixtures;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<CricketFixture>> GetCricketFixtures()
        {
            try
            {
                List<CricketFixture> fixtures = new List<CricketFixture>();
                fixtures = await _context.CricketFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.CricketRosters)
                .Include(e => e.MatchInnings)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .ToListAsync();

                List<Coach> coaches = await _context.Coaches
                        .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);

                fixtures.ForEach(e => e.MatchResult =
                    (e.MatchInnings != null && e.MatchInnings.Count > 0) && e.IsCancelled && !e.IsPostponed ? "Match abandoned" : ""
                );

                foreach (var fixture in fixtures.Where(e => e.MatchInnings != null && e.MatchInnings.Count > 0))
                {

                    string homeTeamscore = "";

                    List<InningScore> inningScores = new List<InningScore>();
                    int homeTeamRuns = 0;
                    int awayTeamRuns = 0;

                    int homeTeamWickets = 0;
                    int awayTeamWickets = 0;

                    foreach (var matchInning in fixture.MatchInnings)
                    {
                        if (matchInning.BattingTeamID == fixture.HomeTeamID)
                        {
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.HomeTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.HomeTeamScore += String.Format("{0}/{1} ", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run ?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.HomeTeamID,
                                Team = fixture.HomeTeam
                            });
                        }

                        string awayTeamscore = "";
                        if (matchInning.BattingTeamID == fixture.AwayTeamID)
                        {
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.AwayTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.AwayTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run ?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.AwayTeamID,
                                Team = fixture.AwayTeam

                            });
                        }
                    }

                    if (fixture.End.HasValue && fixture.End.Value < DateTime.Now)
                    {
                        if (fixture.MatchType.Name == "One 50 Overs")
                        {
                            if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 50))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs)
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    fixture.MatchResult = inningScores[0].Overs < inningScores[1].Overs ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                                }
                            }
                        }
                        else if (fixture.MatchType.Name == "T20")
                        {
                            if (inningScores.Count > 1 && (inningScores.Any(e => e.Overs == 20) || inningScores.Any(e => e.Wickets == 10)))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs || (inningScores[0].Wickets == 10 && inningScores[1].Wickets == 10))
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    if (inningScores[0].Wickets == 10)
                                    {
                                        fixture.MatchResult = inningScores[1].Team.Name + " won by " + (inningScores[0].Wickets - inningScores[1].Wickets) + " wickets";
                                    }
                                    else if(inningScores[1].Wickets == 10)
                                    {
                                        fixture.MatchResult = inningScores[0].Team.Name + " won by " + (inningScores[1].Wickets - inningScores[0].Wickets) + " wickets";
                                    }
                                    else
                                    {
                                        fixture.MatchResult = inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs ? inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets";
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }

                //fixtures.ForEach(e => e.CricketMatch = new List<CricketMatch>
                //{
                //    new CricketMatch
                //    {
                //        TeamID = e.HomeTeamID,
                //        FixtureID = e.ID,
                //        Runs = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Runs),
                //        Wickets = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Wickets),
                //        Overs = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Overs),
                //        Bye = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Bye),
                //        Extras = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Extras),
                //        LegBye = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.LegBye),
                //        NoBall = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.NoBall),
                //        Wide = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Wide)

                //    },
                //    new CricketMatch
                //    {
                //        TeamID = e.AwayTeamID,
                //        FixtureID = e.ID,
                //        Runs = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Runs),
                //        Wickets = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Wickets),
                //        Overs = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Overs),
                //        Bye = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Bye),
                //        Extras = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Extras),
                //        LegBye = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.LegBye),
                //        NoBall = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.NoBall),
                //        Wide = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Wide)
                //    }
                //});


                return fixtures;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketFixtures");
            }

            return null;
        }

        public async Task<IEnumerable<CricketFixture>> GetPastCricketFixtures()
        {
            try
            {
                List<CricketFixture> fixtures = new List<CricketFixture>();
                fixtures = await _context.CricketFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.CricketRosters)
                .Include(e => e.MatchInnings)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.Date.Date < DateTime.Now.Date && e.HomeTeam.Name != "TBD" && e.AwayTeam.Name != "TBD").ToListAsync();

                List<Coach> coaches = await _context.Coaches
                        .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);
                fixtures.ForEach(e => e.MatchResult =
                    (e.MatchInnings != null && e.MatchInnings.Count > 0) && e.IsCancelled && !e.IsPostponed ? "Match abandoned" : ""
                );

                foreach (var fixture in fixtures.Where(e => e.MatchInnings != null && e.MatchInnings.Count > 0))
                {
                    
                    string homeTeamscore = "";

                    List<InningScore> inningScores = new List<InningScore>();
                    int homeTeamRuns = 0;
                    int awayTeamRuns = 0;

                    int homeTeamWickets = 0;
                    int awayTeamWickets = 0;

                    foreach(var matchInning in fixture.MatchInnings)
                    {
                        if(matchInning.BattingTeamID == fixture.HomeTeamID)
                        { 
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.HomeTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.HomeTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run ?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.HomeTeamID,
                                Team = fixture.HomeTeam
                            });
                        }

                        string awayTeamscore = "";
                        if (matchInning.BattingTeamID == fixture.AwayTeamID)
                        {
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.AwayTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.AwayTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.AwayTeamID,
                                Team = fixture.AwayTeam

                            });
                        }
                    }

                    if (fixture.End.HasValue && fixture.End.Value < DateTime.Now)
                    {
                        if (fixture.MatchType.Name == "One 50 Overs")
                        {
                            if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 50))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs)
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    fixture.MatchResult = inningScores[0].Overs < inningScores[1].Overs ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                                }
                            }
                        }
                        else if (fixture.MatchType.Name == "T20")
                        {
                            //if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 20))
                            //{
                            //    if (inningScores[0].Overs == inningScores[1].Overs)
                            //    {
                            //        fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                            //    }
                            //    else
                            //    {
                            //        fixture.MatchResult = inningScores[0].Overs != inningScores[1].Overs && inningScores[0].Wickets < inningScores[1].Wickets ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                            //    }
                            //}

                            if (inningScores.Count > 1 && (inningScores.Any(e => e.Overs == 20) || inningScores.Any(e => e.Wickets == 10)))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs || (inningScores[0].Wickets == 10 && inningScores[1].Wickets == 10))
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    if (inningScores[0].Wickets == 10 || inningScores[1].Wickets == 10)
                                    {
                                        fixture.MatchResult = inningScores[0].Wickets == 10 ? inningScores[1].Team.Name + " won by " + (inningScores[0].Wickets - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (inningScores[1].Wickets - inningScores[0].Wickets) + " wickets";
                                    }
                                    else
                                    {
                                        fixture.MatchResult = inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs ? inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets";
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }

                //fixtures.ForEach(e => e.CricketScores = new List<Cric>
                //{
                //    new CricketMatch
                //    {
                //        TeamID = e.HomeTeamID,
                //        FixtureID = e.ID,
                //        Runs = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Runs),
                //        Wickets = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Wickets),
                //        Overs = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Overs),
                //        Bye = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Bye),
                //        Extras = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Extras),
                //        LegBye = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.LegBye),
                //        NoBall = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.NoBall),
                //        Wide = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Wide)

                //    },
                //    new CricketMatch
                //    {
                //        TeamID = e.AwayTeamID,
                //        FixtureID = e.ID,
                //        Runs = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Runs),
                //        Wickets = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Wickets),
                //        Overs = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Overs),
                //        Bye = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Bye),
                //        Extras = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Extras),
                //        LegBye = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.LegBye),
                //        NoBall = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.NoBall),
                //        Wide = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Wide)
                //    }
                //});


                return fixtures.OrderByDescending(e => e.FixtureTime).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Past Cricket Fixture");
            }

            return null;
        }

        public async Task<IEnumerable<CricketFixture>> GetCricketResults()
        {
            try
            {
                List<CricketFixture> fixtures = new List<CricketFixture>();
                fixtures = await _context.CricketFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.CricketRosters)
                .Include(e => e.MatchInnings)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.Date.Date < DateTime.Now.Date && e.MatchInnings != null && e.MatchInnings.Count > 0).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                        .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);

                fixtures.ForEach(e => e.MatchResult =
                    (e.MatchInnings != null && e.MatchInnings.Count > 0) && e.IsCancelled && !e.IsPostponed ? "Match abandoned" : ""
                );

                foreach (var fixture in fixtures.Where(e => e.MatchInnings != null && e.MatchInnings.Count > 0))
                {
                    string homeTeamscore = "";

                    List<InningScore> inningScores = new List<InningScore>();
                    int homeTeamRuns = 0;
                    int awayTeamRuns = 0;

                    int homeTeamWickets = 0;
                    int awayTeamWickets = 0;

                    foreach (var matchInning in fixture.MatchInnings)
                    {
                        if (matchInning.BattingTeamID == fixture.HomeTeamID)
                        {
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.HomeTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.HomeTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run ?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.HomeTeamID,
                                Team = fixture.HomeTeam
                            });
                        }

                        string awayTeamscore = "";
                        if (matchInning.BattingTeamID == fixture.AwayTeamID)
                        {
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.AwayTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.AwayTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run ?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.AwayTeamID,
                                Team = fixture.AwayTeam

                            });
                        }
                    }

                    if (fixture.End.HasValue && fixture.End.Value < DateTime.Now)
                    {
                        if (fixture.MatchType.Name == "One 50 Overs")
                        {
                            if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 50))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs)
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    fixture.MatchResult = inningScores[0].Overs < inningScores[1].Overs ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                                }
                            }
                        }
                        else if (fixture.MatchType.Name == "T20")
                        {
                            if (inningScores.Count > 1 && (inningScores.Any(e => e.Overs == 20) || inningScores.Any(e => e.Wickets == 10)))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs || (inningScores[0].Wickets == 10 && inningScores[1].Wickets == 10))
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    if (inningScores[0].Wickets == 10 || inningScores[1].Wickets == 10)
                                    {
                                        fixture.MatchResult = inningScores[0].Wickets == 10 ? inningScores[1].Team.Name + " won by " + (inningScores[0].Wickets - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (inningScores[1].Wickets - inningScores[0].Wickets) + " wickets";
                                    }
                                    else
                                    {
                                        fixture.MatchResult = inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs ? inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets";
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }

                //fixtures.ForEach(e => e.CricketMatch = new List<CricketMatch>
                //{
                //    new CricketMatch
                //    {
                //        TeamID = e.HomeTeamID,
                //        FixtureID = e.ID,
                //        Runs = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Runs),
                //        Wickets = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Wickets),
                //        Overs = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Overs),
                //        Bye = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Bye),
                //        Extras = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Extras),
                //        LegBye = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.LegBye),
                //        NoBall = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.NoBall),
                //        Wide = e.CricketScores.Where(x => x.BattingTeamID == e.HomeTeamID).Sum(x => x.Wide)

                //    },
                //    new CricketMatch
                //    {
                //        TeamID = e.AwayTeamID,
                //        FixtureID = e.ID,
                //        Runs = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Runs),
                //        Wickets = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Wickets),
                //        Overs = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Overs),
                //        Bye = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Bye),
                //        Extras = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Extras),
                //        LegBye = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.LegBye),
                //        NoBall = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.NoBall),
                //        Wide = e.CricketScores.Where(x => x.BattingTeamID == e.AwayTeamID).Sum(x => x.Wide)
                //    }
                //});


                return fixtures.OrderByDescending(e => e.FixtureTime).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Past Cricket Fixture");
            }

            return null;
        }

        public async Task<IEnumerable<CricketFixture>> GetUpcomingCricketFixtures()
        {
            try
            {
                List<CricketFixture> fixtures = new List<CricketFixture>();
                fixtures = await _context.CricketFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.CricketRosters)
                .Include(e => e.MatchInnings)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.Date.Date >= DateTime.Now.Date).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                        .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);
                fixtures.ForEach(e => e.MatchResult =
                    (e.MatchInnings != null && e.MatchInnings.Count > 0) && e.IsCancelled && !e.IsPostponed ? "Match abandoned" : ""
                );

                foreach (var fixture in fixtures.Where(e => e.MatchInnings != null && e.MatchInnings.Count > 0))
                {

                    string homeTeamscore = "";

                    List<InningScore> inningScores = new List<InningScore>();
                    int homeTeamRuns = 0;
                    int awayTeamRuns = 0;

                    int homeTeamWickets = 0;
                    int awayTeamWickets = 0;

                    foreach (var matchInning in fixture.MatchInnings)
                    {
                        if (matchInning.BattingTeamID == fixture.HomeTeamID)
                        {
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.HomeTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.HomeTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run ?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.HomeTeamID,
                                Team = fixture.HomeTeam
                            });
                        }

                        string awayTeamscore = "";
                        if (matchInning.BattingTeamID == fixture.AwayTeamID)
                        {
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.AwayTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.AwayTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run ?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.AwayTeamID,
                                Team = fixture.AwayTeam

                            });
                        }
                    }

                    if (fixture.End.HasValue && fixture.End.Value < DateTime.Now)
                    {
                        if (fixture.MatchType.Name == "One 50 Overs")
                        {
                            if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 50))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs)
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    fixture.MatchResult = inningScores[0].Overs < inningScores[1].Overs ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                                }
                            }
                        }
                        else if (fixture.MatchType.Name == "T20")
                        {
                            if (inningScores.Count > 1 && (inningScores.Any(e => e.Overs == 20) || inningScores.Any(e => e.Wickets == 10)))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs || (inningScores[0].Wickets == 10 && inningScores[1].Wickets == 10))
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    if (inningScores[0].Wickets == 10 || inningScores[1].Wickets == 10)
                                    {
                                        fixture.MatchResult = inningScores[0].Wickets == 10 ? inningScores[1].Team.Name + " won by " + (inningScores[0].Wickets - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (inningScores[1].Wickets - inningScores[0].Wickets) + " wickets";
                                    }
                                    else
                                    {
                                        fixture.MatchResult = inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs ? inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets";
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }

                return fixtures.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Upcoming Cricket Fixture");
            }

            return null;
        }

        public async Task<CricketFixture> GetCricketFixture(string id)
        {
            try
            {
                CricketFixture fixture = new CricketFixture();
                fixture = await _context.CricketFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.CricketRosters).ThenInclude(e => e.Player)
                .Include(e => e.CricketRosters).ThenInclude(e => e.Team)
                .Include(e => e.CricketRosters).ThenInclude(e => e.Batters)
                .Include(e => e.CricketRosters).ThenInclude(e => e.Fielders)
                .Include(e => e.MatchInnings).ThenInclude(e => e.BattingTeam)
                .Include(e => e.MatchInnings).ThenInclude(e => e.FieldingTeam)
                .Include(e => e.MatchType)
                .Include(e => e.Season).FirstOrDefaultAsync(e => e.ID == id);

                List<Coach> coaches = await _context.Coaches
                        .ToListAsync();

                fixture.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == fixture.HomeTeamID).ToList();
                fixture.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == fixture.AwayTeamID).ToList();

                fixture.HomeTeam.Name = !String.IsNullOrEmpty(fixture.HomeTeam.Alias) ? fixture.HomeTeam.Alias : fixture.HomeTeam.Name;
                fixture.AwayTeam.Name = !String.IsNullOrEmpty(fixture.AwayTeam.Alias) ? fixture.AwayTeam.Alias : fixture.AwayTeam.Name;

                var table = await leagueTableRepository.GetCricketTableByLeague(fixture.LeagueID);
                if (table != null)
                {
                    table.ForEach(e => e.IsSelectedTeam = (e.TeamID == fixture.HomeTeamID || e.TeamID == fixture.AwayTeamID));

                    fixture.LeagueTable = table.ToList();
                }

                fixture.HeadToHead = await GetCricketHeadToHead(fixture.ID);
                
                if (fixture.CricketRosters != null && fixture.CricketRosters.Count > 0)
                {
                    fixture.CricketRosters.ForEach(e => e.IsHomeTeam = (e.TeamID == fixture.HomeTeamID));
                }

                fixture.MatchResult = (fixture.MatchInnings != null && fixture.MatchInnings.Count > 0) && fixture.IsCancelled && !fixture.IsPostponed ? "Match abandoned" : "";

                if (fixture.End != null && fixture.End < DateTime.Now)
                {
                    if (fixture.MatchInnings != null && fixture.MatchInnings.Count > 0 && !fixture.IsCancelled && !fixture.IsPostponed)
                    {

                        string homeTeamscore = "";

                        List<InningScore> inningScores = new List<InningScore>();
                        int homeTeamRuns = 0;
                        int awayTeamRuns = 0;

                        int homeTeamWickets = 0;
                        int awayTeamWickets = 0;

                        foreach (var matchInning in fixture.MatchInnings)
                        {
                            if (matchInning.BattingTeamID == fixture.HomeTeamID)
                            {
                                if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                    fixture.HomeTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                                else
                                    fixture.HomeTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                                inningScores.Add(new InningScore
                                {
                                    Runs = matchInning.Run ?? 0,
                                    Wickets = matchInning.Wicket ?? 0,
                                    Overs = matchInning.Over ?? 0,
                                    Inning = matchInning.Inning,
                                    TeamID = fixture.HomeTeamID,
                                    Team = fixture.HomeTeam
                                });
                            }

                            string awayTeamscore = "";
                            if (matchInning.BattingTeamID == fixture.AwayTeamID)
                            {
                                if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                    fixture.AwayTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                                else
                                    fixture.AwayTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                                inningScores.Add(new InningScore
                                {
                                    Runs = matchInning.Run ?? 0,
                                    Wickets = matchInning.Wicket ?? 0,
                                    Overs = matchInning.Over ?? 0,
                                    Inning = matchInning.Inning,
                                    TeamID = fixture.AwayTeamID,
                                    Team = fixture.AwayTeam

                                });
                            }
                        }

                        if (fixture.MatchType.Name == "One 50 Overs")
                        {
                            if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 50))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs)
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    fixture.MatchResult = inningScores[0].Overs < inningScores[1].Overs ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                                }
                            }
                        }
                        else if (fixture.MatchType.Name == "T20")
                        {
                            if (inningScores.Count > 1 && (inningScores.Any(e => e.Overs == 20) || inningScores.Any(e => e.Wickets == 10)))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs || (inningScores[0].Wickets == 10 && inningScores[1].Wickets == 10))
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    if (inningScores[0].Wickets == 10 || inningScores[1].Wickets == 10)
                                    {
                                        fixture.MatchResult = inningScores[0].Wickets == 10 ? inningScores[1].Team.Name + " won by " + (inningScores[0].Wickets - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (inningScores[1].Wickets - inningScores[0].Wickets) + " wickets";
                                    }
                                    else
                                    {
                                        fixture.MatchResult = inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs ? inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets";
                                    }
                                }
                            }
                        }
                        else
                        {

                        }

                    }
                }

                return fixture;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Fixture>> GetFootballFixtures()
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();
                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                
                .Include(e => e.Season).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                        .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);

                return fixtures;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Football Fixtures");
            }

            return null;
        }

        public async Task<IEnumerable<Fixture>> GetPastFootballFixtures()
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();
                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.Date.Date < DateTime.Now.Date && e.HomeTeam.Name != "TBD" && e.AwayTeam.Name != "TBD")
                .ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);


                return fixtures;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Past Football Fixtures");
            }

            return null;
        }

        public async Task<IEnumerable<Fixture>> GetFootballResults()
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();
                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                
                .Include(e => e.Season).Where(e => e.Date.Date < DateTime.Now.AddHours(-4).Date && e.Match.HomeTeamScore.HasValue && e.Match.AwayTeamScore.HasValue).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);

                return fixtures;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Past Football Fixtures");
            }

            return null;
        }

        public async Task<IEnumerable<Fixture>> GetUpcomingFootballFixtures()
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();
                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.Date.Date >= DateTime.Now.AddHours(-4).Date).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);

                return fixtures;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Upcoming Football Fixtures");
            }

            return null;
        }

        public async Task<Fixture> GetFootballFixture(string id)
        {
            try
            {
                Fixture fixture = new Fixture();
                fixture = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                //.ThenInclude(e => e.Player)
                //.ThenInclude(e => e.Team)
                //.ThenInclude(e => e.SubstitutePlayer)
                //.ThenInclude(e => e.MatchStats)
                .Include(e => e.Season).FirstOrDefaultAsync(e => e.ID == id);

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixture.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == fixture.HomeTeamID).ToList();
                fixture.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == fixture.AwayTeamID).ToList();

                fixture.HomeTeam.Name = !String.IsNullOrEmpty(fixture.HomeTeam.Alias) ? fixture.HomeTeam.Alias : fixture.HomeTeam.Name;
                fixture.AwayTeam.Name = !String.IsNullOrEmpty(fixture.AwayTeam.Alias) ? fixture.AwayTeam.Alias : fixture.AwayTeam.Name;

                var table = await leagueTableRepository.GetFootballTableByLeague(fixture.LeagueID);
                table.ForEach(e => e.IsSelectedTeam = (e.TeamID == fixture.HomeTeamID || e.TeamID == fixture.AwayTeamID));

                fixture.LeagueTable = table.ToList();

                fixture.HeadToHead = await GetFootballHeadToHead(fixture.ID);

                //var matchRosters = await _context.MatchRosters
                //    .Include(e => e.Player)
                //    .Include(e => e.Team)
                //    .Include(e => e.SubstitutePlayer)
                //    .Include(e => e.MatchStats)
                //    .Where(e => e.FixtureID == fixture.ID).ToListAsync();

                if(fixture.MatchRosters != null && fixture.MatchRosters.Count > 0)
                {
                    fixture.MatchRosters.ForEach(e => e.IsHomeTeam = (e.TeamID == fixture.HomeTeamID));

                    List<MatchRosterSummary> matchRosterSummaries = new List<MatchRosterSummary>();

                    foreach(var roster in fixture.MatchRosters)
                    {
                        if (roster.MatchStats.Count > 0)
                        {
                            for (var i = 0; i < roster.MatchStats.Count; i++)
                            {
                                if (roster.MatchStats[i].Goal > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = roster.FixtureID,
                                        TeamID = roster.TeamID,
                                        PlayerID = roster.PlayerID,
                                        Player = roster.Player,
                                        AssistPlayerID = roster.MatchStats[i].AssistPlayerID,
                                        AssistPlayer = roster.MatchStats[i].AssistPlayer,
                                        Minute = roster.MatchStats[i].GoalTime ?? 0,
                                        Goal = 1,
                                        IsHomeTeam = roster.IsHomeTeam
                                    });
                                }

                                if (roster.MatchStats[i].YellowCard > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = roster.FixtureID,
                                        TeamID = roster.TeamID,
                                        PlayerID = roster.PlayerID,
                                        Player = roster.Player,
                                        Minute = roster.MatchStats[i].YellowCardTime ?? 0,
                                        YellowCard = 1,
                                        IsHomeTeam = roster.IsHomeTeam
                                    });
                                }

                                if (roster.MatchStats[i].RedCard > 0)
                                {
                                    matchRosterSummaries.Add(new MatchRosterSummary
                                    {
                                        FixtureID = roster.FixtureID,
                                        TeamID = roster.TeamID,
                                        PlayerID = roster.PlayerID,
                                        Player = roster.Player,
                                        Minute = roster.MatchStats[i].RedCardTime ?? 0,
                                        RedCard = 1,
                                        IsHomeTeam = roster.IsHomeTeam
                                    });
                                }
                            }
                        }

                        if (roster.SubstitutePlayerID != null && roster.IsStarter)
                        {
                            matchRosterSummaries.Add(new MatchRosterSummary
                            {
                                FixtureID = roster.FixtureID,
                                TeamID = roster.TeamID,
                                PlayerID = roster.PlayerID,
                                Player = roster.Player,
                                SubstitutePlayerID = roster.SubstitutePlayerID,
                                SubsitutePlayer = roster.SubstitutePlayer,
                                Minute = roster.SubstituteTime ?? 0,
                                IsSub = true,
                                IsHomeTeam = roster.IsHomeTeam
                            });

                        }
                    }

                    if (matchRosterSummaries.Any(e => e.Minute == -1))
                        fixture.MatchRosterSummary = matchRosterSummaries;
                    else
                        fixture.MatchRosterSummary = matchRosterSummaries.OrderBy(e => e.Minute).ToList();

                    var matchStats = await _context.MatchStats
                       .Where(e => fixture.MatchRosters.Any(m => m.ID == e.MatchRosterID)).ToListAsync();

                }


                return fixture;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<List<Fixture>> GetFootballHeadToHead(string fixtureID)
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();
                Fixture fixture = await _context.Fixtures.FirstOrDefaultAsync(e => e.ID == fixtureID);

                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                
                .Include(e => e.Season)
                .Include(e => e.Sport)
                .Where(e => ((e.HomeTeamID == fixture.HomeTeamID && e.AwayTeamID == fixture.AwayTeamID) || (e.HomeTeamID == fixture.AwayTeamID && e.AwayTeamID == fixture.HomeTeamID)) && e.ID != fixture.ID && e.Date.Date < DateTime.Now.Date && (e.Match.HomeTeamScore.HasValue && e.Match.AwayTeamScore.HasValue) && !e.IsPostponed).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);

                return fixtures;
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public async Task<List<CricketFixture>> GetCricketHeadToHead(string fixtureID)
        {
            try
            {
                List<CricketFixture> fixtures = new List<CricketFixture>();
                CricketFixture fixture = await _context.CricketFixtures.FirstOrDefaultAsync(e => e.ID == fixtureID);

                fixtures = await _context.CricketFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.CricketRosters)
                .Include(e => e.MatchInnings)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => ((e.HomeTeamID == fixture.HomeTeamID && e.AwayTeamID == fixture.AwayTeamID) || (e.HomeTeamID == fixture.AwayTeamID && e.AwayTeamID == fixture.HomeTeamID)) && e.ID != fixture.ID && e.Date.Date < DateTime.Now.Date && (e.MatchInnings.Count > 0) && !e.IsPostponed).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);

                return fixtures;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Fixture>> GetHomeTeam(string fixtureID)
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();
                Fixture fixture = await _context.Fixtures.FirstOrDefaultAsync(e => e.ID == fixtureID);

                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                
                .Include(e => e.Season)
                .Include(e => e.Sport)
                .Where(e => (e.HomeTeamID == fixture.HomeTeamID || e.AwayTeamID == fixture.HomeTeamID) && e.ID != fixture.ID && (e.Match.HomeTeamScore.HasValue && e.Match.AwayTeamScore.HasValue) && !e.IsPostponed && e.Date.Date < DateTime.Now.Date).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.SelectedTeamID = fixture.HomeTeamID);
                fixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == fixture.HomeTeamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) ? "W" : (e.HomeTeamID == fixture.HomeTeamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) ? "L" : (e.AwayTeamID == fixture.HomeTeamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) ? "W" : (e.AwayTeamID == fixture.HomeTeamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) ? "L" : "D");

                return fixtures;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Fixture>> GetAwayTeam(string fixtureID)
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();
                Fixture fixture = await _context.Fixtures.FirstOrDefaultAsync(e => e.ID == fixtureID);

                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                
                .Include(e => e.Season)
                .Include(e => e.Sport)
                .Where(e => (e.HomeTeamID == fixture.AwayTeamID || e.AwayTeamID == fixture.AwayTeamID) && e.ID != fixture.ID && e.Date.Date < DateTime.Now.Date).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.SelectedTeamID = fixture.HomeTeamID);
                fixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == fixture.AwayTeamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) ? "W" : (e.HomeTeamID == fixture.AwayTeamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) ? "L" : (e.AwayTeamID == fixture.AwayTeamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) ? "W" : (e.AwayTeamID == fixture.AwayTeamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) ? "L" : "D");

                return fixtures;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        //Depracated
        public async Task<IEnumerable<Fixture>> GetAll()
        {
            //return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.AwayTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7" || e.HomeTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7").ToListAsync();
            return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").ToListAsync();
        }


        public async Task<IEnumerable<Fixture>> GetCupFixtures(string LeagueID, string MatchTypeID)
        {
            //return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.AwayTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7" || e.HomeTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7").ToListAsync();
            return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.LeagueID == LeagueID && e.MatchTypeID == MatchTypeID).ToListAsync();
        }

        //Deprecated
        public async Task<IEnumerable<Fixture>> GetByTeam(string teamID)
        {
            return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.AwayTeamID == teamID || e.HomeTeamID == teamID).ToListAsync();
        }

        public async Task<IEnumerable<Fixture>> GetFootballTeamFixtures(string teamID)
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();

                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                
                .Include(e => e.Season)
                .Include(e => e.Sport)
                .Where(e => e.AwayTeamID == teamID || e.HomeTeamID == teamID).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.SelectedTeamID = teamID);
                fixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == teamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) || (e.AwayTeamID == teamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == teamID && e.Match.AwayTeamPenalty > e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == teamID && e.Match.HomeTeamPenalty > e.Match.AwayTeamPenalty) ? "W" : (e.AwayTeamID == teamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) || (e.HomeTeamID == teamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == teamID && e.Match.AwayTeamPenalty < e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == teamID && e.Match.HomeTeamPenalty < e.Match.AwayTeamPenalty) ? "L" : (!e.Match.HomeTeamScore.HasValue || !e.Match.AwayTeamScore.HasValue) ? "" : "D");

                return fixtures;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<BowlingFixture>> GetBowlingTeamFixtures(string teamID)
        {
            try
            {
                List<BowlingFixture> fixtures = new List<BowlingFixture>();

                fixtures = await _context.BowlingFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingGames)
                .Include(e => e.BowlingScore)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.AwayTeamID == teamID || e.HomeTeamID == teamID).ToListAsync();

                fixtures.ForEach(e => e.SelectedTeamID = teamID);
                fixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == teamID && e.BowlingScore.HomeTeamTotalPoints > e.BowlingScore.AwayTeamTotalPoints) || (e.AwayTeamID == teamID && e.BowlingScore.AwayTeamTotalPoints > e.BowlingScore.HomeTeamTotalPoints) ? "W" : (e.AwayTeamID == teamID && e.BowlingScore.AwayTeamTotalPoints < e.BowlingScore.HomeTeamTotalPoints) || (e.HomeTeamID == teamID && e.BowlingScore.HomeTeamTotalPoints < e.BowlingScore.AwayTeamTotalPoints) ? "L" : (!e.BowlingScore.HomeTeamTotalPoints.HasValue || !e.BowlingScore.AwayTeamTotalPoints.HasValue) ? "" : "D");

                return fixtures;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<BowlingFixture>> GetBowlingTeamForm(string teamID)
        {
            try
            {
                List<BowlingFixture> fixtures = new List<BowlingFixture>();

                fixtures = await _context.BowlingFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingGames)
                .Include(e => e.BowlingScore)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => (e.AwayTeamID == teamID || e.HomeTeamID == teamID) && e.Date.Date < DateTime.Now.Date && e.Season.IsCurrent).OrderByDescending(e => e.Date).Take(6).ToListAsync();

                fixtures.ForEach(e => e.SelectedTeamID = teamID);
                fixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == teamID && e.BowlingScore.HomeTeamTotalPoints > e.BowlingScore.AwayTeamTotalPoints) || (e.AwayTeamID == teamID && e.BowlingScore.AwayTeamTotalPoints > e.BowlingScore.HomeTeamTotalPoints) ? "W" : (e.AwayTeamID == teamID && e.BowlingScore.AwayTeamTotalPoints < e.BowlingScore.HomeTeamTotalPoints) || (e.HomeTeamID == teamID && e.BowlingScore.HomeTeamTotalPoints < e.BowlingScore.AwayTeamTotalPoints) ? "L" : (!e.BowlingScore.HomeTeamTotalPoints.HasValue || !e.BowlingScore.AwayTeamTotalPoints.HasValue) ? "" : "D");

                return fixtures;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Fixture>> GetFootballTeamForm(string teamID)
        {
            try
            {
                List<Fixture> fixtures = new List<Fixture>();

                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Include(e => e.Sport)
                .Where(e => (e.AwayTeamID == teamID || e.HomeTeamID == teamID) && e.Date.Date < DateTime.Now.Date && e.Match.HomeTeamScore.HasValue && e.Match.AwayTeamScore.HasValue && e.Season.IsCurrent).OrderByDescending(e => e.Date).Take(6).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.SelectedTeamID = teamID);
                fixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == teamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) || (e.AwayTeamID == teamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == teamID && e.Match.AwayTeamPenalty > e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == teamID && e.Match.HomeTeamPenalty > e.Match.AwayTeamPenalty) ? "W" : (e.AwayTeamID == teamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) || (e.HomeTeamID == teamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == teamID && e.Match.AwayTeamPenalty < e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == teamID && e.Match.HomeTeamPenalty < e.Match.AwayTeamPenalty) ? "L" : (!e.Match.HomeTeamScore.HasValue || !e.Match.AwayTeamScore.HasValue) ? "" : "D");

                return fixtures;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<CricketFixture>> GetCricketTeamFixtures(string teamID)
        {
            try
            {
                List<CricketFixture> fixtures = new List<CricketFixture>();

                fixtures = await _context.CricketFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.CricketRosters)
                .Include(e => e.MatchInnings)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.AwayTeamID == teamID || e.HomeTeamID == teamID).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.SelectedTeamID = teamID);
                //fixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == teamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) || (e.AwayTeamID == teamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == teamID && e.Match.AwayTeamPenalty > e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == teamID && e.Match.HomeTeamPenalty > e.Match.AwayTeamPenalty) ? "W" : (e.AwayTeamID == teamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) || (e.HomeTeamID == teamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == teamID && e.Match.AwayTeamPenalty < e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == teamID && e.Match.HomeTeamPenalty < e.Match.AwayTeamPenalty) ? "L" : (!e.Match.HomeTeamScore.HasValue || !e.Match.AwayTeamScore.HasValue) ? "" : "D");
                fixtures.ForEach(e => e.MatchResult =
                    (e.MatchInnings != null && e.MatchInnings.Count > 0) && e.IsCancelled && !e.IsPostponed ? "Match abandoned" : ""
                );


                foreach (var fixture in fixtures.Where(e => e.MatchInnings != null && e.MatchInnings.Count > 0))
                {

                    string homeTeamscore = "";

                    List<InningScore> inningScores = new List<InningScore>();
                    int homeTeamRuns = 0;
                    int awayTeamRuns = 0;

                    int homeTeamWickets = 0;
                    int awayTeamWickets = 0;

                    foreach (var matchInning in fixture.MatchInnings)
                    {
                        if (matchInning.BattingTeamID == fixture.HomeTeamID)
                        {
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.HomeTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.HomeTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run ?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.HomeTeamID,
                                Team = fixture.HomeTeam
                            });
                        }

                        string awayTeamscore = "";
                        if (matchInning.BattingTeamID == fixture.AwayTeamID)
                        {
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.AwayTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.AwayTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run ?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.AwayTeamID,
                                Team = fixture.AwayTeam

                            });
                        }
                    }

                    if (fixture.End.HasValue && fixture.End.Value < DateTime.Now)
                    {
                        if (fixture.MatchType.Name == "One 50 Overs")
                        {
                            if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 50))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs)
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    fixture.MatchResult = inningScores[0].Overs < inningScores[1].Overs ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                                }
                            }
                        }
                        else if (fixture.MatchType.Name == "T20")
                        {
                            if (inningScores.Count > 1 && (inningScores.Any(e => e.Overs == 20) || inningScores.Any(e => e.Wickets == 10)))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs || (inningScores[0].Wickets == 10 && inningScores[1].Wickets == 10))
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                    fixture.SelectedTeamResult = (inningScores[0].Runs > inningScores[1].Runs && inningScores[0].TeamID == teamID) || (inningScores[1].Runs > inningScores[0].Runs && inningScores[1].TeamID == teamID) ? "W" : "L";
                                }
                                else
                                {
                                    if (inningScores[0].Wickets == 10 || inningScores[1].Wickets == 10)
                                    {
                                        fixture.MatchResult = inningScores[0].Wickets == 10 ? inningScores[1].Team.Name + " won by " + (inningScores[0].Wickets - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (inningScores[1].Wickets - inningScores[0].Wickets) + " wickets";
                                        fixture.SelectedTeamResult = (inningScores[0].Wickets == 10 && inningScores[1].TeamID == teamID) || (inningScores[1].Wickets == 10 && inningScores[0].TeamID == teamID) ? "W" : "L";
                                    }
                                    else
                                    {
                                        fixture.MatchResult = inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs ? inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets";
                                        fixture.SelectedTeamResult = (inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs && inningScores[1].TeamID == teamID) || (inningScores[0].Overs != inningScores[1].Overs && inningScores[0].Runs > inningScores[1].Runs && inningScores[0].TeamID == teamID) ? "W" : "L";
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }

                return fixtures;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<CricketFixture>> GetCricketTeamForm(string teamID)
        {
            try
            {
                List<CricketFixture> fixtures = new List<CricketFixture>();

                fixtures = await _context.CricketFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.CricketRosters)
                .Include(e => e.MatchInnings)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => (e.AwayTeamID == teamID || e.HomeTeamID == teamID) && e.Date < DateTime.Now.Date && (e.MatchInnings != null && e.MatchInnings.Count > 0)).OrderByDescending(f => f.Date).Take(6).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                fixtures.ForEach(e => e.SelectedTeamID = teamID);
                
                fixtures.ForEach(e => e.MatchResult =
                    (e.MatchInnings != null && e.MatchInnings.Count > 0) && e.IsCancelled && !e.IsPostponed ? "Match abandoned" : ""
                );


                foreach (var fixture in fixtures.Where(e => e.MatchInnings != null && e.MatchInnings.Count > 0))
                {
                    
                    string homeTeamscore = "";

                    List<InningScore> inningScores = new List<InningScore>();
                    int homeTeamRuns = 0;
                    int awayTeamRuns = 0;

                    int homeTeamWickets = 0;
                    int awayTeamWickets = 0;

                    foreach(var matchInning in fixture.MatchInnings)
                    {
                        if(matchInning.BattingTeamID == fixture.HomeTeamID)
                        { 
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.HomeTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.HomeTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run ?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.HomeTeamID,
                                Team = fixture.HomeTeam
                            });
                        }

                        string awayTeamscore = "";
                        if (matchInning.BattingTeamID == fixture.AwayTeamID)
                        {
                            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                                fixture.AwayTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                            else
                                fixture.AwayTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                            inningScores.Add(new InningScore
                            {
                                Runs = matchInning.Run?? 0,
                                Wickets = matchInning.Wicket ?? 0,
                                Overs = matchInning.Over ?? 0,
                                Inning = matchInning.Inning,
                                TeamID = fixture.AwayTeamID,
                                Team = fixture.AwayTeam

                            });
                        }
                    }

                    if (fixture.End.HasValue && fixture.End.Value < DateTime.Now)
                    {
                        if (fixture.MatchType.Name == "One 50 Overs")
                        {
                            if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 50))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs)
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                }
                                else
                                {
                                    fixture.MatchResult = inningScores[0].Overs < inningScores[1].Overs ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                                }
                            }
                        }
                        else if (fixture.MatchType.Name == "T20")
                        {
                            if (inningScores.Count > 1 && (inningScores.Any(e => e.Overs == 20) || inningScores.Any(e => e.Wickets == 10)))
                            {
                                if (inningScores[0].Overs == inningScores[1].Overs || (inningScores[0].Wickets == 10 && inningScores[1].Wickets == 10))
                                {
                                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                                    fixture.SelectedTeamResult = (inningScores[0].Runs > inningScores[1].Runs && inningScores[0].TeamID == teamID) || (inningScores[1].Runs > inningScores[0].Runs && inningScores[1].TeamID == teamID) ? "W" : "L";
                                }
                                else
                                {
                                    if (inningScores[0].Wickets == 10 || inningScores[1].Wickets == 10)
                                    {
                                        fixture.MatchResult = inningScores[0].Wickets == 10 ? inningScores[1].Team.Name + " won by " + (inningScores[0].Wickets - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (inningScores[1].Wickets - inningScores[0].Wickets) + " wickets";
                                        fixture.SelectedTeamResult = (inningScores[0].Wickets == 10 && inningScores[1].TeamID == teamID) || (inningScores[1].Wickets == 10 && inningScores[0].TeamID == teamID) ? "W" : "L";
                                    }
                                    else
                                    {
                                        fixture.MatchResult = inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs ? inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets";
                                        fixture.SelectedTeamResult = (inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs && inningScores[1].TeamID == teamID) || (inningScores[0].Overs != inningScores[1].Overs && inningScores[0].Runs > inningScores[1].Runs && inningScores[0].TeamID == teamID) ? "W" : "L";
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }


                //foreach (var fixture in fixtures.Where(e => e.MatchInnings != null && e.MatchInnings.Count > 0))
                //{
                //    string homeTeamscore = "";

                //    List<InningScore> inningScores = new List<InningScore>();
                //    int homeTeamRuns = 0;
                //    int awayTeamRuns = 0;

                //    int homeTeamWickets = 0;
                //    int awayTeamWickets = 0;

                //    foreach (var matchInning in fixture.MatchInnings)
                //    {
                //        if (matchInning.BattingTeamID == fixture.HomeTeamID)
                //        {
                //            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                //                fixture.HomeTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                //            else
                //                fixture.HomeTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                //            inningScores.Add(new InningScore
                //            {
                //                Runs = matchInning.Run.Value,
                //                Wickets = matchInning.Wicket.Value,
                //                Overs = matchInning.Over.Value,
                //                Inning = matchInning.Inning,
                //                TeamID = fixture.HomeTeamID,
                //                Team = fixture.HomeTeam
                //            });
                //        }

                //        string awayTeamscore = "";
                //        if (matchInning.BattingTeamID == fixture.AwayTeamID)
                //        {
                //            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                //                fixture.AwayTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                //            else
                //                fixture.AwayTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                //            inningScores.Add(new InningScore
                //            {
                //                Runs = matchInning.Run.Value,
                //                Wickets = matchInning.Wicket.Value,
                //                Overs = matchInning.Over.Value,
                //                Inning = matchInning.Inning,
                //                TeamID = fixture.AwayTeamID,
                //                Team = fixture.AwayTeam

                //            });
                //        }
                //    }

                //    if (fixture.End.HasValue && fixture.End.Value < DateTime.Now)
                //    {
                //        if (fixture.MatchType.Name == "One 50 Overs")
                //        {
                //            if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 50))
                //            {
                //                if (inningScores[0].Overs == inningScores[1].Overs)
                //                {
                //                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                //                }
                //                else
                //                {
                //                    fixture.MatchResult = inningScores[0].Overs < inningScores[1].Overs ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                //                }
                //            }
                //        }
                //        else if (fixture.MatchType.Name == "T20")
                //        {
                //            if (inningScores.Count > 1 && (inningScores.Any(e => e.Overs == 20) || inningScores.Any(e => e.Wickets == 10)))
                //            {
                //                if (inningScores[0].Overs == inningScores[1].Overs || (inningScores[0].Wickets == 10 && inningScores[1].Wickets == 10))
                //                {
                //                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                //                    fixture.SelectedTeamResult = (inningScores[0].Runs > inningScores[1].Runs && inningScores[0].TeamID == teamID) || (inningScores[1].Runs > inningScores[0].Runs && inningScores[1].TeamID == teamID) ? "W" : "L";
                //                }
                //                else
                //                {
                //                    if (inningScores[0].Wickets == 10 || inningScores[1].Wickets == 10)
                //                    {
                //                        fixture.MatchResult = inningScores[0].Wickets == 10 ? inningScores[1].Team.Name + " won by " + (inningScores[0].Wickets - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (inningScores[1].Wickets - inningScores[0].Wickets) + " wickets";
                //                        fixture.SelectedTeamResult = (inningScores[0].Wickets == 10 && inningScores[1].TeamID == teamID) || (inningScores[1].Wickets == 10 && inningScores[0].TeamID == teamID) ? "W" : "L";
                //                    }
                //                    else
                //                    {
                //                        fixture.MatchResult = inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs ? inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets";
                //                        fixture.SelectedTeamResult = (inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs && inningScores[1].TeamID == teamID) || (inningScores[0].Overs != inningScores[1].Overs && inningScores[0].Runs > inningScores[1].Runs && inningScores[0].TeamID == teamID) ? "W" : "L";
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {

                //        }
                    
                //    }
                //}
                //fixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == teamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) || (e.AwayTeamID == teamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == teamID && e.Match.AwayTeamPenalty > e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == teamID && e.Match.HomeTeamPenalty > e.Match.AwayTeamPenalty) ? "W" : (e.AwayTeamID == teamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) || (e.HomeTeamID == teamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == teamID && e.Match.AwayTeamPenalty < e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == teamID && e.Match.HomeTeamPenalty < e.Match.AwayTeamPenalty) ? "L" : (!e.Match.HomeTeamScore.HasValue || !e.Match.AwayTeamScore.HasValue) ? "" : "D");

                //foreach (var fixture in fixtures.Where(e => e.MatchInnings != null && e.MatchInnings.Count > 0))
                //{

                //    string homeTeamscore = "";

                //    List<InningScore> inningScores = new List<InningScore>();
                //    int homeTeamRuns = 0;
                //    int awayTeamRuns = 0;

                //    int homeTeamWickets = 0;
                //    int awayTeamWickets = 0;

                //    foreach (var matchInning in fixture.MatchInnings)
                //    {
                //        if (matchInning.BattingTeamID == fixture.HomeTeamID)
                //        {
                //            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                //                fixture.HomeTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                //            else
                //                fixture.HomeTeamScore += String.Format("{0}/{1} ", matchInning.Run, matchInning.Wicket);

                //            inningScores.Add(new InningScore
                //            {
                //                Runs = matchInning.Run ?? 0,
                //                Wickets = matchInning.Wicket ?? 0,
                //                Overs = matchInning.Over ?? 0,
                //                Inning = matchInning.Inning,
                //                TeamID = fixture.HomeTeamID,
                //                Team = fixture.HomeTeam
                //            });
                //        }

                //        string awayTeamscore = "";
                //        if (matchInning.BattingTeamID == fixture.AwayTeamID)
                //        {
                //            if (fixture.MatchType.Name == "One 50 Overs" || fixture.MatchType.Name == "T20")
                //                fixture.AwayTeamScore += String.Format("{0}/{1} ({2} Ovr) ", matchInning.Run, matchInning.Wicket, matchInning.Over);
                //            else
                //                fixture.AwayTeamScore += String.Format("{0}/{1}", matchInning.Run, matchInning.Wicket);

                //            inningScores.Add(new InningScore
                //            {
                //                Runs = matchInning.Run ?? 0,
                //                Wickets = matchInning.Wicket ?? 0,
                //                Overs = matchInning.Over ?? 0,
                //                Inning = matchInning.Inning,
                //                TeamID = fixture.AwayTeamID,
                //                Team = fixture.AwayTeam

                //            });
                //        }
                //    }

                //    if (fixture.End.HasValue && fixture.End.Value < DateTime.Now)
                //    {
                //        if (fixture.MatchType.Name == "One 50 Overs")
                //        {
                //            if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 50))
                //            {
                //                if (inningScores[0].Overs == inningScores[1].Overs)
                //                {
                //                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                //                }
                //                else
                //                {
                //                    fixture.MatchResult = inningScores[0].Overs < inningScores[1].Overs ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                //                }
                //            }
                //        }
                //        else if (fixture.MatchType.Name == "T20")
                //        {
                //            if (inningScores.Count > 1 && (inningScores.Any(e => e.Overs == 20) || inningScores.Any(e => e.Wickets == 10)))
                //            {
                //                if (inningScores[0].Overs == inningScores[1].Overs || (inningScores[0].Wickets == 10 && inningScores[1].Wickets == 10))
                //                {
                //                    fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                //                    fixture.SelectedTeamResult = inningScores[0].TeamID == teamID && inningScores[0].Runs > inningScores[1].Runs ? "W" : inningScores[1].TeamID == teamID && inningScores[1].Runs > inningScores[0].Runs ? "W" : inningScores[1].Runs == inningScores[0].Runs ? "D" : "L";
                //                }
                //                else
                //                {
                //                    if (inningScores[0].Wickets == 10)
                //                    {
                //                        fixture.MatchResult = inningScores[1].Team.Name + " won by " + (inningScores[0].Wickets - inningScores[1].Wickets) + " wickets";
                //                        fixture.SelectedTeamResult = inningScores[1].TeamID == teamID ? "W" : "L";
                //                    }
                //                    else if (inningScores[1].Wickets == 10)
                //                    {
                //                        fixture.MatchResult = inningScores[0].Team.Name + " won by " + (inningScores[1].Wickets - inningScores[0].Wickets) + " wickets";
                //                        fixture.SelectedTeamResult = inningScores[0].TeamID == teamID ? "W" : "L";
                //                    }
                //                    else
                //                    {
                //                        fixture.MatchResult = inningScores[0].Overs != inningScores[1].Overs && inningScores[1].Runs > inningScores[0].Runs ? inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets" : inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets";
                //                        fixture.SelectedTeamResult = (inningScores[1].TeamID == teamID && inningScores[1].Overs != inningScores[0].Overs && inningScores[1].Runs > inningScores[0].Runs) || (inningScores[0].TeamID == teamID && inningScores[1].Overs != inningScores[0].Overs && inningScores[0].Runs > inningScores[1].Runs) ? "W" : "L";
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {

                //        }
                //    }
                //}

                return fixtures;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Fixture>> GetFootballFixturesByLeague(string leagueID)
        {
            try
            {
                var fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                
                .Include(e => e.Season)
                .Include(e => e.Sport)
                .Where(e => e.LeagueID == leagueID).ToListAsync();

                List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

                fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
                fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

                return fixtures;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Cricket League Fixtures");
            }

            return null;
        }

        public async Task<IEnumerable<BowlingFixture>> GetBowlingFixturesByLeague(string leagueID)
        {
            try
            {
                List<BowlingRosterListView> bowlingRosterLists = new List<BowlingRosterListView>();

                var fixtures = await _context.BowlingFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingGames)
                .Include(e => e.BowlingScore)
                .Include(e => e.MatchType)
                .Include(e => e.Season)                
                .Where(e => e.LeagueID == leagueID).ToListAsync();

                List<BowlingGameResult> bowlingGameResults = new List<BowlingGameResult>();

                //foreach (var fixture in fixtures)
                //{
                //    foreach (var homeFixtureRoster in fixture.BowlingRosters.Where(e => e.TeamID == fixture.HomeTeamID))
                //    {
                //        var awayFixtureRoster = fixture.BowlingRosters.FirstOrDefault(e => e.Position == homeFixtureRoster.Position && e.TeamID == fixture.AwayTeamID);

                //        bowlingRosterLists.Add(new BowlingRosterListView
                //        {
                //            FixtureID = homeFixtureRoster.BowlingFixtureID,
                //            HomeTeamID = homeFixtureRoster.TeamID,
                //            AwayTeamID = awayFixtureRoster.TeamID,
                //            HomePlayerName = homeFixtureRoster?.BowlingPlayerSeason?.Player?.Name,
                //            AwayPlayerName = awayFixtureRoster?.BowlingPlayerSeason?.Player?.Name
                //        });

                //        if (homeFixtureRoster.BowlingGames != null && homeFixtureRoster.BowlingGames.Count > 0 && awayFixtureRoster.BowlingGames != null && awayFixtureRoster.BowlingGames.Count > 0)
                //        {
                //            foreach (var homeGame in homeFixtureRoster.BowlingGames)
                //            {
                //                var awayGame = awayFixtureRoster.BowlingGames.FirstOrDefault(e => e.Game == homeGame.Game);
                //                bowlingGameResults.Add(new BowlingGameResult
                //                {
                //                    BowlingRosterID1 = homeGame.BowlingRosterID,
                //                    BowlingRoster1 = homeGame.BowlingRoster,
                //                    BowlingRosterID2 = awayGame.BowlingRosterID,
                //                    BowlingRoster2 = awayGame.BowlingRoster,
                //                    Game = homeGame.Game,
                //                    Position = homeFixtureRoster.Position,
                //                    Score1 = homeGame.Score,
                //                    Score2 = awayGame.Score,
                //                    Winner = homeGame.Score > awayGame.Score ? homeGame.BowlingRosterID : awayGame.Score > homeGame.Score ? awayGame.BowlingRosterID : null
                //                });

                //                homeGame.Win = homeGame.Score > awayGame.Score;
                //                awayGame.Win = awayGame.Score > homeGame.Score;
                //            }

                //            fixture.BowlingRosters.ForEach(e => e.BowlingGameResults = bowlingGameResults.Where(d => d.BowlingRosterID1 == e.ID || d.BowlingRosterID2 == e.ID).ToList());

                //        }
                //    }

                //    fixture.BowlingGameResults = bowlingGameResults;

                //    //foreach (var homeRoster in fixture.BowlingRosters.Where(e => e.TeamID == fixture.HomeTeamID))
                //    //{
                //    //    var awayRoster = fixture.BowlingRosters.FirstOrDefault(e => e.Position == homeRoster.Position && e.TeamID == fixture.AwayTeamID);

                //    //    bowlingRosterLists.Add(new BowlingRosterListView
                //    //    {
                //    //        FixtureID = homeRoster.BowlingFixtureID,
                //    //        HomeTeamID = homeRoster.TeamID,
                //    //        AwayTeamID = awayRoster.TeamID,
                //    //        HomePlayerName = homeRoster?.BowlingPlayerSeason?.Player?.Name,
                //    //        AwayPlayerName = awayRoster?.BowlingPlayerSeason?.Player?.Name
                //    //    });
                //    //}

                //    fixture.BowlingRosterList = bowlingRosterLists;

                //    //fixture.BowlingScore.HomeTeamTotalPoints = fixture.BowlingScore.HomeTeamMatchPoints + fixture.BowlingScore.HomeTeamPoints;
                //    //fixture.BowlingScore.AwayTeamTotalPoints = fixture.BowlingScore.AwayTeamMatchPoints + fixture.BowlingScore.AwayTeamPoints;

                //    fixture.HomeTeam.Name = !String.IsNullOrEmpty(fixture.HomeTeam.Alias) ? fixture.HomeTeam.Alias : fixture.HomeTeam.Name;
                //    fixture.AwayTeam.Name = !String.IsNullOrEmpty(fixture.AwayTeam.Alias) ? fixture.AwayTeam.Alias : fixture.AwayTeam.Name;
                //}

                fixtures.ForEach(e => e.HomeTeam.Name = !String.IsNullOrEmpty(e.HomeTeam.Alias) ? e.HomeTeam.Alias : e.HomeTeam.Name);
                fixtures.ForEach(e => e.AwayTeam.Name = !String.IsNullOrEmpty(e.AwayTeam.Alias) ? e.AwayTeam.Alias : e.AwayTeam.Name);

                return fixtures;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Cricket League Fixtures");
            }

            return null;
        }

        public async Task<IEnumerable<CricketFixture>> GetCricketFixturesByLeague(string leagueID)
        {
            try
            {
                var fixtures = await _context.CricketFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.CricketRosters)
                .Include(e => e.MatchInnings)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Where(e => e.LeagueID == leagueID).ToListAsync();

                return fixtures;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Cricket League Fixtures");
            }

            return null;
        }

        public async Task<IEnumerable<spLiveFixtures>> GetLive()
        {
            try
            {
                string sqlQuery = "EXEC [dbo].[spLiveFixtures] ";

                var fixtures = await _context.Set<spLiveFixtures>().FromSqlRaw(sqlQuery).ToListAsync();

                return fixtures;

            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<spLiveCricketFixtures>> GetLiveCricket()
        {
            try
            {
                string sqlQuery = "EXEC [dbo].[spLiveCricketFixtures] ";

                var fixtures = await _context.Set<spLiveCricketFixtures>().FromSqlRaw(sqlQuery).ToListAsync();

                return fixtures;

            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<spLiveFixtures>> GetLiveFootball()
        {
            try
            {
                string sqlQuery = "EXEC [dbo].[spLiveFootballFixtures] ";

                var fixtures = await _context.Set<spLiveFixtures>().FromSqlRaw(sqlQuery).ToListAsync();

                return fixtures;

            }
            catch (Exception ex)
            {

            }

            return null;
        }


        public async Task<IEnumerable<Fixture>> GetUpcoming()
        {
            //return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.Date > DateTime.Now.Date && (TimeSpan.Parse(e.Time)) > DateTime.Now.TimeOfDay && (e.AwayTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7" || e.HomeTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7")).ToListAsync();
             var fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .Include(e => e.Sport)
                .Where(e => e.Date > DateTime.Now.Date).ToListAsync();

            List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

            fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
            fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

            return fixtures;
        }

        public async Task<IEnumerable<Fixture>> GetPast()
        {
            var fixtures =  await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                
                .Include(e => e.Season)
                .Include(e => e.Sport)
                .Where(e => e.Date < DateTime.Now.Date && (TimeSpan.Parse(e.Time)) < DateTime.Now.TimeOfDay && (e.AwayTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7" || e.HomeTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7")).ToListAsync();

            List<Coach> coaches = await _context.Coaches
                    .ToListAsync();

            fixtures.ForEach(e => e.HomeTeam.Coaches = coaches?.Where(x => x.TeamID == e.HomeTeamID).ToList());
            fixtures.ForEach(e => e.AwayTeam.Coaches = coaches?.Where(x => x.TeamID == e.AwayTeamID).ToList());

            return fixtures;
        }

        public async Task Insert(Fixture item)
        {
            try
            {
                Season season = await _context.Seasons.FirstOrDefaultAsync(e => e.IsCurrent);

                item.SeasonID = season.ID;

                _context.Fixtures.Add(item);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            { }
        }

        public async Task Insert(CricketFixture item)
        {
            try
            {
                Season season = await _context.Seasons.FirstOrDefaultAsync(e => e.IsCurrent);

                item.SeasonID = season.ID;

                _context.CricketFixtures.Add(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { }
        }

        public async Task<ImportCricketFixtures> UploadCricketFixtures(IFormFile file)
        {
            try
            {
                List<CricketFixtures> errorFixtures = new List<CricketFixtures>();
                List<CricketFixture> fixtures = new List<CricketFixture>();
                List<Team> teams = await teamRepository.GetCricketTeams();
                List<Field> fields = await _context.Fields.ToListAsync();
                List<MatchType> matchTypes = await _context.MatchTypes.ToListAsync();
                List<League> leagues = await leagueRepository.GetCricketLeagues();
                List<Season> seasons = await seasonRepository.GetCricketSeason();
                League league = null;
                Field field = null;
                MatchType matchType = null;
                Team homeTeam = null;
                Team awayTeam = null;
                Season season = null;
                string TBD = await settingRepository.GetString(Constants.SETTING_CRICKET_TBD_ID);
                //Stream reader = file.OpenReadStream();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;
                    var records = csv.GetRecords<CricketFixtures>();

                    foreach(var record in records)
                    {
                        try
                        {
                            league = leagues.FirstOrDefault(e => e.Name == record.League.Trim());
                            field = fields.FirstOrDefault(e => e.Name == record.Field.Trim());
                            matchType = matchTypes.FirstOrDefault(e => e.Name == record.MatchType.Trim());

                            if (league.Name == "Cup Match")
                            {
                                homeTeam = teams.FirstOrDefault(e => (e.Name.Replace("'", "") == record.Home.Replace("'", "").Trim() || e.Alias == record.Home.Trim()) && e.League.Name == "Cup Match");
                                awayTeam = teams.FirstOrDefault(e => (e.Name.Replace("'", "") == record.Away.Replace("'", "").Trim() || e.Alias == record.Away.Trim()) && e.League.Name == "Cup Match");
                            }
                            else
                            {
                                homeTeam = teams.FirstOrDefault(e => (e.Name.Replace("'", "") == record.Home.Replace("'", "").Trim() || e.Alias == record.Home.Trim()) && e.League.Name != "Cup Match");
                                awayTeam = teams.FirstOrDefault(e => (e.Name.Replace("'", "") == record.Away.Replace("'", "").Trim() || e.Alias == record.Away.Trim()) && e.League.Name != "Cup Match");
                            }

                            season = seasons.FirstOrDefault(e => e.IsCurrent);

                            fixtures.Add(new CricketFixture
                            {
                                Date = record.Date,
                                Time = record.Time.AddHours(4).ToString("HH:mm:ss"),
                                HomeTeamID = homeTeam != null ? homeTeam.ID : TBD,
                                AwayTeamID = awayTeam != null ? awayTeam.ID : TBD,
                                LeagueID = league.ID,
                                MatchTypeID = matchType.ID,
                                FieldID = field.ID,
                                SeasonID = season.ID
                            });
                        }
                        catch(Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorFixtures.Add(record);

                        }
                    }

                    try
                    {
                        await AddFixtures(fixtures);
                    }
                    catch (Exception ex)
                    {
                        return new ImportCricketFixtures
                        {
                            Message = "Error importing schedule to database.",
                            Exception = ex.Message
                        };
                    }
                }

                if(errorFixtures != null && errorFixtures.Count > 0)
                {

                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorFixtures);
                        streamWriter.Flush();

                        return new ImportCricketFixtures
                        {
                            Message = "Successfully imported schedule with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorFixtures,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportCricketFixtures
                {
                    Message = "Successfully imported schedule!"
                };

            }
            catch (Exception ex)
            {
                return new ImportCricketFixtures
                {
                    Message = "Error importing schedule!",
                    Exception = ex.Message
                };
            }
        }

        public async Task<ImportFootballFixtures> UploadFootballFixtures(IFormFile file)
        {
            try
            {
                List<Fixtures> errorFixtures = new List<Fixtures>();
                List<Fixture> fixtures = new List<Fixture>();
                List<Team> teams = await teamRepository.GetFootballTeams();
                List<Field> fields = await _context.Fields.ToListAsync();
                List<MatchType> matchTypes = await _context.MatchTypes.ToListAsync();
                List<League> leagues = await leagueRepository.GetFootballLeagues();
                List<Season> seasons = await seasonRepository.GetFootballSeason();
                League league = null;
                Field field = null;
                MatchType matchType = null;
                Team homeTeam = null;
                Team awayTeam = null;
                Season season = null;
                string TBD = await settingRepository.GetString(Constants.SETTING_FOOTBALL_TBD_ID);

                //Stream reader = file.OpenReadStream();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;
                    var records = csv.GetRecords<Fixtures>();

                    foreach (var record in records)
                    {
                        try
                        {
                            league = leagues.FirstOrDefault(e => e.Name == record.League.Trim());
                            field = fields.FirstOrDefault(e => e.Name == record.Field.Trim());
                            matchType = matchTypes.FirstOrDefault(e => e.Name == record.MatchType.Trim());
                            homeTeam = teams.FirstOrDefault(e => (e.Name.Replace("'", "") == record.Home.Replace("'", "").Trim() || e.Alias == record.Home.Trim()));
                            awayTeam = teams.FirstOrDefault(e => e.Name.Replace("'", "") == record.Away.Replace("'", "").Trim() || e.Alias == record.Away.Trim());
                            season = seasons.FirstOrDefault(e => e.IsCurrent);

                            fixtures.Add(new Fixture
                            {
                                Date = record.Date,
                                Time = record.Time.AddHours(4).ToString("HH:mm:ss"),
                                HomeTeamID = homeTeam != null ? homeTeam.ID : TBD,
                                AwayTeamID = awayTeam != null ? awayTeam.ID : TBD,
                                LeagueID = league.ID,
                                MatchTypeID = matchType.ID,
                                FieldID = field.ID,
                                SeasonID = season.ID,
                                SportID = season.SportID
                            });
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorFixtures.Add(record);

                        }
                    }

                    try
                    {
                        await AddFixtures(fixtures);
                    }
                    catch(Exception ex)
                    {
                        return new ImportFootballFixtures
                        {
                            Message = "Error importing schedule to database.",
                            Exception = ex.Message
                        };
                    }
                }

                if (errorFixtures != null && errorFixtures.Count > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorFixtures);
                        streamWriter.Flush();

                        return new ImportFootballFixtures
                        {
                            Message = "Successfully imported schedule with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorFixtures,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportFootballFixtures
                {
                    Message = "Successfully imported schedule!"
                };

            }
            catch (Exception ex)
            {
                return new ImportFootballFixtures
                {
                    Message = "Error importing schedule!",
                    Exception = ex.Message
                };
            }


        }

        public async Task<ImportBowlingFixtures> UploadBowlingFixtures(IFormFile file)
        {
            try
            {
                List<BowlingFixtures> errorFixtures = new List<BowlingFixtures>();
                List<BowlingFixture> fixtures = new List<BowlingFixture>();
                List<Field> fields = await _context.Fields.ToListAsync();
                List<BowlingTeam> teams = await teamRepository.GetBowlingTeams();
                List<MatchType> matchTypes = await _context.MatchTypes.ToListAsync();
                List<League> leagues = await leagueRepository.GetBowlingLeagues();
                List<Season> seasons = await seasonRepository.GetBowlingSeason();
                League league = null;
                Field field = null;
                MatchType matchType = null;
                BowlingTeam homeTeam = null;
                BowlingTeam awayTeam = null;
                Season season = null;

                //Stream reader = file.OpenReadStream();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;
                    var records = csv.GetRecords<BowlingFixtures>();

                    foreach (var record in records)
                    {
                        try
                        {
                            league = leagues.FirstOrDefault(e => e.Name == record.League.Trim());
                            field = fields.FirstOrDefault(e => e.Name == record.Field.Trim());
                            matchType = matchTypes.FirstOrDefault(e => e.Name == record.MatchType.Trim());
                            homeTeam = teams.FirstOrDefault(e => (e.TeamID == record.HomeTeamID));
                            awayTeam = teams.FirstOrDefault(e => (e.TeamID == record.AwayTeamID));
                            season = seasons.FirstOrDefault(e => e.IsCurrent);

                            fixtures.Add(new BowlingFixture
                            {
                                Date = record.Date,
                                //Time = record.Time.AddHours(4).ToString("HH:mm:ss"),
                                HomeTeamID = homeTeam.ID,
                                AwayTeamID = awayTeam.ID,
                                LeagueID = league.ID,
                                MatchTypeID = matchType.ID,
                                FieldID = field.ID,
                                SeasonID = season.ID,
                                //SportID = season.SportID
                            });
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorFixtures.Add(record);

                        }
                    }

                    try
                    {
                        await AddFixtures(fixtures);
                    }
                    catch (Exception ex)
                    {
                        return new ImportBowlingFixtures
                        {
                            Message = "Error importing schedule to database.",
                            Exception = ex.Message
                        };
                    }
                }

                if (errorFixtures != null && errorFixtures.Count > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorFixtures);
                        streamWriter.Flush();

                        return new ImportBowlingFixtures
                        {
                            Message = "Successfully imported schedule with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorFixtures,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportBowlingFixtures
                {
                    Message = "Successfully imported schedule!"
                };

            }
            catch (Exception ex)
            {
                return new ImportBowlingFixtures
                {
                    Message = "Error importing schedule!",
                    Exception = ex.Message
                };
            }


        }

        public async Task Update(Fixture item)
        {
            try
            { 
                _context.Fixtures.Update(item);

                _context.Matches.Update(item.Match);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Fixture");
            }
        }

        public async Task Update(CricketFixture item)
        {
            try
            {
                var currentFixture = await _context.CricketFixtures.FirstOrDefaultAsync(e => e.ID == item.ID && e.MatchInnings.Count > 1);

                if (currentFixture != null && !currentFixture.End.HasValue && item.End.HasValue)
                {
                    try
                    {
                        _context.Database.BeginTransaction();

                        if (item.MatchType.Name == "One 50 Overs")
                        {
                            //if (inningScores.Count > 1 && inningScores.Any(e => e.Overs == 50))
                            //{
                            //    if (inningScores[0].Overs == inningScores[1].Overs)
                            //    {
                            //        fixture.MatchResult = inningScores[0].Runs > inningScores[1].Runs ? inningScores[0].Team.Name + " won by " + (inningScores[0].Runs - inningScores[1].Runs) + " runs" : inningScores[1].Team.Name + " won by " + (inningScores[1].Runs - inningScores[0].Runs) + " runs";
                            //    }
                            //    else
                            //    {
                            //        fixture.MatchResult = inningScores[0].Overs < inningScores[1].Overs ? inningScores[0].Team.Name + " won by " + (10 - inningScores[0].Wickets) + " wickets" : inningScores[1].Team.Name + " won by " + (10 - inningScores[1].Wickets) + " wickets";
                            //    }
                            //}
                        }
                        else if (item.MatchType.Name == "T20")
                        {
                            if (currentFixture.MatchInnings.Count > 1 && (currentFixture.MatchInnings.Any(e => e.Over == 20) || currentFixture.MatchInnings.Any(e => e.Wicket == 10)))
                            {
                                var cricketLeagueStanding = await _context.CricketLeagueStandings.Where(e => e.TeamID == currentFixture.HomeTeamID || e.TeamID == currentFixture.AwayTeamID).ToListAsync();

                                if (currentFixture.MatchInnings[0].Over == currentFixture.MatchInnings[1].Over || (currentFixture.MatchInnings[0].Wicket == 10 && currentFixture.MatchInnings[1].Wicket == 10))
                                {
                                    if (currentFixture.MatchInnings[0].Run > currentFixture.MatchInnings[1].Run)
                                    {
                                        var winningTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[0].BattingTeamID);
                                        winningTeam.Played = winningTeam.Played + 1;
                                        winningTeam.Wins = winningTeam.Wins + 1;
                                        winningTeam.Points = winningTeam.Points + 10;

                                        var losingTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[1].BattingTeamID);
                                        losingTeam.Played = losingTeam.Played + 1;
                                        losingTeam.Loss = losingTeam.Loss + 1;

                                        _context.CricketLeagueStandings.Update(winningTeam);

                                        _context.CricketLeagueStandings.Update(losingTeam);

                                    }
                                    else
                                    {
                                        var winningTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[1].BattingTeamID);
                                        winningTeam.Played = winningTeam.Played + 1;
                                        winningTeam.Wins = winningTeam.Wins + 1;
                                        winningTeam.Points = winningTeam.Points + 10;

                                        var losingTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[0].BattingTeamID);
                                        losingTeam.Played = losingTeam.Played + 1;
                                        losingTeam.Loss = losingTeam.Loss + 1;

                                        _context.CricketLeagueStandings.Update(winningTeam);

                                        _context.CricketLeagueStandings.Update(losingTeam);
                                    }

                                    //fixture.MatchResult = currentFixture.MatchInnings[0].Runs > currentFixture.MatchInnings[1].Runs ? currentFixture.MatchInnings[0].Team.Name + " won by " + (currentFixture.MatchInnings[0].Runs - currentFixture.MatchInnings[1].Runs) + " runs" : currentFixture.MatchInnings[1].Team.Name + " won by " + (currentFixture.MatchInnings[1].Runs - currentFixture.MatchInnings[0].Runs) + " runs";
                                }
                                else
                                {
                                    if (currentFixture.MatchInnings[0].Wicket == 10)
                                    {
                                        var winningTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[1].BattingTeamID);
                                        winningTeam.Played = winningTeam.Played + 1;
                                        winningTeam.Wins = winningTeam.Wins + 1;
                                        winningTeam.Points = winningTeam.Points + 10;

                                        var losingTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[0].BattingTeamID);
                                        losingTeam.Played = losingTeam.Played + 1;
                                        losingTeam.Loss = losingTeam.Loss + 1;

                                        _context.CricketLeagueStandings.Update(winningTeam);

                                        _context.CricketLeagueStandings.Update(losingTeam);

                                        //fixture.MatchResult = currentFixture.MatchInnings[1].Team.Name + " won by " + (currentFixture.MatchInnings[0].Wicket - currentFixture.MatchInnings[1].Wicket) + " wickets";
                                    }
                                    else if (currentFixture.MatchInnings[1].Wicket == 10)
                                    {
                                        var winningTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[0].BattingTeamID);
                                        winningTeam.Played = winningTeam.Played + 1;
                                        winningTeam.Wins = winningTeam.Wins + 1;
                                        winningTeam.Points = winningTeam.Points + 10;

                                        var losingTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[1].BattingTeamID);
                                        losingTeam.Played = losingTeam.Played + 1;
                                        losingTeam.Loss = losingTeam.Loss + 1;

                                        _context.CricketLeagueStandings.Update(winningTeam);

                                        _context.CricketLeagueStandings.Update(losingTeam);

                                        //fixture.MatchResult = currentFixture.MatchInnings[0].Team.Name + " won by " + (currentFixture.MatchInnings[1].Wicket - currentFixture.MatchInnings[0].Wicket) + " wickets";
                                    }
                                    else
                                    {
                                        if (currentFixture.MatchInnings[0].Over < currentFixture.MatchInnings[1].Over)
                                        {
                                            var winningTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[1].BattingTeamID);
                                            winningTeam.Played = winningTeam.Played + 1;
                                            winningTeam.Wins = winningTeam.Wins + 1;
                                            winningTeam.Points = winningTeam.Points + 10;

                                            var losingTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[0].BattingTeamID);
                                            losingTeam.Played = losingTeam.Played + 1;
                                            losingTeam.Loss = losingTeam.Loss + 1;

                                            _context.CricketLeagueStandings.Update(winningTeam);

                                            _context.CricketLeagueStandings.Update(losingTeam);
                                        }
                                        else
                                        {
                                            var winningTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[0].BattingTeamID);
                                            winningTeam.Played = winningTeam.Played + 1;
                                            winningTeam.Wins = winningTeam.Wins + 1;
                                            winningTeam.Points = winningTeam.Points + 10;

                                            var losingTeam = cricketLeagueStanding.FirstOrDefault(e => e.TeamID == currentFixture.MatchInnings[1].BattingTeamID);
                                            losingTeam.Played = losingTeam.Played + 1;
                                            losingTeam.Loss = losingTeam.Loss + 1;

                                            _context.CricketLeagueStandings.Update(winningTeam);

                                            _context.CricketLeagueStandings.Update(losingTeam);
                                        }

                                        //fixture.MatchResult = currentFixture.MatchInnings[0].Over < currentFixture.MatchInnings[1].Over ? currentFixture.MatchInnings[1].Team.Name + " won by " + (currentFixture.MatchInnings[0].Wicket - currentFixture.MatchInnings[1].Wicket) + " wickets" : currentFixture.MatchInnings[0].Team.Name + " won by " + (currentFixture.MatchInnings[1].Wicket - currentFixture.MatchInnings[0].Wicket) + " wickets";
                                    }
                                }

                                _context.SaveChanges();
                            }
                        }
                        else
                        {

                        }

                        item.End = DateTime.Now.ToUniversalTime();

                        _context.CricketFixtures.Update(item);

                        _context.SaveChanges();

                        _context.Database.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        _context.Database.RollbackTransaction();
                        Debug.WriteLine(ex.Message, "Update Fixture Transaction");
                    }
                }
                else
                {
                    _context.CricketFixtures.Update(item);
                    _context.SaveChanges();
                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Fixture");
            }
        }


        public async Task AddFixtures(List<CricketFixture> items)
        {
            try
            {
                await _context.CricketFixtures.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Fixtures");
            }
        }

        public async Task AddFixtures(List<BowlingFixture> items)
        {
            try
            {
                await _context.BowlingFixtures.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Fixtures");
            }
        }

        public async Task AddFixtures(List<Fixture> items)
        {
            try
            {
                await _context.Fixtures.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Fixtures");
            }
        }

        //public async Task UpdateLiveCricketScores()
        //{
        //    try
        //    {
        //        var todaysFixtures = await _context.CricketFixtures.Where(e => e.Date == DateTime.Now.ToLocalTime().Date).ToListAsync();

        //        if (todaysFixtures != null && todaysFixtures.Count > 0)
        //        {
        //            //Check crichq
        //            HttpClient client = new HttpClient();
        //            client.DefaultRequestHeaders.Add(Constants.CricHQApiKeyName, Constants.CricHQApiKey);
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            var response = await client.GetAsync($"{Constants.CricHQEndpoint}/{Constants.CricHQFixturesEndpoint}");

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var crichqresponse = await response.Content.ReadAsStringAsync();

        //                if (!String.IsNullOrEmpty(crichqresponse))
        //                {
        //                    List<matchcenter> fixtures = JsonConvert.DeserializeObject<List<matchcenter>>(crichqresponse);

        //                }


        //                _context.Database.BeginTransaction();

        //                await _context.SaveChangesAsync();

        //                _context.Database.CommitTransaction();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _context.Database.RollbackTransaction();
        //        Debug.WriteLine(ex.Message, "Update Fixtures");
        //    }
        //}

        public async Task UpdateFixtures(List<CricketFixture> items)
        {
            try
            { 
                _context.CricketFixtures.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Fixtures");
            }
        }

        public async Task UpdateFixtures(List<Fixture> items)
        {
            try
            {
                _context.Fixtures.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Fixtures");
            }
        }

        
    }

    public class InningScore
    {
        public string TeamID { get; set; }
        public int? Runs { get; set; }
        public int? Inning { get; set; }
        public int? Wickets { get; set; }
        public decimal? Overs { get; set; }
        public Team Team { get; set; }
    }
}
