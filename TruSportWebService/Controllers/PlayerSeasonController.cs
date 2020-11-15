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

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerSeasonController : ControllerBase
    {
        private readonly PlayerSeasonRepository _playerSeasonRepository;

        public PlayerSeasonController(IOnTrackRepository<PlayerSeason> playerSeasonRepository)
        {
            _playerSeasonRepository = (PlayerSeasonRepository)playerSeasonRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllPlayerSeasons")]
        public async Task<IActionResult> PlayerSeasons()
        {
            try
            {
                IEnumerable<PlayerSeason> playerSeasons = await _playerSeasonRepository.GetAll();

                if (playerSeasons != null)
                    return Ok(playerSeasons);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "PlayerSeason");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("SeasonPlayerSeasons")]
        public async Task<IActionResult> PlayerSeasonsBySeason(int season)
        {
            try
            {
                IEnumerable<PlayerSeason> playerSeasons = await _playerSeasonRepository.GetBySeason(season);

                if (playerSeasons != null)
                    return Ok(playerSeasons);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "PlayerSeason");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("CurrentBowling")]
        public async Task<IActionResult> BowlingPlayerSeason()
        {
            try
            {
                List<BowlingPlayerSeason> playerSeasons = await _playerSeasonRepository.GetCurrentBowlingPlayerSeason();

                if (playerSeasons != null)
                    return Ok(playerSeasons);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "PlayerSeason");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("ImportBowlingPlayers")]
        public async Task<IActionResult> UploadBowlingPlayers([FromForm(Name = "file")] IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    ImportBowlingPlayer fileUploadResponse = await _playerSeasonRepository.UploadBowlingTeams(file);

                    return Ok(fileUploadResponse);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Upload Bowling Players");
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
                PlayerSeason playerSeason = await _playerSeasonRepository.Get(id);

                if (playerSeason != null)
                    return Ok(playerSeason);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "PlayerSeason");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] PlayerSeason playerSeason)
        {
            try
            {
                await _playerSeasonRepository.Insert(playerSeason);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "PlayerSeason");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] PlayerSeason playerSeason)
        {
            try
            {
                await _playerSeasonRepository.Update(playerSeason);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "PlayerSeason");
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
                await _playerSeasonRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "PlayerSeason");
            }

            return NoContent();
        }
    }
}
