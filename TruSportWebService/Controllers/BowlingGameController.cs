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
    public class BowlingGameController : ControllerBase
    {
        private readonly BowlingGameRepository _bowlingGameRepository;

        public BowlingGameController(IOnTrackRepository<BowlingGame> bowlingGameRepository)
        {
            _bowlingGameRepository = (BowlingGameRepository)bowlingGameRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllBowlingGames")]
        public async Task<IActionResult> BowlingGames()
        {
            try
            {

                IEnumerable<BowlingGame> bowlingGamees = await _bowlingGameRepository.GetAll();

                if (bowlingGamees != null)
                    return Ok(bowlingGamees);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingGame");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("FixtureBowlingGame")]
        public async Task<IActionResult> BowlingGameByFixture(string fixtureID)
        {
            try
            {

                BowlingGame bowlingGame = await _bowlingGameRepository.GetByFixture(fixtureID);

                if (bowlingGame != null)
                    return Ok(bowlingGame);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingGame");
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
                BowlingGame bowlingGame = await _bowlingGameRepository.Get(id);

                if (bowlingGame != null)
                    return Ok(bowlingGame);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingGame");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("Import")]
        public async Task<IActionResult> UploadBowlingGames([FromForm(Name = "file")] IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    ImportBowlingGames fileUploadResponse = await _bowlingGameRepository.UploadBowlingGames(file);

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
        public async Task<IActionResult> Post([FromBody] BowlingGame bowlingGame)
        {
            try
            {
                await _bowlingGameRepository.Insert(bowlingGame);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingGame");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] BowlingGame bowlingGame)
        {
            try
            {
                await _bowlingGameRepository.Update(bowlingGame);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingGame");
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
                await _bowlingGameRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingGame");
            }

            return NoContent();
        }
    }
}
