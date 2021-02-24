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
    public class TicketConfigurationController : ControllerBase
    {
        private readonly TicketConfigurationRepository _ticketConfigurationRepository;

        public TicketConfigurationController(IOnTrackRepository<TicketConfiguration> ticketConfigurationRepository)
        {
            _ticketConfigurationRepository = (TicketConfigurationRepository)ticketConfigurationRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> TicketConfigurations()
        {
            try
            {

                IEnumerable<TicketConfiguration> ticketConfigurations = await _ticketConfigurationRepository.All();

                if (ticketConfigurations != null)
                    return Ok(ticketConfigurations);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketConfiguration");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Team")]
        public async Task<IActionResult> TeamConfiguration()
        {
            try
            {

                TicketConfiguration ticketConfiguration = await _ticketConfigurationRepository.TeamConfiguration(User);

                if(ticketConfiguration != null)
                    return Ok(ticketConfiguration);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ticket Configuration");
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

                TicketConfiguration ticketConfiguration = await _ticketConfigurationRepository.Get(id);

                if (ticketConfiguration != null)
                    return Ok(ticketConfiguration);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketConfiguration");
            }

            return NoContent();
        }

        // POST api/values
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] TicketConfiguration ticketConfiguration)
        {
            try
            {
                await _ticketConfigurationRepository.Insert(ticketConfiguration);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketConfiguration");
            }

            return NoContent();
        }

        // PUT api/values/5
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] TicketConfiguration ticketConfiguration)
        {
            try
            {
                await _ticketConfigurationRepository.Update(ticketConfiguration);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketConfiguration");
            }

            return NoContent();
        }

    }
}
