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
    public class MatchTypeController : ControllerBase
    {
        private readonly MatchTypeRepository _matchTypeRepository;

        public MatchTypeController(IOnTrackRepository<MatchType> matchTypeRepository)
        {
            _matchTypeRepository = (MatchTypeRepository)matchTypeRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllMatchTypes")]
        public async Task<IActionResult> MatchTypes()
        {
            try
            {

                IEnumerable<MatchType> matchTypes = await _matchTypeRepository.GetAll();

                if (matchTypes != null)
                    return Ok(matchTypes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchType");
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
                MatchType matchType = await _matchTypeRepository.Get(id);

                if (matchType != null)
                    return Ok(matchType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchType");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] MatchType matchType)
        {
            try
            {
                await _matchTypeRepository.Insert(matchType);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchType");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] MatchType matchType)
        {
            try
            {
                await _matchTypeRepository.Update(matchType);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchType");
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
                await _matchTypeRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "MatchType");
            }

            return NoContent();
        }
    }
}
