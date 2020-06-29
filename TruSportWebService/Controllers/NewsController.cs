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
    public class NewsController : ControllerBase
    {
        private readonly NewsRepository _newsRepository;

        public NewsController(INewsRepository<RssFeedItem> newsRepository)
        {
            _newsRepository = (NewsRepository)newsRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("Feed")]
        public async Task<IActionResult> Feed()
        {
            try
            {
                List<RssFeedItem> feed = await _newsRepository.Feed();

                return Ok(feed);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("CricketFeed")]
        public async Task<IActionResult> CricketFeed()
        {
            try
            {
                List<RssFeedItem> feed = await _newsRepository.CricketFeed();

                return Ok(feed);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("FootballFeed")]
        public async Task<IActionResult> FootballFeed()
        {
            try
            {
                List<RssFeedItem> feed = await _newsRepository.FootballFeed();

                return Ok(feed);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "News");
            }

            return NoContent();
        }
    }
}
