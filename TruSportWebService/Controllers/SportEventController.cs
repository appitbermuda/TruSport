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
    public class SportEventController : ControllerBase
    {
        private readonly SportEventRepository _sportEventRepository;

        public SportEventController(IOnTrackRepository<SportEvent> sportEventRepository)
        {
            _sportEventRepository = (SportEventRepository)sportEventRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> SportEvents()
        {
            try
            {

                IEnumerable<SportEvent> sportEvents = await _sportEventRepository.GetAll();

                if (sportEvents != null)
                    return Ok(sportEvents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("Tickets")]
        public async Task<IActionResult> Products()
        {
            try
            {

                IEnumerable<SportEvent> sportEvents = await _sportEventRepository.Tickets();

                if (sportEvents != null)
                    return Ok(sportEvents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                SportEvent sportEvent = await _sportEventRepository.Get(id);

                if (sportEvent != null)
                    return Ok(sportEvent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match Ticket");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Upcoming")]
        public async Task<IActionResult> UpcomingFixtures()
        {
            try
            {

                List<SportEvent> upcoming = await _sportEventRepository.GetUpcomingEvents(User);

                if (upcoming != null)
                    return Ok(upcoming);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Today")]
        public async Task<IActionResult> TodayFixture()
        {
            try
            {

                SportEvent sportEvent = await _sportEventRepository.GetTodayEvent(User);

                if (sportEvent != null)
                    return Ok(sportEvent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("TodayByTeam")]
        public async Task<IActionResult> TodayByTeam()
        {
            try
            {

                IEnumerable<EventTicket> sportEvents = await _sportEventRepository.GetTodayByTeam(User);

                if (sportEvents != null)
                    return Ok(sportEvents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("Ticket")]
        public async Task<IActionResult> FixtureTicketProducts(string eventID, string email)
        {
            try
            {

                IEnumerable<EventTicket> eventTickets = await _sportEventRepository.EventTicket(eventID, email);

                if (eventTickets != null)
                    return Ok(eventTickets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Team")]
        public async Task<IActionResult> TeamSportEvents()
        {
            try
            {

                IEnumerable<EventTicket> sportEvents = await _sportEventRepository.Team(User);

                if (sportEvents != null)
                    return Ok(sportEvents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] SportEvent sportEvent)
        {
            try
            {
                await _sportEventRepository.Insert(sportEvent);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] SportEvent sportEvent)
        {
            try
            {
                await _sportEventRepository.Update(sportEvent);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
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
                await _sportEventRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SportEvent");
            }

            return NoContent();
        }
    }
}
