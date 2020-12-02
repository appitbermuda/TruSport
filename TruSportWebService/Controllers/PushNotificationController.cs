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
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;

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

        [HttpPut]
        [Route("installations")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> UpdateInstallation([Required] DeviceInstallation deviceInstallation)
        {
            var success = await _pushNotificationRepository.CreateOrUpdateInstallationAsync(deviceInstallation, HttpContext.RequestAborted);

            if (!success)
                return new UnprocessableEntityResult();

            return new OkResult();
        }

        [HttpDelete()]
        [Route("installations/{installationId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<ActionResult> DeleteInstallation(
            [Required][FromRoute] string installationId)
        {
            var success = await _pushNotificationRepository.DeleteInstallationByIdAsync(installationId, CancellationToken.None);

            if (!success)
                return new UnprocessableEntityResult();

            return new OkResult();
        }

        [HttpPost]
        [Route("requests")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> RequestPush(
            [Required] NotificationRequest notificationRequest)
        {
            if ((notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                (!notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Text)))
                return new BadRequestResult();

            var success = await _pushNotificationRepository.RequestNotificationAsync(notificationRequest, HttpContext.RequestAborted);

            if (!success)
                return new UnprocessableEntityResult();

            return new OkResult();
        }

        [HttpGet]
        [Route("SendFootball")]
        public async Task<IActionResult> SendFootball(string Message)
        {
            try
            {
                await _pushNotificationRepository.SendFootballNotification(Message, HttpContext.RequestAborted);

                return new OkResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Push Notifications");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("Send")]
        public async Task<IActionResult> Send(string Message, string tag)
        {
            try
            {
                await _pushNotificationRepository.SendNotification(Message, HttpContext.RequestAborted, tag);

                return new OkResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Push Notifications");
            }

            return NoContent();
        }


    }
}
