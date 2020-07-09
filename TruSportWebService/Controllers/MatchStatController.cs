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
    public class MatchStatController : ControllerBase
    {
        private readonly MatchStatRepository _matchStatRepository;

        public MatchStatController(IOnTrackRepository<MatchStat> matchStatRepository)
        {
            _matchStatRepository = (MatchStatRepository)matchStatRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllMatchStats")]
        public async Task<IActionResult> MatchStats()
        {
            try
            {

                IEnumerable<MatchStat> matchStats = await _matchStatRepository.GetAll();

                if (matchStats != null)
                    return Ok(matchStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchStat");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("RosterMatchStats")]
        public async Task<IActionResult> MatchStatsByRosterID(string matchRosterID)
        {
            try
            {

                IEnumerable<MatchStat> matchStats = await _matchStatRepository.GetByMatchRosterID(matchRosterID);

                if (matchStats != null)
                    return Ok(matchStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchStat");
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
                MatchStat matchStat = await _matchStatRepository.Get(id);

                if (matchStat != null)
                    return Ok(matchStat);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchStat");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] MatchStat matchStat)
        {
            try
            {
                await _matchStatRepository.Insert(matchStat);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchStat");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("InsertAll")]
        public async Task<IActionResult> Post([FromBody] List<MatchStat> matchStat)
        {
            try
            {
                await _matchStatRepository.InsertAll(matchStat);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchStat");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] MatchStat matchStat)
        {
            try
            {
                await _matchStatRepository.Update(matchStat);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchStat");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("UpdateAll")]
        public async Task<IActionResult> UpdateAll([FromBody] List<MatchStat> matchStats)
        {
            try
            {
                await _matchStatRepository.UpdateAll(matchStats);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchStat");
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
                await _matchStatRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchStat");
            }

            return NoContent();
        }
    }
}
