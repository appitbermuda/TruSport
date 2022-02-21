using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;
using OnTrackWebService.Repository;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCompanyController : ControllerBase
    {
        private readonly TicketCompanyRepository _ticketCompanyRepository;

        public TicketCompanyController(IOnTrackRepository<TicketCompany> ticketCompanyRepository)
        {
            _ticketCompanyRepository = (TicketCompanyRepository)ticketCompanyRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> TicketCompanys()
        {
            try
            {

                IEnumerable<TicketCompany> ticketCompanys = await _ticketCompanyRepository.GetAll();

                if (ticketCompanys != null)
                    return Ok(ticketCompanys);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketCompany");
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
                TicketCompany ticketCompany = await _ticketCompanyRepository.Get(id);

                if (ticketCompany != null)
                    return Ok(ticketCompany);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketCompany");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.TicketScanner)]
        [HttpGet]
        [Route("ActiveScanner")]
        public async Task<IActionResult> ActiveScanner()
        {
            try
            {

                bool scannerActive = await _ticketCompanyRepository.ActiveScanner(User);

                return Ok(scannerActive);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Users");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.TicketOwner)]
        [HttpGet]
        [Route("TicketScanners")]
        public async Task<IActionResult> TicketScanners()
        {
            try
            {

                IEnumerable<TicketScanner> users = await _ticketCompanyRepository.GetScanners(User);

                if (users != null)
                    return Ok(users);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Users");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.TicketOwner)]
        [HttpPost]
        [Route("UpdateTicketScanners")]
        public async Task<IActionResult> Update(List<TicketScanner> scanners)
        {
            try
            {
                bool updated = await _ticketCompanyRepository.UpdateScanners(scanners, User);

                return Ok(updated);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Setup")]
        public async Task<IActionResult> Setup([FromBody] RegisterTicketCompany ticketCompany)
        {
            try
            {
                await _ticketCompanyRepository.Setup(ticketCompany);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketCompany");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] TicketCompany ticketCompany)
        {
            try
            {
                await _ticketCompanyRepository.Insert(ticketCompany);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketCompany");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] TicketCompany ticketCompany)
        {
            try
            {
                await _ticketCompanyRepository.Update(ticketCompany);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketCompany");
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
                await _ticketCompanyRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketCompany");
            }

            return NoContent();
        }
    }
}
