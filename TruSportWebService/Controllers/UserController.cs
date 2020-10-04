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

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthenticationRepository _authenticationRepository;

        public UserController(IDisposable authenticationRepository)
        {
            _authenticationRepository = (AuthenticationRepository)authenticationRepository;
        }

        // GET api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("AllUsers")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                IEnumerable<AllUsers> users = await _authenticationRepository.GetAllUsers();

                if (users != null)
                    return Ok(users);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Users");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                User user = await _authenticationRepository.GetUser(id);

                if (user != null)
                    return Ok(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("UserExists")]
        public async Task<IActionResult> UserExists(string email)
        {
            try
            {
                bool userExists = await _authenticationRepository.UserExists(email);

                return Ok(userExists);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("Validate")]
        public async Task<IActionResult> Validate(string email)
        {
            try
            {
                bool validateUser = await _authenticationRepository.ValidateUser(email);

                return Ok(validateUser);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return NoContent();
        }

        // POST api/values
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            try
            {
                var newUser = await _authenticationRepository.SignUp(user);
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] UserAuthentication userAuthentication)
        {
            try
            {
                var user = await _authenticationRepository.SignInUser(userAuthentication);

                return Ok(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return NoContent();
        }

        //This resource is only For SuperAdmin role
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                await _authenticationRepository.Update(user);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
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
                await _authenticationRepository.ForgotUserPassword(forgotPassword);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordReset passwordReset)
        {
            try
            {
                string resetPassword = await _authenticationRepository.ResetUserPassword(passwordReset);

                return Ok(resetPassword);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return NoContent();
        }
    }
}
