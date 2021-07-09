using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Repository;
using OnTrackWebService.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using OnTrackWebService.Data;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(IOnTrackRepository<Order> orderRepository)
        {
            _orderRepository = (OrderRepository)orderRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("MatchDay")]
        public async Task<IActionResult> MatchDayOrders(string Email)
        {
            try
            {

                IEnumerable<CustomerOrder> orders = await _orderRepository.GetMatchDayOrders(Email);

                if (orders != null)
                    return Ok(orders);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }


        // GET api/values
        [HttpGet]
        [Route("History")]
        public async Task<IActionResult> History(string Email)
        {
            try
            {

                IEnumerable<Order> orders = await _orderRepository.GetOrderHistory(Email);

                if (orders != null)
                    return Ok(orders);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }


        // GET api/values
        [HttpGet]
        [Route("GetHistory")]
        public async Task<IActionResult> GetHistory(string Email)
        {
            try
            {

                IEnumerable<Order> orders = await _orderRepository.GetOrdersHistory(Email);

                if (orders != null)
                    return Ok(orders);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }


        // GET api/values
        [HttpGet]
        [Route("TodayByTeam")]
        public async Task<IActionResult> TodayByTeam()
        {
            try
            {

                IEnumerable<Order> orders = await _orderRepository.TodayByTeam(User);

                if (orders != null)
                    return Ok(orders);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Team")]
        public async Task<IActionResult> Team()
        {
            try
            {
                IEnumerable<Order> orders = await _orderRepository.Team(User);

                if (orders != null)
                    return Ok(orders);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("AllOrders")]
        public async Task<IActionResult> Orders()
        {
            try
            {

                IEnumerable<Order> orders = await _orderRepository.GetAll();

                if (orders != null)
                    return Ok(orders);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                Order order = await _orderRepository.Get(id);

                if (order != null)
                    return Ok(order);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] NewOrder order)
        {
            try
            {
                await _orderRepository.Insert(order);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Order order)
        {
            try
            {
                await _orderRepository.Update(order);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [Authorize(Roles = Roles.AllUsers)]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _orderRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }
    }
}
