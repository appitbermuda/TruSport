using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;

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
                customerTickets = await _context.CustomerTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                    .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                    .ToListAsync();

                customerTickets.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam) ? e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam : e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam) ? e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam : e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam));
            }
            catch(Exception ex)
            { }

            return customerTickets;
        }

        public async Task<IEnumerable<CustomerTicket>> GetCustomerTickets(ClaimsPrincipal user)
        {
            List<CustomerTicket> customerTickets = new List<CustomerTicket>();

            try
            {
                // Get the claims values
                var email = user.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                customerTickets = await _context.CustomerTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                    .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                    .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                    .Where(e => (DateTime.Now.Date <= e.EventTicket.SportEvent.Date.AddDays(1) || e.EventTicket.SportEvent.IsPostponed) && ((e.Order.Customer.Email == email && e.TransferCustomerID == null) || (e.TransferCustomer != null && e.TransferCustomer.Email == email && e.IsTransfer.HasValue && e.IsTransfer.Value)) && !e.Order.IsRefunded)
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
                // Get the claims values
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).ThenInclude(e => e.Role).FirstOrDefaultAsync(e => e.User.Email == email && e.User.Role.Name == role);

                if (companyUser != null && companyUser.User != null && companyUser.User.IsActive)
                {
                    customerTickets = await _context.CustomerTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.Order).ThenInclude(e => e.OrderDetails)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Sport)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                        .Where(e => e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID).ToListAsync();

                    customerTickets.ForEach(e => e.Order.Fixture = e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.HomeTeam + " v " + e.Order.OrderDetails.FirstOrDefault().EventTicket.SportEvent.AwayTeam);
                }
            }
            catch (Exception ex)
            { }

            return customerTickets;
        }

        public async Task<IEnumerable<CustomerTicket>> GetTodayCustomerTickets(ClaimsPrincipal claimsUser)
        {
            List<CustomerTicket> customerTickets = new List<CustomerTicket>();

            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);
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

        public async Task<TicketBilling> SportEventBilling(string fixtureID, ClaimsPrincipal iUser)
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
                    .Where(e => e.EventTicket.SportEvent.ID == fixtureID && e.EventTicket.Product.TicketCompanyID == companyUser.TicketCompanyID && !e.Order.IsRefunded).ToListAsync();

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

        public async Task<bool> Download(string fixtureID, ClaimsPrincipal iUser)
        {
            try
            {
                var email = iUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).FirstOrDefaultAsync(e => e.User.Email == email);

                TicketBilling ticketBilling = await SportEventBilling(fixtureID, iUser);

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
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var user = await _context.TicketCompanyUsers.Include(e => e.User).FirstOrDefaultAsync(e => e.User.Email == email);

                var customerTickets = await _context.CustomerTickets.Include(e => e.EventTicket).ThenInclude(e => e.Product).Where(e => e.EventTicket.Product.TicketCompanyID == user.TicketCompanyID).ToListAsync();
                 //&& customerTickets.Any(d => d.EventTicketID == e.ID)
                ticketReports = await _context.EventTickets.Include(e => e.Product)
                    .Include(e => e.SportEvent).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.SportEvent).ThenInclude(e => e.AwayTeam)
                    .Where(e => e.Product.TicketCompanyID == user.TicketCompanyID && e.SportEvent.Date <= DateTime.Now.Date)
                    .Select(e => new TicketReport
                    {
                        SportEvent = e.SportEvent,
                        FixtureID = e.SportEventID,
                        Fixture = e.SportEvent.HomeTeam + " v " + e.SportEvent.AwayTeam,
                        Date = e.SportEvent.Date.ToString("MMM dd, yyyy")
                    }).ToListAsync();

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
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.Include(e => e.User).ThenInclude(e => e.Role).FirstOrDefaultAsync(e => e.User.Email == email && e.User.Role.Name == role);

                if (companyUser != null && companyUser.User != null && companyUser.User.IsActive)
                {
                    var fixtureProduct = await _context.EventTickets.Include(e => e.Product).FirstOrDefaultAsync(e => e.ID == scannedCustomerTicket.EventTicketID);

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
                            ticketResponse.Ticket = fixtureProduct.Product.Age + " Ticket";
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
            { }

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

        public async Task<PaymentResponse> Purchase(PaymentAuthorize paymentAuthorization)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            _context.Database.BeginTransaction();

            try
            {
                var fixtureProduct = await _context.EventTickets
                    .Include(e => e.Product)
                    .FirstOrDefaultAsync(e => e.SportEventID == paymentAuthorization.SportEventID);

                var customerTicketsList = await _context.CustomerTickets
                    .Include(e => e.EventTicket)
                    .Where(e => e.EventTicket.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var ticketCompanyID = fixtureProduct.Product.TicketCompanyID;

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == ticketCompanyID);

                var ticketCount = customerTicketsList.Count();

                if (ticketCount < ticketConfiguration.Stock && paymentAuthorization.OrderDetails.Sum(e => e.Qty) <= ticketConfiguration.Stock)
                {
                    var processingFee = await _context.Settings.FirstOrDefaultAsync(e => e.ID == Constants.SETTING_PROCESSING_FEE_ID);

                    Decimal ProcessingFeeAmount = Convert.ToDecimal(processingFee.Value);
                    Decimal PaymentAmount = 0.0m;

                    //string payment = String.Format("{0,0:N2}", Decimal.Parse(paymentAuthorization.Amount) / 100.0m);
                    string payment = String.Format("{0,0:N2}", Decimal.Parse(paymentAuthorization.Amount));

                    //string topUp = topUpPayment.Amount;
                    //topUp.Insert(topUp.Length - 2, ".");
                    PaymentAmount = Convert.ToDecimal(payment);
                    var orderSubTotal = paymentAuthorization.OrderDetails.Sum(e => e.Subtotal);

                    if (PaymentAmount != orderSubTotal + (ProcessingFeeAmount * paymentAuthorization.Quantity))
                    {
                        paymentAuthorization.Amount = (orderSubTotal + (ProcessingFeeAmount * paymentAuthorization.Quantity)).ToString();                        
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
                        

                        var updateOrder = await _context.Orders.FirstOrDefaultAsync(e => e.ID == order.ID);
                        updateOrder.Authorisation = response.CreditCardTransactionResults.AuthCode;
                        updateOrder.OrderNumber = response.OrderNumber;

                        _context.Orders.Update(updateOrder);
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
                                        Validated = false
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
                            var sportEvent = await _context.SportEvents
                                .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.SportEventID);

                            var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == paymentAuthorization.CustomerID);
                            await emailRepository.SendPaymentConfirmation(customer, response.CreditCardTransactionResults.AuthCode, order, paymentAuthorization.OrderDetails.Sum(e => e.Qty), sportEvent);

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

                var transferToCustomer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == transferRequest.Email);

                if (transferToCustomer != null)
                {
                    var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == email);

                    var customerTicket = await _context.CustomerTickets
                        .Include(e => e.Order)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.HomeTeam)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.AwayTeam)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                        .FirstOrDefaultAsync(e => e.ID == transferRequest.CustomerTicketID);

                    customerTicket.EventTicket.SportEvent.HomeTeam = customerTicket.EventTicket.SportEvent.HomeTeam;
                    customerTicket.EventTicket.SportEvent.AwayTeam = customerTicket.EventTicket.SportEvent.AwayTeam;

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
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.HomeTeam)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.AwayTeam)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.Field)
                        .Include(e => e.EventTicket).ThenInclude(e => e.Product)
                        .Include(e => e.TransferCustomer)
                        .Where(e => currentDate <= e.EventTicket.SportEvent.Date.AddDays(1) && (e.TransferCustomer.Email == email && ((e.IsTransfer.HasValue && !e.IsTransfer.Value) || (!e.IsTransfer.HasValue))))
                        .ToListAsync();

                    customerTickets.ForEach(e => e.EventTicket.SportEvent.HomeTeam = e.EventTicket.SportEvent.HomeTeam);
                    customerTickets.ForEach(e => e.EventTicket.SportEvent.AwayTeam = e.EventTicket.SportEvent.AwayTeam);

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
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.HomeTeam)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.AwayTeam)
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
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.HomeTeam)
                        .Include(e => e.EventTicket).ThenInclude(e => e.SportEvent).ThenInclude(e => e.AwayTeam)
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

        public async Task<PaymentResponse> PurchaseTest(PaymentAuthorize paymentAuthorization)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            _context.Database.BeginTransaction();

            try
            {
                var fixtureProduct = await _context.EventTickets
                    .Include(e => e.Product)
                    .FirstOrDefaultAsync(e => e.SportEventID == paymentAuthorization.SportEventID);

                var customerTicketsList = await _context.CustomerTickets
                    .Include(e => e.EventTicket)
                    .Where(e => e.EventTicket.SportEventID == paymentAuthorization.SportEventID)
                    .ToListAsync();

                var ticketCompanyID = fixtureProduct.Product.TicketCompanyID;

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TicketCompanyID == ticketCompanyID);

                var ticketCount = customerTicketsList.Count();

                if (ticketCount < ticketConfiguration.Stock && paymentAuthorization.OrderDetails.Sum(e => e.Qty) <= ticketConfiguration.Stock)
                {
                    var processingFee = await _context.Settings.FirstOrDefaultAsync(e => e.ID == Constants.SETTING_PROCESSING_FEE_ID);

                    Decimal ProcessingFeeAmount = Convert.ToDecimal(processingFee.Value);
                    Decimal PaymentAmount = 0.0m;

                    //string payment = String.Format("{0,0:N2}", Decimal.Parse(paymentAuthorization.Amount) / 100.0m);
                    string payment = String.Format("{0,0:N2}", Decimal.Parse(paymentAuthorization.Amount));

                    //string topUp = topUpPayment.Amount;
                    //topUp.Insert(topUp.Length - 2, ".");
                    PaymentAmount = Convert.ToDecimal(payment);
                    var orderSubTotal = paymentAuthorization.OrderDetails.Sum(e => e.Subtotal);

                    if (PaymentAmount != orderSubTotal + (ProcessingFeeAmount * paymentAuthorization.Quantity))
                    {
                        paymentAuthorization.Amount = (orderSubTotal + (ProcessingFeeAmount * paymentAuthorization.Quantity)).ToString();
                    }

                    if(PaymentAmount > Convert.ToDecimal(paymentAuthorization.Amount))
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


                    var updateOrder = await _context.Orders.FirstOrDefaultAsync(e => e.ID == order.ID);
                    updateOrder.Authorisation = "TESTAUTHCODE";
                    updateOrder.OrderNumber = "TESTORDERNUM";

                    _context.Orders.Update(updateOrder);
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
                                    Validated = false
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
                        var fixture = await _context.SportEvents
                            .Include(e => e.HomeTeam)
                            .Include(e => e.AwayTeam)
                            .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.SportEventID);

                        var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == paymentAuthorization.CustomerID);
                        await emailRepository.SendPaymentConfirmation(customer, "TESTAUTHCODE", order, paymentAuthorization.OrderDetails.Sum(e => e.Qty), fixture);

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
                Debug.WriteLine(ex.Message, "Payment");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";

            return paymentResponse;
        }

        public async Task<string> ResendPurchaseConfirmation(string orderNumber)
        {
            try
            {
                try
                {
                    var order = await _context.Orders.Include(e => e.Customer).FirstOrDefaultAsync(e => e.OrderNumber == orderNumber);

                    var orderDetails = await _context.OrderDetails.Include(e => e.EventTicket).Where(e => e.OrderID == order.ID).ToListAsync();

                    var fixture = await _context.SportEvents
                        .Include(e => e.HomeTeam)
                        .Include(e => e.AwayTeam)
                        .FirstOrDefaultAsync(e => e.ID == orderDetails.FirstOrDefault().EventTicket.SportEventID);

                    var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == order.Customer.ID);

                    await emailRepository.SendPaymentConfirmation(customer, order.Authorisation, order, orderDetails.Sum(e => e.Qty), fixture);

                    return "Confirmation sent successfully";
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
    }
}
