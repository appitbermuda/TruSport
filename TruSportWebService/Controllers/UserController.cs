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
        private readonly UserRepository _userRepository;

        public UserController(IDisposable userRepository)
        {
            _userRepository = (UserRepository)userRepository;
        }

        // GET api/values
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        [Route("AllUsers")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                IEnumerable<AllUsers> users = await _userRepository.GetAll();

                if (users != null)
                    return Ok(users);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Users");
            }

            return NoContent();
        }

        [Authorize(Roles = Role.AllUsers)]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                User user = await _userRepository.Get(id);

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
                bool userExists = await _userRepository.UserExists(email);

                return Ok(userExists);
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
                var newUser = await _userRepository.SignUp(user);
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
                var user = await _userRepository.SignIn(userAuthentication);

                return Ok(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return NoContent();
        }

        //This resource is only For SuperAdmin role
        [Authorize(Roles = Role.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                await _userRepository.Update(user);

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
                await _userRepository.ForgotPassword(forgotPassword);

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
                string resetPassword = await _userRepository.ResetPassword(passwordReset);

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
