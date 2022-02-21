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
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly WalletRepository _walletRepository;

        public WalletController(IOnTrackRepository<Wallet> adRepository)
        {
            _walletRepository = (WalletRepository)adRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("List")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> All()
        {
            try
            {
                IEnumerable<Wallet> wallets = await _walletRepository.GetAll();

                if (wallets != null)
                    return Ok(wallets);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> Wallets()
        {
            try
            {

                IEnumerable<Wallet> cards = await _walletRepository.GetWallet(User);

                if (cards != null)
                    return Ok(cards);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Get")]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                Wallet card = await _walletRepository.Get(User, id);

                if (card != null)
                    return Ok(card);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Customer)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Wallet card)
        {
            try
            {
                bool inserted = await _walletRepository.Insert(card);
                return Ok(inserted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Customer)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Wallet card)
        {
            try
            {
                bool updated = await _walletRepository.Update(card);

                return Ok(updated);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
            }

            return NoContent();
        }

        // DELETE api/values/5
        [Authorize(Roles = Roles.Customer)]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                bool deleted = await _walletRepository.Delete(id);

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Wallet");
            }

            return NoContent();
        }
    }
}
