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
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixtureProductController : ControllerBase
    {
        private readonly FixtureProductRepository _fixtureProductRepository;

        public FixtureProductController(IOnTrackRepository<FixtureProduct> fixtureProductRepository)
        {
            _fixtureProductRepository = (FixtureProductRepository)fixtureProductRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> FixtureProducts()
        {
            try
            {

                IEnumerable<FixtureProduct> fixtureProducts = await _fixtureProductRepository.GetAll();

                if (fixtureProducts != null)
                    return Ok(fixtureProducts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
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
                FixtureProduct fixtureProduct = await _fixtureProductRepository.Get(id);

                if (fixtureProduct != null)
                    return Ok(fixtureProduct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Match Ticket");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("TodayFixture")]
        public async Task<IActionResult> TodayFixture(string teamID)
        {
            try
            {

                Fixture fixture = await _fixtureProductRepository.GetTodayFixture(teamID);

                if (fixture != null)
                    return Ok(fixture);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("TodayByTeam")]
        public async Task<IActionResult> FixtureProducts(string teamID)
        {
            try
            {

                IEnumerable<FixtureProduct> fixtureProducts = await _fixtureProductRepository.GetTodayByTeam(teamID);

                if (fixtureProducts != null)
                    return Ok(fixtureProducts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Fixture")]
        public async Task<IActionResult> FixtureFixtureProducts(string fixtureID)
        {
            try
            {

                IEnumerable<FixtureProduct> fixtureProducts = await _fixtureProductRepository.Fixture(fixtureID);

                if (fixtureProducts != null)
                    return Ok(fixtureProducts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Team")]
        public async Task<IActionResult> TeamFixtureProducts(string teamID)
        {
            try
            {

                IEnumerable<FixtureProduct> fixtureProducts = await _fixtureProductRepository.Team(teamID);

                if (fixtureProducts != null)
                    return Ok(fixtureProducts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] FixtureProduct fixtureProduct)
        {
            try
            {
                await _fixtureProductRepository.Insert(fixtureProduct);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] FixtureProduct fixtureProduct)
        {
            try
            {
                await _fixtureProductRepository.Update(fixtureProduct);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [Authorize(Roles = Roles.AllUsers)]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _fixtureProductRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "FixtureProduct");
            }

            return NoContent();
        }
    }
}
