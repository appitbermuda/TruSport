using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Repository
{
    public class ContactTraceRepository : IOnTrackRepository<ContactTrace>
    {
        OnTrackContext _context;
        EmailRepository emailRepository;

        public ContactTraceRepository(OnTrackContext context)
        {
            _context = context;
            emailRepository = new EmailRepository(context);
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ContactTrace> Get(string ID)
        {
            ContactTrace ContactTrace = new ContactTrace();

            try
            {
                ContactTrace = await _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .FirstOrDefaultAsync(e => e.ID == ID);

            }
            catch (Exception ex)
            { }

            return ContactTrace;
        }

        public async Task<IEnumerable<ContactTrace>> GetAll()
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                var ContactTracesList = await _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .ToListAsync();

                ContactTraces = await _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .ToListAsync();

                ContactTraces.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.Date);

                return ContactTraces.OrderBy(e => e.LastName).Distinct().ToList();

            }
            catch(Exception ex)
            { }

            return ContactTraces;
        }

        public async Task<IEnumerable<ContactTrace>> Team(ClaimsPrincipal iUser)
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                var email = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

                //var Order
                ContactTraces = await _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Where(e => e.Order.OrderDetails.Any(x => x.FixtureProduct.Product.TeamID == user.TeamID)).ToListAsync();

                ContactTraces.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.Date);

                return ContactTraces.OrderBy(e => e.LastName).Distinct().ToList();
            }
            catch (Exception ex)
            { }

            return ContactTraces;
        }

        public async Task<IEnumerable<ContactTrace>> GetTodayContactTraces(ClaimsPrincipal iUser)
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                var email = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

                ContactTraces = await _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Where(e => e.Order.OrderDetails.Any(x => x.FixtureProduct.Product.TeamID ==  user.TeamID && x.FixtureProduct.Fixture.Date == DateTime.Now.AddHours(-4).Date)).ToListAsync();

                ContactTraces.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.Date);

                return ContactTraces.OrderBy(e => e.LastName).Distinct().ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetTodayContactTraces");
            }

            return ContactTraces;
        }

        public async Task<IEnumerable<ContactTrace>> Fixture(string fixtureID)
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                ContactTraces = await _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Where(e => e.Order.OrderDetails.Any(x => x.FixtureProduct.FixtureID == fixtureID)).ToListAsync();

                ContactTraces.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.Date);

                return ContactTraces.OrderBy(e => e.LastName).Distinct().ToList();

            }
            catch (Exception ex)
            { }

            return ContactTraces;
        }

        public async Task<bool> Download(string fixtureID, ClaimsPrincipal iUser)
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                var email = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

                ContactTraces = await _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Where(e => e.Order.OrderDetails.Any(x => x.FixtureProduct.FixtureID == fixtureID)).ToListAsync();

                ContactTraces.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.Date);

                try
                {
                    await emailRepository.DownloadContactTracing(user.Email, ContactTraces.FirstOrDefault().Order.Fixture, user.FirstName, ContactTraces.OrderBy(e => e.LastName).Distinct().ToList());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Payment Email");
                }

                return true;
            }
            catch (Exception ex)
            { }

            return false;
        }

        public async Task Insert(ContactTrace item)
        {
            try
            {
                _context.ContactTraces.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Contract Trace");
            }
        }

        public async Task Update(ContactTrace item)
        {
            try
            {
                _context.ContactTraces.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Contract Trace");
            }
        }
    }
}
