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
    public class MatchTicketRepository : IOnTrackRepository<MatchTicket>
    {
        OnTrackContext _context;

        public MatchTicketRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<MatchTicket> Get(string FixtureID)
        {
            MatchTicket matchTicket = new MatchTicket();

            try
            {
                List<Product> products = await _context.Products
                                        .Include(e => e.ProductType).ThenInclude(e => e.Sport)
                                        .Include(e => e.ProductType).ThenInclude(e => e.MatchType)
                                        .Include(e => e.Team).ToListAsync();

                Fixture fixture = await _context.Fixtures
                                        .Include(e => e.HomeTeam)
                                        .Include(e => e.AwayTeam)
                                        .Include(e => e.Field)
                                        .Include(e => e.League)
                                        .Include(e => e.MatchType)
                                        .Include(e => e.Season)
                                        .Include(e => e.Sport).FirstOrDefaultAsync(e => e.ID == FixtureID && e.Season.IsCurrent && e.Date == DateTime.Now.Date.AddHours(-4));
                //.Include(e => e.Sport).Where(e => products.Any(p => (p.TeamID == e.HomeTeamID && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null)) && e.Season.IsCurrent && e.Date < DateTime.Now.Date.AddDays(3) && e.Date > DateTime.Now.Date.AddDays(-1)).ToListAsync();

                if (fixture != null)
                {
                    var product = products.FirstOrDefault(p => (p.TeamID == fixture.HomeTeamID && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null));

                    if (product != null)
                    {
                        matchTicket.Fixture = fixture;
                        matchTicket.Product = product;
                    }
                }
            }
            catch (Exception ex)
            { }

            return matchTicket;
        }

        public async Task<IEnumerable<MatchTicket>> GetAll()
        {
            List<MatchTicket> matchTickets = new List<MatchTicket>();

            try
            {
                List<Product> products = await _context.Products
                                        .Include(e => e.ProductType).ThenInclude(e => e.Sport)
                                        .Include(e => e.ProductType).ThenInclude(e => e.MatchType)
                                        .Include(e => e.Inventory)
                                        .Include(e => e.Team).ToListAsync();

                List<Fixture> fixtures = await _context.Fixtures
                                        .Include(e => e.HomeTeam)
                                        .Include(e => e.AwayTeam)
                                        .Include(e => e.Field)
                                        .Include(e => e.League)
                                        .Include(e => e.MatchType)
                                        .Include(e => e.Season)
                                        .Include(e => e.Sport).Where(e => e.Season.IsCurrent && e.Date == DateTime.Now.Date).ToListAsync();
                //.Include(e => e.Sport).Where(e => products.Any(p => (p.TeamID == e.HomeTeamID && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null)) && e.Season.IsCurrent && e.Date == DateTime.Now.Date.AddHours(-4)).ToListAsync();
                //.Include(e => e.Sport).Where(e => products.Any(p => (p.TeamID == e.HomeTeamID && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null)) && e.Season.IsCurrent && e.Date < DateTime.Now.Date.AddDays(3) && e.Date > DateTime.Now.Date.AddDays(-1)).ToListAsync();

                foreach (var fixture in fixtures)
                {
                    var product = products.FirstOrDefault(p => (p.TeamID == fixture.HomeTeamID && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null));

                    if (product != null)
                    {
                        matchTickets.Add(new MatchTicket
                        {
                            Fixture = fixture,
                            Product = product
                        });
                    }
                }

            }
            catch(Exception ex)
            { }

            return matchTickets;
        }

        public async Task Insert(MatchTicket item)
        {
            try
            {
                //_context.MatchTickets.Add(item);

                //await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            { }
        }

        public async Task Update(MatchTicket item)
        {
            try
            {
                //_context.MatchTickets.Update(item);

                //await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { }
        }
    }
}
