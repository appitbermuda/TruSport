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
    public class MatchRepository : IOnTrackRepository<Match>
    {
        OnTrackContext _context;

        public MatchRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Match> Get(string id)
        {
            return await _context.Matches.Include("Fixture").FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Match>> GetAll()
        {
            return await _context.Matches.Include("Fixture").ToListAsync();
        }

        public async Task<Match> GetByFixture(string fixtureID)
        {
            return await _context.Matches.Include("Fixture").FirstOrDefaultAsync(e => e.FixtureID == fixtureID);
        }

        public Task Insert(Match item)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Match item)
        {
            try
            {
                _context.Matches.Update(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match");
            }
        }
    }
}
