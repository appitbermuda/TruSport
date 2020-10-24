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
    public class UserTypeController : ControllerBase
    {
        private readonly UserTypeRepository _userTypeRepository;

        public UserTypeController(IOnTrackRepository<UserType> userTypeRepository)
        {
            _userTypeRepository = (UserTypeRepository)userTypeRepository;
        }

        // GET api/values
        //Depracated
        [HttpGet]
        [Route("AllUserTypes")]
        public async Task<IActionResult> UserTypes()
        {
            try
            {

                IEnumerable<UserType> userTypes = await _userTypeRepository.GetSelectable();

                if (userTypes != null)
                    return Ok(userTypes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "UserType");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetUserTypes()
        {
            try
            {

                IEnumerable<UserType> userTypes = await _userTypeRepository.GetAll();

                if (userTypes != null)
                    return Ok(userTypes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "UserType");
            }

            return NoContent();
        }

        [HttpGet]
        [Route("Roles")]
        public async Task<IActionResult> GetSelectableRoles()
        {
            try
            {

                IEnumerable<UserType> userTypes = await _userTypeRepository.GetSelectable();

                if (userTypes != null)
                    return Ok(userTypes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "UserType");
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
                UserType userType = await _userTypeRepository.Get(id);

                if (userType != null)
                    return Ok(userType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "UserType");
            }

            return NoContent();
        }

        //// POST api/values
        //[HttpPost]
        //[Route("Insert")]
        //public async Task<IActionResult> Post([FromBody] UserType userType)
        //{
        //    try
        //    {
        //        await _userTypeRepository.Insert(userType);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "UserType");
        //    }

        //    return NoContent();
        //}

        //[HttpPost]
        //[Route("Update")]
        //public async Task<IActionResult> Update([FromBody] UserType userType)
        //{
        //    try
        //    {
        //        await _userTypeRepository.Update(userType);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "UserType");
        //    }

        //    return NoContent();
        //}

        //// DELETE api/values/5
        //[HttpDelete]
        //[Route("Delete")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    try
        //    {
        //        await _userTypeRepository.Delete(id);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message, "UserType");
        //    }

        //    return NoContent();
        //}
    }
}
