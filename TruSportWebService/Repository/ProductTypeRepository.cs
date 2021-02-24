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
    public class ProductTypeRepository : IOnTrackRepository<ProductType>
    {
        OnTrackContext _context;

        public ProductTypeRepository(OnTrackContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                var ProductType = await _context.ProductTypes.FirstOrDefaultAsync(e => e.ID == id);

                _context.ProductTypes.Remove(ProductType);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<ProductType> Get(string id)
        {
            return await _context.ProductTypes.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<ProductType>> GetAll()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        public Task<IEnumerable<ProductType>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Insert(ProductType item)
        {
            try
            {
                _context.ProductTypes.Add(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> Update(ProductType item)
        {
            try
            {
                _context.ProductTypes.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        Task IOnTrackRepository<ProductType>.Delete(string id)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<ProductType>.Insert(ProductType item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<ProductType>.Update(ProductType item)
        {
            throw new NotImplementedException();
        }
    }
}
