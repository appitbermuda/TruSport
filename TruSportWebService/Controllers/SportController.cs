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
    public class SportController : ControllerBase
    {
        private readonly SportRepository _SportRepository;

        public SportController(IOnTrackRepository<Sport> SportRepository)
        {
            _SportRepository = (SportRepository)SportRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllSports")]
        public async Task<IActionResult> Sports()
        {
            try
            {

                IEnumerable<Sport> Sports = await _SportRepository.GetAll();

                if (Sports != null)
                    return Ok(Sports);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamSports")]
        public async Task<IActionResult> SportsByTeam(string teamID)
        {
            try
            {

                IEnumerable<Sport> Sports = await _SportRepository.GetByTeam(teamID);

                if (Sports != null)
                    return Ok(Sports);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport");
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
                Sport Sport = await _SportRepository.Get(id);

                if (Sport != null)
                    return Ok(Sport);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Sport Sport)
        {
            try
            {
                await _SportRepository.Insert(Sport);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Sport Sport)
        {
            try
            {
                await _SportRepository.Update(Sport);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport");
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
                await _SportRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport");
            }

            return NoContent();
        }
    }
}
