using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Order> Get(string id)
        {
            return await _context.Orders.Include(e => e.Customer).Include(e => e.OrderDetail).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.Include(e => e.Customer).Include(e => e.OrderDetail).ToListAsync();
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
                        var product = await _context.Products.FirstOrDefaultAsync(e => e.ID == orderItems.ProductID);

                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderID = order.ID;
                        orderDetail.ProductID = orderItems.ProductID;
                        orderDetail.Qty = orderItems.Qty;
                        orderDetail.Subtotal = product.Price * orderItems.Qty;

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
