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
    public class CoachController : ControllerBase
    {
        private readonly CoachRepository _coachRepository;

        public CoachController(IOnTrackRepository<Coach> coachRepository)
        {
            _coachRepository = (CoachRepository)coachRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllCoaches")]
        public async Task<IActionResult> Coaches()
        {
            try
            {

                IEnumerable<Coach> coaches = await _coachRepository.GetAll();

                if (coaches != null)
                    return Ok(coaches);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Coach");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamCoaches")]
        public async Task<IActionResult> CoachesByTeam(string teamID)
        {
            try
            {

                IEnumerable<Coach> coaches = await _coachRepository.GetByTeam(teamID);

                if (coaches != null)
                    return Ok(coaches);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Coach");
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
                Coach coach = await _coachRepository.Get(id);

                if (coach != null)
                    return Ok(coach);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Coach");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Coach coach)
        {
            try
            {
                await _coachRepository.Insert(coach);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Coach");
            }

            return NoContent();
        }

        [Authorize(Roles = Role.TeamAdmin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Coach coach)
        {
            try
            {
                await _coachRepository.Update(coach);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Coach");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [Authorize(Roles = Role.Admin)]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _coachRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Coach");
            }

            return NoContent();
        }
    }
}
