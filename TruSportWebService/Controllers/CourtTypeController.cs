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
    public class CourtTypeController : ControllerBase
    {
        private readonly CourtTypeRepository _courtTypeRepository;

        public CourtTypeController(IOnTrackRepository<CourtType> courtTypeRepository)
        {
            _courtTypeRepository = (CourtTypeRepository)courtTypeRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllCourtTypes")]
        public async Task<IActionResult> CourtTypes()
        {
            try
            {

                IEnumerable<CourtType> courtTypes = await _courtTypeRepository.GetAll();

                if (courtTypes != null)
                    return Ok(courtTypes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CourtType");
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
                CourtType courtType = await _courtTypeRepository.Get(id);

                if (courtType != null)
                    return Ok(courtType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CourtType");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] CourtType courtType)
        {
            try
            {
                await _courtTypeRepository.Insert(courtType);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CourtType");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] CourtType courtType)
        {
            try
            {
                await _courtTypeRepository.Update(courtType);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CourtType");
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
                await _courtTypeRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "CourtType");
            }

            return NoContent();
        }
    }
}
