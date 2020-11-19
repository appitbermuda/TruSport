using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class BowlingRosterRepository : IOnTrackRepository<BowlingRoster>
    {
        OnTrackContext _context;

        public BowlingRosterRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<BowlingRoster> Get(string id)
        {
            return await _context.BowlingRosters.Include("BowlingFixture").Include("Player").Include("Team").Include("SubstitutePlayer").FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<BowlingRoster> GetByPlayerID(string fixtureID, string playerID)
        {
            return await _context.BowlingRosters.Include("BowlingFixture").Include("Player").Include("Team").Include("SubstitutePlayer").Where(e => e.BowlingFixtureID == fixtureID).FirstOrDefaultAsync();
        }

        public async Task<List<BowlingRoster>> GetAll()
        {
            try
            {
                var rosters = await _context.BowlingRosters
                    .Include(e => e.BowlingFixture)
                    .Include(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                    .Include(e => e.BowlingPlayerSeason).ThenInclude(e => e.Team)
                    .Include(e => e.BowlingPlayerSeason).ThenInclude(e => e.Season)
                    .Include(e => e.Team)
                    .Where(e => e.BowlingPlayerSeason.Season.IsCurrent).ToListAsync();

                return rosters;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "All Bowling Roster");
            }

            return null;
        }

        public async Task<IEnumerable<BowlingRoster>> GetByFixture(string fixtureID)
        {
            
            try
            {
                List<BowlingGameResult> bowlingGameResults = new List<BowlingGameResult>();

                var rosters = await _context.BowlingRosters
                    .Include(e => e.BowlingFixture)
                    .Include(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                    .Include(e => e.BowlingPlayerSeason).ThenInclude(e => e.Team)
                    .Include(e => e.BowlingPlayerSeason).ThenInclude(e => e.Season)
                    .Include(e => e.Team)
                    .Include(e => e.BowlingGames)
                    .Where(e => e.BowlingFixtureID == fixtureID).ToListAsync();

                foreach(var homeRoster in rosters)
                {
                    foreach(var awayRoster in rosters)
                    {
                        if(homeRoster.Position == awayRoster.Position && homeRoster.TeamID != awayRoster.TeamID)
                        {
                            foreach(var homeGame in homeRoster.BowlingGames)
                            {
                                foreach(var awayGame in awayRoster.BowlingGames)
                                {
                                    if(homeGame.Game == awayGame.Game)
                                    {
                                        bowlingGameResults.Add(new BowlingGameResult
                                        {
                                            BowlingRosterID1 = homeGame.BowlingRosterID,
                                            BowlingRoster1 = homeGame.BowlingRoster,
                                            BowlingRosterID2 = awayGame.BowlingRosterID,
                                            BowlingRoster2 = awayGame.BowlingRoster,
                                            Game = homeGame.Game,
                                            Score1 = homeGame.Score,
                                            Score2 = awayGame.Score,
                                            Winner = homeGame.Score > awayGame.Score ? homeGame.BowlingRosterID : awayGame.Score > homeGame.Score ? awayGame.BowlingRosterID : null
                                        });

                                        homeGame.Win = homeGame.Score > awayGame.Score;
                                        awayGame.Win = awayGame.Score > homeGame.Score;

                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    }
                }

                rosters.ForEach(e => e.BowlingGameResults = bowlingGameResults.Where(d => d.BowlingRosterID1 == e.ID || d.BowlingRosterID2 == e.ID).ToList());

                return rosters;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "All Bowling Fixture Roster");
            }

            return null;
        }

        public async Task<IEnumerable<BowlingRoster>> GetByFixtureByTeam(string fixtureID, string teamID)
        {
            return await _context.BowlingRosters.Include("BowlingFixture").Include("Player").Include("Team").Include("SubstitutePlayer").Where(e => e.BowlingFixtureID == fixtureID && e.TeamID == teamID).ToListAsync();
        }

        public async Task Insert(BowlingRoster item)
        {
            try
            {
                _context.BowlingRosters.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
        }

        public async Task InsertAll(List<BowlingRoster> items)
        {
            foreach (var item in items)
            {
                _context.BowlingRosters.Add(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(BowlingRoster item)
        {
            _context.BowlingRosters.Update(item);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAll(List<BowlingRoster> items)
        {
            foreach (var item in items)
            {
                _context.BowlingRosters.Update(item);
                await _context.SaveChangesAsync();
            }
        }

        Task<IEnumerable<BowlingRoster>> IOnTrackRepository<BowlingRoster>.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
