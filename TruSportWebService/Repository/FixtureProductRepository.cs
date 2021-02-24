using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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

        public async Task<bool> Delete(string id)
        {
            try
            {
                var fixtureProduct = await _context.FixtureProducts.FirstOrDefaultAsync(e => e.ID == id);

                _context.FixtureProducts.Remove(fixtureProduct);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture Product");
            }

            return false;
        }



        public async Task<IEnumerable<FixtureProduct>> AllFixtureProducts()
        {
            List<FixtureProduct> fixtureProducts = new List<FixtureProduct>();

            try
            {
                fixtureProducts = await _context.FixtureProducts
                    .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType)
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .ToListAsync();

            }
            catch (Exception ex)
            { }

            return fixtureProducts;
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
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .FirstOrDefaultAsync(e => e.FixtureID == FixtureID);
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
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => DateTime.Now.AddHours(-4).Date <= e.Fixture.Date && e.IsActive)
                    .ToListAsync();

                fixtureProductsList.ForEach(e => e.Fixture.HomeTeam.Name = !String.IsNullOrEmpty(e.Fixture.HomeTeam.Alias) ? e.Fixture.HomeTeam.Alias : e.Fixture.HomeTeam.Name);
                fixtureProductsList.ForEach(e => e.Fixture.AwayTeam.Name = !String.IsNullOrEmpty(e.Fixture.AwayTeam.Alias) ? e.Fixture.AwayTeam.Alias : e.Fixture.AwayTeam.Name);

                foreach (var fixtureProduct in fixtureProductsList)
                {
                    //ticketConfig.Any(t => t.TeamID == e.Product.TeamID && DateTime.Now.Date > e.Fixture.Date.AddDays(-(t.ValidFrom)))

                    var ticketConfiguration = ticketConfigurations.FirstOrDefault(e => e.TicketCompanyID == fixtureProduct.Product.TicketCompanyID);

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
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => DateTime.Now.AddHours(-4).Date <= e.Fixture.Date && e.IsActive)
                    .ToListAsync();

                fixtureProductsList.ForEach(e => e.Fixture.HomeTeam.Name = !String.IsNullOrEmpty(e.Fixture.HomeTeam.Alias) ? e.Fixture.HomeTeam.Alias : e.Fixture.HomeTeam.Name);
                fixtureProductsList.ForEach(e => e.Fixture.AwayTeam.Name = !String.IsNullOrEmpty(e.Fixture.AwayTeam.Alias) ? e.Fixture.AwayTeam.Alias : e.Fixture.AwayTeam.Name);

                foreach (var fixtureProduct in fixtureProductsList)
                {
                    var ticketConfiguration = ticketConfigurations.FirstOrDefault(e => e.TicketCompanyID == fixtureProduct.Product.TicketCompanyID);

                    if (DateTime.Now.AddHours(-4).Date >= fixtureProduct.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                    {
                        var matchTicketsListw = await _context.MatchTickets.ToListAsync();

                        var matchTicketsList = await _context.MatchTickets
                        .Include(e => e.FixtureProduct)
                        .Where(e => e.FixtureProduct.FixtureID == fixtureProduct.FixtureID)
                        .ToListAsync();

                        var ticketCount = matchTicketsList.Count();
                        var availableTickets = (ticketConfiguration.Stock - ticketCount) < 0 ? 0 : (ticketConfiguration.Stock - ticketCount);

                        if (availableTickets > 0)
                        {
                            fixtureProduct.Fixture.TicketAvailable = availableTickets;
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

        public async Task<IEnumerable<FixtureProduct>> Team(ClaimsPrincipal claimsUser)
        {
            List<FixtureProduct> fixtureProducts = new List<FixtureProduct>();

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TicketCompanyID == companyUser.TicketCompanyID);

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
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID && e.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.Fixture.Date).ToListAsync();



                foreach (var fixtureProduct in fixtureProductsList)
                {
                    if (DateTime.Now.Date > fixtureProduct.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                        fixtureProducts.Add(fixtureProduct);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return fixtureProducts;
        }

        public async Task<Fixture> GetTodayFixture(ClaimsPrincipal claimsUser)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

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
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompanyID)
                    .FirstOrDefaultAsync(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID && e.Fixture.Date == DateTime.Now.AddHours(-4).Date);

                return fixtureProduct.Fixture;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<List<Fixture>> GetUpcomingFixtures(ClaimsPrincipal claimsUser)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var fixtures = await _context.FixtureProducts
                    .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID)
                    .Select(e => e.Fixture).ToListAsync();

                return fixtures;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<FixtureProduct>> GetTodayByTeam(ClaimsPrincipal claimsUser)
        {
            List<FixtureProduct> fixtureProducts = new List<FixtureProduct>();

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var ticketConfig = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == companyUser.TicketCompanyID);

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
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID && e.Fixture.Date.AddDays(-(ticketConfig.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.Fixture.Date).ToListAsync();
                

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
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.FixtureID == fixtureID && DateTime.Now.Date <= e.Fixture.Date && e.IsActive).ToListAsync();

                foreach (var fixtureProduct in fixtureProductsList)
                {
                    var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TicketCompanyID == fixtureProduct.Product.TicketCompanyID);

                    if (DateTime.Now.Date >= fixtureProduct.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                        fixtureProducts.Add(fixtureProduct);
                }
            }
            catch (Exception ex)
            { }

            return fixtureProducts;
        }

        public async Task<IEnumerable<FixtureProduct>> FixtureTicket(string fixtureID, string email)
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
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.FixtureID == fixtureID && DateTime.Now.Date <= e.Fixture.Date && e.IsActive).ToListAsync();

                var product = fixtureProductsList.FirstOrDefault(e => e.FixtureID == fixtureID).Product;
                var ticketMember = await _context.TicketMembers.FirstOrDefaultAsync(e => e.Customer.Email == email);

                foreach (var fixtureProduct in fixtureProductsList)
                {
                    var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TicketCompanyID == fixtureProduct.Product.TicketCompanyID);

                    if (DateTime.Now.Date >= fixtureProduct.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                    {
                        if (ticketMember != null)
                        {
                            fixtureProduct.Product.Price = fixtureProduct.Product.MemberPrice ?? fixtureProduct.Product.Price;
                        }

                        fixtureProducts.Add(fixtureProduct);
                    }
                }
            }
            catch (Exception ex)
            { }

            return fixtureProducts;
        }

        public async Task<bool> Insert(FixtureProduct item)
        {
            try
            {
                _context.FixtureProducts.Add(item);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Fixture Product");
            }

            return false;
        }

        public async Task<bool> Update(FixtureProduct item)
        {
            try
            {
                _context.FixtureProducts.Update(item);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture Product");
            }

            return false;
        }

        Task IOnTrackRepository<FixtureProduct>.Delete(string id)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<FixtureProduct>.Insert(FixtureProduct item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<FixtureProduct>.Update(FixtureProduct item)
        {
            throw new NotImplementedException();
        }
    }
}
