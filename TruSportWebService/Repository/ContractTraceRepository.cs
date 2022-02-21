using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;

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

        [Obsolete]
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

        public async Task<IEnumerable<ContactTrace>> AllContactTraces()
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                ContactTraces = _context.ContactTraces
                     .Include(e => e.Order).ThenInclude(e => e.Customer)
                     .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product)
                     .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).AsEnumerable()
                     .Where(e => e.Order.OrderDetails.Any(x => x.EventTicket != null))
                     .GroupBy(e => new { e.Name, e.Phone })
                     .Select(e => e.First())
                     .ToList();
                //&& e.Order.OrderDetails.Any(x => x.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID)).ToList();

                ContactTraces = ContactTraces.Where(e => e.Order.OrderDetails.Any(x => x.EventTicket != null)).ToList();

                ContactTraces.ForEach(e => e.Order.Fixture = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.Date);

                return ContactTraces.OrderBy(e => e.LastName).Distinct().ToList();

            }
            catch (Exception ex)
            { }

            return ContactTraces;
        }

        [Obsolete]
        public async Task<IEnumerable<ContactTrace>> Team(ClaimsPrincipal iUser)
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                var email = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                //var Order
                ContactTraces = await _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Where(e => e.Order.OrderDetails.Any(x => x.FixtureProduct.Product.TicketCompanyID == companyUser.TicketCompanyID)).ToListAsync();

                ContactTraces.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.Date);

                return ContactTraces.OrderBy(e => e.LastName).Distinct().ToList();
            }
            catch (Exception ex)
            { }

            return ContactTraces;
        }

        public async Task<IEnumerable<ContactTrace>> TeamContactTraces(ClaimsPrincipal iUser)
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                var email = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                //var Order
                ContactTraces = _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).AsEnumerable()
                    .Where(e => e.Order.OrderDetails != null)
                    .GroupBy(e => new { e.Name, e.Phone })
                    .Select(e => e.First())
                    .ToList();
                //&& e.Order.OrderDetails.Any(x => x.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID)).ToList();

                ContactTraces = ContactTraces.Where(e => e.Order.OrderDetails.Any(x => x.EventTicket != null && x.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID)).ToList();

                ContactTraces.ForEach(e => e.Order.Fixture = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.Date);

                return ContactTraces.OrderBy(e => e.LastName).Distinct().ToList();
            }
            catch (Exception ex)
            { }

            return ContactTraces;
        }

        public async Task<IEnumerable<ContactTrace>> TeamContactTracesOntrackr(ClaimsPrincipal iUser)
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                var username = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.UserName == username);

                //var Order
                ContactTraces = _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).AsEnumerable()
                    .Where(e => e.Order.OrderDetails != null)
                    .GroupBy(e => new { e.Name, e.Phone })
                    .Select(e => e.First())
                    .ToList();
                //&& e.Order.OrderDetails.Any(x => x.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID)).ToList();

                ContactTraces = ContactTraces.Where(e => e.Order.OrderDetails.Any(x => x.EventTicket != null && x.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID)).ToList();

                ContactTraces.ForEach(e => e.Order.Fixture = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.Date);

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

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);
                DateTime currentDate = DateTime.Now.AddHours(-4).Date;

                ContactTraces = _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).AsEnumerable()
                    .Where(e => e.Order.OrderDetails.Any(x => x.EventTicket != null && x.EventTicket.Product.TicketCompanyID == companyUser.User.TeamID && x.EventTicket.SportEvent.Date == currentDate)).ToList();

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

        public async Task<IEnumerable<ContactTrace>> GetTodayContactTracesForOntrackr(ClaimsPrincipal iUser)
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                var username = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.UserName == username);
                DateTime currentDate = DateTime.Now.AddHours(-4).Date;

                ContactTraces = _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).AsEnumerable()
                    .Where(e => e.Order.OrderDetails.Any(x => x.EventTicket != null && x.EventTicket.Product.TicketCompanyID == companyUser.User.TeamID && x.EventTicket.SportEvent.Date == currentDate)).ToList();

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

        [Obsolete]
        public async Task<IEnumerable<ContactTrace>> GetContactTracesForToday(ClaimsPrincipal iUser)
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                var email = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);
                DateTime currentDate = DateTime.Now.AddHours(-4).Date;

                ContactTraces = await _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Where(e => e.Order.OrderDetails.Any(x => x.FixtureProduct.Product.TicketCompanyID == companyUser.User.TeamID && x.FixtureProduct.Fixture.Date == currentDate)).ToListAsync();

                ContactTraces.ForEach(e => e.Order.Fixture = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.Date);

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

        public async Task<IEnumerable<ContactTrace>> Event(string eventID)
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
                    .Where(e => e.Order.OrderDetails.Any(x => x.EventTicket.SportEventID == eventID)).ToListAsync();

                ContactTraces.ForEach(e => e.Order.Fixture = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.Date);

                return ContactTraces.OrderBy(e => e.LastName).Distinct().ToList();

            }
            catch (Exception ex)
            { }

            return ContactTraces;
        }

        [Obsolete]
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

                var stream = CreatePDF(ContactTraces);

                string fileName = "ContactTrace" + DateTime.Now.Date.ToString("MMM-dd-yyyy") + ".pdf";

                try
                {
                    if (stream == null)
                        await emailRepository.DownloadContactTracing(user.Email, ContactTraces.FirstOrDefault().Order.Fixture, user.FirstName, ContactTraces);
                    else
                        await emailRepository.DownloadContactTracing(user.Email, ContactTraces.FirstOrDefault().Order.Fixture, user.FirstName, stream, fileName);
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

        public async Task<bool> DownloadReport(string eventID, ClaimsPrincipal iUser)
        {
            List<ContactTrace> ContactTraces = new List<ContactTrace>();

            try
            {
                var username = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var user = await _context.Users.FirstOrDefaultAsync(e => e.UserName == username);

                ContactTraces = _context.ContactTraces
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).AsEnumerable()
                    //.Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    //.Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Where(e => e.Order.OrderDetails.Any(x => x.EventTicket != null && x.EventTicket.SportEventID == eventID))
                    .GroupBy(e => new { e.Name, e.Phone })
                    .Select(e => e.First())
                    .ToList();

                ContactTraces.ForEach(e => e.Order.Fixture = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);
                ContactTraces.ForEach(e => e.Order.FixtureDate = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.Date);

                var stream = CreatePDF(ContactTraces);

                string fileName = "ContactTrace" + DateTime.Now.Date.ToString("MMM-dd-yyyy") + ".pdf";

                try
                {
                    if (stream == null)
                        await emailRepository.DownloadContactTracing(user.Email, ContactTraces.FirstOrDefault().Order.Fixture, user.FirstName, ContactTraces);
                    else
                        await emailRepository.DownloadContactTracing(user.Email, ContactTraces.FirstOrDefault().Order.Fixture, user.FirstName, stream, fileName);
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

        public async Task<List<TicketReport>> Reports(ClaimsPrincipal claimsUser)
        {
            List<TicketReport> contactTraces = new List<TicketReport>();

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var matchTickets = await _context.MatchTickets.Include(e => e.FixtureProduct).ThenInclude(e => e.Product).Where(e => e.FixtureProduct.Product.TicketCompanyID == companyUser.TicketCompanyID).ToListAsync();
                 //&& matchTickets.Any(d => d.FixtureProductID == e.ID)
                var fixtures = await _context.FixtureProducts.Include(e => e.Product)
                    .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Where(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID && e.Fixture.Date <= DateTime.Now.Date)
                    .ToListAsync();

                contactTraces = fixtures.Select(e => new TicketReport
                {
                    FixtureID = e.FixtureID,
                    Fixture = e.Fixture.HomeTeam.Name + " v " + e.Fixture.AwayTeam.Name,
                    Date = e.Fixture.Date.ToString("MMM dd, yyyy")
                }).ToList();

                return contactTraces;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return contactTraces;
        }

        public async Task<List<TicketReport>> EventReports(ClaimsPrincipal claimsUser)
        {
            List<TicketReport> contactTraces = new List<TicketReport>();

            try
            {
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.UserName == username);

                //var customerTickets = await _context.CustomerTickets.Include(e => e.EventTicket).ThenInclude(e => e.Product).Where(e => e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID).ToListAsync();
                //&& matchTickets.Any(d => d.FixtureProductID == e.ID)

                //var sportEvent = await _context.SportEvents
                //    .Where(e => e.)
                //    .ToListAsync();

                var sportEvents = await _context.EventTickets.Include(e => e.Product)
                    .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                    .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                    .Where(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID && e.SportEvent.Date <= DateTime.Now.Date)
                    .Select(e => e.SportEvent).Distinct().ToListAsync();

                contactTraces = sportEvents.Select(e => new TicketReport
                {
                    //FixtureID = e.FixtureID,
                    Fixture = e.HomeTeam + " v " + e.AwayTeam,
                    Date = e.Date.ToString("MMM dd, yyyy"),
                    SportEvent = e
                }).ToList();

                return contactTraces;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return contactTraces;
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

        public Stream CreatePDF(List<ContactTrace> contactTraces)
        {
            try
            {
                //Create a new PDF document
                PdfDocument doc = new PdfDocument();

                //Add a page
                PdfPage page = doc.Pages.Add();

                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;

                //Loads the image as stream
                FileStream imageStream = new FileStream("wwwroot/images/OnTrack_WordMark.png", FileMode.Open, FileAccess.Read);
                RectangleF bounds = new RectangleF(55, 0, 390, 60);
                PdfImage image = PdfImage.FromStream(imageStream);
                //Draws the image to the PDF page
                page.Graphics.DrawImage(image, bounds);

                bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);

                //Set the standard font
                PdfFont headerFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
                PdfFont subHeaderFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
                PdfFont bodyFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);

                //Draw the text
                PdfTextElement element = new PdfTextElement("CONTACT TRACING", headerFont);


                PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));

                element = new PdfTextElement(contactTraces.FirstOrDefault().Order.Fixture, subHeaderFont);
                result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 15));

                element = new PdfTextElement(contactTraces.FirstOrDefault().Order.FixtureDate.ToString("MMM dd, yyyy"), subHeaderFont);
                result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 15));

                //graphics.DrawString("Contact Tracing", headerFont, PdfBrushes.Black, new PointF(0, 0));
                //graphics.DrawString(contactTraces.FirstOrDefault().Order.Fixture, subHeaderFont, PdfBrushes.Black, new PointF(10, bounds.Top + 8));
                //graphics.DrawString(contactTraces.FirstOrDefault().Order.FixtureDate.ToString("MMM dd, yyyy"), subHeaderFont, PdfBrushes.Black, new PointF(0, 0));

                //Create a PdfGrid
                PdfGrid pdfGrid = new PdfGrid();

                //Create a DataTable
                DataTable dataTable = new DataTable();

                //Add columns to the DataTable
                dataTable.Columns.Add("Name");
                dataTable.Columns.Add("Phone #");

                //Add rows to the DataTable
                foreach(var contactTrace in contactTraces)
                {
                    dataTable.Rows.Add(new object[] { contactTrace.Name, contactTrace.Phone });
                }

                //Assign data source
                pdfGrid.DataSource = dataTable;
                //Draw grid to the page of PDF document

                //Creates the grid cell styles
                PdfGridCellStyle cellStyle = new PdfGridCellStyle();
                cellStyle.Borders.All = PdfPens.White;
                PdfGridRow header = pdfGrid.Headers[0];

                //Creates the header style
                PdfGridCellStyle headerStyle = new PdfGridCellStyle();
                headerStyle.Borders.All = new PdfPen(new PdfColor(14, 21, 80));
                headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(14, 21, 80));
                headerStyle.TextBrush = PdfBrushes.White;
                headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 14f, PdfFontStyle.Regular);

                header.ApplyStyle(headerStyle);
                cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
                cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
                cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(0, 0, 0));

                //Creates the layout format for grid
                PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
                // Creates layout format settings to allow the table pagination
                layoutFormat.Layout = PdfLayoutType.Paginate;
                //Draws the grid to the PDF page.
                PdfGridLayoutResult gridResult = pdfGrid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 40), new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

                //Save the PDF document to stream
                MemoryStream stream = new MemoryStream();
                doc.Save(stream);
                //If the position is not set to '0' then the PDF will be empty.
                stream.Position = 0;
                //Close the document.
                doc.Close(true);
                //Defining the ContentType for pdf file.
                //string contentType = "application/pdf";
                
                //Creates a FileContentResult object by using the file contents, content type, and file name.
                return stream;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "PDF");
            }

            return null;
        }

        public Stream CreateTicketPDF(CustomerTicket customerTicket)
        {
            try
            {
                //Create a new PDF document
                PdfDocument doc = new PdfDocument();

                //Add a page
                PdfPage page = doc.Pages.Add();

                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;

                //Loads the image as stream
                FileStream imageStream = new FileStream("wwwroot/images/OnTrack_WordMark.png", FileMode.Open, FileAccess.Read);
                RectangleF bounds = new RectangleF(55, 0, 390, 60);
                PdfImage image = PdfImage.FromStream(imageStream);
                //Draws the image to the PDF page
                page.Graphics.DrawImage(image, bounds);

                bounds = new RectangleF(0, bounds.Bottom + 90, graphics.ClientSize.Width, 30);

                //Set the standard font
                PdfFont headerFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
                PdfFont subHeaderFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);
                PdfFont bodyFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);

                //Draw the text
                PdfTextElement element = new PdfTextElement("ONTRACK Event Ticket", headerFont);


                PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));

                element = new PdfTextElement(customerTicket.Order.Fixture, subHeaderFont);
                result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 15));

                element = new PdfTextElement(customerTicket.Order.FixtureDate.ToString("MMM dd, yyyy"), subHeaderFont);
                result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 15));

                //graphics.DrawString("Contact Tracing", headerFont, PdfBrushes.Black, new PointF(0, 0));
                //graphics.DrawString(contactTraces.FirstOrDefault().Order.Fixture, subHeaderFont, PdfBrushes.Black, new PointF(10, bounds.Top + 8));
                //graphics.DrawString(contactTraces.FirstOrDefault().Order.FixtureDate.ToString("MMM dd, yyyy"), subHeaderFont, PdfBrushes.Black, new PointF(0, 0));


                //Drawing QR barcode 
                PdfQRBarcode qrBarcode = new PdfQRBarcode();

                //Set Error Correction Level
                qrBarcode.ErrorCorrectionLevel = PdfErrorCorrectionLevel.High;

                //Set XDimension
                qrBarcode.XDimension = 3;
                qrBarcode.Text = JsonConvert.SerializeObject(customerTicket.CustomerMatchTicket);
                //Draw string
                page.Graphics.DrawString("Ticket QR", new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(20, 180));
                //Printing barcode on to the PDF
                qrBarcode.Draw(page, new PointF(50, 200));

                //Save the PDF document to stream
                MemoryStream stream = new MemoryStream();
                doc.Save(stream);
                //If the position is not set to '0' then the PDF will be empty.
                stream.Position = 0;
                //Close the document.
                doc.Close(true);
                //Defining the ContentType for pdf file.
                //string contentType = "application/pdf";

                //Creates a FileContentResult object by using the file contents, content type, and file name.
                return stream;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "PDF");
            }

            return null;
        }
    }
}
