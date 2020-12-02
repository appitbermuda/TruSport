using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}
