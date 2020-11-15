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
using Microsoft.AspNetCore.Http;
using OnTrackWebService.Models.Imports;

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

        // GET api/values
        [HttpGet]
        [Route("GetFixtures")]
        public async Task<IActionResult> GetFixtures()
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.Get();

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("UploadCricket")]
        public async Task<IActionResult> UploadCricketFixtures([FromForm(Name = "file")]IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    ImportCricketFixtures fileUploadResponse = await _fixtureRepository.UploadCricketFixtures(file);

                    return Ok(fileUploadResponse);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Upload Fixture");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("UploadFootball")]
        public async Task<IActionResult> UploadFootballFixtures([FromForm(Name = "file")]IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    ImportFootballFixtures fileUploadResponse = await _fixtureRepository.UploadFootballFixtures(file);

                    return Ok(fileUploadResponse);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Upload Fixture");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("UploadBowling")]
        public async Task<IActionResult> UploadBowlingFixtures([FromForm(Name = "file")] IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    ImportBowlingFixtures fileUploadResponse = await _fixtureRepository.UploadBowlingFixtures(file);

                    return Ok(fileUploadResponse);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Upload Bowling");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("AllCricket")]
        public async Task<IActionResult> GetCricketFixtures()
        {
            try
            {
                IEnumerable<CricketFixture> fixtures = await _fixtureRepository.GetCricketFixtures();

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Cricket")]
        public async Task<IActionResult> GetCricketFixture(string id)
        {
            try
            {
                CricketFixture fixture = await _fixtureRepository.GetCricketFixture(id);

                if (fixture != null)
                    return Ok(fixture);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("AllFootball")]
        public async Task<IActionResult> GetFootballFixtures()
        {
            try
            {
                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetFootballFixtures();

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Football")]
        public async Task<IActionResult> GetFootballFixture(string id)
        {
            try
            {
                Fixture fixture = await _fixtureRepository.GetFootballFixture(id);

                if (fixture != null)
                    return Ok(fixture);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("AllBowling")]
        public async Task<IActionResult> GetBowlingFixtures()
        {
            try
            {
                IEnumerable<BowlingFixture> fixtures = await _fixtureRepository.GetBowlingFixtures();

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Bowling")]
        public async Task<IActionResult> GetBowlingFixture(string id)
        {
            try
            {
                BowlingFixture fixture = await _fixtureRepository.GetBowlingFixture(id);

                if (fixture != null)
                    return Ok(fixture);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Sport")]
        public async Task<IActionResult> GetSportFixtures(string Sport)
        {
            try
            {
                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetBySport(Sport);

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("FootballHeadToHead")]
        public async Task<IActionResult> GetFootballHeadToHeadFixtures(string FixtureID)
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetFootballHeadToHead(FixtureID);

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("CricketHeadToHead")]
        public async Task<IActionResult> GetCricketHeadToHeadFixtures(string FixtureID)
        {
            try
            {

                IEnumerable<CricketFixture> fixtures = await _fixtureRepository.GetCricketHeadToHead(FixtureID);

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("BowlingHeadToHead")]
        public async Task<IActionResult> GetBowlingHeadToHeadFixtures(string FixtureID)
        {
            try
            {

                IEnumerable<BowlingFixture> fixtures = await _fixtureRepository.GetBowlingHeadToHead(FixtureID);

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }


        // GET api/values
        [HttpGet]
        [Route("GetHomeFixtures")]
        public async Task<IActionResult> GetHomeFixtures(string FixtureID)
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetHomeTeam(FixtureID);

                if (fixtures != null)
                    return Ok(fixtures);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }


        // GET api/values
        [HttpGet]
        [Route("GetAwayFixtures")]
        public async Task<IActionResult> GetAwayFixtures(string FixtureID)
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetAwayTeam(FixtureID);

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
        [Route("CupFixtures")]
        public async Task<IActionResult> CupFixtures(string LeagueID, string MatchTypeID)
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetCupFixtures(LeagueID, MatchTypeID);

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
        [Route("FootballTeam")]
        public async Task<IActionResult> GetFootballFixturesByTeam(string teamID)
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetFootballTeamFixtures(teamID);

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
        [Route("FootballTeamForm")]
        public async Task<IActionResult> GetFootballFormByTeam(string teamID)
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetFootballTeamForm(teamID);

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
        [Route("BowlingTeam")]
        public async Task<IActionResult> GetBowlingFixturesByTeam(string teamID)
        {
            try
            {

                IEnumerable<BowlingFixture> fixtures = await _fixtureRepository.GetBowlingTeamFixtures(teamID);

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
        [Route("BowlingTeamForm")]
        public async Task<IActionResult> GetBowlingFormByTeam(string teamID)
        {
            try
            {

                IEnumerable<BowlingFixture> fixtures = await _fixtureRepository.GetBowlingTeamForm(teamID);

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
        [Route("CricketTeam")]
        public async Task<IActionResult> GetCricketFixturesByTeam(string teamID)
        {
            try
            {

                IEnumerable<CricketFixture> fixtures = await _fixtureRepository.GetCricketTeamFixtures(teamID);

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
        [Route("CricketTeamForm")]
        public async Task<IActionResult> GetCricketFormByTeam(string teamID)
        {
            try
            {

                IEnumerable<CricketFixture> fixtures = await _fixtureRepository.GetCricketTeamForm(teamID);

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
        [Route("CricketLeague")]
        public async Task<IActionResult> CricketFixturesByLeague(string leagueID)
        {
            try
            {

                IEnumerable<CricketFixture> fixtures = await _fixtureRepository.GetCricketFixturesByLeague(leagueID);

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
        [Route("FootballLeague")]
        public async Task<IActionResult> FixturesByLeague(string leagueID)
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetFootballFixturesByLeague(leagueID);

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
        [Route("BowlingLeague")]
        public async Task<IActionResult> FixturesByBowlingLeague(string leagueID)
        {
            try
            {

                IEnumerable<BowlingFixture> fixtures = await _fixtureRepository.GetBowlingFixturesByLeague(leagueID);

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
        [Route("Live")]
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

        [HttpGet]
        [Route("CricketResults")]
        public async Task<IActionResult> CricketResults()
        {
            try
            {

                IEnumerable<CricketFixture> fixtures = await _fixtureRepository.GetCricketResults();

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
        [Route("PastCricket")]
        public async Task<IActionResult> PastCricketFixtures()
        {
            try
            {

                IEnumerable<CricketFixture> fixtures = await _fixtureRepository.GetPastCricketFixtures();

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
        [Route("UpcomingCricket")]
        public async Task<IActionResult> UpcomingCricketFixtures()
        {
            try
            {

                IEnumerable<CricketFixture> fixtures = await _fixtureRepository.GetUpcomingCricketFixtures();

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
        [Route("LiveCricket")]
        public async Task<IActionResult> LiveCricketFixtures()
        {
            try
            {

                IEnumerable<spLiveCricketFixtures> fixtures = await _fixtureRepository.GetLiveCricket();

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
        [Route("FootballResults")]
        public async Task<IActionResult> FootballResults()
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetFootballResults();

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
        [Route("PastFootball")]
        public async Task<IActionResult> PastFootballFixtures()
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetPastFootballFixtures();

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
        [Route("UpcomingFootball")]
        public async Task<IActionResult> UpcomingFootballFixtures()
        {
            try
            {

                IEnumerable<Fixture> fixtures = await _fixtureRepository.GetUpcomingFootballFixtures();

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
        [Route("LiveFootball")]
        public async Task<IActionResult> LiveFootballFixtures()
        {
            try
            {

                IEnumerable<spLiveFixtures> fixtures = await _fixtureRepository.GetLiveFootball();

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
        [Route("BowlingResults")]
        public async Task<IActionResult> BowlingResults()
        {
            try
            {

                IEnumerable<BowlingFixture> fixtures = await _fixtureRepository.GetBowlingResults();

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
        [Route("PastBowling")]
        public async Task<IActionResult> PastBowlingFixtures()
        {
            try
            {

                IEnumerable<BowlingFixture> fixtures = await _fixtureRepository.GetPastBowlingFixtures();

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
        [Route("UpcomingBowling")]
        public async Task<IActionResult> UpcomingBowlingFixtures()
        {
            try
            {

                IEnumerable<BowlingFixture> fixtures = await _fixtureRepository.GetUpcomingBowlingFixtures();

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
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("InsertFootball")]
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

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("InsertCricket")]
        public async Task<IActionResult> Post(CricketFixture fixture)
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

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("UpdateFootball")]
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

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("UpdateFootballList")]
        public async Task<IActionResult> UpdateFixtures([FromBody] List<Fixture> fixtures)
        {
            try
            {
                await _fixtureRepository.UpdateFixtures(fixtures);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("UpdateCricket")]
        public async Task<IActionResult> Update([FromBody] CricketFixture fixture)
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

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("UpdateCricketList")]
        public async Task<IActionResult> UpdateFixtures([FromBody] List<CricketFixture> fixtures)
        {
            try
            {
                await _fixtureRepository.UpdateFixtures(fixtures);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Fixture");
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
