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
    public class MatchRosterController : ControllerBase
    {
        private readonly MatchRosterRepository _matchRosterRepository;

        public MatchRosterController(IOnTrackRepository<MatchRoster> matchRosterRepository)
        {
            _matchRosterRepository = (MatchRosterRepository)matchRosterRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllMatchRosters")]
        public async Task<IActionResult> MatchRosters()
        {
            try
            {

                IEnumerable<MatchRoster> matchRosters = await _matchRosterRepository.GetAll();

                if (matchRosters != null)
                    return Ok(matchRosters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchRoster");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("FixtureMatchRosters")]
        public async Task<IActionResult> MatchRostersByFixture(string fixtureID)
        {
            try
            {

                IEnumerable<MatchRoster> matchRosters = await _matchRosterRepository.GetByFixture(fixtureID);

                if (matchRosters != null)
                    return Ok(matchRosters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchRoster");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamMatchRosters")]
        public async Task<IActionResult> MatchRostersByTeam(string fixtureID, string teamID)
        {
            try
            {

                IEnumerable<MatchRoster> matchRosters = await _matchRosterRepository.GetByFixtureByTeam(fixtureID, teamID);

                if (matchRosters != null)
                    return Ok(matchRosters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchRoster");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("MatchRosterPlayer")]
        public async Task<IActionResult> MatchRosterByPlayer(string fixtureID, string playerID)
        {
            try
            {

                MatchRoster matchRosters = await _matchRosterRepository.GetByPlayerID(fixtureID, playerID);

                if (matchRosters != null)
                    return Ok(matchRosters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchRoster");
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
                MatchRoster matchRoster = await _matchRosterRepository.Get(id);

                if (matchRoster != null)
                    return Ok(matchRoster);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchRoster");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] MatchRoster matchRoster)
        {
            try
            {
                await _matchRosterRepository.Insert(matchRoster);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchRoster");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("InsertAll")]
        public async Task<IActionResult> Post([FromBody] List<MatchRoster> matchRoster)
        {
            try
            {
                await _matchRosterRepository.InsertAll(matchRoster);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchRoster");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] MatchRoster matchRoster)
        {
            try
            {
                await _matchRosterRepository.Update(matchRoster);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchRoster");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("UpdateAll")]
        public async Task<IActionResult> UpdateAll([FromBody] List<MatchRoster> matchRosters)
        {
            try
            {
                await _matchRosterRepository.UpdateAll(matchRosters);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchRoster");
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
                await _matchRosterRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchRoster");
            }

            return NoContent();
        }
    }
}
