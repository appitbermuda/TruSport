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
    public class BowlingRosterController : ControllerBase
    {
        private readonly BowlingRosterRepository _bowlingRosterRepository;

        public BowlingRosterController(IOnTrackRepository<BowlingRoster> bowlingRosterRepository)
        {
            _bowlingRosterRepository = (BowlingRosterRepository)bowlingRosterRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllBowlingRosters")]
        public async Task<IActionResult> BowlingRosters()
        {
            try
            {

                IEnumerable<BowlingRoster> bowlingRosters = await _bowlingRosterRepository.GetAll();

                if (bowlingRosters != null)
                    return Ok(bowlingRosters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingRoster");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("FixtureBowlingRosters")]
        public async Task<IActionResult> BowlingRostersByFixture(string fixtureID)
        {
            try
            {

                IEnumerable<BowlingRoster> bowlingRosters = await _bowlingRosterRepository.GetByFixture(fixtureID);

                if (bowlingRosters != null)
                    return Ok(bowlingRosters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingRoster");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamBowlingRosters")]
        public async Task<IActionResult> BowlingRostersByTeam(string fixtureID, string teamID)
        {
            try
            {

                IEnumerable<BowlingRoster> bowlingRosters = await _bowlingRosterRepository.GetByFixtureByTeam(fixtureID, teamID);

                if (bowlingRosters != null)
                    return Ok(bowlingRosters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingRoster");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("BowlingRosterPlayer")]
        public async Task<IActionResult> BowlingRosterByPlayer(string fixtureID, string playerID)
        {
            try
            {

                BowlingRoster bowlingRosters = await _bowlingRosterRepository.GetByPlayerID(fixtureID, playerID);

                if (bowlingRosters != null)
                    return Ok(bowlingRosters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingRoster");
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
                BowlingRoster bowlingRoster = await _bowlingRosterRepository.Get(id);

                if (bowlingRoster != null)
                    return Ok(bowlingRoster);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingRoster");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] BowlingRoster bowlingRoster)
        {
            try
            {
                await _bowlingRosterRepository.Insert(bowlingRoster);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingRoster");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("InsertAll")]
        public async Task<IActionResult> Post([FromBody] List<BowlingRoster> bowlingRoster)
        {
            try
            {
                await _bowlingRosterRepository.InsertAll(bowlingRoster);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingRoster");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] BowlingRoster bowlingRoster)
        {
            try
            {
                await _bowlingRosterRepository.Update(bowlingRoster);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingRoster");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("UpdateAll")]
        public async Task<IActionResult> UpdateAll([FromBody] List<BowlingRoster> bowlingRosters)
        {
            try
            {
                await _bowlingRosterRepository.UpdateAll(bowlingRosters);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingRoster");
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
                await _bowlingRosterRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingRoster");
            }

            return NoContent();
        }
    }
}
