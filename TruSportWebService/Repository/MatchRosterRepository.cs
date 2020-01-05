using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class MatchRosterRepository : IOnTrackRepository<MatchRoster>
    {
        OnTrackContext _context;

        public MatchRosterRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<MatchRoster> Get(string id)
        {
            return await _context.MatchRosters.Include("Fixture").Include("Player").Include("Team").Include("SubstitutePlayer").Include(e => e.MatchStats).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<MatchRoster> GetByPlayerID(string fixtureID, string playerID)
        {
            return await _context.MatchRosters.Include("Fixture").Include("Player").Include("Team").Include("SubstitutePlayer").Include(e => e.MatchStats).Where(e => e.FixtureID == fixtureID && e.PlayerID == playerID).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MatchRoster>> GetAll()
        {
            return await _context.MatchRosters.Include("Fixture").Include("Player").Include("Team").Include("SubstitutePlayer").Include(e => e.MatchStats).ToListAsync();
        }

        public async Task<IEnumerable<MatchRoster>> GetByFixture(string fixtureID)
        {
            return await _context.MatchRosters.Include("Fixture").Include("Player").Include("Team").Include("SubstitutePlayer").Include(e => e.MatchStats).Where(e => e.FixtureID == fixtureID).ToListAsync();
        }

        public async Task<IEnumerable<MatchRoster>> GetByFixtureByTeam(string fixtureID, string teamID)
        {
            return await _context.MatchRosters.Include("Fixture").Include("Player").Include("Team").Include("SubstitutePlayer").Include(e => e.MatchStats).Where(e => e.FixtureID == fixtureID && e.TeamID == teamID).ToListAsync();
        }

        public async Task Insert(MatchRoster item)
        {
            try
            {
                _context.MatchRosters.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
        }

        public async Task InsertAll(List<MatchRoster> items)
        {
            foreach (var item in items)
            {
                _context.MatchRosters.Add(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(MatchRoster item)
        {
            _context.MatchRosters.Update(item);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAll(List<MatchRoster> items)
        {
            foreach (var item in items)
            {
                _context.MatchRosters.Update(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
