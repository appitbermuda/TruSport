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
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepository;

        public ProductController(IOnTrackRepository<Product> productRepository)
        {
            _productRepository = (ProductRepository)productRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> Products()
        {
            try
            {

                IEnumerable<Product> products = await _productRepository.GetAll();

                if (products != null)
                    return Ok(products);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Product");
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
                Product product = await _productRepository.Get(id);

                if (product != null)
                    return Ok(product);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Product");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            try
            {
                bool inserted = await _productRepository.Insert(product);
                return Ok(inserted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Product");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Product product)
        {
            try
            {
                bool updated = await _productRepository.Update(product);

                return Ok(updated);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Product");
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
                await _productRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Product");
            }

            return NoContent();
        }
    }
}
