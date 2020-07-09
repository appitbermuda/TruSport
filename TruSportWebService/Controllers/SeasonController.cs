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
    public class SeasonController : ControllerBase
    {
        private readonly SeasonRepository _SeasonRepository;

        public SeasonController(IOnTrackRepository<Season> SeasonRepository)
        {
            _SeasonRepository = (SeasonRepository)SeasonRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllSeasons")]
        public async Task<IActionResult> Seasons()
        {
            try
            {

                IEnumerable<Season> Seasons = await _SeasonRepository.GetAll();

                if (Seasons != null)
                    return Ok(Seasons);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Season");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("Cricket")]
        public async Task<IActionResult> CricketSeasons()
        {
            try
            {

                IEnumerable<Season> Seasons = await _SeasonRepository.GetCricketSeason();

                if (Seasons != null)
                    return Ok(Seasons);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Season");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("Football")]
        public async Task<IActionResult> FootballSeasons()
        {
            try
            {

                IEnumerable<Season> Seasons = await _SeasonRepository.GetFootballSeason();

                if (Seasons != null)
                    return Ok(Seasons);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Season");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamSeasons")]
        public async Task<IActionResult> SeasonsByTeam(string teamID)
        {
            try
            {

                IEnumerable<Season> Seasons = await _SeasonRepository.GetByTeam(teamID);

                if (Seasons != null)
                    return Ok(Seasons);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Season");
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
                Season Season = await _SeasonRepository.Get(id);

                if (Season != null)
                    return Ok(Season);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Season");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Season Season)
        {
            try
            {
                await _SeasonRepository.Insert(Season);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Season");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Season Season)
        {
            try
            {
                await _SeasonRepository.Update(Season);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Season");
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
                await _SeasonRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Season");
            }

            return NoContent();
        }
    }
}
