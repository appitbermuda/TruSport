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
    public class RankingController : ControllerBase
    {
        private readonly RankingRepository _rankingRepository;

        public RankingController(IOnTrackRepository<TennisRanking> rankingRepository)
        {
            _rankingRepository = (RankingRepository)rankingRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> Rankings()
        {
            try
            {
                IEnumerable<TennisRanking> rankings = await _rankingRepository.GetAll();

                if (rankings != null)
                    return Ok(rankings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ranking");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("MensCurrentTennisRanking")]
        public async Task<IActionResult> MensCurrentTennisRanking()
        {
            try
            {
                IEnumerable<TennisRanking> rankings = await _rankingRepository.GetMensCurrentTennisRanking();

                if (rankings != null)
                    return Ok(rankings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ranking");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("WomensCurrentTennisRanking")]
        public async Task<IActionResult> WomensCurrentTennisRanking()
        {
            try
            {
                IEnumerable<TennisRanking> rankings = await _rankingRepository.GetWomensCurrentTennisRanking();

                if (rankings != null)
                    return Ok(rankings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ranking");
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
                TennisRanking ranking = await _rankingRepository.Get(id);

                if (ranking != null)
                    return Ok(ranking);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Ranking");
            }

            return NoContent();
        }
    }
}
