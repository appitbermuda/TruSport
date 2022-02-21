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
    public class ContactTraceController : ControllerBase
    {
        private readonly ContactTraceRepository _contactTraceRepository;

        public ContactTraceController(IOnTrackRepository<ContactTrace> contactTraceRepository)
        {
            _contactTraceRepository = (ContactTraceRepository)contactTraceRepository;
        }

        // GET api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> AllContactTraces()
        {
            try
            {
                IEnumerable<ContactTrace> contactTraces = await _contactTraceRepository.AllContactTraces();

                if (contactTraces != null)
                    return Ok(contactTraces);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ContactTrace");
            }

            return NoContent();
        }

        // GET api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> ContactTraces()
        {
            try
            {
                IEnumerable<ContactTrace> contactTraces = await _contactTraceRepository.GetAll();

                if (contactTraces != null)
                    return Ok(contactTraces);
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
        [Route("Today")]
        public async Task<IActionResult> TodaysContactTraces(string teamID)
        {
            try
            {

                IEnumerable<ContactTrace> contactTraces = await _contactTraceRepository.GetContactTracesForToday(User);

                if (contactTraces != null)
                    return Ok(contactTraces);
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
        [Route("ForToday")]
        public async Task<IActionResult> ContactTracesForToday()
        {
            try
            {

                IEnumerable<ContactTrace> contactTraces = await _contactTraceRepository.GetTodayContactTracesForOntrackr(User);

                if (contactTraces != null)
                    return Ok(contactTraces);
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
        [Route("Event")]
        public async Task<IActionResult> FixtureContactTraces(string eventID)
        {
            try
            {

                IEnumerable<ContactTrace> contactTraces = await _contactTraceRepository.Event(eventID);

                if (contactTraces != null)
                    return Ok(contactTraces);
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
        [Route("Download")]
        public async Task<IActionResult> Download(string fixtureID)
        {
            try
            {

                bool downloaded = await _contactTraceRepository.Download(fixtureID, User);

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
        [Route("DownloadReport")]
        public async Task<IActionResult> DownloadReport(string eventID)
        {
            try
            {

                bool downloaded = await _contactTraceRepository.DownloadReport(eventID, User);

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

                List<TicketReport> fixtures = await _contactTraceRepository.Reports(User);

                return Ok(fixtures);
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
        [Route("EventReports")]
        public async Task<IActionResult> EventReports()
        {
            try
            {

                List<TicketReport> fixtures = await _contactTraceRepository.EventReports(User);

                return Ok(fixtures);
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
        [Route("Team")]
        public async Task<IActionResult> TeamContactTraces()
        {
            try
            {
                IEnumerable<ContactTrace> contactTraces = await _contactTraceRepository.Team(User);

                if (contactTraces != null)
                    return Ok(contactTraces);
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
        [Route("ForTeam")]
        public async Task<IActionResult> ContactTracesForTeam()
        {
            try
            {
                IEnumerable<ContactTrace> contactTraces = await _contactTraceRepository.TeamContactTraces(User);

                if (contactTraces != null)
                    return Ok(contactTraces);
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
        [Route("ForTeamOntrackr")]
        public async Task<IActionResult> ContactTracesForTeamOntrackr()
        {
            try
            {
                IEnumerable<ContactTrace> contactTraces = await _contactTraceRepository.TeamContactTracesOntrackr(User);

                if (contactTraces != null)
                    return Ok(contactTraces);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ContactTrace");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] ContactTrace contactTrace)
        {
            try
            {
                await _contactTraceRepository.Update(contactTrace);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ContactTrace");
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
                await _contactTraceRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ContactTrace");
            }

            return NoContent();
        }
    }
}
