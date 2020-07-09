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
    public class SportRepository : IOnTrackRepository<Sport>
    {
        OnTrackContext _context;

        public SportRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Sport> Get(string id)
        {
            return await _context.Sports.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Sport>> GetAll()
        {
            return await _context.Sports.ToListAsync();
        }

        public Task<IEnumerable<Sport>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(Sport item)
        {
            throw new NotImplementedException();
        }

        public Task Update(Sport item)
        {
            throw new NotImplementedException();
        }
    }
}
