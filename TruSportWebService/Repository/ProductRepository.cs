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
    public class ProductRepository : IOnTrackRepository<Product>
    {
        OnTrackContext _context;

        public ProductRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> Get(string id)
        {
            return await _context.Products.Include(e => e.ProductType).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.Include(e => e.ProductType).ToListAsync();
        }

        public async Task Insert(Product item)
        {
            try
            {
                _context.Products.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            { }
        }

        public async Task Update(Product item)
        {
            try
            {
                _context.Products.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { }
        }
    }
}
