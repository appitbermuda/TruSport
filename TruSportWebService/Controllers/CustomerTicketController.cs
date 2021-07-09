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
    public class CustomerTicketController : ControllerBase
    {
        private readonly CustomerTicketRepository _customerTicketRepository;

        public CustomerTicketController(IOnTrackRepository<CustomerTicket> customerTicketRepository)
        {
            _customerTicketRepository = (CustomerTicketRepository)customerTicketRepository;
        }

        // GET api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> CustomerTickets()
        {
            try
            {

                IEnumerable<CustomerTicket> customerTickets = await _customerTicketRepository.GetAll();

                if (customerTickets != null)
                    return Ok(customerTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.Customer)]
        [HttpGet]
        [Route("Tickets")]
        public async Task<IActionResult> CustomerEventTickets()
        {
            try
            {
                IEnumerable<CustomerTicket> customerTickets = await _customerTicketRepository.GetCustomerTickets(User);

                if (customerTickets != null)
                    return Ok(customerTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.Customer)]
        [HttpGet]
        [Route("TransferRequests")]
        public async Task<IActionResult> TransferRequests()
        {
            try
            {
                IEnumerable<AcceptTransfer> customerTickets = await _customerTicketRepository.GetTransferRequests(User);

                if (customerTickets != null)
                    return Ok(customerTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
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
                CustomerTicket customerTicket = await _customerTicketRepository.Get(id);

                if (customerTicket != null)
                    return Ok(customerTicket);
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
        public async Task<IActionResult> TodaysCustomerTickets()
        {
            try
            {

                IEnumerable<CustomerTicket> customerTickets = await _customerTicketRepository.GetTodayCustomerTickets(User);

                if (customerTickets != null)
                    return Ok(customerTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.TicketAdmin)]
        [HttpGet]
        [Route("Event")]
        public async Task<IActionResult> FixtureCustomerTickets(string eventID)
        {
            try
            {

                IEnumerable<CustomerTicket> customerTickets = await _customerTicketRepository.SportEvent(eventID);

                if (customerTickets != null)
                    return Ok(customerTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.TicketAdmin)]
        [HttpGet]
        [Route("EventBilling")]
        public async Task<IActionResult> FixtureContactTraces(string eventID)
        {
            try
            {

                TicketBilling ticketBilling = await _customerTicketRepository.SportEventBilling(eventID, User);

                if (ticketBilling != null)
                    return Ok(ticketBilling);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ContactTrace");
            }

            return NoContent();
        }

        // GET api/values/5
        [Authorize(Roles = Roles.TicketAdmin)]
        [HttpGet]
        [Route("Validate")]
        public async Task<IActionResult> ValidateCustomer(string customerTicketID)
        {
            try
            {
                bool validated = await _customerTicketRepository.ValidateCustomer(customerTicketID);

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
        public async Task<IActionResult> TeamCustomerTickets()
        {
            try
            {

                IEnumerable<CustomerTicket> customerTickets = await _customerTicketRepository.Team(User);

                if (customerTickets != null)
                    return Ok(customerTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.TicketAdmin)]
        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> Download(string fixtureID)
        {
            try
            {

                bool downloaded = await _customerTicketRepository.Download(fixtureID, User);

                return Ok(downloaded);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ContactTrace");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.TicketAdmin)]
        [HttpGet]
        [Route("Reports")]
        public async Task<IActionResult> Reports()
        {
            try
            {

                List<TicketReport> reports = await _customerTicketRepository.Reports(User);

                return Ok(reports);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ContactTrace");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.TicketAdmin)]
        [HttpPost]
        [Route("Scan")]
        public async Task<IActionResult> Scan([FromBody] CustomerTicket customerTicket)
        {
            try
            {
                
                TicketResponse ticketResponse = await _customerTicketRepository.Scan(customerTicket, User);

                return Ok(ticketResponse);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
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
                    PaymentResponse authorized = await _customerTicketRepository.Purchase(User, paymentAuthorize); 

                    return Ok(authorized);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Purchase Match Ticket");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Customer)]
        [HttpPost]
        [Route("PurchaseTest")]
        public async Task<IActionResult> PurchaseTest([FromBody] PaymentAuthorize paymentAuthorize)
        {
            try
            {
                if (paymentAuthorize != null && paymentAuthorize.CardNumber != null && paymentAuthorize.CVV != null && paymentAuthorize.Expiry != null && paymentAuthorize.Amount != null)
                {
                    PaymentResponse authorized = await _customerTicketRepository.PurchaseTest(paymentAuthorize);

                    return Ok(authorized);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Purchase Match Ticket");
            }

            return NoContent();
        }


        //[Authorize(Roles = Roles.Ad)]
        [HttpGet]
        [Route("ResendPurchaseConfirmation")]
        public async Task<IActionResult> ResendPurchaseConfirmation(string OrderNumber)
        {
            try
            {
                if (!String.IsNullOrEmpty(OrderNumber))
                {
                    string confirmation = await _customerTicketRepository.ResendPurchaseConfirmation(OrderNumber);

                    return Ok(confirmation);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Resend Match Ticket Confirmation");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Customer)]
        [HttpPost]
        [Route("Transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest transferRequest)
        {
            try
            {
                if (transferRequest != null && transferRequest.CustomerTicketID != null && transferRequest.Email != null)
                {
                    string transferred = await _customerTicketRepository.TransferRequest(transferRequest, User);

                    return Ok(transferred);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer Match Ticket");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Customer)]
        [HttpPost]
        [Route("AcceptTransfer")]
        public async Task<IActionResult> AcceptTransfer([FromBody] AcceptTransfer accept)
        {
            try
            {
                if (accept != null && accept.CustomerID != null && accept.CustomerTicketID != null && accept.TransferCustomerID != null && accept.Accept)
                {
                    string transferred = await _customerTicketRepository.AcceptTransferRequest(accept);

                    return Ok(transferred);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer Match Ticket");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] CustomerTicket customerTicket)
        {
            try
            {
                await _customerTicketRepository.Insert(customerTicket);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] CustomerTicket customerTicket)
        {
            try
            {
                await _customerTicketRepository.Update(customerTicket);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
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
                await _customerTicketRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CustomerTicket");
            }

            return NoContent();
        }
    }
}
