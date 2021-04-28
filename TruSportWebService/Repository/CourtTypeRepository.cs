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
    public class CourtTypeRepository : IOnTrackRepository<CourtType>
    {
        OnTrackContext _context;

        public CourtTypeRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<CourtType> Get(string id)
        {
            return await _context.CourtTypes.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<CourtType>> GetAll()
        {
            return await _context.CourtTypes.ToListAsync();
        }

        public Task<IEnumerable<CourtType>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(CourtType item)
        {
            throw new NotImplementedException();
        }

        public Task Update(CourtType item)
        {
            throw new NotImplementedException();
        }
    }
}
