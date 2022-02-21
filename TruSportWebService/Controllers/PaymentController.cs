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
using OnTrackWebService.Models.Ticket;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository<PaymentAuthorize> paymentRepository)
        {
            _paymentRepository = (PaymentRepository)paymentRepository;
        }

        // POST api/values
        //[Authorize(Roles = Roles.Customer)]
        [HttpPost]
        [Route("Authorize")]
        public async Task<IActionResult> PurchaseTicket([FromBody] PaymentAuthorize paymentAuthorize)
        {
            try
            {
                if (paymentAuthorize != null && paymentAuthorize.CardNumber != null && paymentAuthorize.CVV != null && paymentAuthorize.Expiry != null && paymentAuthorize.Amount != null)
                {
                    PaymentResponse authorized = await _paymentRepository.Authorize(paymentAuthorize);

                    return Ok(authorized);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Purchase Match Ticket");
            }

            return NoContent();
        }

        //[Authorize(Roles = Roles.Customer)]
        [HttpPost]
        [Route("AuthorizeTest")]
        public async Task<IActionResult> PurchaseTest([FromBody] PaymentAuthorize paymentAuthorize)
        {
            try
            {
                if (paymentAuthorize != null && paymentAuthorize.CardNumber != null && paymentAuthorize.CVV != null && paymentAuthorize.Expiry != null && paymentAuthorize.Amount != null)
                {
                    PaymentResponse authorized = await _paymentRepository.AuthorizeTest(paymentAuthorize);

                    return Ok(authorized);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Purchase Match Ticket");
            }

            return NoContent();
        }
    }
}
