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
    public class OrderRepository : IOnTrackRepository<Order>
    {
        OnTrackContext _context;
        EmailRepository emailRepository;

        public OrderRepository(OnTrackContext context)
        {
            _context = context;
            emailRepository = new EmailRepository(context);
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public async Task<Order> Get(string id)
        {
            try
            {
                var order = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .FirstOrDefaultAsync(e => e.ID == id);

                order.Fixture = (!String.IsNullOrEmpty(order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name);

                return order;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get Order");
            }

            return null;
        }

        public async Task<Order> GetOrder(string id)
        {
            try
            {
                var order = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .FirstOrDefaultAsync(e => e.ID == id);

                order.Fixture = order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + order.OrderDetails.FirstOrDefault().EventTicket.SportEvent;

                return order;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get Order");
            }

            return null;
        }

        [Obsolete]
        public async Task<IEnumerable<CustomerOrder>> GetMatchDayOrders(string email)
        {
            List<CustomerOrder> matchDayOrders = new List<CustomerOrder>();
            try
            {


                var orders = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Where(e => e.Customer.Email == email)
                    .ToListAsync();

                foreach (var order in orders)
                {
                    foreach(var orderDetail in order.OrderDetails)
                    {
                        if (orderDetail.FixtureProduct.Fixture.Date >= DateTime.Now.Date.AddDays(-1))
                        {
                            var fixture = orderDetail.FixtureProduct.Fixture;

                            for (var i = 0; i < orderDetail.Qty; i++)
                            {
                                matchDayOrders.Add(new CustomerOrder
                                {
                                    OrderID = order.ID,
                                    FixtureID = fixture.ID,
                                    CustomerID = order.CustomerID,
                                    OrderNumber = order.OrderNumber,
                                    Product = orderDetail.FixtureProduct.Product.Age + " Ticket",
                                    FirstName = order.Customer.FirstName,
                                    LastName = order.Customer.LastName,
                                    Email = order.Customer.Email,
                                    Phone = order.Customer.Phone,
                                    FixtureDate = fixture.Date,
                                    Time = fixture.Time,
                                    FieldName = fixture.Field.Name,
                                    HomeTeamName = !String.IsNullOrEmpty(fixture.HomeTeam.Alias) ? fixture.HomeTeam.Alias : fixture.HomeTeam.Name,
                                    AwayTeamName = !String.IsNullOrEmpty(fixture.HomeTeam.Alias) ? fixture.HomeTeam.Alias : fixture.AwayTeam.Name,
                                    HomeTeamLogo = fixture.HomeTeam.TeamLogo,
                                    AwayTeamLogo = fixture.AwayTeam.TeamLogo,
                                    //Validated = order.Validated
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order Team");
            }

            return matchDayOrders;
        }

        public async Task<IEnumerable<CustomerOrder>> GetEventDayOrders(string email)
        {
            List<CustomerOrder> matchDayOrders = new List<CustomerOrder>();
            try
            {
                var orders = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Where(e => e.Customer.Email == email)
                    .ToListAsync();

                foreach (var order in orders)
                {
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        if (orderDetail.EventTicket.SportEvent.Date >= DateTime.Now.Date.AddDays(-1))
                        {
                            var sportEvent = orderDetail.EventTicket.SportEvent;

                            for (var i = 0; i < orderDetail.Qty; i++)
                            {
                                matchDayOrders.Add(new CustomerOrder
                                {
                                    OrderID = order.ID,
                                    FixtureID = sportEvent?.ID,
                                    CustomerID = order.CustomerID,
                                    OrderNumber = order.OrderNumber,
                                    Product = orderDetail.EventTicket.Product.Age + " Ticket",
                                    FirstName = order.Customer.FirstName,
                                    LastName = order.Customer.LastName,
                                    Email = order.Customer.Email,
                                    Phone = order.Customer.Phone,
                                    FixtureDate = sportEvent.Date,
                                    Time = sportEvent.Time,
                                    FieldName = sportEvent.Field.Name,
                                    HomeTeamName = sportEvent.HomeTeam,
                                    AwayTeamName = sportEvent.AwayTeam,
                                    HomeTeamLogo = sportEvent.HomeTeamLogo,
                                    AwayTeamLogo = sportEvent.AwayTeamLogo,
                                    //Validated = order.Validated
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order Team");
            }

            return matchDayOrders;
        }

        [Obsolete]
        public async Task<IEnumerable<Order>> TodayByTeam(ClaimsPrincipal claimsUser)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                      .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var orders = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Where(e => e.OrderDetails.Any(e => e.FixtureProduct.Product.TicketCompanyID == companyUser.TicketCompanyID && e.FixtureProduct.Fixture.Date == DateTime.Now.Date))
                    .ToListAsync();

                orders.ForEach(e => e.Fixture = (!String.IsNullOrEmpty(e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));

                return orders;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order Team");
            }

            return null;
        }


        public async Task<IEnumerable<Order>> CompanyOrdersByToday(ClaimsPrincipal claimsUser)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                      .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var orders = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Where(e => e.OrderDetails.Any(e => e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID && e.EventTicket.SportEvent.Date == DateTime.Now.Date))
                    .ToListAsync();

                orders.ForEach(e => e.Fixture = e.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);

                return orders;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order Team");
            }

            return null;
        }

        [Obsolete]
        public async Task<IEnumerable<Order>> Team(ClaimsPrincipal claimsUser)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                         .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);


#if DEBUG
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic/Bermuda");
#else
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");
#endif
                var orders = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Where(e => e.OrderDetails.Any(e => e.FixtureProduct.Product.TicketCompanyID == companyUser.TicketCompanyID))
                    .ToListAsync();

                orders.ForEach(e => e.Fixture = (!String.IsNullOrEmpty(e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));

                try
                {
                    orders.ForEach(e => e.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(e.Date, DateTimeKind.Unspecified), timeInfo));
                }
                catch (Exception ex)
                {

                }

                return orders;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order Team");
            }

            return null;
        }

        public async Task<IEnumerable<Order>> TeamOrders(ClaimsPrincipal claimsUser)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                         .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);


#if DEBUG
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic/Bermuda");
#else
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");
#endif
                var orders = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Where(e => e.OrderDetails.Any(e => e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID))
                    .ToListAsync();

                orders.ForEach(e => e.Fixture = e.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);

                try
                {
                    orders.ForEach(e => e.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(e.Date, DateTimeKind.Unspecified), timeInfo));
                }
                catch (Exception ex)
                {

                }

                return orders;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order Team");
            }

            return null;
        }

        [Obsolete]
        public async Task<IEnumerable<Order>> GetOrderHistory(string email)
        {
            List<Order> ordersList = new List<Order>();

            try
            {
#if DEBUG
            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic/Bermuda");
#else
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");
#endif
                var orders = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Where(e => e.Customer.Email == email)
                    .ToListAsync();

                orders.ForEach(e => e.Fixture = (!String.IsNullOrEmpty(e.OrderDetails.FirstOrDefault()?.FixtureProduct.Fixture.HomeTeam.Alias) ? e.OrderDetails.FirstOrDefault()?.FixtureProduct.Fixture.HomeTeam.Alias : e.OrderDetails.FirstOrDefault()?.FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.OrderDetails.FirstOrDefault()?.FixtureProduct.Fixture.AwayTeam.Alias) ? e.OrderDetails.FirstOrDefault()?.FixtureProduct.Fixture.AwayTeam.Alias : e.OrderDetails.FirstOrDefault()?.FixtureProduct.Fixture.AwayTeam.Name));

                try
                {
                    orders.ForEach(e => e.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(e.Date, DateTimeKind.Unspecified), timeInfo));
                }
                catch (Exception ex)
                {

                }

                //DateTime orderTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(order.Date, DateTimeKind.Unspecified), timeInfo);

                //var orders = await _context.Orders
                //    .Include(e => e.Customer)
                //    .Where(e => e.Customer.Email == email && e.Date.Date >= DateTime.Now.Date.AddDays(-1))
                //    .ToListAsync();

                //var orderDetails = await _context.OrderDetails
                //    .Include(e => e.Order).ThenInclude(e => e.Customer)
                //    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                //    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                //    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                //    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                //    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product)
                //    .Where(e => e.Order.Customer.Email == email && e.Order.Date.Date >= DateTime.Now.Date.AddDays(-1)).ToListAsync();

                //orders.ForEach(e => e.OrderDetails = orderDetails.Where(o => o.OrderID == e.ID).ToList());
                //foreach(var orderDetail in orderDetails)
                //{
                //    orderDetail.Order.OrderDetail = orderDetail;

                //    ordersList.Add(orderDetail.Order);
                //}

                return orders;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order History");
            }

            return ordersList;
        }

        public async Task<IEnumerable<Order>> GetOrdersHistory(string email)
        {
            List<Order> ordersList = new List<Order>();

            try
            {
#if DEBUG
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic/Bermuda");
#else
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");
#endif
                var orders = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .Where(e => e.Customer.Email == email)
                    .ToListAsync();

                orders.ForEach(e => e.Fixture = e.OrderDetails.FirstOrDefault()?.EventTicket.SportEvent.HomeTeam + " v " + e.OrderDetails.FirstOrDefault()?.EventTicket.SportEvent.AwayTeam);

                try
                {
                    orders.ForEach(e => e.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(e.Date, DateTimeKind.Unspecified), timeInfo));
                }
                catch (Exception ex)
                {

                }

                //DateTime orderTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(order.Date, DateTimeKind.Unspecified), timeInfo);

                //var orders = await _context.Orders
                //    .Include(e => e.Customer)
                //    .Where(e => e.Customer.Email == email && e.Date.Date >= DateTime.Now.Date.AddDays(-1))
                //    .ToListAsync();

                //var orderDetails = await _context.OrderDetails
                //    .Include(e => e.Order).ThenInclude(e => e.Customer)
                //    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                //    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                //    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                //    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                //    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product)
                //    .Where(e => e.Order.Customer.Email == email && e.Order.Date.Date >= DateTime.Now.Date.AddDays(-1)).ToListAsync();

                //orders.ForEach(e => e.OrderDetails = orderDetails.Where(o => o.OrderID == e.ID).ToList());
                //foreach(var orderDetail in orderDetails)
                //{
                //    orderDetail.Order.OrderDetail = orderDetail;

                //    ordersList.Add(orderDetail.Order);
                //}

                return orders;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order History");
            }

            return ordersList;
        }

        [Obsolete]
        public async Task<IEnumerable<Order>> GetAll()
        {
            try
            {
#if DEBUG
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic/Bermuda");
#else
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");
#endif
                var orders = await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .ToListAsync();

                orders.ForEach(e => e.Fixture = (!String.IsNullOrEmpty(e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));

                try
                {
                    orders.ForEach(e => e.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(e.Date, DateTimeKind.Unspecified), timeInfo));
                }
                catch (Exception ex)
                {

                }

                return orders;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get All");
            }

            return null;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            try
            {
#if DEBUG
            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic/Bermuda");
#else
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time");
#endif
                var orders =  await _context.Orders.Include(e => e.Customer)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .Include(e => e.OrderDetails).ThenInclude(e => e.EventTicket).ThenInclude(e => e.SportEvent)
                    .ToListAsync();

                orders.ForEach(e => e.Fixture = e.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);

                try
                {
                    orders.ForEach(e => e.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(e.Date, DateTimeKind.Unspecified), timeInfo));
                }
                catch (Exception ex)
                {

                }

                return orders;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get All");
            }

            return null;
        }

        public async Task NewOrder(NewOrder item)
        {
            if (!String.IsNullOrEmpty(item.FirstName) && !String.IsNullOrEmpty(item.LastName) && (!String.IsNullOrEmpty(item.Email) || !String.IsNullOrEmpty(item.Phone)) && item.OrderDetails != null)
            {
                try
                {
                    _context.Database.BeginTransaction();

                    Customer customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == item.Email);

                    if (customer == null)
                    {
                        customer = new Customer();
                        customer.FirstName = item.FirstName;
                        customer.LastName = item.LastName;
                        customer.Email = item.Email;
                        customer.Phone = item.Phone;

                        _context.Customers.Add(customer);
                        await _context.SaveChangesAsync();
                    }

                    Order order = new Order();
                    order.OrderNumber = DateTime.Now.ToString("yyMMdd-HHmmss");
                    order.CustomerID = customer.ID;
                    order.Date = DateTime.Now;
                    order.Total = item.Total;

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();


                    foreach (var orderItems in item.OrderDetails)
                    {
                        var eventTicket = await _context.EventTickets.Include(e => e.Product).FirstOrDefaultAsync(e => e.ID == orderItems.EventTicketID);

                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderID = order.ID;
                        orderDetail.EventTicketID = orderItems.EventTicketID;
                        orderDetail.Qty = orderItems.Qty;
                        orderDetail.Subtotal = eventTicket.Product.Price * orderItems.Qty;

                        _context.OrderDetails.Add(orderDetail);
                        await _context.SaveChangesAsync();
                    }

                    var orderDetails = _context.OrderDetails.Where(e => e.OrderID == order.ID);

                    var totalQty = orderDetails.Sum(e => e.Qty);

                    if (totalQty >= 3)
                    {
                        order.Discount = Math.Abs(totalQty / 3) * 8.00m;
                    }

                    order.Total = orderDetails.Sum(e => e.Subtotal) - order.Discount;

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                    //send email
                    //await emailRepository.SendOrderConfirmation(customer.FirstName + " " + customer.LastName, customer.Email, order);

                    _context.Database.CommitTransaction();
                }
                catch (Exception ex)
                {
                    _context.Database.RollbackTransaction();

                }
            }
        }

        public async Task Insert(NewOrder item)
        {
            if (!String.IsNullOrEmpty(item.FirstName) && !String.IsNullOrEmpty(item.LastName) && (!String.IsNullOrEmpty(item.Email) || !String.IsNullOrEmpty(item.Phone)) && item.OrderDetails != null)
            {
                try
                {
                    _context.Database.BeginTransaction();

                    Customer customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == item.Email);

                    if (customer == null)
                    {
                        customer = new Customer();
                        customer.FirstName = item.FirstName;
                        customer.LastName = item.LastName;
                        customer.Email = item.Email;
                        customer.Phone = item.Phone;

                        _context.Customers.Add(customer);
                        await _context.SaveChangesAsync();
                    }

                    Order order = new Order();
                    order.OrderNumber = DateTime.Now.ToString("yyMMdd-HHmmss");
                    order.CustomerID = customer.ID;
                    order.Date = DateTime.Now;
                    order.Total = item.Total;

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();


                    foreach (var orderItems in item.OrderDetails)
                    {
                        var fixtureProduct = await _context.FixtureProducts.Include(e => e.Product).FirstOrDefaultAsync(e => e.ID == orderItems.FixtureProductID);

                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderID = order.ID;
                        orderDetail.FixtureProductID = orderItems.FixtureProductID;
                        orderDetail.Qty = orderItems.Qty;
                        orderDetail.Subtotal = fixtureProduct.Product.Price * orderItems.Qty;

                        _context.OrderDetails.Add(orderDetail);
                        await _context.SaveChangesAsync();
                    }

                    var orderDetails = _context.OrderDetails.Where(e => e.OrderID == order.ID);

                    var totalQty = orderDetails.Sum(e => e.Qty);

                    if(totalQty >= 3)
                    {
                        order.Discount = Math.Abs(totalQty / 3) * 8.00m;
                    }

                    order.Total = orderDetails.Sum(e => e.Subtotal) - order.Discount;

                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                    //send email
                    //await emailRepository.SendOrderConfirmation(customer.FirstName + " " + customer.LastName, customer.Email, order);

                    _context.Database.CommitTransaction();
                }
                catch (Exception ex)
                {
                    _context.Database.RollbackTransaction();

                }
            }
        }

        public Task Insert(Order item)
        {
            throw new NotImplementedException();
        }

        //public async Task<bool> ValidateCustomer(string orderID)
        //{
        //    try
        //    {
        //        var order = await _context.Orders.FirstOrDefaultAsync(e => e.ID == orderID);

        //        if (!order.Validated)
        //        {
        //            order.Validated = true;

        //            _context.Orders.Update(order);
        //            await _context.SaveChangesAsync();

        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    { }

        //    return false;
        //}

        public async Task Update(Order item)
        {
            try
            {
                _context.Orders.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { }
        }
    }
}
