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
    public class PushNotificationController : ControllerBase
    {
        private readonly PushNotificationRepository _pushNotificationRepository;

        public PushNotificationController(IPushNotificationRepository<Push> pushNotificationRepository)
        {
            _pushNotificationRepository = (PushNotificationRepository)pushNotificationRepository;
        }

        // GET api/values
        [Authorize(Roles = Role.AllUsers)]
        [HttpGet]
        [Route("Send")]
        public async Task<IActionResult> Send(string name, string title, string body)
        {
            try
            {

                string pushNotification = await _pushNotificationRepository.Send(name, title, body);

                return Ok(pushNotification);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Push Notifications");
            }

            return NoContent();
        }

        
    }
}
