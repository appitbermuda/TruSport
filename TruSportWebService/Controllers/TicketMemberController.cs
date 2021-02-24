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
    public class TicketMemberController : ControllerBase
    {
        private readonly TicketMemberRepository _ticketMemberRepository;

        public TicketMemberController(IOnTrackRepository<TicketMember> ticketMemberRepository)
        {
            _ticketMemberRepository = (TicketMemberRepository)ticketMemberRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> TicketMembers()
        {
            try
            {

                IEnumerable<TicketMember> ticketMembers = await _ticketMemberRepository.GetAll(User);

                if (ticketMembers != null)
                    return Ok(ticketMembers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketMember");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("AddMember")]
        public async Task<IActionResult> AddMember(string Email)
        {
            try
            {

                TicketMember ticketMember = await _ticketMemberRepository.AddMember(Email, User);

                if (ticketMember != null)
                    return Ok(ticketMember);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketMember");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("RemoveMember")]
        public async Task<IActionResult> RemoveMember(string Email)
        {
            try
            {

                bool ticketMemberRemoved = await _ticketMemberRepository.RemoveMember(Email, User);

                return Ok(ticketMemberRemoved);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketMember");
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
                TicketMember ticketMember = await _ticketMemberRepository.Get(id);

                if (ticketMember != null)
                    return Ok(ticketMember);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketMember");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] TicketMember ticketMember)
        {
            try
            {
                await _ticketMemberRepository.Insert(ticketMember);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketMember");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] TicketMember ticketMember)
        {
            try
            {
                await _ticketMemberRepository.Update(ticketMember);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketMember");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [Authorize(Roles = Roles.AllUsers)]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _ticketMemberRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketMember");
            }

            return NoContent();
        }
    }
}
