using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Repository;
using OnTrackWebService.Models;
using System.Diagnostics;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailRepository _emailRepository;



        public EmailController(IEmailRepository<string> emailRepository)
        {
            _emailRepository = (EmailRepository)emailRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("SendSignUpEmail")]
        public async Task<IActionResult> Emails(string name, string email, string role, string team)
        {
            try
            {

                await _emailRepository.SendSignUpEmail(name, email, role, team);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Email");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("SendPaymentConfirmationTest")]
        public async Task<IActionResult> SendPaymentConfirmationTest()
        {
            try
            {

                await _emailRepository.SendPaymentConfirmationTest();

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Email");
            }

            return NoContent();
        }
    }
}
