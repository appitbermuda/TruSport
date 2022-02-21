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
using Microsoft.AspNetCore.Http;
using OnTrackWebService.Models.Imports;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamRepository _teamRepository;

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
        [Route("All")]
        public async Task<IActionResult> AllTeams()
        {
            try
            {
                IEnumerable<Team> teams = await _teamRepository.GetAll();

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
        [Route("GetTeamsBySport")]
        public async Task<IActionResult> TeamsBySport(string SportType)
        {
            try
            {
                IEnumerable<TeamSeason> teams = await _teamRepository.GetTeamBySport(SportType);

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
        [Route("AllBasketball")]
        public async Task<IActionResult> GetBasketballTeams()
        {
            try
            {
                IEnumerable<Team> teams = await _teamRepository.GetBasketballTeams();

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
        [Route("AllBasketballTeams")]
        public async Task<IActionResult> GetAllBasketballTeams()
        {
            try
            {
                IEnumerable<Team> teams = await _teamRepository.GetAllBasketballTeams();

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
        [Route("AllCricket")]
        public async Task<IActionResult> GetCricketTeams()
        {
            try
            {
                IEnumerable<Team> teams = await _teamRepository.GetCricketTeams();

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
        [Route("AllCricketTeams")]
        public async Task<IActionResult> GetAllCricketTeams()
        {
            try
            {
                IEnumerable<Team> teams = await _teamRepository.GetAllCricketTeams();

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
        [Route("AllBowling")]
        public async Task<IActionResult> GetBowlingTeams()
        {
            try
            {
                IEnumerable<BowlingTeam> teams = await _teamRepository.GetBowlingTeams();

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
        [Route("AllBowlingTeams")]
        public async Task<IActionResult> GetAllBowlingTeams()
        {
            try
            {
                IEnumerable<BowlingTeam> teams = await _teamRepository.GetAllBowlingTeams();

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
        [Route("Ticketing")]
        public async Task<IActionResult> Ticketing(string SportID)
        {
            try
            {
                IEnumerable<TicketCompany> companys = await _teamRepository.GetTicketingTeams(SportID);

                if (companys != null)
                    return Ok(companys);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("AllFootball")]
        public async Task<IActionResult> GetFootballTeams()
        {
            try
            {
                IEnumerable<Team> teams = await _teamRepository.GetFootballTeams();

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
        [Route("AllFootballTeams")]
        public async Task<IActionResult> GetAllFootballTeams()
        {
            try
            {
                IEnumerable<Team> teams = await _teamRepository.GetAllFootballTeams();

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

        [HttpGet]
        [Route("BowlingLeague")]
        public async Task<IActionResult> TeamsByBowlingLeague(string leagueID)
        {
            try
            {
                IEnumerable<BowlingTeamSeason> teams = await _teamRepository.GetTeamsByBowlingLeague(leagueID);

                if (teams != null)
                    return Ok(teams);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        //// GET api/values/5
        //[HttpGet]
        //[Route("Get")]
        //public async Task<IActionResult> Get(string id)
        //{
        //    try
        //    {
        //        TeamSeason team = await _teamRepository.GetTeam(id);

        //        if (team != null)
        //            return Ok(team);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "Team");
        //    }

        //    return NoContent();
        //}

        // GET api/values/5
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                Team team = await _teamRepository.Get(id);

                if (team != null)
                    return Ok(team);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Bowling")]
        public async Task<IActionResult> GetBowlingTeam(string id)
        {
            try
            {
                BowlingTeamSeason team = await _teamRepository.GetBowlingTeam(id);

                if (team != null)
                    return Ok(team);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Football")]
        public async Task<IActionResult> GetFootballTeam(string id)
        {
            try
            {
                TeamSeason team = await _teamRepository.GetFootballTeam(id);

                if (team != null)
                    return Ok(team);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Basketball")]
        public async Task<IActionResult> GetBasketballTeam(string id)
        {
            try
            {
                TeamSeason team = await _teamRepository.GetBasketballTeam(id);

                if (team != null)
                    return Ok(team);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("BasketballProfile")]
        public async Task<IActionResult> GetBasketballProfile(string id)
        {
            try
            {
                Team team = await _teamRepository.GetBasketballProfile(id);

                if (team != null)
                    return Ok(team);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Cricket")]
        public async Task<IActionResult> GetCricketTeam(string id)
        {
            try
            {
                TeamSeason team = await _teamRepository.GetCricketTeam(id);

                if (team != null)
                    return Ok(team);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("CricketProfile")]
        public async Task<IActionResult> GetCricketProfile(string id)
        {
            try
            {
                Team team = await _teamRepository.GetCricketProfile(id);

                if (team != null)
                    return Ok(team);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("FootballProfile")]
        public async Task<IActionResult> GetFootballProfile(string id)
        {
            try
            {
                Team team = await _teamRepository.GetFootballProfile(id);

                if (team != null)
                    return Ok(team);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("BowlingProfile")]
        public async Task<IActionResult> GetBowlingProfile(string id)
        {
            try
            {
                BowlingTeam team = await _teamRepository.GetBowlingProfile(id);

                if (team != null)
                    return Ok(team);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("Import")]
        public async Task<IActionResult> UploadBasketballFixtures([FromForm(Name = "file")] IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    ImportTeam fileUploadResponse = await _teamRepository.UploadTeams(file);

                    return Ok(fileUploadResponse);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Upload Basketball Team");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("ImportBowling")]
        public async Task<IActionResult> UploadBowlingFixtures([FromForm(Name = "file")] IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    ImportBowlingTeam fileUploadResponse = await _teamRepository.UploadBowlingTeams(file);

                    return Ok(fileUploadResponse);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Upload Bowling Team");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
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

        [Authorize(Roles = Roles.TeamAdmin)]
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

        [Authorize(Roles = Roles.TeamAdmin)]
        [HttpPost]
        [Route("UpdateBowling")]
        public async Task<IActionResult> UpdateBowling([FromBody] BowlingTeam team)
        {
            try
            {
                await _teamRepository.UpdateBowling(team);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
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
                await _teamRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete]
        [Route("DeleteBowling")]
        public async Task<IActionResult> DeleteBowling(string id)
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
