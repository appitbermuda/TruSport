using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Repository;
using OnTrackWebService.Models;
using System.Diagnostics;
using OnTrackWebService.Data;
using Microsoft.AspNetCore.Authorization;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly TournamentRepository _tournamentRepository;

        public TournamentController(IOnTrackRepository<TennisTournament> tournamentRepository)
        {
            _tournamentRepository = (TournamentRepository)tournamentRepository;
        }

        // GET api/values
        //Deprecated
        [HttpGet]
        [Route("AllTournaments")]
        public async Task<IActionResult> Tournaments()
        {
            try
            {
                IEnumerable<TennisTournament> tournaments = await _tournamentRepository.GetAll();

                if (tournaments != null)
                    return Ok(tournaments);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tournament");
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
                TennisTournament tournament = await _tournamentRepository.Get(id);

                if (tournament != null)
                    return Ok(tournament);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tournament");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] TennisTournament tournament)
        {
            try
            {
                await _tournamentRepository.Insert(tournament);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tournament");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] TennisTournament tournament)
        {
            try
            {
                await _tournamentRepository.Update(tournament);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tournament");
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
                await _tournamentRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Tournament");
            }

            return NoContent();
        }
    }
}
