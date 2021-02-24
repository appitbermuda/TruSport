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
using OnTrackWebService.Models.Ad;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpressionController : ControllerBase
    {
        private readonly ImpressionRepository _impressionRepository;

        public ImpressionController(IOnTrackRepository<Impression> impressionRepository)
        {
            _impressionRepository = (ImpressionRepository)impressionRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> Impressions()
        {
            try
            {

                IEnumerable<Impression> impressions = await _impressionRepository.GetAll();

                if (impressions != null)
                    return Ok(impressions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Impression");
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
                Impression impression = await _impressionRepository.Get(id);

                if (impression != null)
                    return Ok(impression);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Impression");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Impression impression)
        {
            try
            {
                await _impressionRepository.Insert(impression);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Impression");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.TeamAdmin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Impression impression)
        {
            try
            {
                await _impressionRepository.Update(impression);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Impression");
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
                await _impressionRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Impression");
            }

            return NoContent();
        }
    }
}
