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
            try
            {
                 var sport = await _context.Sports.FirstOrDefaultAsync(e => e.ID == id);

                return sport;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport");
            }

            return null;
        }

        public async Task<IEnumerable<Sport>> GetAll()
        {
            try
            {
                var sports = await _context.Sports.ToListAsync();

                return sports;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport");
            }

            return null;
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
