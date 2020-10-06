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
    public class FixtureProductRepository : IOnTrackRepository<FixtureProduct>
    {
        OnTrackContext _context;

        public FixtureProductRepository(OnTrackContext context)
        {
            _context = context;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<FixtureProduct> Get(string id)
        {
            try
            {
                return await _context.FixtureProducts
                    .Include(e => e.Fixture)
                    .Include(e => e.Product)
                    .FirstOrDefaultAsync(e => e.ID == id);
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<FixtureProduct>> GetAll()
        {
            try
            {
                List<FixtureProduct> fixtureProducts =  await _context.FixtureProducts
                    .Include(e => e.Fixture)
                    .Include(e => e.Product).ToListAsync();

                return fixtureProducts;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<FixtureProduct>> GetByTeam(string teamID)
        {
            try
            {
                List<FixtureProduct> fixtureProducts = await _context.FixtureProducts
                    .Include(e => e.Fixture)
                    .Include(e => e.Product)
                    .Where(e => e.Product.TeamID == teamID)
                    .ToListAsync();

                return fixtureProducts;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public Task Insert(FixtureProduct item)
        {
            throw new NotImplementedException();
        }

        public async Task Update(FixtureProduct item)
        {
            try
            {
                _context.FixtureProducts.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture Product");
            }
        }
    }
}
