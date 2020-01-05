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
    public class FieldRepository : IOnTrackRepository<Field>
    {
        OnTrackContext _context;

        public FieldRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Field> Get(string id)
        {
            return await _context.Fields.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Field>> GetAll()
        {
            return await _context.Fields.ToListAsync();
        }

        public Task<IEnumerable<Field>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(Field item)
        {
            throw new NotImplementedException();
        }

        public Task Update(Field item)
        {
            throw new NotImplementedException();
        }
    }
}
