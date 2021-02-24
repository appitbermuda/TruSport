using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Product product = new Product();
            try
            {

            product = await _context.Products
                .Include(e => e.ProductType).ThenInclude(e => e.MatchType)
                .Include(e => e.ProductType).ThenInclude(e => e.Sport)
                .Include(e => e.TicketCompany).FirstOrDefaultAsync(e => e.ID == id);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            List<Product> products = new List<Product>();
            try
            {
                products = await _context.Products
                    .Include(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.ProductType).ThenInclude(e => e.Sport)                    
                    .Include(e => e.TicketCompany).ToListAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return products;
        }

        public async Task<bool> Insert(Product item)
        {
            try
            {
                _context.Products.Add(item);

                await _context.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            { }

            return false;
        }

        public async Task<bool> Update(Product item)
        {
            try
            {
                _context.Products.Update(item);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            { }

            return false;
        }

        Task IOnTrackRepository<Product>.Insert(Product item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Product>.Update(Product item)
        {
            throw new NotImplementedException();
        }
    }
}
