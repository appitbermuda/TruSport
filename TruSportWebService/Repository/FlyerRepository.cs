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
    public class FlyerRepository : IOnTrackRepository<Flyer>
    {
        OnTrackContext _context;

        public FlyerRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Flyer> Get(string id)
        {
            return await _context.Flyers.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Flyer>> GetAll()
        {
            return await _context.Flyers.Where(e => e.ExpiryDate > DateTime.Now).ToListAsync();
        }

        public async Task Insert(Flyer item)
        {
            try
            {
                _context.Flyers.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            { }
        }

        public async Task Update(Flyer item)
        {
            try
            {
                _context.Flyers.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { }
        }
    }
}
