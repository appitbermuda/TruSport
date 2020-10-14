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
    public class InventoryController : ControllerBase
    {
        private readonly InventoryRepository _inventoryRepository;

        public InventoryController(IOnTrackRepository<Inventory> inventoryRepository)
        {
            _inventoryRepository = (InventoryRepository)inventoryRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> Inventorys()
        {
            try
            {

                IEnumerable<Inventory> inventorys = await _inventoryRepository.GetAll();

                if (inventorys != null)
                    return Ok(inventorys);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Inventory");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Team")]
        public async Task<IActionResult> Get(string teamid)
        {
            try
            {
                int inventoryLevel = await _inventoryRepository.TeamInventoryLevel(teamid);

                return Ok(inventoryLevel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Inventory");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Ticket")]
        public async Task<IActionResult> Ticket(string fixtureID)
        {
            try
            {
                int inventoryLevel = await _inventoryRepository.TicketInventoryLevel(fixtureID);

                return Ok(inventoryLevel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Inventory");
            }

            return NoContent();
        }

        // GET api/values/5
        [HttpGet]
        [Route("Check")]
        public async Task<IActionResult> CheckInventory(string fixtureID)
        {
            try
            {
                bool hasInventory = await _inventoryRepository.CheckInventory(fixtureID);

                return Ok(hasInventory);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Inventory");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Inventory inventory)
        {
            try
            {
                await _inventoryRepository.Insert(inventory);
                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Inventory");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Inventory inventory)
        {
            try
            {
                await _inventoryRepository.Update(inventory);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Inventory");
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
                await _inventoryRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Inventory");
            }

            return NoContent();
        }
    }
}
