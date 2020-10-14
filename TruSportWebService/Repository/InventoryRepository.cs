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
        
        public async Task<int> TeamInventoryLevel(string teamid)
        {
            try
            {
                //var product = await _context.Products.FirstOrDefaultAsync(e => e.TeamID == teamid);

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TeamID == teamid);

                var orders = await _context.OrderDetails
                    .Include(e => e.FixtureProduct)
                    .Where(e => e.FixtureProduct.Product.TeamID == teamid)
                    .ToListAsync();

                var orderCount = orders.Sum(e => e.Qty);

                
                return ticketConfiguration.Stock - orderCount;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Check Inventory");
            }

            return 0;
        }

        public async Task<int> TicketInventoryLevel(string fixtureID)
        {
            try
            {
                //var product = await _context.Products.FirstOrDefaultAsync(e => e.TeamID == teamid);
                var fixtureProduct = await _context.FixtureProducts.Include(e => e.Product).FirstOrDefaultAsync(e => e.FixtureID == fixtureID);

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TeamID == fixtureProduct.Product.TeamID);

                //var orders = await _context.OrderDetails
                //    .Include(e => e.FixtureProduct)
                //    .Where(e => e.FixtureProduct.Product.TeamID == teamid)
                //    .ToListAsync();

                var matchTicketsList = await _context.MatchTickets
                    .Where(e => e.FixtureProduct.FixtureID == fixtureID)
                    .ToListAsync();

                var ticketCount = matchTicketsList.Count();


                return (ticketConfiguration.Stock - ticketCount) < 0 ? 0 : (ticketConfiguration.Stock - ticketCount);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Check Inventory");
            }

            return 0;
        }

        public async Task<bool> CheckInventory(string fixtureID)
        {
            try
            {
                var fixtureProduct = await _context.FixtureProducts
                    .Include(e => e.Product)
                    .Include(e => e.Fixture)
                    .FirstOrDefaultAsync(e => e.FixtureID == fixtureID);

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TeamID == fixtureProduct.Product.TeamID);

                var matchTicketsList = await _context.MatchTickets
                    .Where(e => e.FixtureProduct.FixtureID == fixtureID)
                    .ToListAsync();

                //var orders = await _context.OrderDetails
                //    .Include(e => e.FixtureProduct)
                //    .Where(e => e.FixtureProduct.ProductID == productID)
                //    .ToListAsync();

                var ticketCount = matchTicketsList.Count();

                if (ticketCount < ticketConfiguration.Stock)
                    return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Check Inventory");
            }

            return false;
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
