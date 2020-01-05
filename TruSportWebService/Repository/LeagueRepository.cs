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
    public class LeagueRepository : IOnTrackRepository<League>
    {
        OnTrackContext _context;

        public LeagueRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<League> Get(string id)
        {
            return await _context.Leagues.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<League>> GetAll()
        {
            return await _context.Leagues.ToListAsync();
        }

        public Task<IEnumerable<League>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(League item)
        {
            throw new NotImplementedException();
        }

        public Task Update(League item)
        {
            throw new NotImplementedException();
        }
    }
}
