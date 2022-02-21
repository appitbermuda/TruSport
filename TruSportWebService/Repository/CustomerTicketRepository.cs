using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;
using OnTrackWebService.Models.Ticket;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;

namespace OnTrackWebService.Repository
{
    public class CustomerTicketRepository : IOnTrackRepository<CustomerTicket>
    {
        OnTrackContext _context;
        EmailRepository emailRepository;        

        public CustomerTicketRepository(OnTrackContext context, IOptions<NotificationHubOptions> options, ILogger<PushNotificationRepository> logger)
        {
            _context = context;
            emailRepository = new EmailRepository(context);            
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerTicket> Get(string ID)
        {
            CustomerTicket customerTicket = new CustomerTicket();

            try
            {
                customerTicket = await _context.CustomerTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                    .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                    .FirstOrDefaultAsync(e => e.ID == ID);

            }
            catch (Exception ex)
            {

            }

            return customerTicket;
        }

        public async Task<IEnumerable<CustomerTicket>> GetAll()
        {
            List<CustomerTicket> customerTickets = new List<CustomerTicket>();

            try
            {
                List<Season> currentSeasons = await _context.Seasons.Where(e => e.IsCurrent).ToListAsync();

                customerTickets = _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.Order).ThenInclude(e => e.ContactTraces)
                        .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType).AsEnumerable()
                        .Where(e => currentSeasons.Any(d => d.SportID == e.EventTicket.SportEvent.SportID && d.Date == e.EventTicket.SportEvent.Season))
                    .ToList();

                customerTickets.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam) ? e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam : e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam) ? e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam : e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam));

                customerTickets.ForEach(e => e.ScanStatistic = new ScanStatistic
                {
                    Scanned = customerTickets.Where(d => d.Validated && e.EventTicket.SportEventID == d.EventTicket.SportEventID).Count(),
                    NotScanned = customerTickets.Where(d => !d.Validated && e.EventTicket.SportEventID == d.EventTicket.SportEventID).Count(),
                });
            }
            catch(Exception ex)
            { }

            return customerTickets;
        }

        public async Task<IEnumerable<CustomerTicket>> Search()
        {
            List<CustomerTicket> customerTickets = new List<CustomerTicket>();

            try
            {
                List<Season> currentSeasons = await _context.Seasons.Where(e => e.IsCurrent).ToListAsync();

                customerTickets = await _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                        .Where(e => e.EventTicket.SportEvent.Date > DateTime.Now.AddDays(-1))
                    .ToListAsync();

                customerTickets.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam) ? e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam : e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam) ? e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam : e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam));
            }
            catch (Exception ex)
            { }

            return customerTickets;
        }

        public async Task<IEnumerable<CustomerTicket>> GetCustomerTickets(ClaimsPrincipal user)
        {
            List<CustomerTicket> customerTickets = new List<CustomerTicket>();

            try
            {
                List<Season> currentSeasons = await _context.Seasons.Where(e => e.IsCurrent).ToListAsync();

                // Get the claims values
                var email = user.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                customerTickets = await _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                    .Where(e => (DateTime.Now.AddHours(-4).Date <= e.EventTicket.SportEvent.Date || e.EventTicket.SportEvent.IsPostponed) && ((e.Order.Customer.Email == email && e.TransferCustomerID == null) || (e.TransferCustomer != null && e.TransferCustomer.Email == email && e.IsTransfer.HasValue && e.IsTransfer.Value)) && !e.Order.IsRefunded)
                    .ToListAsync();
                                
                customerTickets.ForEach(e => e.CustomerMatchTicket = new CustomerMatchTicket
                {
                    EventTicketID = e.EventTicketID,
                    ID = e.ID,
                    OrderID = e.OrderID
                });
            }
            catch (Exception ex)
            { }

            return customerTickets;
        }

        public async Task<IEnumerable<CustomerTicket>> Team(ClaimsPrincipal claimsUser)
        {
            List<CustomerTicket> customerTickets = new List<CustomerTicket>();

            try
            {
                List<Season> currentSeasons = await _context.Seasons.Where(e => e.IsCurrent).ToListAsync();

                // Get the claims values
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).ThenInclude(e => e.Role).FirstOrDefaultAsync(e => e.User.UserName == username && e.User.Role.Name == role);

                if (companyUser != null && companyUser.User != null && companyUser.User.IsActive)
                {
                    customerTickets = _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.Order).ThenInclude(e => e.ContactTraces)
                        .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType).AsEnumerable()
                        .Where(e => e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID && currentSeasons.Any(d => d.SportID == e.EventTicket.SportEvent.SportID && d.Date == e.EventTicket.SportEvent.Season))
                        .ToList();

                    customerTickets.ForEach(e => e.Order.Fixture = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);

                    customerTickets.ForEach(e => e.ScanStatistic = new ScanStatistic
                    {
                        Scanned = customerTickets.Where(d => d.Validated && e.EventTicket.SportEventID == d.EventTicket.SportEventID).Count(),
                        NotScanned = customerTickets.Where(d => !d.Validated && e.EventTicket.SportEventID == d.EventTicket.SportEventID).Count(),
                    });
                }
            }
            catch (Exception ex)
            { }

            return customerTickets.OrderByDescending(e => e.Order.Date);
        }

        public async Task<IEnumerable<CustomerTicket>> TeamSearch(ClaimsPrincipal claimsUser)
        {
            List<CustomerTicket> customerTickets = new List<CustomerTicket>();

            try
            {
                List<Season> currentSeasons = await _context.Seasons.Where(e => e.IsCurrent).ToListAsync();

                // Get the claims values
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).ThenInclude(e => e.Role).FirstOrDefaultAsync(e => e.User.UserName == username && e.User.Role.Name == role);

                if (companyUser != null && companyUser.User != null && companyUser.User.IsActive)
                {
                    customerTickets = _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.Order).ThenInclude(e => e.ContactTraces)
                        .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType).AsEnumerable()
                        .Where(e => e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID && e.EventTicket.SportEvent.Date > DateTime.Now.AddDays(-1) && currentSeasons.Any(d => d.SportID == e.EventTicket.SportEvent.SportID && d.Date == e.EventTicket.SportEvent.Season))
                    .ToList();

                    customerTickets.ForEach(e => e.Order.Fixture = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);
                }
            }
            catch (Exception ex)
            { }

            return customerTickets.OrderByDescending(e => e.Order.Date);
        }

        public async Task<ScanStatistic> Stats(ClaimsPrincipal claimsUser)
        {
            ScanStatistic scanStatistic = new ScanStatistic();

            try
            {
                // Get the claims values
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).ThenInclude(e => e.Role).FirstOrDefaultAsync(e => e.User.Email == email && e.User.Role.Name == role);

                if (companyUser != null && companyUser.User != null && companyUser.User.IsActive)
                {
                    var customerTickets = await _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                        .Where(e => e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID).ToListAsync();

                    scanStatistic.Scanned = customerTickets.Where(e => e.Validated).Count();
                    scanStatistic.NotScanned = customerTickets.Where(e => !e.Validated).Count();
                }
            }
            catch (Exception ex)
            { }

            return scanStatistic;
        }

        public async Task<ScanStatistic> Stats(ClaimsPrincipal claimsUser, string SportEventID)
        {
            ScanStatistic scanStatistic = new ScanStatistic();

            try
            {
                // Get the claims values
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).ThenInclude(e => e.Role).FirstOrDefaultAsync(e => e.User.UserName == username && e.User.Role.Name == role);

                if (companyUser != null && companyUser.User != null && companyUser.User.IsActive)
                {
                    var customerTickets = await _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                        .Where(e => e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID && e.EventTicket.SportEventID == SportEventID).ToListAsync();

                    scanStatistic.Scanned = customerTickets.Where(e => e.Validated).Count();
                    scanStatistic.NotScanned = customerTickets.Where(e => !e.Validated).Count();
                }
            }
            catch (Exception ex)
            { }

            return scanStatistic;
        }

        public async Task<IEnumerable<CustomerTicket>> GetTodayCustomerTickets(ClaimsPrincipal claimsUser)
        {
            List<CustomerTicket> customerTickets = new List<CustomerTicket>();

            try
            {
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.UserName == username);
                DateTime currentDate = DateTime.Now.AddHours(-4).Date;

                customerTickets = await _context.CustomerTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                    .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                    .Where(e => e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID && e.EventTicket.SportEvent.Date == currentDate).ToListAsync();

                customerTickets.ForEach(e => e.Order.Fixture = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetTodayCustomerTickets");
            }

            return customerTickets;
        }

        public async Task<TicketBilling> SportEventBilling(string eventID, ClaimsPrincipal iUser)
        {
            TicketBilling ticketBilling = new TicketBilling();

            try
            {
                var username = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).FirstOrDefaultAsync(e => e.User.UserName == username);

                var members = await _context.TicketMembers.Include(e => e.TicketCompany).Where(e => e.TicketCompanyID == companyUser.TicketCompanyID).ToListAsync();

                var ticketFees = await _context.TicketFees.ToListAsync();

                var orderDetails = await _context.OrderDetails
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Where(e => e.EventTicket.SportEvent.ID == eventID && e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID && !e.Order.IsRefunded).ToListAsync();

                List<Bill> billing = orderDetails.Select(e => new Bill
                {
                    CustomerID = e.Order.CustomerID,
                    Name = e.Order.Customer.Name,
                    Quantity = e.Qty,
                    Product = e.EventTicket.Product.Age + " " + e.EventTicket.Product.Name,
                    Price = members.FirstOrDefault(m => m.CustomerID == e.Order.CustomerID) != null ? Convert.ToDouble(e.EventTicket.Product.MemberPrice) : Convert.ToDouble(e.EventTicket.Product.Price),
                    Subtotal = Convert.ToDouble(e.Subtotal),
                    Total = Convert.ToDouble(e.Subtotal - (e.Qty * (ticketFees.FirstOrDefault(d => d.Price == e.EventTicket.Product.Price).Fee)))
                }).ToList();

                string fixture = orderDetails.FirstOrDefault().EventTicket.SportEvent.Event;
                double qty = billing.Sum(e => e.Quantity);
                double subtotal = Convert.ToDouble(billing.Sum(e => e.Subtotal));
                double fee = billing.Sum(e => (Convert.ToDouble((ticketFees.FirstOrDefault(d => d.Price == Convert.ToDecimal(e.Price)).Fee)) * e.Quantity));
                double total = subtotal - fee; 


                ticketBilling = new TicketBilling
                {
                    Fixture = fixture,
                    Quantity = qty,
                    Subtotal = subtotal,
                    Fee = fee,
                    Total = total,
                    Billing = billing
                };

                return ticketBilling;

            }
            catch (Exception ex)
            { }

            return ticketBilling;
        }

        public async Task<bool> Download(string eventID, ClaimsPrincipal iUser)
        {
            try
            {
                var username = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).FirstOrDefaultAsync(e => e.User.UserName == username);

                TicketBilling ticketBilling = await SportEventBilling(eventID, iUser);

                try
                {
                    await emailRepository.DownloadTicketBilling(companyUser.User.FirstName, companyUser.User.Email, ticketBilling);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Payment Email");
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<List<TicketReport>> Reports(ClaimsPrincipal claimsUser)
        {
            List<TicketReport> ticketReports = new List<TicketReport>();

            try
            {
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var user = await _context.TicketCompanyUsers.Include(e => e.User).FirstOrDefaultAsync(e => e.User.UserName == username);

                //var customerTickets = await _context.CustomerTickets.Include(e => e.EventTicket).ThenInclude(e => e.Product).Where(e => e.EventTicket.Product.TicketCompanyID == user.TicketCompanyID).ToListAsync();
                //&& customerTickets.Any(d => d.EventTicketID == e.ID)

                var sportEvents = await _context.EventTickets.Include(e => e.Product)
                   .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                   .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                   .Where(e => e.Product.TicketCompanyID == user.TicketCompanyID && e.SportEvent.Date <= DateTime.Now.Date)
                   .Select(e => e.SportEvent).Distinct().ToListAsync();

                ticketReports = sportEvents.Select(e => new TicketReport
                {
                    //FixtureID = e.FixtureID,
                    Fixture = e.HomeTeam + " v " + e.AwayTeam,
                    Date = e.Date.ToString("MMM dd, yyyy"),
                    SportEvent = e
                }).ToList();
                //ticketReports = await _context.EventTickets.Include(e => e.Product)
                //    .Include(e => e.SportEvent)
                //    .Include(e => e.SportEvent)
                //    .Where(e => e.Product.TicketCompanyID == user.TicketCompanyID && e.SportEvent.Date <= DateTime.Now.Date)
                //    .Select(e => new TicketReport
                //    {
                //        SportEvent = e.SportEvent,
                //        FixtureID = e.SportEventID,
                //        Fixture = e.SportEvent.HomeTeam + " v " + e.SportEvent.AwayTeam,
                //        Date = e.SportEvent.Date.ToString("MMM dd, yyyy")
                //    }).ToListAsync();

                return ticketReports;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return ticketReports;
        }

        public async Task<bool> ValidateCustomer(string customerTicketID)
        {
            try
            {
                var customerTicket = await _context.CustomerTickets.FirstOrDefaultAsync(e => e.ID == customerTicketID);

                if (!customerTicket.Validated)
                {
                    customerTicket.Validated = true;
                    customerTicket.ValidatedTime = DateTime.Now.ToUniversalTime();

                    _context.CustomerTickets.Update(customerTicket);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            { }

            return false;
        }

        public async Task<TicketBilling> Billing(string eventID, ClaimsPrincipal iUser)
        {
            TicketBilling ticketBilling = new TicketBilling();

            try
            {
                var email = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).FirstOrDefaultAsync(e => e.User.Email == email);

                var members = await _context.TicketMembers.Include(e => e.TicketCompany).Where(e => e.TicketCompanyID == companyUser.TicketCompanyID).ToListAsync();

                var orderDetails = await _context.OrderDetails
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Where(e => e.EventTicket.SportEventID == eventID && e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID && !e.Order.IsRefunded).ToListAsync();

                List<Bill> billing = orderDetails.Select(e => new Bill
                {
                    CustomerID = e.Order.CustomerID,
                    Name = e.Order.Customer.Name,
                    Quantity = e.Qty,
                    Product = e.EventTicket.Product.Age + " " + e.EventTicket.Product.Name,
                    Price = members.FirstOrDefault(m => m.CustomerID == e.Order.CustomerID) != null ? Convert.ToDouble(e.EventTicket.Product.MemberPrice) : Convert.ToDouble(e.EventTicket.Product.Price),
                    Subtotal = Convert.ToDouble(e.Subtotal),
                    Total = Convert.ToDouble(e.Subtotal) - ((e.Qty * .25))
                }).ToList();

                string fixture = orderDetails.FirstOrDefault().Order.Fixture;
                double qty = billing.Sum(e => e.Quantity);
                double subtotal = Convert.ToDouble(billing.Sum(e => e.Subtotal));
                double fee = billing.Sum(e => (.25 * e.Quantity));
                double total = subtotal - fee;


                ticketBilling = new TicketBilling
                {
                    Fixture = fixture,
                    Quantity = qty,
                    Subtotal = subtotal,
                    Fee = fee,
                    Total = total,
                    Billing = billing
                };

                return ticketBilling;

            }
            catch (Exception ex)
            { }

            return ticketBilling;
        }

        public async Task<IEnumerable<CustomerTicket>> SportEvent(string fixtureID)
        {
            List<CustomerTicket> customerTickets = new List<CustomerTicket>();

            try
            {
                var fixtureProducts = await _context.EventTickets
                       .Include(e => e.SportEvent)
                       .Include(e => e.Product).ToListAsync();

                var customerTicketsList = await _context.CustomerTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                    .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                    .Where(e => e.EventTicket.SportEventID == fixtureID && DateTime.Now.Date <= e.EventTicket.SportEvent.Date).ToListAsync();

                foreach (var customerTicket in customerTicketsList)
                {
                    var fixtureProduct = fixtureProducts.FirstOrDefault(e => e.ID == customerTicket.EventTicketID);
                    var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TicketCompanyID == fixtureProduct.Product.TicketCompanyID);

                    if (DateTime.Now.Date > customerTicket.EventTicket.SportEvent.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                        customerTickets.Add(customerTicket);
                }
            }
            catch (Exception ex)
            { }

            return customerTickets;
        }

        public async Task<TicketResponse> Scan(CustomerTicket scannedCustomerTicket, ClaimsPrincipal claimsUser)
        {
            TicketResponse ticketResponse = new TicketResponse();

            try
            {
                List<string> tickets = new List<string>();

                ////Get the current claims principal
                //var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                // Get the claims values
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).ThenInclude(e => e.Role).FirstOrDefaultAsync(e => e.User.UserName == username && e.User.Role.Name == role);

                if (companyUser != null && companyUser.User != null && companyUser.IsActive)
                {
                    var eventTicket = await _context.EventTickets.Include(e => e.Product).FirstOrDefaultAsync(e => e.ID == scannedCustomerTicket.EventTicketID);

                    var customerTicket = await _context.CustomerTickets
                        .Include(e => e.Order)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                        .FirstOrDefaultAsync(e => e.ID == scannedCustomerTicket.ID && e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID);

                    if (customerTicket != null)
                    {
                        if (!customerTicket.Validated)
                        {
                            customerTicket.Validated = true;
                            customerTicket.ValidatedTime = DateTime.Now.ToUniversalTime();

                            _context.CustomerTickets.Update(customerTicket);
                            await _context.SaveChangesAsync();

                            ticketResponse.Response = "Validated Successfully!";
                            ticketResponse.Ticket = customerTicket.EventTicket.Product.Age + " Ticket";
                            ticketResponse.IsValidated = true;

                            return ticketResponse;
                        }

                        ticketResponse.Response = "Ticket already validated!";
                        ticketResponse.IsValidated = false;

                        return ticketResponse;
                    }

                    ticketResponse.Response = "Invalid Ticket!";
                    ticketResponse.IsValidated = false;

                    return ticketResponse;

                }

                ticketResponse.Response = "You are not authorized to scan!";
                ticketResponse.IsValidated = false;

                return ticketResponse;
            }
            catch (Exception ex)
            {

            }

            ticketResponse.Response = "Error Validating Ticket!";
            ticketResponse.IsValidated = false;

            return ticketResponse;
        }

        //public async Task<CustomerTicket> GetTodayByTeam(string teamID)
        //{
        //    CustomerTicket customerTicket = new CustomerTicket();

        //    try
        //    {
        //        var fixtureProduct = await _context.EventTickets
        //               .Include(e => e.SportEvent)
        //               .Include(e => e.Product)
        //               .FirstOrDefaultAsync(e => e.Product.TeamID == teamID);

        //        var ticketConfig = await _context.TicketConfigurations
        //            .FirstOrDefaultAsync(e => e.TeamID == teamID);

        //        customerTicket = await _context.CustomerTickets
        //            .Include(e => e.Order).ThenInclude(e => e.Customer)
        //            .Include(e => e.SportEvent).ThenInclude(e => e.HomeTeam)
        //            .Include(e => e.SportEvent).ThenInclude(e => e.AwayTeam)
        //            .Include(e => e.SportEvent).ThenInclude(e => e.Field)
        //            .Include(e => e.SportEvent).ThenInclude(e => e.League)
        //            .Include(e => e.SportEvent).ThenInclude(e => e.MatchType)
        //            .Include(e => e.SportEvent).ThenInclude(e => e.Season)
        //            .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
        //            .FirstOrDefaultAsync(e => e.SportEventID == fixtureProduct.SportEventID && e.SportEvent.Date.AddDays(-(ticketConfig.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.SportEvent.Date);
        //        //.FirstOrDefaultAsync(e => e.Product.TeamID == teamID && e.ValidFrom.Value < DateTime.Now.Date && DateTime.Now.Date <= e.SportEvent.Date);

        //    }
        //    catch (Exception ex)
        //    { }

        //    return customerTicket;
        //}

        [Obsolete("Use Purchase Ticket")]
        public async Task<PaymentResponse> Purchase(ClaimsPrincipal claimsUser, PaymentAuthorize paymentAuthorization, bool isAdhoc = false)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                     .Select(c => c.Value).SingleOrDefault();

                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == (isAdhoc ? Constants.TicketingEmail : email));

                paymentAuthorization.CustomerID = customer.ID;

                var sportEvent = await _context.SportEvents
                    .Include(e => e.TicketCompany)
                    .Include(e => e.Sport)
                    .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.SportEventID);

                var eventTickets = await _context.EventTickets
                    .Include(e => e.Product)
                    .Where(e => e.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var customerTicketsList = await _context.CustomerTickets
                    .Include(e => e.Order)
                    .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Where(e => e.EventTicket.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                //var ticketCompanyID = sportEvent.Product.TicketCompanyID;

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == sportEvent.TicketCompanyID);

                var ticketCount = customerTicketsList.Count();

                //Ticket Member
                //Member Tickets Bought
                bool isTicketMember = false;

                var ticketMember = await _context.TicketMembers.FirstOrDefaultAsync(e => e.CustomerID == customer.ID && e.TicketCompanyID == sportEvent.TicketCompanyID);

                if (ticketMember != null)
                    isTicketMember = true;
                //var isTicketMember = await _context.TicketMembers.AnyAsync(e => e.CustomerID == customer.ID && e.TicketCompanyID == sportEvent.TicketCompanyID);

                if (!isTicketMember)
                    isTicketMember = await _context.Members.AnyAsync(e => e.Email == email && e.TicketCompanyID == sportEvent.TicketCompanyID);

                var memberTickets = customerTicketsList.Where(e => e.Order.CustomerID == customer.ID && e.IsMemberTicket).ToList();

                if (isTicketMember && memberTickets.Count >= ticketMember.NoOfTickets)
                    isTicketMember = false;

                if (ticketCount < ticketConfiguration.Stock && (paymentAuthorization.OrderDetails.Sum(e => e.Qty)) <= ticketConfiguration.Stock)
                {
                    //var processingFee = await _context.Settings.FirstOrDefaultAsync(e => e.ID == Constants.SETTING_PROCESSING_FEE_ID);

                    Decimal ProcessingFeeAmount = 0.0m;
                    Decimal PaymentAmount = 0.0m;
                    Decimal SubtotalAmount = 0.0m;
                                        
                    string payment = String.Format("{0,0:N2}", Decimal.Parse(paymentAuthorization.Amount));

                    PaymentAmount = Convert.ToDecimal(payment);

                    foreach (var ticket in paymentAuthorization.OrderDetails)
                    {
                        if (ticket.Qty > 0)
                        {
                            decimal fee = 0;
                            decimal price = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;
                            decimal memberPrice = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.MemberPrice ?? 0;

                            if (isTicketMember && ticket.IsMemberTicket)
                            {
                                if (price != memberPrice)
                                    price = memberPrice;
                            }

                            //var price = isTicketMember && ticket.IsMemberTicket ? (eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.MemberPrice ?? eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price) : eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;
                            //var price = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;

                            if (price > 0)
                                fee = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Fee;

                            ProcessingFeeAmount += (ticket.Qty * fee);
                            SubtotalAmount += (ticket.Qty * price);
                        }
                    }

                    if (PaymentAmount != (SubtotalAmount + ProcessingFeeAmount))
                    {
                        paymentAuthorization.Amount = (SubtotalAmount + ProcessingFeeAmount).ToString();                        
                    }

                    if (PaymentAmount > Convert.ToDecimal(paymentAuthorization.Amount))
                    {
                        PaymentAmount = Convert.ToDecimal(paymentAuthorization.Amount);
                    }

                    PaymentAmount = PaymentAmount != Convert.ToDecimal(paymentAuthorization.Amount) ? Convert.ToDecimal(paymentAuthorization.Amount) : PaymentAmount;

                    paymentAuthorization.Amount = paymentAuthorization.Amount.Replace(".", "");

                    Order order = new Order
                    {
                        Total = PaymentAmount,
                        CustomerID = paymentAuthorization.CustomerID,
                        Date = DateTime.Now,
                        Discount = 0.0m,
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    var response = await request.Payment(paymentAuthorization);

                    if (response.CreditCardTransactionResults.ResponseCode == "1")
                    {
                        _context.Database.BeginTransaction();

                        if (paymentAuthorization.SaveCard)
                        {
                            Wallet wallet = new Wallet
                            {
                                CustomerID = customer.ID,
                                TokenPAN = response.CreditCardTransactionResults.TokenizedPAN,
                                Expiry = paymentAuthorization.Expiry,
                                //IsDefault = 
                            };

                            _context.Wallets.Add(wallet);
                            await _context.SaveChangesAsync();
                        }

                        //var updateOrder = await _context.Orders.FirstOrDefaultAsync(e => e.ID == order.ID);
                        order.Authorisation = response.CreditCardTransactionResults.AuthCode;
                        order.OrderNumber = response.OrderNumber;

                        _context.Orders.Update(order);
                        await _context.SaveChangesAsync();



                        if (paymentAuthorization.OrderDetails != null && paymentAuthorization.OrderDetails.Count > 0)
                        {
                            try
                            {
                                paymentAuthorization.OrderDetails.ForEach(e => e.OrderID = order.ID);
                                _context.OrderDetails.AddRange(paymentAuthorization.OrderDetails);
                                await _context.SaveChangesAsync();
                            }
                            catch(Exception ex)
                            { }

                            List<CustomerTicket> customerTickets = new List<CustomerTicket>();
                            foreach(var orderDetail in paymentAuthorization.OrderDetails)
                            {
                                for(var i = 0; i < orderDetail.Qty; i++)
                                {
                                    customerTickets.Add(new CustomerTicket
                                    {
                                        EventTicketID = orderDetail.EventTicketID,
                                        OrderID = order.ID,
                                        Validated = false,
                                        IsMemberTicket = orderDetail.IsMemberTicket
                                    });
                                }
                            }

                            _context.CustomerTickets.AddRange(customerTickets);
                            await _context.SaveChangesAsync();

                        }

                        if (paymentAuthorization.ContactTraces != null && paymentAuthorization.ContactTraces.Count > 0)
                        {
                            paymentAuthorization.ContactTraces.ForEach(e => e.OrderID = order.ID);

                            _context.ContactTraces.AddRange(paymentAuthorization.ContactTraces);
                            await _context.SaveChangesAsync();
                        }

                        _context.Database.CommitTransaction();
                        paymentResponse.IsApproved = true;

                        try
                        {
                            if (isAdhoc)
                            {
                                var thisOrder = await _context.Orders
                                .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                                .Include(e => e.Customer)
                                .FirstOrDefaultAsync(e => e.ID == order.ID);

                                if (!String.IsNullOrEmpty(paymentAuthorization.Email))
                                {
                                    customer.FirstName = paymentAuthorization.NameOnCard;
                                    customer.Email = paymentAuthorization.Email;
                                }

                                await emailRepository.SendPaymentConfirmation(customer, response.CreditCardTransactionResults.AuthCode, thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent);
                            }
                            else
                            {
                                var printCustomerTickets = await _context.CustomerTickets
                                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                                    .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                    .Where(e => e.OrderID == order.ID)
                                    .ToListAsync();

                                printCustomerTickets.ForEach(e => e.CustomerMatchTicket = new CustomerMatchTicket
                                {
                                    EventTicketID = e.EventTicketID,
                                    ID = e.ID,
                                    OrderID = e.OrderID
                                });

                                var thisOrder = printCustomerTickets.Select(e => e.Order).FirstOrDefault();

                                List<PrintTicket> ticketsToPrint = new List<PrintTicket>();
                                //int i = 1;
                                for (var i = 0; i < printCustomerTickets.Count; i++)
                                {
                                    var stream = CreateTicketPDF(thisOrder, printCustomerTickets[i], "Ticket #" + (i + 1));

                                    ticketsToPrint.Add(new PrintTicket
                                    {
                                        File = stream,
                                        FileName = "EventTicket" + (i + 1) + ".pdf"
                                    });
                                }

                                await emailRepository.SendPaymentConfirmation(customer, response.CreditCardTransactionResults.AuthCode, thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent, ticketsToPrint);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message, "Payment Email");
                        }
                    }

                    paymentResponse.Code = response.CreditCardTransactionResults.ResponseCode;
                    paymentResponse.Description = response.CreditCardTransactionResults.ReasonCodeDescription;

                    return paymentResponse;
                }
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();

                //var response = await request.Payment(paymentAuthorization);
                Debug.WriteLine(ex.Message, "Payment");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";

            return paymentResponse;
        }

        public async Task<PaymentResponse> PurchaseTicket(ClaimsPrincipal claimsUser, PaymentAuthorize paymentAuthorization, bool isAdhoc = false)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                     .Select(c => c.Value).SingleOrDefault();

                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == (isAdhoc ? Constants.TicketingEmail : email));

                paymentAuthorization.CustomerID = customer.ID;

                var sportEvent = await _context.SportEvents
                    .Include(e => e.TicketCompany)
                    .Include(e => e.Sport)
                    .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.SportEventID);

                var eventTickets = await _context.EventTickets
                    .Include(e => e.Product)
                    .Where(e => e.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var customerTicketsList = await _context.CustomerTickets
                    .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Where(e => e.EventTicket.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                //var ticketCompanyID = sportEvent.Product.TicketCompanyID;

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == sportEvent.TicketCompanyID);

                var ticketCount = customerTicketsList.Count();

                //Ticket Member
                var isTicketMember = await _context.TicketMembers.AnyAsync(e => e.CustomerID == customer.ID && e.TicketCompanyID == sportEvent.TicketCompanyID);

                if (!isTicketMember)
                    isTicketMember = await _context.Members.AnyAsync(e => e.Email == email && e.TicketCompanyID == sportEvent.TicketCompanyID);


                if (ticketCount < ticketConfiguration.Stock && (paymentAuthorization.OrderDetails.Sum(e => e.Qty)) <= ticketConfiguration.Stock)
                {
                    //var processingFee = await _context.Settings.FirstOrDefaultAsync(e => e.ID == Constants.SETTING_PROCESSING_FEE_ID);

                    Decimal ProcessingFeeAmount = 0.0m;
                    Decimal PaymentAmount = 0.0m;
                    Decimal SubtotalAmount = 0.0m;

                    string payment = String.Format("{0,0:N2}", Decimal.Parse(paymentAuthorization.Amount));

                    PaymentAmount = Convert.ToDecimal(payment);

                    foreach (var ticket in paymentAuthorization.OrderDetails)
                    {
                        if (ticket.Qty > 0)
                        {
                            decimal fee = 0;
                            decimal price = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;
                            decimal memberPrice = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.MemberPrice ?? 0;

                            if (isTicketMember && ticket.IsMemberTicket)
                            {
                                if (price != memberPrice)
                                    price = memberPrice;
                            }

                            //var price = isTicketMember ? (eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.MemberPrice ?? eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price) : eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;
                            //var price = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;

                            if (price > 0)
                                fee = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Fee;

                            ProcessingFeeAmount += (ticket.Qty * fee);
                            SubtotalAmount += (ticket.Qty * price);
                        }
                    }

                    if (PaymentAmount != (SubtotalAmount + ProcessingFeeAmount))
                    {
                        paymentAuthorization.Amount = (SubtotalAmount + ProcessingFeeAmount).ToString();
                    }

                    if (PaymentAmount > Convert.ToDecimal(paymentAuthorization.Amount))
                    {
                        PaymentAmount = Convert.ToDecimal(paymentAuthorization.Amount);
                    }

                    PaymentAmount = PaymentAmount != Convert.ToDecimal(paymentAuthorization.Amount) ? Convert.ToDecimal(paymentAuthorization.Amount) : PaymentAmount;

                    paymentAuthorization.Amount = paymentAuthorization.Amount.Replace(".", "");

                    Order order = new Order
                    {
                        Total = PaymentAmount,
                        CustomerID = paymentAuthorization.CustomerID,
                        Date = DateTime.Now,
                        Discount = 0.0m,
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    var response = await request.Payment(paymentAuthorization);

                    if (response.CreditCardTransactionResults.ResponseCode == "1")
                    {
                        _context.Database.BeginTransaction();

                        //var updateOrder = await _context.Orders.FirstOrDefaultAsync(e => e.ID == order.ID);
                        order.Authorisation = response.CreditCardTransactionResults.AuthCode;
                        order.OrderNumber = response.OrderNumber;

                        _context.Orders.Update(order);
                        await _context.SaveChangesAsync();



                        if (paymentAuthorization.OrderDetails != null && paymentAuthorization.OrderDetails.Count > 0)
                        {
                            try
                            {
                                paymentAuthorization.OrderDetails.ForEach(e => e.OrderID = order.ID);
                                _context.OrderDetails.AddRange(paymentAuthorization.OrderDetails);
                                await _context.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            { }

                            List<CustomerTicket> customerTickets = new List<CustomerTicket>();
                            foreach (var orderDetail in paymentAuthorization.OrderDetails)
                            {
                                for (var i = 0; i < orderDetail.Qty; i++)
                                {
                                    customerTickets.Add(new CustomerTicket
                                    {
                                        EventTicketID = orderDetail.EventTicketID,
                                        OrderID = order.ID,
                                        Validated = false,
                                        IsMemberTicket = orderDetail.IsMemberTicket
                                    });
                                }
                            }

                            _context.CustomerTickets.AddRange(customerTickets);
                            await _context.SaveChangesAsync();

                        }

                        if (paymentAuthorization.ContactTraces != null && paymentAuthorization.ContactTraces.Count > 0)
                        {
                            paymentAuthorization.ContactTraces.ForEach(e => e.OrderID = order.ID);

                            _context.ContactTraces.AddRange(paymentAuthorization.ContactTraces);
                            await _context.SaveChangesAsync();
                        }

                        _context.Database.CommitTransaction();
                        paymentResponse.IsApproved = true;

                        try
                        {
                            if (isAdhoc)
                            {
                                var thisOrder = await _context.Orders
                                .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                                .Include(e => e.Customer)
                                .FirstOrDefaultAsync(e => e.ID == order.ID);

                                if (!String.IsNullOrEmpty(paymentAuthorization.Email))
                                {
                                    customer.FirstName = paymentAuthorization.NameOnCard;
                                    customer.Email = paymentAuthorization.Email;
                                }

                                await emailRepository.SendPaymentConfirmation(customer, response.CreditCardTransactionResults.AuthCode, thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent);
                            }
                            else
                            {
                                var printCustomerTickets = await _context.CustomerTickets
                                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                                    .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                    .Where(e => e.OrderID == order.ID)
                                    .ToListAsync();

                                printCustomerTickets.ForEach(e => e.CustomerMatchTicket = new CustomerMatchTicket
                                {
                                    EventTicketID = e.EventTicketID,
                                    ID = e.ID,
                                    OrderID = e.OrderID
                                });

                                var thisOrder = printCustomerTickets.Select(e => e.Order).FirstOrDefault();

                                List<PrintTicket> ticketsToPrint = new List<PrintTicket>();
                                //int i = 1;
                                for (var i = 0; i < printCustomerTickets.Count; i++)
                                {
                                    var stream = CreateTicketPDF(thisOrder, printCustomerTickets[i], "Ticket #" + (i + 1));

                                    ticketsToPrint.Add(new PrintTicket
                                    {
                                        File = stream,
                                        FileName = "EventTicket" + (i + 1) + ".pdf"
                                    });
                                }

                                await emailRepository.SendPaymentConfirmation(customer, response.CreditCardTransactionResults.AuthCode, thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent, ticketsToPrint);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message, "Payment Email");
                        }
                    }

                    paymentResponse.Code = response.CreditCardTransactionResults.ResponseCode;
                    paymentResponse.Description = response.CreditCardTransactionResults.ReasonCodeDescription;

                    return paymentResponse;
                }
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();

                //var response = await request.Payment(paymentAuthorization);
                Debug.WriteLine(ex.Message, "Payment");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";

            return paymentResponse;
        }

        public async Task<PaymentResponse> ZeroPurchase(ClaimsPrincipal claimsUser, PaymentAuthorize paymentAuthorization, bool isAdhoc = false)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                     .Select(c => c.Value).SingleOrDefault();

                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == (isAdhoc ? Constants.TicketingEmail : email));

                paymentAuthorization.CustomerID = customer.ID;

                var sportEvent = await _context.SportEvents
                    .Include(e => e.TicketCompany)
                    .Include(e => e.Sport)
                    .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.SportEventID);

                var eventTickets = await _context.EventTickets
                    .Include(e => e.Product)
                    .Where(e => e.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var customerTicketsList = await _context.CustomerTickets
                    .Include(e => e.EventTicket)
                    .Where(e => e.EventTicket.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == sportEvent.TicketCompanyID);

                var ticketCount = customerTicketsList.Count();

                if (ticketCount < ticketConfiguration.Stock && paymentAuthorization.OrderDetails.Sum(e => e.Qty) <= ticketConfiguration.Stock)
                {
                    Order order = new Order
                    {
                        Total = 0.00m,
                        CustomerID = paymentAuthorization.CustomerID,
                        Date = DateTime.Now,
                        Discount = 0.0m,
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    _context.Database.BeginTransaction();

                    //var updateOrder = await _context.Orders.FirstOrDefaultAsync(e => e.ID == order.ID);
                    order.Authorisation = "ZERO DOLLAR PURCHASE";
                    order.OrderNumber = DateTime.Now.ToString("MMddyyyyHHmmss");

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                    if (paymentAuthorization.OrderDetails != null && paymentAuthorization.OrderDetails.Count > 0)
                    {
                        try
                        {
                            paymentAuthorization.OrderDetails.ForEach(e => e.OrderID = order.ID);
                            _context.OrderDetails.AddRange(paymentAuthorization.OrderDetails);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        { }

                        List<CustomerTicket> customerTickets = new List<CustomerTicket>();
                        foreach (var orderDetail in paymentAuthorization.OrderDetails)
                        {
                            for (var i = 0; i < orderDetail.Qty; i++)
                            {
                                customerTickets.Add(new CustomerTicket
                                {
                                    EventTicketID = orderDetail.EventTicketID,
                                    OrderID = order.ID,
                                    Validated = false,
                                    IsMemberTicket = orderDetail.IsMemberTicket
                                });
                            }
                        }

                        _context.CustomerTickets.AddRange(customerTickets);
                        await _context.SaveChangesAsync();

                    }

                    if (paymentAuthorization.ContactTraces != null && paymentAuthorization.ContactTraces.Count > 0)
                    {
                        paymentAuthorization.ContactTraces.ForEach(e => e.OrderID = order.ID);

                        _context.ContactTraces.AddRange(paymentAuthorization.ContactTraces);
                        await _context.SaveChangesAsync();
                    }

                    _context.Database.CommitTransaction();
                    paymentResponse.IsApproved = true;

                    try
                    {
                        if (isAdhoc)
                        {
                            var thisOrder = await _context.Orders
                            .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                            .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                            .Include(e => e.Customer)
                            .FirstOrDefaultAsync(e => e.ID == order.ID);

                            if (!String.IsNullOrEmpty(paymentAuthorization.Email))
                            {
                                customer.FirstName = paymentAuthorization.NameOnCard;
                                customer.Email = paymentAuthorization.Email;
                            }

                            await emailRepository.SendPaymentConfirmation(customer, order.Authorisation, thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent);
                        }
                        else
                        {
                            var printCustomerTickets = await _context.CustomerTickets
                                .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                .Include(e => e.Order).ThenInclude(e => e.Customer)
                                .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                .Where(e => e.OrderID == order.ID)
                                .ToListAsync();

                            printCustomerTickets.ForEach(e => e.CustomerMatchTicket = new CustomerMatchTicket
                            {
                                EventTicketID = e.EventTicketID,
                                ID = e.ID,
                                OrderID = e.OrderID
                            });

                            var thisOrder = printCustomerTickets.Select(e => e.Order).FirstOrDefault();

                            //var contactTraces = await _context.ContactTraces
                            //.Include(e => e.Order).ThenInclude(e => e.OrderDetails)
                            //.Include(e => e.Order).ThenInclude(e => e.Customer)
                            //.Where(e => e.OrderID == order.ID)
                            //.ToListAsync();

                            //This is new, dont publish new version until tested
                            List<PrintTicket> ticketsToPrint = new List<PrintTicket>();
                            //int i = 1;
                            for (var i = 0; i < printCustomerTickets.Count; i++)
                            {
                                var stream = CreateTicketPDF(thisOrder, printCustomerTickets[i], "Ticket #" + (i + 1));

                                ticketsToPrint.Add(new PrintTicket
                                {
                                    File = stream,
                                    FileName = "EventTicket" + (i + 1) + ".pdf"
                                });
                            }

                            await emailRepository.SendPaymentConfirmation(customer, order.Authorisation, thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent, ticketsToPrint);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "Payment Email");
                    }

                    paymentResponse.Code = "1";
                    paymentResponse.Description = "Success";

                    return paymentResponse;
                }
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();

                //var response = await request.Payment(paymentAuthorization);
                Debug.WriteLine(ex.Message, "Payment");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";

            return paymentResponse;
        }

        
        public async Task<string> TransferRequest(TransferRequest transferRequest, ClaimsPrincipal user)
        {
            try
            {
                var email = user.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var transferToCustomer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == transferRequest.Email && e.Email != email);

                if (transferToCustomer != null)
                {
                    var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == email);

                    var customerTicket = await _context.CustomerTickets
                        .Include(e => e.Order)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                        .FirstOrDefaultAsync(e => e.ID == transferRequest.CustomerTicketID);

                    if (customerTicket != null && customer != null)
                    {
                        if (!customerTicket.Validated)
                        {
                            try
                            {
                                customerTicket.TransferCustomerID = transferToCustomer.ID;
                                customerTicket.IsTransfer = false;
                                customerTicket.TransferTime = null;

                                _context.CustomerTickets.Update(customerTicket);
                                await _context.SaveChangesAsync();

                                await emailRepository.SendTransferRequest(customer, transferToCustomer, customerTicket);

                                return "Ticket transferred successfully.";
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message, "Transfer Request Email");
                            }

                            return "There was an issue transferring the match ticket.";
                        }
                        else
                            return "This ticket is not valid for transferring.";
                    }
                }

                return "Please provide a valid user account.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TransferRequest");
            }

            return "There was an issue transferring the match ticket.";
        }

        public async Task<List<AcceptTransfer>> GetTransferRequests(ClaimsPrincipal user)
        {
            List<AcceptTransfer> transferRequests = new List<AcceptTransfer>();

            try
            {
                var email = user.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                if (email != null)
                {
                    var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == email);
                    DateTime currentDate = DateTime.Now.Date;

                    var customerTickets = await _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                        .Include(e => e.TransferCustomer)
                        .Where(e => currentDate <= e.EventTicket.SportEvent.Date.AddHours(-4) && (e.TransferCustomer.Email == email && ((e.IsTransfer.HasValue && !e.IsTransfer.Value) || (!e.IsTransfer.HasValue))))
                        .ToListAsync();

                    foreach(var ticket in customerTickets)
                    {
                        ticket.Order.Customer.Password = null;

                        transferRequests.Add(new Models.Shop.AcceptTransfer
                        {
                            CustomerID = ticket.Order.CustomerID,
                            TransferCustomerID = ticket.TransferCustomerID,
                            CustomerTicketID = ticket.ID,
                            CustomerTicket = ticket,
                            Customer = ticket.Order.Customer,
                            TransferCustomer = ticket.TransferCustomer,
                            Accept = false
                        });
                    }
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TransferRequest");
            }

            return transferRequests;
        }

        public async Task<string> AcceptTransferRequest(AcceptTransfer acceptTransfer)
        {
            try
            {
                if (acceptTransfer.Accept)
                {
                    var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == acceptTransfer.CustomerID);

                    var transferToCustomer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == acceptTransfer.TransferCustomerID);

                    var customerTicket = await _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                        .Include(e => e.TransferCustomer)
                        .FirstOrDefaultAsync(e => e.ID == acceptTransfer.CustomerTicketID && e.TransferCustomerID != null);

                    if (customerTicket != null && transferToCustomer != null)
                    {
                        try
                        {
                            customerTicket.IsTransfer = true;
                            customerTicket.TransferTime = DateTime.Now;

                            _context.CustomerTickets.Update(customerTicket);
                            await _context.SaveChangesAsync();

                            try
                            {
                                await emailRepository.SendTransferAccept(customer, transferToCustomer, customerTicket);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message, "Transfer Request Email");
                            }

                            return "Transfer completed successfully!";
                        }
                        catch(Exception ex)
                        {
                            Debug.WriteLine(ex.Message, "Transfer Request Accept");
                        }

                        return "There was an issue completing this match ticket transfer request.";
                    }

                }
                else
                {
                    var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == acceptTransfer.CustomerID);

                    var transferToCustomer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == acceptTransfer.TransferCustomerID);

                    var customerTicket = await _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                        .Include(e => e.TransferCustomer)
                        .FirstOrDefaultAsync(e => e.ID == acceptTransfer.CustomerTicketID && e.TransferCustomerID != null);

                    if (customerTicket != null && transferToCustomer != null)
                    {
                        try
                        {
                            customerTicket.IsTransfer = false;
                            customerTicket.TransferTime = null;
                            customerTicket.TransferCustomerID = null;

                            _context.CustomerTickets.Update(customerTicket);
                            await _context.SaveChangesAsync();

                            try
                            {
                                await emailRepository.SendTransferReject(customer, transferToCustomer, customerTicket);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message, "Transfer Request Email");
                            }

                            return "Transfer rejected successfully!";
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message, "Transfer Request Reject");
                        }
                    }

                    return "There was an issue completing this match ticket transfer request.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TransferRequest");
            }

            return "There was an issue completing your transfer.";
        }

        [Obsolete]
        public async Task<PaymentResponse> PurchaseTest(ClaimsPrincipal claimsUser, PaymentAuthorize paymentAuthorization, bool isAdhoc = false)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            _context.Database.BeginTransaction();

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                        .Select(c => c.Value).SingleOrDefault();

                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == (isAdhoc ? Constants.TicketingEmail : email));

                paymentAuthorization.CustomerID = customer.ID;

                var sportEvent = await _context.SportEvents
                    .Include(e => e.TicketCompany)
                    .Include(e => e.Sport)
                    .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.SportEventID);

                var eventTickets = await _context.EventTickets
                    .Include(e => e.Product)
                    .Where(e => e.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var customerTicketsList = await _context.CustomerTickets
                    .Include(e => e.EventTicket)
                    .Where(e => e.EventTicket.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == sportEvent.TicketCompanyID);

                //Ticket Member
                var isTicketMember = await _context.TicketMembers.AnyAsync(e => e.CustomerID == customer.ID && e.TicketCompanyID == sportEvent.TicketCompanyID);

                if (!isTicketMember)
                    isTicketMember = await _context.Members.AnyAsync(e => e.Email == email && e.TicketCompanyID == sportEvent.TicketCompanyID);

                var ticketCount = customerTicketsList.Count();

                if (ticketCount < ticketConfiguration.Stock && paymentAuthorization.OrderDetails.Sum(e => e.Qty) <= ticketConfiguration.Stock)
                {
                    //var processingFee = await _context.Settings.FirstOrDefaultAsync(e => e.ID == Constants.SETTING_PROCESSING_FEE_ID);

                    Decimal ProcessingFeeAmount = 0.0m;
                    Decimal PaymentAmount = 0.0m;
                    Decimal SubtotalAmount = 0.0m;

                    string payment = String.Format("{0,0:N2}", Decimal.Parse(paymentAuthorization.Amount));

                    //string topUp = topUpPayment.Amount;
                    //topUp.Insert(topUp.Length - 2, ".");
                    PaymentAmount = Convert.ToDecimal(payment);

                    foreach (var ticket in paymentAuthorization.OrderDetails)
                    {
                        if (ticket.Qty > 0)
                        {
                            var fee = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Fee;
                            var price = isTicketMember ? (eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.MemberPrice ?? eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price) : eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;

                            ProcessingFeeAmount += (ticket.Qty * fee);
                            SubtotalAmount += (ticket.Qty * price);
                        }
                    }

                    if (PaymentAmount != (SubtotalAmount + ProcessingFeeAmount))
                    {
                        paymentAuthorization.Amount = (SubtotalAmount + ProcessingFeeAmount).ToString();
                    }

                    if (PaymentAmount > Convert.ToDecimal(paymentAuthorization.Amount))
                    {
                        PaymentAmount = Convert.ToDecimal(paymentAuthorization.Amount);
                    }

                    PaymentAmount = PaymentAmount != Convert.ToDecimal(paymentAuthorization.Amount) ? Convert.ToDecimal(paymentAuthorization.Amount) : PaymentAmount;

                    paymentAuthorization.Amount = paymentAuthorization.Amount.Replace(".", "");

                    Order order = new Order
                    {
                        Total = PaymentAmount,
                        CustomerID = paymentAuthorization.CustomerID,
                        Date = DateTime.Now,
                        Discount = 0.0m,
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    //var response = await request.Payment(paymentAuthorization);

                    //if (response.CreditCardTransactionResults.ResponseCode == "1")
                    //{

                    order.Authorisation = "TESTAUTHCODE";
                    order.OrderNumber = "TESTORDERNUM";

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                    if (paymentAuthorization.OrderDetails != null && paymentAuthorization.OrderDetails.Count > 0)
                    {
                        try
                        {
                            paymentAuthorization.OrderDetails.ForEach(e => e.OrderID = order.ID);
                            _context.OrderDetails.AddRange(paymentAuthorization.OrderDetails);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        { }

                        List<CustomerTicket> customerTickets = new List<CustomerTicket>();
                        foreach (var orderDetail in paymentAuthorization.OrderDetails)
                        {
                            for (var i = 0; i < orderDetail.Qty; i++)
                            {
                                customerTickets.Add(new CustomerTicket
                                {
                                    EventTicketID = orderDetail.EventTicketID,
                                    OrderID = order.ID,
                                    Validated = false,
                                    IsMemberTicket = orderDetail.IsMemberTicket
                                });
                            }
                        }

                        _context.CustomerTickets.AddRange(customerTickets);
                        await _context.SaveChangesAsync();

                    }

                    if (paymentAuthorization.ContactTraces != null && paymentAuthorization.ContactTraces.Count > 0)
                    {
                        paymentAuthorization.ContactTraces.ForEach(e => e.OrderID = order.ID);

                        _context.ContactTraces.AddRange(paymentAuthorization.ContactTraces);
                        await _context.SaveChangesAsync();
                    }

                    _context.Database.CommitTransaction();
                    paymentResponse.IsApproved = true;

                    try
                    {
                        if (isAdhoc)
                        {
                            var thisOrder = await _context.Orders
                            .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                            .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                            .Include(e => e.Customer)
                            .FirstOrDefaultAsync(e => e.ID == order.ID);

                            if (!String.IsNullOrEmpty(paymentAuthorization.Email))
                            {
                                customer.FirstName = paymentAuthorization.NameOnCard;
                                customer.Email = paymentAuthorization.Email;
                            }

                            await emailRepository.SendPaymentConfirmation(customer, "TESTAUTHCODE", thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent);
                        }
                        else
                        {
                            var printCustomerTickets = await _context.CustomerTickets
                                .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                .Include(e => e.Order).ThenInclude(e => e.Customer)
                                .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                .Where(e => e.OrderID == order.ID)
                                .ToListAsync();

                            printCustomerTickets.ForEach(e => e.CustomerMatchTicket = new CustomerMatchTicket
                            {
                                EventTicketID = e.EventTicketID,
                                ID = e.ID,
                                OrderID = e.OrderID
                            });

                            var thisOrder = printCustomerTickets.Select(e => e.Order).FirstOrDefault();

                            //This is new, dont publish new version until tested
                            List<PrintTicket> ticketsToPrint = new List<PrintTicket>();
                            //int i = 1;
                            for (var i = 0; i < printCustomerTickets.Count; i++)
                            {
                                var stream = CreateTicketPDF(thisOrder, printCustomerTickets[i], "Ticket #" + (i + 1));

                                ticketsToPrint.Add(new PrintTicket
                                {
                                    File = stream,
                                    FileName = "EventTicket" + (i + 1) + ".pdf"
                                });
                            }

                            await emailRepository.SendPaymentConfirmation(customer, "TESTAUTHCODE", thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent, ticketsToPrint);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "Payment Email");
                    }
                }

                paymentResponse.Code = "1";
                paymentResponse.Description = "Success";

                return paymentResponse;
                //}
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                Debug.WriteLine(ex.Message, "Payment Test");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";

            return paymentResponse;
        }

        public async Task<PaymentResponse> PurchaseTicketTest(ClaimsPrincipal claimsUser, PaymentAuthorize paymentAuthorization, bool isAdhoc = false)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            _context.Database.BeginTransaction();

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                        .Select(c => c.Value).SingleOrDefault();

                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == (isAdhoc ? Constants.TicketingEmail : email));

                paymentAuthorization.CustomerID = customer.ID;

                var sportEvent = await _context.SportEvents
                    .Include(e => e.TicketCompany)
                    .Include(e => e.Sport)
                    .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.SportEventID);

                var eventTickets = await _context.EventTickets
                    .Include(e => e.Product)
                    .Where(e => e.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var customerTicketsList = await _context.CustomerTickets
                    .Include(e => e.EventTicket)
                    .Where(e => e.EventTicket.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == sportEvent.TicketCompanyID);

                //Ticket Member
                var isTicketMember = await _context.TicketMembers.AnyAsync(e => e.CustomerID == customer.ID && e.TicketCompanyID == sportEvent.TicketCompanyID);

                if (!isTicketMember)
                    isTicketMember = await _context.Members.AnyAsync(e => e.Email == email && e.TicketCompanyID == sportEvent.TicketCompanyID);

                var ticketCount = customerTicketsList.Count();

                if (ticketCount < ticketConfiguration.Stock && paymentAuthorization.OrderDetails.Sum(e => e.Qty) <= ticketConfiguration.Stock)
                {
                    //var processingFee = await _context.Settings.FirstOrDefaultAsync(e => e.ID == Constants.SETTING_PROCESSING_FEE_ID);

                    Decimal ProcessingFeeAmount = 0.0m;
                    Decimal PaymentAmount = 0.0m;
                    Decimal SubtotalAmount = 0.0m;

                    string payment = String.Format("{0,0:N2}", Decimal.Parse(paymentAuthorization.Amount));

                    //string topUp = topUpPayment.Amount;
                    //topUp.Insert(topUp.Length - 2, ".");
                    PaymentAmount = Convert.ToDecimal(payment);

                    foreach (var ticket in paymentAuthorization.OrderDetails)
                    {
                        //if (ticket.Qty > 0)
                        //{
                        //    var fee = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Fee;
                        //    var price = isTicketMember ? (eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.MemberPrice ?? eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price) : eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;

                        //    ProcessingFeeAmount += (ticket.Qty * fee);
                        //    SubtotalAmount += (ticket.Qty * price);
                        //}

                        if (ticket.Qty > 0)
                        {
                            decimal fee = 0;
                            decimal price = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;
                            decimal memberPrice = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.MemberPrice ?? 0;

                            if (isTicketMember && ticket.IsMemberTicket)
                            {
                                if (price != memberPrice)
                                    price = memberPrice;
                            }

                            //var price = isTicketMember ? (eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.MemberPrice ?? eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price) : eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;
                            //var price = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price;

                            if (price > 0)
                                fee = eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Fee;

                            //ticket.IsMemberTicket = (eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.MemberPrice == eventTickets.FirstOrDefault(e => e.ID == ticket.EventTicketID).Product.Price ? true : false);

                            ProcessingFeeAmount += (ticket.Qty * fee);
                            SubtotalAmount += (ticket.Qty * price);
                        }
                    }

                    if (PaymentAmount != (SubtotalAmount + ProcessingFeeAmount))
                    {
                        paymentAuthorization.Amount = (SubtotalAmount + ProcessingFeeAmount).ToString();
                    }

                    if (PaymentAmount > Convert.ToDecimal(paymentAuthorization.Amount))
                    {
                        PaymentAmount = Convert.ToDecimal(paymentAuthorization.Amount);
                    }

                    PaymentAmount = PaymentAmount != Convert.ToDecimal(paymentAuthorization.Amount) ? Convert.ToDecimal(paymentAuthorization.Amount) : PaymentAmount;

                    paymentAuthorization.Amount = paymentAuthorization.Amount.Replace(".", "");

                    Order order = new Order
                    {
                        Total = PaymentAmount,
                        CustomerID = paymentAuthorization.CustomerID,
                        Date = DateTime.Now,
                        Discount = 0.0m,
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    //var response = await request.Payment(paymentAuthorization);

                    //if (response.CreditCardTransactionResults.ResponseCode == "1")
                    //{

                    order.Authorisation = "TESTAUTHCODE";
                    order.OrderNumber = "TESTORDERNUM";

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                    if (paymentAuthorization.OrderDetails != null && paymentAuthorization.OrderDetails.Count > 0)
                    {
                        try
                        {
                            paymentAuthorization.OrderDetails.ForEach(e => e.OrderID = order.ID);
                            _context.OrderDetails.AddRange(paymentAuthorization.OrderDetails);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        { }

                        List<CustomerTicket> customerTickets = new List<CustomerTicket>();
                        foreach (var orderDetail in paymentAuthorization.OrderDetails)
                        {
                            for (var i = 0; i < orderDetail.Qty; i++)
                            {
                                customerTickets.Add(new CustomerTicket
                                {
                                    EventTicketID = orderDetail.EventTicketID,
                                    OrderID = order.ID,
                                    Validated = false,
                                    IsMemberTicket = orderDetail.IsMemberTicket
                                });
                            }
                        }

                        _context.CustomerTickets.AddRange(customerTickets);
                        await _context.SaveChangesAsync();

                    }

                    if (paymentAuthorization.ContactTraces != null && paymentAuthorization.ContactTraces.Count > 0)
                    {
                        paymentAuthorization.ContactTraces.ForEach(e => e.OrderID = order.ID);

                        _context.ContactTraces.AddRange(paymentAuthorization.ContactTraces);
                        await _context.SaveChangesAsync();
                    }

                    _context.Database.CommitTransaction();
                    paymentResponse.IsApproved = true;

                    try
                    {
                        if (isAdhoc)
                        {
                            var thisOrder = await _context.Orders
                            .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                            .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                            .Include(e => e.Customer)
                            .FirstOrDefaultAsync(e => e.ID == order.ID);

                            if (!String.IsNullOrEmpty(paymentAuthorization.Email))
                            {
                                customer.FirstName = paymentAuthorization.NameOnCard;
                                customer.Email = paymentAuthorization.Email;
                            }

                            await emailRepository.SendPaymentConfirmation(customer, "TESTAUTHCODE", thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent);
                        }
                        else
                        {
                            var printCustomerTickets = await _context.CustomerTickets
                                .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                .Include(e => e.Order).ThenInclude(e => e.Customer)
                                .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                .Where(e => e.OrderID == order.ID)
                                .ToListAsync();

                            printCustomerTickets.ForEach(e => e.CustomerMatchTicket = new CustomerMatchTicket
                            {
                                EventTicketID = e.EventTicketID,
                                ID = e.ID,
                                OrderID = e.OrderID
                            });

                            var thisOrder = printCustomerTickets.Select(e => e.Order).FirstOrDefault();

                            //This is new, dont publish new version until tested
                            List<PrintTicket> ticketsToPrint = new List<PrintTicket>();
                            //int i = 1;
                            for (var i = 0; i < printCustomerTickets.Count; i++)
                            {
                                var stream = CreateTicketPDF(thisOrder, printCustomerTickets[i], "Ticket #" + (i + 1));

                                ticketsToPrint.Add(new PrintTicket
                                {
                                    File = stream,
                                    FileName = "EventTicket" + (i + 1) + ".pdf"
                                });
                            }

                            await emailRepository.SendPaymentConfirmation(customer, "TESTAUTHCODE", thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent, ticketsToPrint);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "Payment Email");
                    }
                }

                paymentResponse.Code = "1";
                paymentResponse.Description = "Success";

                return paymentResponse;
                //}
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                Debug.WriteLine(ex.Message, "Payment Test");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";

            return paymentResponse;
        }

        public async Task<PaymentResponse> ZeroPurchaseTest(ClaimsPrincipal claimsUser, PaymentAuthorize paymentAuthorization, bool isAdhoc = false)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            _context.Database.BeginTransaction();

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                        .Select(c => c.Value).SingleOrDefault();

                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == (isAdhoc ? Constants.TicketingEmail : email));

                paymentAuthorization.CustomerID = customer.ID;

                var sportEvent = await _context.SportEvents
                     .Include(e => e.TicketCompany)
                     .Include(e => e.Sport)
                     .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.SportEventID);

                var eventTickets = await _context.EventTickets
                    .Include(e => e.Product)
                    .Where(e => e.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var customerTicketsList = await _context.CustomerTickets
                    .Include(e => e.EventTicket)
                    .Where(e => e.EventTicket.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();


                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == sportEvent.TicketCompanyID);

                var ticketCount = customerTicketsList.Count();

                if (ticketCount < ticketConfiguration.Stock && paymentAuthorization.OrderDetails.Sum(e => e.Qty) <= ticketConfiguration.Stock)
                {                    
                    Order order = new Order
                    {
                        Total = 0.00m,
                        CustomerID = paymentAuthorization.CustomerID,
                        Date = DateTime.Now,
                        Discount = 0.0m,
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    order.Authorisation = "TESTZEROPURCHASEAUTHCODE";
                    order.OrderNumber = DateTime.Now.ToString("MMddyyyyHHmmss");

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                    if (paymentAuthorization.OrderDetails != null && paymentAuthorization.OrderDetails.Count > 0)
                    {
                        try
                        {
                            paymentAuthorization.OrderDetails.ForEach(e => e.OrderID = order.ID);
                            _context.OrderDetails.AddRange(paymentAuthorization.OrderDetails);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        { }

                        List<CustomerTicket> customerTickets = new List<CustomerTicket>();
                        foreach (var orderDetail in paymentAuthorization.OrderDetails)
                        {
                            for (var i = 0; i < orderDetail.Qty; i++)
                            {
                                customerTickets.Add(new CustomerTicket
                                {
                                    EventTicketID = orderDetail.EventTicketID,
                                    OrderID = order.ID,
                                    Validated = false,
                                    IsMemberTicket = orderDetail.IsMemberTicket
                                });
                            }
                        }

                        _context.CustomerTickets.AddRange(customerTickets);
                        await _context.SaveChangesAsync();

                    }

                    if (paymentAuthorization.ContactTraces != null && paymentAuthorization.ContactTraces.Count > 0)
                    {
                        paymentAuthorization.ContactTraces.ForEach(e => e.OrderID = order.ID);

                        _context.ContactTraces.AddRange(paymentAuthorization.ContactTraces);
                        await _context.SaveChangesAsync();
                    }

                    _context.Database.CommitTransaction();
                    paymentResponse.IsApproved = true;

                    try
                    {
                        if (isAdhoc)
                        {
                            var thisOrder = await _context.Orders
                                .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                                .Include(e => e.Customer)
                                .FirstOrDefaultAsync(e => e.ID == order.ID);

                            if (!String.IsNullOrEmpty(paymentAuthorization.Email))
                            {
                                customer.FirstName = paymentAuthorization.NameOnCard;
                                customer.Email = paymentAuthorization.Email;
                            }

                            await emailRepository.SendPaymentConfirmation(customer, order.Authorisation, thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent);
                        }
                        else
                        {
                            var printCustomerTickets = await _context.CustomerTickets
                                .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                .Include(e => e.Order).ThenInclude(e => e.Customer)
                                .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                                .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                                .Where(e => e.OrderID == order.ID)
                                .ToListAsync();

                            printCustomerTickets.ForEach(e => e.CustomerMatchTicket = new CustomerMatchTicket
                            {
                                EventTicketID = e.EventTicketID,
                                ID = e.ID,
                                OrderID = e.OrderID
                            });

                            var thisOrder = printCustomerTickets.Select(e => e.Order).FirstOrDefault();

                            //var contactTraces = await _context.ContactTraces
                            //    .Include(e => e.Order).ThenInclude(e => e.OrderDetails)
                            //    .Include(e => e.Order).ThenInclude(e => e.Customer)
                            //    .Where(e => e.OrderID == order.ID)
                            //    .ToListAsync();

                            //This is new, dont publish new version until tested
                            List<PrintTicket> ticketsToPrint = new List<PrintTicket>();
                            //int i = 1;
                            for (var i = 0; i < printCustomerTickets.Count; i++)
                            {
                                var stream = CreateTicketPDF(thisOrder, printCustomerTickets[i], "Ticket #" + (i + 1));

                                ticketsToPrint.Add(new PrintTicket
                                {
                                    File = stream,
                                    FileName = "EventTicket" + (i + 1) + ".pdf"
                                });
                            }

                            await emailRepository.SendPaymentConfirmation(customer, "TESTAUTHCODE", thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent, ticketsToPrint);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "Payment Email");
                    }
                }

                paymentResponse.Code = "1";
                paymentResponse.Description = "Success";

                return paymentResponse;
                //}
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                Debug.WriteLine(ex.Message, "Payment Test");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";

            return paymentResponse;
        }

        //public async Task<PaymentResponse> AdhocPurchaseTest(ClaimsPrincipal claimsUser, PaymentAuthorize paymentAuthorization)
        //{
        //    Request request = new Request();
        //    PaymentResponse paymentResponse = new PaymentResponse();
        //    paymentResponse.IsApproved = false;

        //    try
        //    {
        //        var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
        //                             .Select(c => c.Value).SingleOrDefault();

        //        var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == Constants.TicketingEmail);

        //        paymentAuthorization.CustomerID = customer.ID;

        //        var eventTicket = await _context.EventTickets
        //            .Include(e => e.Product)
        //            .FirstOrDefaultAsync(e => e.SportEventID == paymentAuthorization.SportEventID);

        //        var customerTicketsList = await _context.CustomerTickets
        //            .Include(e => e.EventTicket)
        //            .Where(e => e.EventTicket.SportEventID == paymentAuthorization.SportEventID)
        //            .ToListAsync();

        //        var ticketCompanyID = eventTicket.Product.TicketCompanyID;

        //        var ticketConfiguration = await _context.TicketConfigurations
        //            .FirstOrDefaultAsync(e => e.TicketCompanyID == ticketCompanyID);

        //        var ticketCount = customerTicketsList.Count();

        //        if (ticketCount < ticketConfiguration.Stock && paymentAuthorization.OrderDetails.Sum(e => e.Qty) <= ticketConfiguration.Stock)
        //        {
        //            var processingFee = await _context.Settings.FirstOrDefaultAsync(e => e.ID == Constants.SETTING_PROCESSING_FEE_ID);

        //            Decimal ProcessingFeeAmount = eventTicket.Product.Fee ?? Convert.ToDecimal(processingFee.Value);
        //            Decimal PaymentAmount = 0.0m;

        //            string payment = String.Format("{0,0:N2}", Decimal.Parse(paymentAuthorization.Amount));

        //            PaymentAmount = Convert.ToDecimal(payment);
        //            var orderSubTotal = paymentAuthorization.OrderDetails.Sum(e => e.Subtotal);

        //            if (PaymentAmount != orderSubTotal + (ProcessingFeeAmount * paymentAuthorization.Quantity))
        //            {
        //                paymentAuthorization.Amount = (orderSubTotal + (ProcessingFeeAmount * paymentAuthorization.Quantity)).ToString();
        //            }

        //            if (PaymentAmount > Convert.ToDecimal(paymentAuthorization.Amount))
        //            {
        //                PaymentAmount = Convert.ToDecimal(paymentAuthorization.Amount);
        //            }

        //            PaymentAmount = PaymentAmount != Convert.ToDecimal(paymentAuthorization.Amount) ? Convert.ToDecimal(paymentAuthorization.Amount) : PaymentAmount;

        //            paymentAuthorization.Amount = paymentAuthorization.Amount.Replace(".", "");

        //            Order order = new Order
        //            {
        //                Total = PaymentAmount,
        //                CustomerID = paymentAuthorization.CustomerID,
        //                Date = DateTime.Now,
        //                Discount = 0.0m,
        //            };

        //            _context.Orders.Add(order);
        //            await _context.SaveChangesAsync();

        //                _context.Database.BeginTransaction();

        //                order.Authorisation = "TESTAUTHCODE";
        //                order.OrderNumber = "TESTORDERNUM";

        //                _context.Orders.Update(order);
        //                await _context.SaveChangesAsync();

        //                if (paymentAuthorization.OrderDetails != null && paymentAuthorization.OrderDetails.Count > 0)
        //                {
        //                    try
        //                    {
        //                        paymentAuthorization.OrderDetails.ForEach(e => e.OrderID = order.ID);
        //                        _context.OrderDetails.AddRange(paymentAuthorization.OrderDetails);
        //                        await _context.SaveChangesAsync();
        //                    }
        //                    catch (Exception ex)
        //                    { }

        //                    List<CustomerTicket> customerTickets = new List<CustomerTicket>();
        //                    foreach (var orderDetail in paymentAuthorization.OrderDetails)
        //                    {
        //                        for (var i = 0; i < orderDetail.Qty; i++)
        //                        {
        //                            customerTickets.Add(new CustomerTicket
        //                            {
        //                                EventTicketID = orderDetail.EventTicketID,
        //                                OrderID = order.ID,
        //                                Validated = false
        //                            });
        //                        }
        //                    }

        //                    _context.CustomerTickets.AddRange(customerTickets);
        //                    await _context.SaveChangesAsync();

        //                }

        //                if (paymentAuthorization.ContactTraces != null && paymentAuthorization.ContactTraces.Count > 0)
        //                {
        //                    paymentAuthorization.ContactTraces.ForEach(e => e.OrderID = order.ID);

        //                    _context.ContactTraces.AddRange(paymentAuthorization.ContactTraces);
        //                    await _context.SaveChangesAsync();
        //                }

        //                _context.Database.CommitTransaction();
        //                paymentResponse.IsApproved = true;

        //                try
        //                {
        //                    var sportEvent = await _context.SportEvents
        //                        .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.SportEventID);

        //                    var thisOrder = await _context.Orders
        //                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
        //                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
        //                    .Include(e => e.Customer)
        //                    .FirstOrDefaultAsync(e => e.ID == order.ID);

        //                    if (!String.IsNullOrEmpty(paymentAuthorization.Email))
        //                    {
        //                        customer.FirstName = paymentAuthorization.NameOnCard;
        //                        customer.Email = paymentAuthorization.Email;
        //                    }

        //                    await emailRepository.SendPaymentConfirmation(customer, "TESTAUTHCODE", thisOrder, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Debug.WriteLine(ex.Message, "Payment Email");
        //                }

        //            paymentResponse.Code = "1";
        //            paymentResponse.Description = "Success";

        //            return paymentResponse;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _context.Database.RollbackTransaction();

        //        //var response = await request.Payment(paymentAuthorization);
        //        Debug.WriteLine(ex.Message, "Payment");
        //    }

        //    paymentResponse.Description = "There was an issue with your payment, please try again.";

        //    return paymentResponse;
        //}

        public async Task<string> ResendPurchaseConfirmation(string orderNumber)
        {
            try
            {
                try
                {
                    

                    ////var orderDetails = await _context.OrderDetails.Include(e => e.EventTicket).Where(e => e.OrderID == order.ID).ToListAsync();

                    //var fixture = await _context.SportEvents
                    //    .FirstOrDefaultAsync(e => e.ID == orderDetails.FirstOrDefault().EventTicket.SportEventID);


                    //await emailRepository.SendPaymentConfirmation(customer, order.Authorisation, order, orderDetails.Sum(e => e.Qty), fixture);


                    try
                    {
                        //var order = await _context.Orders
                            //.Include(e => e.Customer)
                            //.Include(e => e.OrderDetails)
                            //.FirstOrDefaultAsync(e => e.OrderNumber == orderNumber);

                        //var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == order.Customer.ID);

                        //var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == paymentAuthorization.CustomerID);
                        //var sportEvent = await _context.SportEvents
                        //    .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.SportEventID);

                        var printCustomerTickets = await _context.CustomerTickets
                            .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                            .Include(e => e.Order).ThenInclude(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                            .Include(e => e.Order).ThenInclude(e => e.Customer)
                            .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                            .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                            .Where(e => e.Order.OrderNumber == orderNumber)
                            .ToListAsync();

                        printCustomerTickets.ForEach(e => e.CustomerMatchTicket = new CustomerMatchTicket
                        {
                            EventTicketID = e.EventTicketID,
                            ID = e.ID,
                            OrderID = e.OrderID
                        });



                        var thisOrder = printCustomerTickets.Select(e => e.Order).FirstOrDefault();

                        var contactTraces = await _context.ContactTraces
                            .Include(e => e.Order).ThenInclude(e => e.OrderDetails)
                            .Include(e => e.Order).ThenInclude(e => e.Customer)
                            .Where(e => e.Order.OrderNumber == orderNumber)
                            .ToListAsync();

                        //This is new, dont publish new version until tested
                        List<PrintTicket> ticketsToPrint = new List<PrintTicket>();
                        //int i = 1;
                        for (var i = 0; i < printCustomerTickets.Count; i++)
                        {
                            var stream = CreateTicketPDF(thisOrder, printCustomerTickets[i], "Ticket #" + (i + 1));

                                ticketsToPrint.Add(new PrintTicket
                                {
                                    File = stream,
                                    FileName = "EventTicket" + (i + 1) + ".pdf"
                                });                            
                        }

                        await emailRepository.SendPaymentConfirmation(printCustomerTickets.FirstOrDefault().Order.Customer, printCustomerTickets.FirstOrDefault().Order.Authorisation, printCustomerTickets.FirstOrDefault().Order, printCustomerTickets.FirstOrDefault().Order.OrderDetails.Sum(e => e.Qty), printCustomerTickets.FirstOrDefault().EventTicket.SportEvent, ticketsToPrint);

                        return "Confirmation sent successfully";
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "Payment Email");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "Payment Email");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Payment");
            }

            return "Failed to resend confirmation";
        }

        public async Task Insert(CustomerTicket item)
        {
            try
            {
                _context.CustomerTickets.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Match Ticket");
            }
        }

        public async Task Update(CustomerTicket item)
        {
            try
            {
                _context.CustomerTickets.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Match Ticket");
            }
        }

        public Stream CreateTicketPDF(Order order, CustomerTicket customerTicket, string TicketNumber)
        {
            try
            {
#if DEBUG
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic/Bermuda");
#else
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");
#endif

                //Create a new PDF document
                PdfDocument doc = new PdfDocument();

                //Add a page
                PdfPage page = doc.Pages.Add();

                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;

                //Loads the image as stream
                FileStream imageStream = new FileStream("wwwroot/images/OnTrackBannerSmall.png", FileMode.Open, FileAccess.Read);
                RectangleF bounds = new RectangleF(10, 0, 480, 189);
                PdfImage image = PdfImage.FromStream(imageStream);
                //Draws the image to the PDF page
                page.Graphics.DrawImage(image, bounds);

                bounds = new RectangleF(0, bounds.Bottom + 30, graphics.ClientSize.Width, 30);

                //Set the standard font
                PdfFont headerFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20, PdfFontStyle.Bold);
                PdfFont subHeaderFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12);
                PdfFont bodyFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10);
                PdfFont bodyBoldFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 13, PdfFontStyle.Bold);
                PdfFont lightHeaderFont = new PdfStandardFont(PdfFontFamily.Helvetica, 9);
                PdfFont smallInfoFont = new PdfStandardFont(PdfFontFamily.Helvetica, 8);
                PdfBrush lightHeaderBrush = new PdfSolidBrush(Color.Gray);
                PdfStringFormat smallInfoFormat = new PdfStringFormat();
                smallInfoFormat.WordWrap = PdfWordWrapType.Word;
                smallInfoFormat.Alignment = PdfTextAlignment.Center;

                PdfStringFormat headerFormat = new PdfStringFormat();
                headerFormat.Alignment = PdfTextAlignment.Center;

                //Draw the text
                //Event
                //PdfTextElement element = new PdfTextElement(customerTicket.EventTicket.SportEvent.Event, headerFont);
                //PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));

                graphics.DrawString(customerTicket.EventTicket.SportEvent.Event,
                    headerFont, PdfBrushes.Black, new RectangleF(10, bounds.Top + 2, page.GetClientSize().Width, 30), headerFormat);

                PdfTextElement element = new PdfTextElement(TicketNumber, bodyFont);
                PdfLayoutResult result = element.Draw(page, new PointF(250, bounds.Bottom + 4));

                //Ticket Type
                element = new PdfTextElement("Ticket Type", lightHeaderFont, lightHeaderBrush);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 15));

                element = new PdfTextElement(customerTicket.EventTicket.SportEvent.Title + " - " + customerTicket.EventTicket.Product.Age + (customerTicket.IsMemberTicket ? " (Member)" : string.Empty), subHeaderFont);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 4));

                //Event Date
                element = new PdfTextElement("Event Date", lightHeaderFont, lightHeaderBrush);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 8));

                DateTime eventTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(customerTicket.EventTicket.SportEvent.EventTime, DateTimeKind.Utc), timeInfo);

                element = new PdfTextElement(eventTime.AddHours(-1).ToString("MMM dd, yyyy - h:mm tt"), bodyFont);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 4));

                //Venue
                element = new PdfTextElement("Venue", lightHeaderFont, lightHeaderBrush);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 8));

                element = new PdfTextElement(customerTicket.EventTicket.SportEvent.Field.Name, bodyFont);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 4));


                //Order Info
                element = new PdfTextElement("Order Information", bodyBoldFont);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 20));

                //Order Number
                element = new PdfTextElement("Order Number", lightHeaderFont, lightHeaderBrush);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 8));

                element = new PdfTextElement(order.OrderNumber, bodyFont);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 4));

                //Order Date
                element = new PdfTextElement("Order Date", lightHeaderFont, lightHeaderBrush);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 8));

                DateTime orderDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(order.Date, DateTimeKind.Unspecified), timeInfo);

                element = new PdfTextElement(orderDate.ToString("MMM dd, yyyy - h:mm tt"), bodyFont);
                result = element.Draw(page, new PointF(250, result.Bounds.Bottom + 4));

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
                ////Draw string
                //page.Graphics.DrawString("Ticket QR", new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(20, 180));
                //Printing barcode on to the PDF
                qrBarcode.Draw(page, new PointF(10, bounds.Top + 47));


                //Information
                graphics.DrawString("There are no refunds or exchanges. All terms and conditions related to this ticket and event can be found on www.ontrackbda.com/ticket/terms. Please print this ticket or display it on your smartphone for entry into the event. Do not duplicate ticket, as each ticket has a unique qr code. For more information, please contact us at tickets@ontrackbda.com.",
                    smallInfoFont, PdfBrushes.Black, new RectangleF(10, result.Bounds.Bottom + qrBarcode.Size.Height + 15, page.GetClientSize().Width - 20, 50), smallInfoFormat);
                

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
