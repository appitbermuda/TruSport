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
    public class ProductTypeController : ControllerBase
    {
        private readonly ProductTypeRepository _productTypeRepository;

        public ProductTypeController(IOnTrackRepository<ProductType> productTypeRepository)
        {
            _productTypeRepository = (ProductTypeRepository)productTypeRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> ProductTypes()
        {
            try
            {

                IEnumerable<ProductType> productTypes = await _productTypeRepository.GetAll();

                if (productTypes != null)
                    return Ok(productTypes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ProductType");
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
                ProductType productType = await _productTypeRepository.Get(id);

                if (productType != null)
                    return Ok(productType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ProductType");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] ProductType productType)
        {
            try
            {
                await _productTypeRepository.Insert(productType);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ProductType");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] ProductType productType)
        {
            try
            {
                await _productTypeRepository.Update(productType);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ProductType");
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
                await _productTypeRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "ProductType");
            }

            return NoContent();
        }
    }
}
