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
    public class MatchTicketController : ControllerBase
    {
        private readonly MatchTicketRepository _matchTicketRepository;

        public MatchTicketController(IOnTrackRepository<MatchTicket> matchTicketRepository)
        {
            _matchTicketRepository = (MatchTicketRepository)matchTicketRepository;
        }

        // GET api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> MatchTickets()
        {
            try
            {

                IEnumerable<MatchTicket> matchTickets = await _matchTicketRepository.GetAll();

                if (matchTickets != null)
                    return Ok(matchTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.Customer)]
        [HttpGet]
        [Route("Customer")]
        public async Task<IActionResult> CustomerMatchTickets()
        {
            try
            {
                IEnumerable<MatchTicket> matchTickets = await _matchTicketRepository.GetCustomerTickets(User);

                if (matchTickets != null)
                    return Ok(matchTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }

            return NoContent();
        }

        // GET api/values/5
        [Authorize(Roles = Roles.Tickets)]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                MatchTicket matchTicket = await _matchTicketRepository.Get(id);

                if (matchTicket != null)
                    return Ok(matchTicket);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match Ticket");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.Tickets)]
        [HttpGet]
        [Route("Today")]
        public async Task<IActionResult> TodaysMatchTickets(string teamID)
        {
            try
            {

                IEnumerable<MatchTicket> matchTickets = await _matchTicketRepository.GetTodayMatchTickets(teamID);

                if (matchTickets != null)
                    return Ok(matchTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.TicketAdmin)]
        [HttpGet]
        [Route("Fixture")]
        public async Task<IActionResult> FixtureMatchTickets(string fixtureID)
        {
            try
            {

                IEnumerable<MatchTicket> matchTickets = await _matchTicketRepository.Fixture(fixtureID);

                if (matchTickets != null)
                    return Ok(matchTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }

            return NoContent();
        }

        // GET api/values/5
        [Authorize(Roles = Roles.TicketAdmin)]
        [HttpGet]
        [Route("Validate")]
        public async Task<IActionResult> ValidateCustomer(string matchTicketID)
        {
            try
            {
                bool validated = await _matchTicketRepository.ValidateCustomer(matchTicketID);

                return Ok(validated);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Order");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.TicketAdmin)]
        [HttpGet]
        [Route("Team")]
        public async Task<IActionResult> TeamMatchTickets(string teamID)
        {
            try
            {

                IEnumerable<MatchTicket> matchTickets = await _matchTicketRepository.Team(teamID);

                if (matchTickets != null)
                    return Ok(matchTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.TicketAdmin)]
        [HttpPost]
        [Route("Scan")]
        public async Task<IActionResult> Scan([FromBody] MatchTicket matchTicket)
        {
            try
            {
                
                TicketResponse ticketResponse = await _matchTicketRepository.Scan(matchTicket);

                return Ok(ticketResponse);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Customer)]
        [HttpPost]
        [Route("Purchase")]
        public async Task<IActionResult> Purchase([FromBody] PaymentAuthorize paymentAuthorize)
        {
            try
            {
                if (paymentAuthorize != null && paymentAuthorize.CardNumber != null && paymentAuthorize.CVV != null && paymentAuthorize.Expiry != null && paymentAuthorize.Amount != null)
                {
                    PaymentResponse authorized = await _matchTicketRepository.Purchase(paymentAuthorize); 

                    return Ok(authorized);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Purchase Match Ticket");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] MatchTicket matchTicket)
        {
            try
            {
                await _matchTicketRepository.Insert(matchTicket);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] MatchTicket matchTicket)
        {
            try
            {
                await _matchTicketRepository.Update(matchTicket);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
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
                await _matchTicketRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchTicket");
            }

            return NoContent();
        }
    }
}
