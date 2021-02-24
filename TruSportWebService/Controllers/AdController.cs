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
    public class AdController : ControllerBase
    {
        private readonly AdRepository _adRepository;

        public AdController(IOnTrackRepository<Ad> adRepository)
        {
            _adRepository = (AdRepository)adRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> All()
        {
            try
            {

                IEnumerable<Ad> ads = await _adRepository.GetAll();

                if (ads != null)
                    return Ok(ads);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ad");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Retrieve")]
        public async Task<IActionResult> Ads()
        {
            try
            {

                IEnumerable<Ad> ads = await _adRepository.GetAll();

                if (ads != null)
                    return Ok(ads);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ad");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Impression")]
        public async Task<IActionResult> Impression(string id)
        {
            try
            {
                await _adRepository.Impression(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ad");
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
                Ad ad = await _adRepository.Get(id);

                if (ad != null)
                    return Ok(ad);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ad");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Ad ad)
        {
            try
            {
                bool inserted = await _adRepository.Insert(ad);
                return Ok(inserted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ad");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.TeamAdmin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Ad ad)
        {
            try
            {
                bool updated = await _adRepository.Update(ad);

                return Ok(updated);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ad");
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
                bool deleted = await _adRepository.Delete(id);

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ad");
            }

            return NoContent();
        }
    }
}
