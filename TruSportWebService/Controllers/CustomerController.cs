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
using System.Security.Claims;
using OnTrackWebService.Data;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerController(IDisposable customerRepository)
        {
            _customerRepository = (CustomerRepository)customerRepository;
        }

        // GET api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                IEnumerable<Customer> customers = await _customerRepository.GetAll();

                if (customers != null)
                    return Ok(customers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customers");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Customer)]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                Customer customer = await _customerRepository.Get(id);

                if (customer != null)
                    return Ok(customer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("ByDate")]
        public async Task<IActionResult> CustomerByDate(DateTime date)
        {
            try
            {
                List<Customer> customers = await _customerRepository.GetCustomersByDate(date);

                return Ok(customers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("ByTeam")]
        public async Task<IActionResult> CustomerByTeam(string TeamID)
        {
            try
            {
                List<Customer> customers = await _customerRepository.GetCustomersByTeam(TeamID);

                return Ok(customers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamByDate")]
        public async Task<IActionResult> GetCustomersByDateByTeam(DateTime date, string TeamID)
        {
            try
            {
                List<Customer> customers = await _customerRepository.GetCustomersByDateByTeam(date, TeamID);

                return Ok(customers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("CustomerExists")]
        public async Task<IActionResult> CustomerExists(string email)
        {
            try
            {
                bool customerExists = await _customerRepository.CustomerExists(email);

                return Ok(customerExists);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("Validate")]
        public async Task<IActionResult> Validate(string email)
        {
            try
            {
                bool validateCustomer = await _customerRepository.Validate(email);

                return Ok(validateCustomer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }

        // POST api/values
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] Customer customer)
        {
            try
            {
                var newCustomer = await _customerRepository.SignUp(customer);
                return Ok(newCustomer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] UserAuthentication customerAuthentication)
        {
            try
            {
                var customer = await _customerRepository.SignIn(customerAuthentication);

                return Ok(customer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }

        //This resource is only For SuperAdmin role
        [Authorize(Roles = Roles.Customer)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(Customer customer)
        {
            try
            {
                await _customerRepository.Update(customer);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }

        //This resource is only For SuperAdmin role
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            try
            {
                await _customerRepository.ForgotPassword(forgotPassword);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordReset passwordReset)
        {
            try
            {
                string resetPassword = await _customerRepository.ResetPassword(passwordReset);

                return Ok(resetPassword);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return NoContent();
        }
    }
}
