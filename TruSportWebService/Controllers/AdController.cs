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
    }
}
