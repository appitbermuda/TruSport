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

        public async Task<FixtureProduct> Get(string FixtureID)
        {
            FixtureProduct fixtureProduct = new FixtureProduct();

            try
            {
                fixtureProduct = await _context.FixtureProducts
                    .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.Team)
                    .FirstOrDefaultAsync(e => e.Fixture.ID == FixtureID);

                //List<Product> products = await _context.Products
                //                        .Include(e => e.ProductType).ThenInclude(e => e.Sport)
                //                        .Include(e => e.ProductType).ThenInclude(e => e.MatchType)
                //                        .Include(e => e.Team).ToListAsync();

                //Fixture fixture = await _context.Fixtures
                //                        .Include(e => e.HomeTeam)
                //                        .Include(e => e.AwayTeam)
                //                        .Include(e => e.Field)
                //                        .Include(e => e.League)
                //                        .Include(e => e.MatchType)
                //                        .Include(e => e.Season)
                //                        .Include(e => e.Sport).FirstOrDefaultAsync(e => e.ID == FixtureID && e.Season.IsCurrent && e.Date == DateTime.Now.Date.AddHours(-4));
                //.Include(e => e.Sport).Where(e => products.Any(p => (p.TeamID == e.HomeTeamID && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null)) && e.Season.IsCurrent && e.Date < DateTime.Now.Date.AddDays(3) && e.Date > DateTime.Now.Date.AddDays(-1)).ToListAsync();

                //if (fixture != null)
                //{
                //    var product = products.FirstOrDefault(p => (p.TeamID == fixture.HomeTeamID && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null));

                //    if (product != null)
                //    {
                //        matchTicket.Fixture = fixture;
                //        matchTicket.Product = product;
                //    }
                //}
            }
            catch (Exception ex)
            { }

            return fixtureProduct;
        }

        public async Task<IEnumerable<FixtureProduct>> GetAll()
        {
            List<FixtureProduct> fixtureProducts = new List<FixtureProduct>();

            try
            {
                var ticketConfigurations = await _context.TicketConfigurations.ToListAsync();

                var fixtureProductsList = await _context.FixtureProducts
                    .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.Team)
                    .Where(e => DateTime.Now.Date <= e.Fixture.Date.AddDays(1))
                    .ToListAsync();

                fixtureProductsList.ForEach(e => e.Fixture.HomeTeam.Name = !String.IsNullOrEmpty(e.Fixture.HomeTeam.Alias) ? e.Fixture.HomeTeam.Alias : e.Fixture.HomeTeam.Name);
                fixtureProductsList.ForEach(e => e.Fixture.AwayTeam.Name = !String.IsNullOrEmpty(e.Fixture.AwayTeam.Alias) ? e.Fixture.AwayTeam.Alias : e.Fixture.AwayTeam.Name);

                foreach (var fixtureProduct in fixtureProductsList)
                {
                    //ticketConfig.Any(t => t.TeamID == e.Product.TeamID && DateTime.Now.Date > e.Fixture.Date.AddDays(-(t.ValidFrom)))

                    var ticketConfiguration = ticketConfigurations.FirstOrDefault(e => e.TeamID == fixtureProduct.Product.TeamID);

                    if (DateTime.Now.Date >= fixtureProduct.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                        fixtureProducts.Add(fixtureProduct);
                }
                //List<Product> products = await _context.Products
                //                        .Include(e => e.ProductType).ThenInclude(e => e.Sport)
                //                        .Include(e => e.ProductType).ThenInclude(e => e.MatchType)
                //                        .Include(e => e.Inventory)
                //                        .Include(e => e.Team).ToListAsync();

                //List<Fixture> fixtures = await _context.Fixtures
                //                        .Include(e => e.HomeTeam)
                //                        .Include(e => e.AwayTeam)
                //                        .Include(e => e.Field)
                //                        .Include(e => e.League)
                //                        .Include(e => e.MatchType)
                //                        .Include(e => e.Season)
                //                        .Include(e => e.Sport).Where(e => e.Season.IsCurrent && e.Date == DateTime.Now.Date).ToListAsync();
                //.Include(e => e.Sport).Where(e => products.Any(p => (p.TeamID == e.HomeTeamID && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null)) && e.Season.IsCurrent && e.Date == DateTime.Now.Date.AddHours(-4)).ToListAsync();
                //.Include(e => e.Sport).Where(e => products.Any(p => (p.TeamID == e.HomeTeamID && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null)) && e.Season.IsCurrent && e.Date < DateTime.Now.Date.AddDays(3) && e.Date > DateTime.Now.Date.AddDays(-1)).ToListAsync();

                //foreach (var fixture in fixtures)
                //{
                //    var product = products.FirstOrDefault(p => (p.TeamID == fixture.HomeTeamID && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null));

                //    if (product != null)
                //    {
                //        matchTickets.Add(new MatchTicket
                //        {
                //            Fixture = fixture,
                //            Product = product
                //        });
                //    }
                //}

                return fixtureProducts.OrderByDescending(e => e.FixtureID);

            }
            catch (Exception ex)
            { }

            return fixtureProducts;
        }

        public async Task<IEnumerable<Fixture>> Products()
        {
            List<Fixture> fixtures = new List<Fixture>();

            try
            {
                var ticketConfigurations = await _context.TicketConfigurations.ToListAsync();

                var fixtureProductsList = await _context.FixtureProducts
                    .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.Team)
                    .Where(e => DateTime.Now.AddHours(-4).Date <= e.Fixture.Date)
                    .ToListAsync();

                fixtureProductsList.ForEach(e => e.Fixture.HomeTeam.Name = !String.IsNullOrEmpty(e.Fixture.HomeTeam.Alias) ? e.Fixture.HomeTeam.Alias : e.Fixture.HomeTeam.Name);
                fixtureProductsList.ForEach(e => e.Fixture.AwayTeam.Name = !String.IsNullOrEmpty(e.Fixture.AwayTeam.Alias) ? e.Fixture.AwayTeam.Alias : e.Fixture.AwayTeam.Name);

                foreach (var fixtureProduct in fixtureProductsList)
                {
                    //ticketConfig.Any(t => t.TeamID == e.Product.TeamID && DateTime.Now.Date > e.Fixture.Date.AddDays(-(t.ValidFrom)))

                    var ticketConfiguration = ticketConfigurations.FirstOrDefault(e => e.TeamID == fixtureProduct.Product.TeamID);

                    if (DateTime.Now.Date >= fixtureProduct.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                    {
                        var matchTicketsList = await _context.MatchTickets
                        .Where(e => e.FixtureProduct.FixtureID == fixtureProduct.FixtureID)
                        .ToListAsync();

                        var ticketCount = matchTicketsList.Count();
                        var availableTickets = (ticketConfiguration.Stock - ticketCount) < 0 ? 0 : (ticketConfiguration.Stock - ticketCount);

                        if (availableTickets > 0)
                        {
                            //if (availableTickets <= 10)
                            //{
                                fixtureProduct.Fixture.TicketAvailable = availableTickets;
                            //}

                            fixtures.Add(fixtureProduct.Fixture);
                        }
                    }
                }

                return fixtures.Distinct().ToList();
            }
            catch (Exception ex)
            { }

            return fixtures;
        }

        public async Task<IEnumerable<FixtureProduct>> Team(string teamID)
        {
            List<FixtureProduct> fixtureProducts = new List<FixtureProduct>();

            try
            {
                var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TeamID == teamID);

                var fixtureProductsList = await _context.FixtureProducts
                    .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.Team)
                    .Where(e => e.Product.TeamID == teamID && e.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.Fixture.Date).ToListAsync();



                foreach (var fixtureProduct in fixtureProductsList)
                {
                    //ticketConfig.Any(t => t.TeamID == e.Product.TeamID && DateTime.Now.Date > e.Fixture.Date.AddDays(-(t.ValidFrom)))



                    if (DateTime.Now.Date > fixtureProduct.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                        fixtureProducts.Add(fixtureProduct);
                }

                //List<Product> products = await _context.Products
                //                        .Include(e => e.ProductType).ThenInclude(e => e.Sport)
                //                        .Include(e => e.ProductType).ThenInclude(e => e.MatchType)
                //                        .Include(e => e.Inventory)
                //                        .Include(e => e.Team).Where(e => e.TeamID == teamID).ToListAsync();

                //List<Fixture> fixtures = await _context.Fixtures
                //                        .Include(e => e.HomeTeam)
                //                        .Include(e => e.AwayTeam)
                //                        .Include(e => e.Field)
                //                        .Include(e => e.League)
                //                        .Include(e => e.MatchType)
                //                        .Include(e => e.Season)
                //                        .Include(e => e.Sport).Where(e => e.Season.IsCurrent && e.Date == DateTime.Now.Date && e.HomeTeamID == teamID).ToListAsync();
                //.Include(e => e.Sport).Where(e => products.Any(p => (p.TeamID == e.HomeTeamID && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null)) && e.Season.IsCurrent && e.Date == DateTime.Now.Date.AddHours(-4)).ToListAsync();
                //.Include(e => e.Sport).Where(e => products.Any(p => (p.TeamID == e.HomeTeamID && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null)) && e.Season.IsCurrent && e.Date < DateTime.Now.Date.AddDays(3) && e.Date > DateTime.Now.Date.AddDays(-1)).ToListAsync();

                //foreach (var fixture in fixtures)
                //{
                //    var product = products.FirstOrDefault(p => (p.TeamID == fixture.HomeTeamID && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null));

                //    if (product != null)
                //    {
                //        matchTickets.Add(new MatchTicket
                //        {
                //            Fixture = fixture,
                //            Product = product
                //        });
                //    }
                //}

            }
            catch (Exception ex)
            { }

            return fixtureProducts;
        }

        public async Task<Fixture> GetTodayFixture(string teamID)
        {
            

            try
            {
                var ticketConfig = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TeamID == teamID);

                var fixtureProduct = await _context.FixtureProducts
                    .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.Team)
                    .FirstOrDefaultAsync(e => e.Product.TeamID == teamID && e.Fixture.Date == DateTime.Now.AddHours(-4).Date);
                //.FirstOrDefaultAsync(e => e.Product.TeamID == teamID && e.Fixture.Date.AddDays(-(ticketConfig.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.Fixture.Date);

                return fixtureProduct.Fixture;
            }
            catch (Exception ex)
            { }

            return null;
        }

        public async Task<IEnumerable<FixtureProduct>> GetTodayByTeam(string teamID)
        {
            List<FixtureProduct> fixtureProducts = new List<FixtureProduct>();

            try
            {
                var ticketConfig = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TeamID == teamID);

                fixtureProducts = await _context.FixtureProducts
                    .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.Team)
                    .Where(e => e.Product.TeamID == teamID && e.Fixture.Date.AddDays(-(ticketConfig.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.Fixture.Date).ToListAsync();
                

            }
            catch (Exception ex)
            { }

            return fixtureProducts;
        }

        public async Task<IEnumerable<FixtureProduct>> Fixture(string fixtureID)
        {
            List<FixtureProduct> fixtureProducts = new List<FixtureProduct>();

            try
            {
                var fixtureProductsList = await _context.FixtureProducts
                    .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.Team)
                    .Where(e => e.FixtureID == fixtureID && DateTime.Now.Date <= e.Fixture.Date).ToListAsync();

                foreach (var fixtureProduct in fixtureProductsList)
                {
                    var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TeamID == fixtureProduct.Product.TeamID);

                    if (DateTime.Now.Date >= fixtureProduct.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                        fixtureProducts.Add(fixtureProduct);
                }
            }
            catch (Exception ex)
            { }

            return fixtureProducts;
        }

        public async Task Insert(FixtureProduct item)
        {
            try
            {
                _context.FixtureProducts.Add(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Fixture Product");
            }
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
