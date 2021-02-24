using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;
using OnTrackWebService.Repository;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTeamController : ControllerBase
    {
        private readonly TicketTeamRepository _ticketTeamRepository;

        public TicketTeamController(IOnTrackRepository<TicketTeam> ticketTeamRepository)
        {
            _ticketTeamRepository = (TicketTeamRepository)ticketTeamRepository;
        }

        // GET api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> TicketTeams()
        {
            try
            {

                IEnumerable<TicketTeam> ticketTeams = await _ticketTeamRepository.GetAll();

                if (ticketTeams != null)
                    return Ok(ticketTeams);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketTeam");
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
                TicketTeam ticketTeam = await _ticketTeamRepository.Get(id);

                if (ticketTeam != null)
                    return Ok(ticketTeam);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketTeam");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] TicketTeam ticketTeam)
        {
            try
            {
                await _ticketTeamRepository.Insert(ticketTeam);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketTeam");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] TicketTeam ticketTeam)
        {
            try
            {
                await _ticketTeamRepository.Update(ticketTeam);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketTeam");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _ticketTeamRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketTeam");
            }

            return NoContent();
        }
    }
}
