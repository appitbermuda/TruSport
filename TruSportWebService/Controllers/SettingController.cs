using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Repository;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly SettingRepository _settingRepository;

        public SettingController(ISettingRepository<Setting> settingRepository)
        {
            _settingRepository = (SettingRepository)settingRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> Settings()
        {
            try
            {

                IEnumerable<Setting> settings = await _settingRepository.All();

                if (settings != null)
                    return Ok(settings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Setting");
            }

            return NoContent();
        }



        // GET api/values
        [HttpGet]
        [Route("HomeMobileFeatureImages")]
        public async Task<IActionResult> SportsMenuImages()
        {
            try
            {

                SportFeature sportFeature = await _settingRepository.HomeMobileFeatureImages();

                if (sportFeature != null)
                    return Ok(sportFeature);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Setting");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("ProcessingFee")]
        public async Task<IActionResult> ProcessingFee()
        {
            try
            {

                decimal? processingFee = await _settingRepository.ProcessingFee();

                if(processingFee.HasValue)
                    return Ok(processingFee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Setting");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Route("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {

                Setting setting = await _settingRepository.Get(id);

                if (setting != null)
                    return Ok(setting);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Setting");
            }

            return NoContent();
        }

        // POST api/values
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Setting setting)
        {
            try
            {

                string id = await _settingRepository.Insert(setting);

                if (id != null)
                    return Ok(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Setting");
            }

            return NoContent();
        }

        // PUT api/values/5
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Setting setting)
        {
            try
            {
                await _settingRepository.Update(setting);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Setting");
            }

            return NoContent();
        }

    }
}
