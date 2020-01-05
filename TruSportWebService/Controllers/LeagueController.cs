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
    public class LeagueController : ControllerBase
    {
        private readonly LeagueRepository _leagueRepository;

        public LeagueController(IOnTrackRepository<League> leagueRepository)
        {
            _leagueRepository = (LeagueRepository)leagueRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllLeagues")]
        public async Task<IActionResult> Leagues()
        {
            try
            {

                IEnumerable<League> leagues = await _leagueRepository.GetAll();

                if (leagues != null)
                    return Ok(leagues);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamLeagues")]
        public async Task<IActionResult> LeaguesByTeam(string teamID)
        {
            try
            {

                IEnumerable<League> leagues = await _leagueRepository.GetByTeam(teamID);

                if (leagues != null)
                    return Ok(leagues);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
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
                League league = await _leagueRepository.Get(id);

                if (league != null)
                    return Ok(league);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] League league)
        {
            try
            {
                await _leagueRepository.Insert(league);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }

            return NoContent();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] League league)
        {
            try
            {
                await _leagueRepository.Update(league);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [Authorize(Roles = Role.Admin)]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _leagueRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League");
            }

            return NoContent();
        }
    }
}
