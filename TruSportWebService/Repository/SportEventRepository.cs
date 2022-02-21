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
using OnTrackWebService.Models.Shop;

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
                    .Where(e => currentDate <= e.SportEvent.Date && !e.SportEvent.IsPostponed && !e.SportEvent.IsCancelled)
                    .ToListAsync();

                foreach (var eventTicket in eventTickets)
                {
                    //ticketConfig.Any(t => t.TeamID == e.Product.TeamID && DateTime.Now.Date > e.Fixture.Date.AddDays(-(t.ValidFrom)))

                    var settings = await _context.Settings.Where(e => e.Key.Contains("Term")).ToListAsync();
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
                            eventTicket.SportEvent.DefaultContactTraceTerm = settings.FirstOrDefault(e => e.Key == Constants.SETTING_DEFAULT_CONTACT_TRACING_TERM).Value;
                            eventTicket.SportEvent.DefaultTicketTerm = settings.FirstOrDefault(e => e.Key == Constants.SETTING_DEFAULT_TICKET_TERM).Value;
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
                    .Where(e => e.Date >= currentDate && !e.IsPostponed && !e.IsCancelled)
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
                    var settings = await _context.Settings.Where(e => e.Key.Contains("Term")).ToListAsync();

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
                                eventTicket.SportEvent.DefaultContactTraceTerm = settings.FirstOrDefault(e => e.Key == Constants.SETTING_DEFAULT_CONTACT_TRACING_TERM).Value;
                                eventTicket.SportEvent.DefaultTicketTerm = settings.FirstOrDefault(e => e.Key == Constants.SETTING_DEFAULT_TICKET_TERM).Value;
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
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.UserName == username);

                var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TicketCompanyID == companyUser.TicketCompanyID);

                var eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                     .Where(e => e.SportEvent.TicketCompanyID == companyUser.TicketCompanyID && e.SportEvent.Date.AddDays(-(ticketConfiguration.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.SportEvent.Date).ToListAsync();



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
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.UserName == username);
                DateTime currentDate = DateTime.Now.AddHours(-4).Date;

                //var eventTickets = await _context.EventTickets
                //     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                //     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                //     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                //     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                //     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                //    .FirstOrDefaultAsync(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID && e.SportEvent.Date == currentDate);

                var sportEvent = await _context.SportEvents
                     .Include(e => e.Field)
                     .Include(e => e.Sport)
                     .Include(e => e.TicketCompany)
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == companyUser.TicketCompanyID && e.Date == currentDate);

                return sportEvent;
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
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.UserName == username);

                //var eventTickets = await _context.EventTickets
                //     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                //     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                //     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                //     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                //     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                //    .Where(e => e.Product.TicketCompanyID == companyUser.TicketCompanyID && e.SportEvent.Date.Date >= DateTime.Now.AddHours(-4).Date)
                //    .Select(e => e.SportEvent).Distinct().OrderBy(e => e.Date).ToListAsync();


                var sportEvents = await _context.SportEvents
                     .Include(e => e.Field)
                     .Include(e => e.Sport)
                     .Include(e => e.TicketCompany)
                    .Where(e => e.TicketCompanyID == companyUser.TicketCompanyID && e.Date.Date >= DateTime.Now.AddHours(-4).Date)
                    .Distinct().OrderBy(e => e.Date).ToListAsync();


                return sportEvents;
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
                var username = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.UserName == username);

                var ticketConfig = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == companyUser.TicketCompanyID);

                eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.SportEvent.TicketCompanyID == companyUser.TicketCompanyID && e.SportEvent.Date.AddDays(-(ticketConfig.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.SportEvent.Date).ToListAsync();
                

            }
            catch (Exception ex)
            { }

            return eventTickets;
        }

        public async Task<IEnumerable<EventTicket>> Event(string eventID)
        {
            List<EventTicket> eventTicketList = new List<EventTicket>();
            DateTime currentDate = DateTime.Now.AddHours(-4).Date;

            try
            {
                var eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.SportEventID == eventID && currentDate <= e.SportEvent.Date && e.IsActive && !e.SportEvent.IsPostponed && !e.SportEvent.IsCancelled).ToListAsync();

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

        [Obsolete]
        public async Task<IEnumerable<EventTicket>> CustomerEventTicket(string eventID, string email)
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
                var ticketMember = await _context.TicketMembers.FirstOrDefaultAsync(e => e.Customer.Email == email && e.TicketCompanyID == product.TicketCompanyID);

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

        
        public async Task<IEnumerable<EventTicket>> CustomerEventTickets(string eventID, string email)
        {
            List<EventTicket> eventTicketsList = new List<EventTicket>();
            List<CustomerTicket> memberTickets = new List<CustomerTicket>();
            DateTime currentDate = DateTime.Now.AddHours(-4).Date;

            try
            {

                var eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.SportEventID == eventID && currentDate <= e.SportEvent.Date && e.IsActive).ToListAsync();

                var sportEvent = eventTickets.FirstOrDefault().SportEvent;
                var ticketMember = await _context.TicketMembers.FirstOrDefaultAsync(e => e.Customer.Email == email && e.TicketCompanyID == sportEvent.TicketCompanyID);

                if (ticketMember != null)
                {
                    //Maybe need to see if ticket transfer
                    memberTickets = _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.EventTicket)
                        .AsEnumerable()
                        .Where(e => eventTickets.Any(d => d.ID == e.EventTicketID) && e.Order.CustomerID == ticketMember?.CustomerID && e.IsMemberTicket).ToList();
                }

                foreach (var eventTicket in eventTickets)
                {
                    var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TicketCompanyID == eventTicket.Product.TicketCompanyID);

                    if (DateTime.Now.Date >= eventTicket.SportEvent.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                    {
                        eventTicketsList.Add(eventTicket);

                        if (ticketMember != null && memberTickets.Count < ticketMember.NoOfTickets)
                        {
                            EventTicket memberTicket = new EventTicket
                            {
                                ID = eventTicket.ID,
                                ProductID = eventTicket.ProductID,
                                SportEvent = eventTicket.SportEvent,
                                SportEventID = eventTicket.SportEventID,
                                IsActive = true
                            };

                            memberTicket.Product = new Product
                            {
                                ID = eventTicket.Product.ID,
                                Age = eventTicket.Product.Age + " (Member)",
                                Price = eventTicket.Product.MemberPrice ?? eventTicket.Product.Price,
                                MemberPrice = eventTicket.Product.MemberPrice,
                                ProductTypeID = eventTicket.Product.ProductTypeID,
                                ProductType = eventTicket.Product.ProductType,
                                Name = eventTicket.Product.Name,
                                TicketCompany = eventTicket.Product.TicketCompany,
                                TicketCompanyID = eventTicket.Product.TicketCompanyID,
                                Fee = eventTicket.Product.Fee,
                                Image = eventTicket.Product.Image,
                                MemberTicketCount = ticketMember.NoOfTickets
                            };

                            eventTicketsList.Add(memberTicket);
                        }

                    }
                }
            }
            catch (Exception ex)
            { }

            return eventTicketsList;
        }

        public async Task<IEnumerable<EventTicket>> EventTicket(string eventID, bool isMember = false)
        {
            List<EventTicket> eventTicketsList = new List<EventTicket>();
            DateTime currentDate = DateTime.Now.AddHours(-4).Date;

            try
            {

                var eventTickets = await _context.EventTickets
                     .Include(e => e.SportEvent).ThenInclude(e => e.Field)
                     .Include(e => e.SportEvent).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                     .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                     .Include(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.SportEventID == eventID && currentDate <= e.SportEvent.Date && e.IsActive).ToListAsync();

                var product = eventTickets.FirstOrDefault(e => e.SportEventID == eventID).Product;
                //var ticketMember = await _context.TicketMembers.FirstOrDefaultAsync(e => e.Customer.Email == email);

                foreach (var eventTicket in eventTickets)
                {
                    var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TicketCompanyID == eventTicket.Product.TicketCompanyID);

                    if (DateTime.Now.Date >= eventTicket.SportEvent.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                    {
                        if (isMember)
                        {
                            eventTicket.Product.Price = eventTicket.Product.MemberPrice ?? eventTicket.Product.Price;

                            //eventTicket.MemberProduct = eventTicket.Product;
                            //eventTicket.MemberProduct.Price = eventTicket.Product.MemberPrice ?? eventTicket.Product.Price;
                            //eventTicket.MemberProduct.MemberTicketCount = 

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

        public async Task<string> CreateSportEvent(RegisterSportEvent item)
        {
            try
            {
                _context.Database.BeginTransaction();

                if (item != null && !String.IsNullOrEmpty(item.TicketCompanyID) && !String.IsNullOrEmpty(item.HomeTeamID))
                {
                    TicketCompany ticketCompany = await _context.TicketCompanys
                        .Include(e => e.Sport)
                        .FirstOrDefaultAsync(e => e.ID == item.TicketCompanyID);

                    if (ticketCompany.Sport.Name == Constants.Football)
                    {
                        List<Fixture> fixtures = await _context.Fixtures
                            .Include(e => e.League)
                            .Include(e => e.MatchType)
                            .Include(e => e.AwayTeam)
                            .Include(e => e.HomeTeam)
                            .Include(e => e.Season)
                            .Where(e => e.Season.IsCurrent && e.HomeTeamID == item.HomeTeamID)
                            .ToListAsync();

                        List<SportEvent> sportEvents = new List<SportEvent>();
                        foreach (var fixture in fixtures)
                        {
                            sportEvents.Add(new SportEvent
                            {
                                SportID = fixture.SportID,
                                FieldID = fixture.FieldID,
                                Title = fixture.League.Name,
                                Description = fixture.MatchType.Name,
                                HomeTeam = fixture.HomeTeam.Name,
                                AwayTeam = fixture.AwayTeam.Name,
                                Date = fixture.Date,
                                Time = fixture.Time,
                                Season = fixture.Season.Date,
                                HomeTeamLogo = fixture.HomeTeam.TeamLogo,
                                AwayTeamLogo = fixture.AwayTeam.TeamLogo,
                                TicketCompanyID = ticketCompany.ID
                            });
                        }

                        //_context.SportEvents.Add(item);

                        await _context.SportEvents.AddRangeAsync(sportEvents);
                        await _context.SaveChangesAsync();

                        List<Product> products = await _context.Products.Where(e => e.TicketCompanyID == ticketCompany.ID).ToListAsync();

                        List<EventTicket> eventTickets = new List<EventTicket>();
                        foreach(var sportEvent in sportEvents)
                        {
                            foreach(var product in products)
                            {
                                eventTickets.Add(new EventTicket
                                {
                                    SportEventID = sportEvent.ID,
                                    ProductID = product.ID,
                                    IsActive = true
                                });
                            }
                        }


                        await _context.EventTickets.AddRangeAsync(eventTickets);
                        await _context.SaveChangesAsync();
                    }


                    _context.Database.CommitTransaction();

                    return "Success";
                }
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                Debug.WriteLine(ex.Message, "Create Sport Event");
            }

            return "Failed";
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
