using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Repository
{
    public class InventoryRepository : IOnTrackRepository<Inventory>
    {
        OnTrackContext _context;

        public InventoryRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Inventory> Get(string id)
        {
            return await _context.Inventorys
                .Include(e => e.Product).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Inventory>> GetAll()
        {
            return await _context.Inventorys
                .Include(e => e.Product).ToListAsync();
        }

        public async Task Insert(Inventory item)
        {
            try
            {
                _context.Inventorys.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            { }
        }

        public async Task Update(Inventory item)
        {
            try
            {
                _context.Inventorys.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { }
        }
    }
}
