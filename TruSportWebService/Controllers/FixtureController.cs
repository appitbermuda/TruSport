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

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixtureController : ControllerBase
    {
        private readonly FixtureRepository _fixtureRepository;

        public FixtureController(IOnTrackRepository<Fixture> fixtureRepository)
        {
            _fixtureRepository = (FixtureRepository)fixtureRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllFixtures")]
        public async Task<IActionResult> Fixtures()
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetAll();

                    if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamFixtures")]
        public async Task<IActionResult> FixturesByTeam(string teamID)
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetByTeam(teamID);

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("LeagueFixtures")]
        public async Task<IActionResult> FixturesByLeague(string leagueID)
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetByLeague(leagueID);

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("LiveFixtures")]
        public async Task<IActionResult> LiveFixtures()
        {
            try
            {

                IEnumerable<spLiveFixtures> fixtures = await _fixtureRepository.GetLive();

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
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
                Fixture fixture = await _fixtureRepository.Get(id);

                if (fixture != null)
                    return Ok(fixture);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post(Fixture fixture)
        {
            try
            {
                await _fixtureRepository.Insert(fixture);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        [Authorize(Roles = Role.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Fixture fixture)
        {
            try
            {
                await _fixtureRepository.Update(fixture);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
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
                await _fixtureRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }
    }
}
