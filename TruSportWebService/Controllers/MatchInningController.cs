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
    public class MatchInningController : ControllerBase
    {
        private readonly MatchInningRepository _matchInningRepository;

        public MatchInningController(IOnTrackRepository<MatchInning> matchInningRepository)
        {
            _matchInningRepository = (MatchInningRepository)matchInningRepository;
        }

        [HttpGet]
        [Route("Fixture")]
        public async Task<IActionResult> MatchInnings(string fixtureID)
        {
            try
            {

                IEnumerable<MatchInning> matchInnings = await _matchInningRepository.GetByFixture(fixtureID);

                if (matchInnings != null)
                    return Ok(matchInnings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchInning");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("New")]
        public async Task<IActionResult> NewInning(string fixtureID)
        {
            try
            {

                bool newInning = await _matchInningRepository.AddNewInning(fixtureID);

                return Ok(newInning);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchInning");
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
                MatchInning matchInning = await _matchInningRepository.Get(id);

                if (matchInning != null)
                    return Ok(matchInning);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchInning");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] MatchInning matchInning)
        {
            try
            {
                await _matchInningRepository.Insert(matchInning);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchInning");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("InsertAll")]
        public async Task<IActionResult> Post([FromBody] List<MatchInning> matchInning)
        {
            try
            {
                await _matchInningRepository.InsertAll(matchInning);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchInning");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] MatchInning matchInning)
        {
            try
            {
                await _matchInningRepository.Update(matchInning);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchInning");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("UpdateAll")]
        public async Task<IActionResult> UpdateAll([FromBody] List<MatchInning> matchInnings)
        {
            try
            {
                await _matchInningRepository.UpdateAll(matchInnings);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchInning");
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
                await _matchInningRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchInning");
            }

            return NoContent();
        }
    }
}
