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
    public class MatchTypeRepository : IOnTrackRepository<MatchType>
    {
        OnTrackContext _context;

        public MatchTypeRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<MatchType> Get(string id)
        {
            return await _context.MatchTypes.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<MatchType>> GetAll()
        {
            return await _context.MatchTypes.ToListAsync();
        }

        public Task<IEnumerable<MatchType>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(MatchType item)
        {
            throw new NotImplementedException();
        }

        public Task Update(MatchType item)
        {
            throw new NotImplementedException();
        }
    }
}
