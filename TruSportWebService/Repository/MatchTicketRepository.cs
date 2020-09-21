using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Repository
{
    public class MatchTicketRepository : IOnTrackRepository<FixtureProduct>
    {
        OnTrackContext _context;

        public MatchTicketRepository(OnTrackContext context)
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
                    .Include(e => e.Product).ThenInclude(e => e.Inventory)
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
                    .Include(e => e.Product).ThenInclude(e => e.Inventory)
                    .Include(e => e.Product).ThenInclude(e => e.Team)
                    .Where(e => e.Fixture.Date.AddDays(-1) < DateTime.Now.Date && DateTime.Now.Date < e.Fixture.Date.AddDays(1))
                    .ToListAsync();

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

            }
            catch(Exception ex)
            { }

            return fixtureProducts;
        }

        public async Task<IEnumerable<FixtureProduct>> Team(string teamID)
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
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.Sport)
                    .Include(e => e.Product).ThenInclude(e => e.ProductType).ThenInclude(e => e.MatchType)
                    .Include(e => e.Product).ThenInclude(e => e.Inventory)
                    .Include(e => e.Product).ThenInclude(e => e.Team)
                    .Where(e => e.Product.TeamID == teamID && e.Fixture.Date.AddDays(-1) < DateTime.Now.Date && DateTime.Now.Date < e.Fixture.Date.AddDays(1)).ToListAsync();

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

        public async Task<FixtureProduct> GetTodayByTeam(string teamID)
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
                    .Include(e => e.Product).ThenInclude(e => e.Inventory)
                    .Include(e => e.Product).ThenInclude(e => e.Team)
                    .FirstOrDefaultAsync(e => e.Product.TeamID == teamID && e.Fixture.Date.AddDays(-1) < DateTime.Now.Date && DateTime.Now.Date < e.Fixture.Date.AddDays(1));

                //Product product = await _context.Products
                //                        .Include(e => e.ProductType).ThenInclude(e => e.Sport)
                //                        .Include(e => e.ProductType).ThenInclude(e => e.MatchType)
                //                        .Include(e => e.Inventory)
                //                        .Include(e => e.Team).FirstOrDefaultAsync(e => e.TeamID == teamID);

                //Fixture fixture = await _context.Fixtures
                //                        .Include(e => e.HomeTeam)
                //                        .Include(e => e.AwayTeam)
                //                        .Include(e => e.Field)
                //                        .Include(e => e.League)
                //                        .Include(e => e.MatchType)
                //                        .Include(e => e.Season)
                //                        .Include(e => e.Sport).FirstOrDefaultAsync(e => e.Season.IsCurrent && e.Date == DateTime.Now.Date && e.HomeTeamID == teamID);
                //.Include(e => e.Sport).Where(e => products.Any(p => (p.TeamID == e.HomeTeamID && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null)) && e.Season.IsCurrent && e.Date == DateTime.Now.Date.AddHours(-4)).ToListAsync();
                //.Include(e => e.Sport).Where(e => products.Any(p => (p.TeamID == e.HomeTeamID && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == e.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null)) && e.Season.IsCurrent && e.Date < DateTime.Now.Date.AddDays(3) && e.Date > DateTime.Now.Date.AddDays(-1)).ToListAsync();

                //var product = product.TeamID == fixture.HomeTeamID && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == fixture.MatchTypeID) || (p.TeamID == null && p.ProductType.MatchTypeID == null));

                //if (product != null && fixture != null)
                //{
                //    matchTicket = new MatchTicket
                //    {
                //        Fixture = fixture,
                //        Product = product
                //    };
                //}
                

            }
            catch (Exception ex)
            { }

            return fixtureProduct;
        }

        public async Task<PaymentResponse> Purchase(PaymentAuthorize paymentAuthorization)
        {
            _context.Database.BeginTransaction();

            Request request = new Request();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.IsApproved = false;

            try
            {
                var processingFee = await _context.Settings.FirstOrDefaultAsync(e => e.ID == Constants.SETTING_PROCESSING_FEE_ID);

                Decimal ProcessingFeeAmount = Convert.ToDecimal(processingFee);
                Decimal PaymentAmount = 0.0m;

                string payment = String.Format("{0,0:N2}", Decimal.Parse(paymentAuthorization.Amount) / 100.0m);

                Setting ProcessingFee = await _context.Settings.FirstOrDefaultAsync(e => e.Key == "Processing Fee");
                ProcessingFeeAmount = Convert.ToDecimal(ProcessingFee.Value);

                //string topUp = topUpPayment.Amount;
                //topUp.Insert(topUp.Length - 2, ".");
                PaymentAmount = Convert.ToDecimal(payment);


                var response = await request.Payment(paymentAuthorization);

                if (response.CreditCardTransactionResults.ResponseCode == "1")
                {
                    Order order = new Order
                    {
                        Total = PaymentAmount + ProcessingFeeAmount,
                        CustomerID = paymentAuthorization.CustomerID,
                        Authorisation = response.CreditCardTransactionResults.AuthCode,
                        Date = DateTime.Now
                    };

                    _context.Orders.Add(order);

                    OrderDetail orderDetail = new OrderDetail
                    {
                        ProductID = paymentAuthorization.ProductID,
                        Qty = 1,
                        Subtotal = PaymentAmount,
                        OrderID = order.ID
                    };

                    _context.OrderDetails.Add(orderDetail);
                    await _context.SaveChangesAsync();


                    //var accountBalance = await _context.AccountBalances.FirstOrDefaultAsync(e => e.UserID == paymentAuthorization.CustomerID);

                    //if (accountBalance == null)
                    //{
                    //    AccountBalance balance = new AccountBalance
                    //    {
                    //        UserID = topUpPayment.UserID,
                    //        Balance = TopUpAmount
                    //    };

                    //    _context.AccountBalances.Add(accountBalance);
                    //    await _context.SaveChangesAsync();
                    //}
                    //else
                    //{
                    //    accountBalance.Balance = accountBalance.Balance + TopUpAmount;

                    //    _context.AccountBalances.Update(accountBalance);
                    //    await _context.SaveChangesAsync();
                    //}

                    _context.Database.CommitTransaction();
                    paymentResponse.IsApproved = true;

                    try
                    {
                        var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == paymentAuthorization.CustomerID);
                        //await emailRepository.SendPaymentConfirmation(customer, response.CreditCardTransactionResults.ReferenceNumber, PaymentAmount, ProcessingFeeAmount);
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
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                Debug.WriteLine(ex.Message, "Payment");
            }

            paymentResponse.Description = "There was an issue with your payment, please try again.";

            return paymentResponse;
        }

        public async Task Insert(FixtureProduct item)
        {
            try
            {
                _context.FixtureProducts.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Match Ticket");
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
                Debug.WriteLine(ex.Message, "Update Match Ticket");
            }
        }
    }
}
