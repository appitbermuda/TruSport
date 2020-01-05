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
    public class FlyerController : ControllerBase
    {
        private readonly FlyerRepository _flyerRepository;

        public FlyerController(IOnTrackRepository<Flyer> flyerRepository)
        {
            _flyerRepository = (FlyerRepository)flyerRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllFlyers")]
        public async Task<IActionResult> Flyers()
        {
            try
            {

                IEnumerable<Flyer> flyeres = await _flyerRepository.GetAll();

                if (flyeres != null)
                    return Ok(flyeres);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Flyer");
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
                Flyer flyer = await _flyerRepository.Get(id);

                if (flyer != null)
                    return Ok(flyer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Flyer");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Role.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Flyer flyer)
        {
            try
            {
                await _flyerRepository.Insert(flyer);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Flyer");
            }

            return NoContent();
        }

        [Authorize(Roles = Role.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Flyer flyer)
        {
            try
            {
                await _flyerRepository.Update(flyer);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Flyer");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [Authorize(Roles = Role.AllUsers)]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _flyerRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Flyer");
            }

            return NoContent();
        }
    }
}
