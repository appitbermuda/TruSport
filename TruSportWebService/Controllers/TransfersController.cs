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
    public class TransferController : ControllerBase
    {
        private readonly TransferRepository _transferRepository;

        public TransferController(IOnTrackRepository<Transfer> transferRepository)
        {
            _transferRepository = (TransferRepository)transferRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllTransfers")]
        public async Task<IActionResult> Transfer()
        {
            try
            {

                IEnumerable<Transfer> transfers = await _transferRepository.GetAll();

                if (transfers != null)
                    return Ok(transfers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Basketball")]
        public async Task<IActionResult> BasketballTransfer()
        {
            try
            {

                IEnumerable<Transfer> transfers = await _transferRepository.Basketball();

                if (transfers != null)
                    return Ok(transfers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Cricket")]
        public async Task<IActionResult> CricketTransfer()
        {
            try
            {

                IEnumerable<Transfer> transfers = await _transferRepository.Cricket();

                if (transfers != null)
                    return Ok(transfers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Football")]
        public async Task<IActionResult> FootballTransfer()
        {
            try
            {

                IEnumerable<Transfer> transfers = await _transferRepository.Football();

                if (transfers != null)
                    return Ok(transfers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamTransfers")]
        public async Task<IActionResult> TransfersByTeam(string teamID)
        {
            try
            {
                //League league = _context.Leagues.FirstOrDefaultAsync(e => e.ID == leagueID)
                IEnumerable<Transfer> transfers = await _transferRepository.GetByTeam(teamID);

                if (transfers != null)
                    return Ok(transfers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TransferByLeague")]
        public async Task<IActionResult> TransfersByLeague(string leagueID)
        {
            try
            {

                IEnumerable<Transfer> transfers = await _transferRepository.GetByLeague(leagueID);

                if (transfers != null)
                    return Ok(transfers);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
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
                Transfer transfer = await _transferRepository.Get(id);

                if (transfer != null)
                    return Ok(transfer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }

            return NoContent();
        }

        // POST api/values
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Transfer transfer)
        {
            try
            {
                await _transferRepository.Insert(transfer);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }

            return NoContent();
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Transfer transfer)
        {
            try
            {
                await _transferRepository.Update(transfer);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
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
                await _transferRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Transfer");
            }

            return NoContent();
        }
    }
}
