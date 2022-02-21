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
    public class AwardController : ControllerBase
    {
        private readonly AwardRepository _playerAwardRepository;

        public AwardController(IOnTrackRepository<Award> playerAwardRepository)
        {
            _playerAwardRepository = (AwardRepository)playerAwardRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> Awards()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetAll();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("SportAwards")]
        public async Task<IActionResult> AwardsBySport(string SportID)
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetBySport(SportID);

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("Basketball")]
        public async Task<IActionResult> BasketballAwards()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetBasketballAwards();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("Cricket")]
        public async Task<IActionResult> CricketAwards()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetCricketAwards();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }



        [HttpGet]
        [Route("Football")]
        public async Task<IActionResult> FootballAwards()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetFootballAwards();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("BasketballPlayerOfTheWeek")]
        public async Task<IActionResult> BasketballPlayerOfTheWeek()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetBasketballPlayerOfTheWeek();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("CricketPlayerOfTheWeek")]
        public async Task<IActionResult> CricketPlayerOfTheWeek()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetCricketPlayerOfTheWeek();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("FootballPlayerOfTheWeek")]
        public async Task<IActionResult> FootballPlayerOfTheWeek()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetFootballPlayerOfTheWeek();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("BasketballPlayerOfTheMonth")]
        public async Task<IActionResult> BasketballPlayerOfTheMonth()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetBasketballPlayerOfTheMonth();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("CricketPlayerOfTheMonth")]
        public async Task<IActionResult> CricketPlayerOfTheMonth()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetCricketPlayerOfTheMonth();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("FootballPlayerOfTheMonth")]
        public async Task<IActionResult> FootballPlayerOfTheMonth()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetFootballPlayerOfTheMonth();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("BasketballPlayerOfTheYear")]
        public async Task<IActionResult> BasketballPlayerOfTheYear()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetBasketballPlayerOfTheYear();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("CricketPlayerOfTheYear")]
        public async Task<IActionResult> CricketPlayerOfTheYear()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetCricketPlayerOfTheYear();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("FootballPlayerOfTheYear")]
        public async Task<IActionResult> FootballPlayerOfTheYear()
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetFootballPlayerOfTheYear();

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GetBySport")]
        public async Task<IActionResult> GetBySport(string SportType)
        {
            try
            {
                IEnumerable<Award> playerAwards = await _playerAwardRepository.GetBySportType(SportType);

                if (playerAwards != null)
                    return Ok(playerAwards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
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
                Award playerAward = await _playerAwardRepository.Get(id);

                if (playerAward != null)
                    return Ok(playerAward);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("BasketballPlayer")]
        public async Task<IActionResult> GetBasketballPlayer(string playerID)
        {
            try
            {
                IEnumerable<Award> playerAward = await _playerAwardRepository.GetBasketballPlayer(playerID);

                if (playerAward != null)
                    return Ok(playerAward);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("CricketPlayer")]
        public async Task<IActionResult> GetCricketPlayer(string playerID)
        {
            try
            {
                IEnumerable<Award> playerAward = await _playerAwardRepository.GetCricketPlayer(playerID);

                if (playerAward != null)
                    return Ok(playerAward);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("FootballPlayer")]
        public async Task<IActionResult> GetFootballPlayer(string playerID)
        {
            try
            {
                IEnumerable<Award> playerAward = await _playerAwardRepository.GetFootballPlayer(playerID);

                if (playerAward != null)
                    return Ok(playerAward);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        // POST api/values

        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Award playerAward)
        {
            try
            {
                await _playerAwardRepository.Insert(playerAward);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Award playerAward)
        {
            try
            {
                await _playerAwardRepository.Update(playerAward);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
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
                await _playerAwardRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Award");
            }

            return NoContent();
        }
    }
}
