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
    public class SportEventRepository : IOnTrackRepository<SportEvent>
    {
        OnTrackContext _context;

        public SportEventRepository(OnTrackContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var sportEvent = await _context.SportEvents.FirstOrDefaultAsync(e => e.ID == id);

                _context.SportEvents.Remove(sportEvent);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport Event");
            }

            return false;
        }



        public async Task<IEnumerable<SportEvent>> GetAll()
        {
            List<SportEvent> sportEvents = new List<SportEvent>();

            try
            {
                sportEvents = await _context.SportEvents
                    .Include(e => e.Field)
                    .Include(e => e.Sport)
                    .ToListAsync();

            }
            catch (Exception ex)
            { }

            return sportEvents;
        }

        public async Task<SportEvent> Get(string ID)
        {
            SportEvent sportEvent = new SportEvent();

            try
            {
                sportEvent = await _context.SportEvents
                    .Include(e => e.Field)
                    .Include(e => e.Sport)
                    .FirstOrDefaultAsync(e => e.ID == ID);
            }
            catch (Exception ex)
            { }

            return sportEvent;
        }

        public async Task<IEnumerable<EventTicket>> GetEventTickets()
        {
            List<EventTicket> eventTicketsList = new List<EventTicket>();

            try
            {
                var ticketConfigurations = await _context.TicketConfigurations.ToListAsync();
                DateTime currentDate = DateTime.Now.AddHours(-4).Date;

                var eventTickets = await _context.EventTickets
                    .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                    .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => currentDate <= e.SportEvent.Date)
                    .ToListAsync();

                foreach (var eventTicket in eventTickets)
                {
                    //ticketConfig.Any(t => t.TeamID == e.Product.TeamID && DateTime.Now.Date > e.Fixture.Date.AddDays(-(t.ValidFrom)))

                    var ticketConfiguration = ticketConfigurations.FirstOrDefault(e => e.TicketCompanyID == eventTicket.Product.TicketCompanyID);

                    if (DateTime.Now.Date >= eventTicket.SportEvent.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                    {
                        var matchTicketsList = await _context.CustomerTickets
                        .Include(e => e.EventTicket)
                        .Where(e => e.EventTicket.SportEventID == eventTicket.SportEventID)
                        .ToListAsync();

                        var ticketCount = matchTicketsList.Count();
                        var availableTickets = (ticketConfiguration.Stock - ticketCount) < 0 ? 0 : (ticketConfiguration.Stock - ticketCount);

                        if (availableTickets > 0)
                        {
                            eventTicket.SportEvent.TicketsAvailable = availableTickets;
                            //fixtures.Add(eventTicket.SportEvent);

                            eventTicketsList.Add(eventTicket);
                        }

                    }
                }

                return eventTicketsList.OrderByDescending(e => e.SportEvent.Date);

            }
            catch (Exception ex)
            { }

            return eventTicketsList;
        }

        public async Task<IEnumerable<SportEvent>> Tickets()
        {
            List<SportEvent> sportEvents = new List<SportEvent>();

            try
            {
                var ticketConfigurations = await _context.TicketConfigurations.ToListAsync();
                DateTime currentDate = DateTime.Now.AddHours(-4).Date;

                var sportsEventList = await _context.SportEvents
                    .Include(e => e.Field)
                    .Include(e => e.Sport)
                    .Where(e => e.Date >= currentDate)
                    .ToListAsync();

                var eventTickets = await _context.EventTickets
                    .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                    .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .ToListAsync();

                foreach (var sportEvent in sportsEventList)
                {
                    var eventTicket = eventTickets.FirstOrDefault(e => e.SportEventID == sportEvent.ID);
                    var ticketConfiguration = ticketConfigurations.FirstOrDefault(e => e.TicketCompanyID == eventTicket?.Product?.TicketCompanyID);

                    if (ticketConfiguration != null)
                    {
                        if (DateTime.Now.AddHours(-4).Date >= sportEvent.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                        {
                            //var matchTicketsListw = await _context.CustomerTickets.ToListAsync();

                            var matchTicketsList = await _context.CustomerTickets
                            .Include(e => e.EventTicket)
                            .Where(e => e.EventTicket.SportEventID == eventTicket.SportEventID)
                            .ToListAsync();

                            var ticketCount = matchTicketsList.Count();
                            var availableTickets = (ticketConfiguration.Stock - ticketCount) < 0 ? 0 : (ticketConfiguration.Stock - ticketCount);

                            if (availableTickets > 0)
                            {
                                eventTicket.SportEvent.TicketsAvailable = availableTickets;
                                sportEvents.Add(eventTicket.SportEvent);
                            }
                        }
                    }
                }

                return sportEvents.Distinct().ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return sportEvents;
        }

        public async Task<IEnumerable<EventTicket>> Team(ClaimsPrincipal claimsUser)
        {
            List<EventTicket> eventTicketsList = new List<EventTicket>();

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TicketCompanyID == companyUser.TicketCompanyID);

                var eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                     .Where(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID && e.SportEvent.Date.AddDays(-(ticketConfiguration.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.SportEvent.Date).ToListAsync();



                foreach (var eventTicket in eventTickets)
                {
                    if (DateTime.Now.Date > eventTicket.SportEvent.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                        eventTicketsList.Add(eventTicket);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return eventTicketsList;
        }

        public async Task<SportEvent> GetTodayEvent(ClaimsPrincipal claimsUser)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);
                DateTime currentDate = DateTime.Now.AddHours(-4).Date;

                var eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .FirstOrDefaultAsync(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID && e.SportEvent.Date == currentDate);

                return eventTickets.SportEvent;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<List<SportEvent>> GetUpcomingEvents(ClaimsPrincipal claimsUser)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID)
                    .Select(e => e.SportEvent).ToListAsync();

                return eventTickets;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<EventTicket>> GetTodayByTeam(ClaimsPrincipal claimsUser)
        {
            List<EventTicket> eventTickets = new List<EventTicket>();

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var ticketConfig = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == companyUser.TicketCompanyID);

                eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID && e.SportEvent.Date.AddDays(-(ticketConfig.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.SportEvent.Date).ToListAsync();
                

            }
            catch (Exception ex)
            { }

            return eventTickets;
        }

        public async Task<IEnumerable<EventTicket>> Event(string eventID)
        {
            List<EventTicket> eventTicketList = new List<EventTicket>();

            try
            {
                var eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.SportEventID == eventID && DateTime.Now.Date <= e.SportEvent.Date && e.IsActive).ToListAsync();

                foreach (var eventTicket in eventTickets)
                {
                    var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TicketCompanyID == eventTicket.Product.TicketCompanyID);

                    if (DateTime.Now.Date >= eventTicket.SportEvent.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                        eventTicketList.Add(eventTicket);
                }
            }
            catch (Exception ex)
            { }

            return eventTicketList;
        }

        public async Task<IEnumerable<EventTicket>> EventTicket(string eventID, string email)
        {
            List<EventTicket> eventTicketsList = new List<EventTicket>();

            try
            {
                
                var eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.SportEventID == eventID && DateTime.Now.Date <= e.SportEvent.Date && e.IsActive).ToListAsync();

                var product = eventTickets.FirstOrDefault(e => e.SportEventID == eventID).Product;
                var ticketMember = await _context.TicketMembers.FirstOrDefaultAsync(e => e.Customer.Email == email);

                foreach (var eventTicket in eventTickets)
                {
                    var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TicketCompanyID == eventTicket.Product.TicketCompanyID);

                    if (DateTime.Now.Date >= eventTicket.SportEvent.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                    {
                        if (ticketMember != null)
                        {
                            eventTicket.Product.Price = eventTicket.Product.MemberPrice ?? eventTicket.Product.Price;
                        }

                        eventTicketsList.Add(eventTicket);
                    }
                }
            }
            catch (Exception ex)
            { }

            return eventTicketsList;
        }

        public async Task<bool> Insert(SportEvent item)
        {
            try
            {
                _context.SportEvents.Add(item);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Sport Event");
            }

            return false;
        }

        public async Task<bool> Update(SportEvent item)
        {
            try
            {
                _context.SportEvents.Update(item);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport Event");
            }

            return false;
        }

        Task IOnTrackRepository<SportEvent>.Delete(string id)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<SportEvent>.Insert(SportEvent item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<SportEvent>.Update(SportEvent item)
        {
            throw new NotImplementedException();
        }
    }
}
