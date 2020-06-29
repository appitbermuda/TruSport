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
    public class FieldController : ControllerBase
    {
        private readonly FieldRepository _fieldRepository;

        public FieldController(IOnTrackRepository<Field> fieldRepository)
        {
            _fieldRepository = (FieldRepository)fieldRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("AllFields")]
        public async Task<IActionResult> fields()
        {
            try
            {

                IEnumerable<Field> fields = await _fieldRepository.GetAll();

                if (fields != null)
                    return Ok(fields);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Field");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("TeamFields")]
        public async Task<IActionResult> FieldsByTeam(string teamID)
        {
            try
            {

                IEnumerable<Field> fields = await _fieldRepository.GetByTeam(teamID);

                if (fields != null)
                    return Ok(fields);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Field");
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
                Field field = await _fieldRepository.Get(id);

                if (field != null)
                    return Ok(field);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Field");
            }

            return NoContent();
        }

        // POST api/values
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Post([FromBody] Field field)
        {
            try
            {
                await _fieldRepository.Insert(field);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Field");
            }

            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Field field)
        {
            try
            {
                await _fieldRepository.Update(field);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Field");
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
                await _fieldRepository.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Field");
            }

            return NoContent();
        }
    }
}
