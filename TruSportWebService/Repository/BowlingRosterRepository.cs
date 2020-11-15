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
                return await _context.BowlingRosters
                    .Include(e => e.BowlingFixture)
                    .Include(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                    .Include(e => e.BowlingPlayerSeason).ThenInclude(e => e.Team)
                    .Include(e => e.BowlingPlayerSeason).ThenInclude(e => e.Season)
                    .Include(e => e.Team)
                    .Where(e => e.BowlingPlayerSeason.Season.IsCurrent).ToListAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "All Bowling Roster");
            }

            return null;
        }

        public async Task<IEnumerable<BowlingRoster>> GetByFixture(string fixtureID)
        {
            return await _context.BowlingRosters.Include("BowlingFixture").Include("Player").Include("Team").Include("SubstitutePlayer").Where(e => e.BowlingFixtureID == fixtureID).ToListAsync();
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
