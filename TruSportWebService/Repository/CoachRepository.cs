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
    public class CoachRepository : IOnTrackRepository<Coach>
    {
        OnTrackContext _context;

        public CoachRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Coach> Get(string id)
        {
            return await _context.Coaches.Include("Team").FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Coach>> GetAll()
        {
            return await _context.Coaches.Include("Team").ToListAsync();
        }

        public async Task<IEnumerable<Coach>> GetByTeam(string teamID)
        {
            return await _context.Coaches.Include("Team").Where(e => e.TeamID == teamID).ToListAsync();
        }

        public Task Insert(Coach item)
        {
            throw new NotImplementedException();
        }

        public Task Update(Coach item)
        {
            throw new NotImplementedException();
        }
    }
}
