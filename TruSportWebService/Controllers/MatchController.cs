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
    public class MatchController : ControllerBase
    {
        private readonly MatchRepository _matchRepository;

        public MatchController(IOnTrackRepository<Match> matchRepository)
        {
            _matchRepository = (MatchRepository)matchRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllMatches")]
        public async Task<IActionResult> Matches()
        {
            try
            {

                IEnumerable<Match> matches = await _matchRepository.GetAll();

                if (matches != null)
                    return Ok(matches);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("FixtureMatch")]
        public async Task<IActionResult> MatchByFixture(string fixtureID)
        {
            try
            {

                Match match = await _matchRepository.GetByFixture(fixtureID);

                if (match != null)
                    return Ok(match);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match");
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
                Match match = await _matchRepository.Get(id);

                if (match != null)
                    return Ok(match);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Match match)
        {
            try
            {
                await _matchRepository.Insert(match);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Match match)
        {
            try
            {
                await _matchRepository.Update(match);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match");
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
                await _matchRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match");
            }

            return NoContent();
        }
    }
}
