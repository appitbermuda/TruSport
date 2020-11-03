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
    public class MatchTicketRepository : IOnTrackRepository<MatchTicket>
    {
        OnTrackContext _context;
        EmailRepository emailRepository;

        public MatchTicketRepository(OnTrackContext context)
        {
            _context = context;
            emailRepository = new EmailRepository(context);
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<MatchTicket> Get(string ID)
        {
            MatchTicket matchTicket = new MatchTicket();

            try
            {
                matchTicket = await _context.MatchTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                    .FirstOrDefaultAsync(e => e.ID == ID);

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
                matchTickets = await _context.MatchTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                    .ToListAsync();

                matchTickets.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));
            }
            catch(Exception ex)
            { }

            return matchTickets;
        }

        public async Task<IEnumerable<MatchTicket>> GetCustomerTickets(ClaimsPrincipal user)
        {
            List<MatchTicket> matchTickets = new List<MatchTicket>();

            try
            {
                // Get the claims values
                var email = user.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                matchTickets = await _context.MatchTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .Where(e => DateTime.Now.Date <= e.FixtureProduct.Fixture.Date.AddDays(1) && ((e.Order.Customer.Email == email && e.TransferCustomerID == null) || (e.TransferCustomer != null && e.TransferCustomer.Email == email && e.IsTransfer)))
                    .ToListAsync();

                matchTickets.ForEach(e => e.FixtureProduct.Fixture.HomeTeam.Name = !String.IsNullOrEmpty(e.FixtureProduct.Fixture.HomeTeam.Alias) ? e.FixtureProduct.Fixture.HomeTeam.Alias : e.FixtureProduct.Fixture.HomeTeam.Name);
                matchTickets.ForEach(e => e.FixtureProduct.Fixture.AwayTeam.Name = !String.IsNullOrEmpty(e.FixtureProduct.Fixture.AwayTeam.Alias) ? e.FixtureProduct.Fixture.AwayTeam.Alias : e.FixtureProduct.Fixture.AwayTeam.Name);

                matchTickets.ForEach(e => e.CustomerMatchTicket = new CustomerMatchTicket
                {
                    FixtureProductID = e.FixtureProductID,
                    ID = e.ID,
                    OrderID = e.OrderID
                });
            }
            catch (Exception ex)
            { }

            return matchTickets;
        }

        public async Task<IEnumerable<MatchTicket>> Team(string teamID)
        {
            List<MatchTicket> matchTickets = new List<MatchTicket>();

            try
            {
                matchTickets = await _context.MatchTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                    .Where(e => e.FixtureProduct.Product.TeamID == teamID).ToListAsync();

                matchTickets.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));
                
            }
            catch (Exception ex)
            { }

            return matchTickets;
        }

        public async Task<IEnumerable<MatchTicket>> GetTodayMatchTickets(string teamID)
        {
            List<MatchTicket> matchTickets = new List<MatchTicket>();

            try
            {
                matchTickets = await _context.MatchTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.Order).ThenInclude(e => e.OrderDetails)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                    .Where(e => e.FixtureProduct.Product.TeamID == teamID && e.FixtureProduct.Fixture.Date == DateTime.Now.AddHours(-4).Date).ToListAsync();

                matchTickets.ForEach(e => e.Order.Fixture = (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.HomeTeam.Name) + " v " + (!String.IsNullOrEmpty(e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias) ? e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Alias : e.Order.OrderDetails.FirstOrDefault().FixtureProduct.Fixture.AwayTeam.Name));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetTodayMatchTickets");
            }

            return matchTickets;
        }

        public async Task<bool> ValidateCustomer(string matchTicketID)
        {
            try
            {
                var matchTicket = await _context.MatchTickets.FirstOrDefaultAsync(e => e.ID == matchTicketID);

                if (!matchTicket.Validated)
                {
                    matchTicket.Validated = true;
                    matchTicket.ValidatedTime = DateTime.Now.ToUniversalTime();

                    _context.MatchTickets.Update(matchTicket);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            { }

            return false;
        }

        public async Task<IEnumerable<MatchTicket>> Fixture(string fixtureID)
        {
            List<MatchTicket> matchTickets = new List<MatchTicket>();

            try
            {
                var fixtureProducts = await _context.FixtureProducts
                       .Include(e => e.Fixture)
                       .Include(e => e.Product).ToListAsync();

                var matchTicketsList = await _context.MatchTickets
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.League)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.MatchType)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Season)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Sport)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.ProductType)
                    .Where(e => e.FixtureProduct.FixtureID == fixtureID && DateTime.Now.Date <= e.FixtureProduct.Fixture.Date).ToListAsync();

                foreach (var matchTicket in matchTicketsList)
                {
                    var fixtureProduct = fixtureProducts.FirstOrDefault(e => e.ID == matchTicket.FixtureProductID);
                    var ticketConfiguration = await _context.TicketConfigurations.FirstOrDefaultAsync(e => e.TeamID == fixtureProduct.Product.TeamID);

                    if (DateTime.Now.Date > matchTicket.FixtureProduct.Fixture.Date.AddDays(-(ticketConfiguration.ValidFrom)))
                        matchTickets.Add(matchTicket);
                }
            }
            catch (Exception ex)
            { }

            return matchTickets;
        }

        public async Task<TicketResponse> Scan(MatchTicket scannedMatchTicket, ClaimsPrincipal claimsUser)
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

                var user = await _context.Users.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == email && e.Role.Name == role);

                var fixtureProduct = await _context.FixtureProducts.Include(e => e.Product).FirstOrDefaultAsync(e => e.ID == scannedMatchTicket.FixtureProductID);

                var matchTicket = await _context.MatchTickets
                    .Include(e => e.Order)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .FirstOrDefaultAsync(e => e.ID == scannedMatchTicket.ID && e.FixtureProduct.Product.TeamID == user.TeamID);

                if (matchTicket != null)
                {
                    if (!matchTicket.Validated)
                    {
                        matchTicket.Validated = true;
                        matchTicket.ValidatedTime = DateTime.Now.ToUniversalTime();

                        _context.MatchTickets.Update(matchTicket);
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
            catch (Exception ex)
            { }

            ticketResponse.Response = "Error Validating Ticket!";
            ticketResponse.IsValidated = false;

            return ticketResponse;
        }

        //public async Task<MatchTicket> GetTodayByTeam(string teamID)
        //{
        //    MatchTicket matchTicket = new MatchTicket();

        //    try
        //    {
        //        var fixtureProduct = await _context.FixtureProducts
        //               .Include(e => e.Fixture)
        //               .Include(e => e.Product)
        //               .FirstOrDefaultAsync(e => e.Product.TeamID == teamID);

        //        var ticketConfig = await _context.TicketConfigurations
        //            .FirstOrDefaultAsync(e => e.TeamID == teamID);

        //        matchTicket = await _context.MatchTickets
        //            .Include(e => e.Order).ThenInclude(e => e.Customer)
        //            .Include(e => e.Fixture).ThenInclude(e => e.HomeTeam)
        //            .Include(e => e.Fixture).ThenInclude(e => e.AwayTeam)
        //            .Include(e => e.Fixture).ThenInclude(e => e.Field)
        //            .Include(e => e.Fixture).ThenInclude(e => e.League)
        //            .Include(e => e.Fixture).ThenInclude(e => e.MatchType)
        //            .Include(e => e.Fixture).ThenInclude(e => e.Season)
        //            .Include(e => e.Fixture).ThenInclude(e => e.Sport)
        //            .FirstOrDefaultAsync(e => e.FixtureID == fixtureProduct.FixtureID && e.Fixture.Date.AddDays(-(ticketConfig.ValidFrom)) < DateTime.Now.Date && DateTime.Now.Date <= e.Fixture.Date);
        //        //.FirstOrDefaultAsync(e => e.Product.TeamID == teamID && e.ValidFrom.Value < DateTime.Now.Date && DateTime.Now.Date <= e.Fixture.Date);

        //    }
        //    catch (Exception ex)
        //    { }

        //    return matchTicket;
        //}

        public async Task<PaymentResponse> Purchase(PaymentAuthorize paymentAuthorization)
        {
            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            _context.Database.BeginTransaction();

            try
            {
                var fixtureProduct = await _context.FixtureProducts
                    .Include(e => e.Product)
                    .FirstOrDefaultAsync(e => e.FixtureID == paymentAuthorization.FixtureID);

                var matchTicketsList = await _context.MatchTickets
                    .Include(e => e.FixtureProduct)
                    .Where(e => e.FixtureProduct.FixtureID == paymentAuthorization.FixtureID)
                    .ToListAsync();

                var teamID = fixtureProduct.Product.TeamID;

                var ticketConfiguration = await _context.TicketConfigurations
                    .FirstOrDefaultAsync(e => e.TeamID == teamID);

                var ticketCount = matchTicketsList.Count();

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

                            List<MatchTicket> matchTickets = new List<MatchTicket>();
                            foreach(var orderDetail in paymentAuthorization.OrderDetails)
                            {
                                for(var i = 0; i < orderDetail.Qty; i++)
                                {
                                    matchTickets.Add(new MatchTicket
                                    {
                                        FixtureProductID = orderDetail.FixtureProductID,
                                        OrderID = order.ID,
                                        Validated = false
                                    });
                                }
                            }

                            _context.MatchTickets.AddRange(matchTickets);
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
                            var fixture = await _context.Fixtures
                                .Include(e => e.HomeTeam)
                                .Include(e => e.AwayTeam)
                                .FirstOrDefaultAsync(e => e.ID == paymentAuthorization.FixtureID);

                            var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == paymentAuthorization.CustomerID);
                            await emailRepository.SendPaymentConfirmation(customer, response.CreditCardTransactionResults.AuthCode, order, paymentAuthorization.OrderDetails.Sum(e => e.Qty), fixture);

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

                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == email);

                var transferToCustomer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == transferRequest.Email);

                var matchTicket = await _context.MatchTickets
                    .Include(e => e.Order)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product)
                    .FirstOrDefaultAsync(e => e.ID == transferRequest.MatchTicketID);

                matchTicket.FixtureProduct.Fixture.HomeTeam.Name = !String.IsNullOrEmpty(matchTicket.FixtureProduct.Fixture.HomeTeam.Alias) ? matchTicket.FixtureProduct.Fixture.HomeTeam.Alias : matchTicket.FixtureProduct.Fixture.HomeTeam.Name;
                matchTicket.FixtureProduct.Fixture.AwayTeam.Name = !String.IsNullOrEmpty(matchTicket.FixtureProduct.Fixture.AwayTeam.Alias) ? matchTicket.FixtureProduct.Fixture.AwayTeam.Alias : matchTicket.FixtureProduct.Fixture.AwayTeam.Name;


                if (matchTicket != null && customer != null && transferToCustomer != null)
                {
                    if (!matchTicket.Validated)
                    {
                        try
                        {
                            matchTicket.TransferCustomerID = transferToCustomer.ID;
                            matchTicket.IsTransfer = false;
                            matchTicket.TransferTime = null;

                            _context.MatchTickets.Add(matchTicket);
                            await _context.SaveChangesAsync();

                            await emailRepository.SendTransferRequest(customer, transferToCustomer, matchTicket);

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message, "Transfer Request Email");
                        }

                        return "Ticket transferred successfully.";
                    }
                    else
                        return "This ticket is not valid for transferring.";
                }
                
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

                    var matchTickets = await _context.MatchTickets
                        .Include(e => e.Order).ThenInclude(e => e.Customer)
                        .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.HomeTeam)
                        .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.AwayTeam)
                        .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture).ThenInclude(e => e.Field)
                        .Include(e => e.FixtureProduct).ThenInclude(e => e.Product)
                        .Include(e => e.TransferCustomer)
                        .Where(e => DateTime.Now.Date <= e.FixtureProduct.Fixture.Date.AddDays(1) && (e.TransferCustomer.Email == email && !e.IsTransfer))
                        .ToListAsync();

                    matchTickets.ForEach(e => e.FixtureProduct.Fixture.HomeTeam.Name = !String.IsNullOrEmpty(e.FixtureProduct.Fixture.HomeTeam.Alias) ? e.FixtureProduct.Fixture.HomeTeam.Alias : e.FixtureProduct.Fixture.HomeTeam.Name);
                    matchTickets.ForEach(e => e.FixtureProduct.Fixture.AwayTeam.Name = !String.IsNullOrEmpty(e.FixtureProduct.Fixture.AwayTeam.Alias) ? e.FixtureProduct.Fixture.AwayTeam.Alias : e.FixtureProduct.Fixture.AwayTeam.Name);

                    foreach(var ticket in matchTickets)
                    {
                        transferRequests.Add(new Models.Shop.AcceptTransfer
                        {
                            CustomerID = ticket.Order.CustomerID,
                            TransferCustomerID = ticket.TransferCustomerID,
                            MatchTicketID = ticket.ID,
                            MatchTicket = ticket,
                            Customer = ticket.Order.Customer,
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

                    var matchTicket = await _context.MatchTickets
                        .FirstOrDefaultAsync(e => e.ID == acceptTransfer.MatchTicketID && e.TransferCustomerID != null);

                    if (matchTicket != null && transferToCustomer != null)
                    {
                        try
                        {
                            matchTicket.IsTransfer = true;
                            matchTicket.TransferTime = DateTime.Now;

                            _context.MatchTickets.Add(matchTicket);
                            await _context.SaveChangesAsync();

                            await emailRepository.SendTransferAccept(customer, transferToCustomer, matchTicket);

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message, "Transfer Request Email");
                        }
                    }

                    return "Transfer completed successfully!";
                }
                else
                {
                    var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == acceptTransfer.CustomerID);

                    var transferToCustomer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == acceptTransfer.TransferCustomerID);

                    var matchTicket = await _context.MatchTickets
                        .FirstOrDefaultAsync(e => e.ID == acceptTransfer.MatchTicketID && e.TransferCustomerID != null);

                    if (matchTicket != null && transferToCustomer != null)
                    {
                        try
                        {
                            matchTicket.IsTransfer = false;
                            matchTicket.TransferTime = null;
                            matchTicket.TransferCustomerID = null;

                            _context.MatchTickets.Add(matchTicket);
                            await _context.SaveChangesAsync();

                            await emailRepository.SendTransferReject(customer, transferToCustomer, matchTicket);

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message, "Transfer Request Email");
                        }
                    }

                    return "Transfer rejected successfully!";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TransferRequest");
            }

            return "There was an issue completing your transfer.";
        }

        public async Task Insert(MatchTicket item)
        {
            try
            {
                _context.MatchTickets.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Match Ticket");
            }
        }

        public async Task Update(MatchTicket item)
        {
            try
            {
                _context.MatchTickets.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Match Ticket");
            }
        }
    }
}
