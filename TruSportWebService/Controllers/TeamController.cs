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
    public class TeamController : ControllerBase
    {
        private readonly TeamRepository _teamRepository;
        //private readonly IOnTrackRepository<Team> _teamRepository;

        public TeamController(IOnTrackRepository<Team> teamRepository)
        {
            _teamRepository = (TeamRepository)teamRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllTeams")]
        public async Task<IActionResult> Teams()
        {
            try
            {
                IEnumerable<TeamSeason> teams = await _teamRepository.GetTeams();

                if (teams != null)
                    return Ok(teams);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("AllTeamsBySeason")]
        public async Task<IActionResult> Teams(int Season)
        {
            try
            {
                IEnumerable<TeamSeason> teams = await _teamRepository.GetTeamsBySeason(Season);

                if (teams != null)
                    return Ok(teams);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("LeagueTeams")]
        public async Task<IActionResult> TeamsByLeague(string leagueID)
        {
            try
            {
                IEnumerable<TeamSeason> teams = await _teamRepository.GetTeamsByLeague(leagueID);

                if (teams != null)
                    return Ok(teams);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
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
                TeamSeason team = await _teamRepository.GetTeam(id);

                if (team != null)
                    return Ok(team);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Team team)
        {
            try
            {
                await _teamRepository.Insert(team);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        [Authorize(Roles = Role.TeamAdmin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Team team)
        {
            try
            {
                await _teamRepository.Update(team);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
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
                await _teamRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }
    }
}
