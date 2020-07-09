using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Repository;
using OnTrackWebService.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using OnTrackWebService.Data;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueStatController : ControllerBase
    {
        private readonly LeagueStatRepository _leagueStatRepository;

        public LeagueStatController(IOnTrackRepository<LeagueStat> leagueStatRepository)
        {
            _leagueStatRepository = (LeagueStatRepository)leagueStatRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllLeagueStats")]
        public async Task<IActionResult> LeagueStats()
        {
            try
            {

                IEnumerable<LeagueStat> leagueStats = await _leagueStatRepository.GetAll();

                if (leagueStats != null)
                    return Ok(leagueStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GoalsConcededByTeam")]
        public async Task<IActionResult> GoalsConcededByTeam()
        {
            try
            {

                IEnumerable<GoalsConcededByTeam> leagueStats = await _leagueStatRepository.GetGoalsConcededByTeam();

                if (leagueStats != null)
                    return Ok(leagueStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GoalsScoredByTeam")]
        public async Task<IActionResult> GoalsScoredByTeam()
        {
            try
            {

                IEnumerable<GoalsScoredByTeam> leagueStats = await _leagueStatRepository.GetGoalsScoredByTeam();

                if (leagueStats != null)
                    return Ok(leagueStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GoalsScoredByPlayer")]
        public async Task<IActionResult> GoalsScoredByPlayer()
        {
            try
            {
                IEnumerable<GoalsScoredByPlayer> leagueStats = await _leagueStatRepository.GetGoalsScoredByPlayer();

                if (leagueStats != null)
                    return Ok(leagueStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("RunsByPlayer")]
        public async Task<IActionResult> GetRunsByPlayer()
        {
            try
            {
                IEnumerable<RunsByPlayer> leagueStats = await _leagueStatRepository.GetRunsByPlayer();

                if (leagueStats != null)
                    return Ok(leagueStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("WicketsByPlayer")]
        public async Task<IActionResult> GetWicketsByPlayer()
        {
            try
            {
                IEnumerable<WicketsByPlayer> leagueStats = await _leagueStatRepository.GetWicketsByPlayer();

                if (leagueStats != null)
                    return Ok(leagueStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GoalsScoredByPlayerByTeam")]
        public async Task<IActionResult> GoalsScoredByPlayerByTeam(string teamID)
        {
            try
            {

                IEnumerable<GoalsScoredByPlayer> leagueStats = await _leagueStatRepository.GetGoalsScoredByPlayerByTeam(teamID);

                if (leagueStats != null)
                    return Ok(leagueStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GoalsConcededByTeamByLeague")]
        public async Task<IActionResult> GoalsConcededByTeamByLeague(string leagueID)
        {
            try
            {
                IEnumerable<GoalsConcededByTeam> leagueStats = await _leagueStatRepository.GetGoalsConcededByTeamByLeague(leagueID);

                if (leagueStats != null)
                    return Ok(leagueStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GoalsScoredByTeamByLeague")]
        public async Task<IActionResult> GoalsScoredByTeamByLeague(string leagueID)
        {
            try
            {
                IEnumerable<GoalsScoredByTeam> leagueStats = await _leagueStatRepository.GetGoalsScoredByTeamByLeague(leagueID);

                if (leagueStats != null)
                    return Ok(leagueStats);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
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
                LeagueStat leagueStat = await _leagueStatRepository.Get(id);

                if (leagueStat != null)
                    return Ok(leagueStat);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueStat");
            }

            return NoContent();
        }

        //// POST api/values
        //[Authorize(Roles = Roles.Admin)]
        //[HttpPost]
        //[Route("Insert")]
        //public async Task<IActionResult> Post([FromBody] LeagueStat leagueStat)
        //{
        //    try
        //    {
        //        await _leagueStatRepository.Insert(leagueStat);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "LeagueStat");
        //    }

        //    return NoContent();
        //}

        //[Authorize(Roles = Roles.Admin)]
        //[HttpPost]
        //[Route("Update")]
        //public async Task<IActionResult> Update([FromBody] LeagueStat leagueStat)
        //{
        //    try
        //    {
        //        await _leagueStatRepository.Update(leagueStat);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "LeagueStat");
        //    }

        //    return NoContent();
        //}

        //// DELETE api/values/5
        //[Authorize(Roles = Roles.Admin)]
        //[HttpDelete]
        //[Route("Delete")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    try
        //    {
        //        await _leagueStatRepository.Delete(id);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "LeagueStat");
        //    }

        //    return NoContent();
        //}
    }
}
