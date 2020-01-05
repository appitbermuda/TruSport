using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Repository;
using OnTrackWebService.Models;
using System.Diagnostics;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueTableController : ControllerBase
    {
        private readonly LeagueTableRepository _leagueTableRepository;

        public LeagueTableController(IOnTrackRepository<LTable> leagueTableRepository)
        {
            _leagueTableRepository = (LeagueTableRepository)leagueTableRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllLeagueTables")]
        public async Task<IActionResult> LeagueTables()
        {
            try
            {

                IEnumerable<LeagueTable> leagueTables = await _leagueTableRepository.GetAll();

                if (leagueTables != null)
                    return Ok(leagueTables);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("PremierLeagueTable")]
        public async Task<IActionResult> PremierLeagueTable()
        {
            try
            {

                IEnumerable<PremierLeagueTable> leagueTables = await _leagueTableRepository.GetPremierLeagueTable();

                if (leagueTables != null)
                    return Ok(leagueTables);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("FirstDivisionTable")]
        public async Task<IActionResult> FirstDivisionTable()
        {
            try
            {

                IEnumerable<FirstDivisionTable> leagueTables = await _leagueTableRepository.GetFirstDivisionTable();

                if (leagueTables != null)
                    return Ok(leagueTables);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("CoronaLeagueTable")]
        public async Task<IActionResult> CoronaLeagueTable()
        {
            try
            {
                IEnumerable<CoronaLeagueTable> leagueTables = await _leagueTableRepository.GetCoronaLeagueTable();

                if (leagueTables != null)
                    return Ok(leagueTables);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("PremierLeagueTableByTeam")]
        public async Task<IActionResult> PremierLeagueTableByTeam(string teamID)
        {
            try
            {

                PremierLeagueTable leagueTables = await _leagueTableRepository.GetPremierLeagueTable(teamID);

                if (leagueTables != null)
                    return Ok(leagueTables);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("FirstDivisionTableByTeam")]
        public async Task<IActionResult> FirstDivisionTableByTeam(string teamID)
        {
            try
            {

                FirstDivisionTable leagueTables = await _leagueTableRepository.GetFirstDivisionTable(teamID);

                if (leagueTables != null)
                    return Ok(leagueTables);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamTable")]
        public async Task<IActionResult> LeagueTablesByTeam(string teamID, string leagueID)
        {
            try
            {
                //League league = _context.Leagues.FirstOrDefaultAsync(e => e.ID == leagueID)
                IEnumerable<LeagueTable> leagueTables = await _leagueTableRepository.GetByTeam(teamID, leagueID);

                if (leagueTables != null)
                    return Ok(leagueTables);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("LeagueTableByLeague")]
        public async Task<IActionResult> LeagueTablesByLeague(string leagueID)
        {
            try
            {

                IEnumerable<LeagueTable> leagueTables = await _leagueTableRepository.GetByLeague(leagueID);

                if (leagueTables != null)
                    return Ok(leagueTables);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
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
                LeagueTable leagueTable = await _leagueTableRepository.Get(id);

                if (leagueTable != null)
                    return Ok(leagueTable);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }

        // POST api/values
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] LeagueTable leagueTable)
        {
            try
            {
                await _leagueTableRepository.Insert(leagueTable);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] LeagueTable leagueTable)
        {
            try
            {
                await _leagueTableRepository.Update(leagueTable);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _leagueTableRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "LeagueTable");
            }

            return NoContent();
        }
    }
}
