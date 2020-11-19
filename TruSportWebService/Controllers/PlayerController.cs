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
    public class PlayerController : ControllerBase
    {
        private readonly PlayerRepository _playerRepository;

        public PlayerController(IOnTrackRepository<Player> playerRepository)
        {
            _playerRepository = (PlayerRepository)playerRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllPlayers")]
        public async Task<IActionResult> Players()
        {
            try
            {
                IEnumerable<PlayerSeason> players = await _playerRepository.GetPlayers();

                if (players != null)
                    return Ok(players);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamPlayers")]
        public async Task<IActionResult> PlayersByTeam(string teamID)
        {
            try
            {
                IEnumerable<PlayerSeason> players = await _playerRepository.GetPlayersByTeam(teamID);

                if (players != null)
                    return Ok(players);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
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
                Player player = await _playerRepository.Get(id);

                if (player != null)
                    return Ok(player);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("GetPlayer")]
        public async Task<IActionResult> GetPlayer(string id)
        {
            try
            {
                PlayerSeason player = await _playerRepository.GetPlayer(id);

                if (player != null)
                    return Ok(player);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("CricketPlayers")]
        public async Task<IActionResult> CricketPlayers()
        {
            try
            {
                IEnumerable<CricketPlayerSeason> players = await _playerRepository.GetCricketPlayers();

                if (players != null)
                    return Ok(players);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("CricketTeamPlayers")]
        public async Task<IActionResult> CricketPlayersByTeam(string teamID)
        {
            try
            {
                IEnumerable<CricketPlayerSeason> players = await _playerRepository.GetCricketPlayersByTeam(teamID);

                if (players != null)
                    return Ok(players);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GetCricketPlayer")]
        public async Task<IActionResult> GetCricketPlayer(string id)
        {
            try
            {
                CricketPlayerSeason player = await _playerRepository.GetCricketPlayer(id);

                if (player != null)
                    return Ok(player);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("BowlingPlayers")]
        public async Task<IActionResult> BowlingPlayers()
        {
            try
            {
                IEnumerable<BowlingPlayerSeason> players = await _playerRepository.GetBowlingPlayers();

                if (players != null)
                    return Ok(players);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("BowlingTeamPlayers")]
        public async Task<IActionResult> BowlingPlayersByTeam(string teamID)
        {
            try
            {
                IEnumerable<BowlingPlayerSeason> players = await _playerRepository.GetBowlingPlayersByTeam(teamID);

                if (players != null)
                    return Ok(players);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GetBowlingPlayer")]
        public async Task<IActionResult> GetBowlingPlayer(string id)
        {
            try
            {
                BowlingPlayerSeason player = await _playerRepository.GetBowlingPlayer(id);

                if (player != null)
                    return Ok(player);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Player player)
        {
            try
            {
                await _playerRepository.Insert(player);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] PlayerSeason player)
        {
            try
            {
                await _playerRepository.Update(player);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("RemovePlayer")]
        public async Task<IActionResult> RemovePlayer([FromBody] PlayerSeason player)
        {
            try
            {
                await _playerRepository.RemovePlayer(player);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
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
                await _playerRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Player");
            }

            return NoContent();
        }
    }
}
