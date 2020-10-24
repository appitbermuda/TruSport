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
    public class RoleController : ControllerBase
    {
        private readonly RoleRepository _roleRepository;

        public RoleController(IOnTrackRepository<Role> roleRepository)
        {
            _roleRepository = (RoleRepository)roleRepository;
        }

        // GET api/values
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> Roles()
        {
            try
            {
                IEnumerable<Role> roles = await _roleRepository.GetAll();

                if (roles != null)
                    return Ok(roles);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Role");
            }

            return NoContent();
        }

        // GET api/values
        [HttpGet]
        [Route("Selectable")]
        public async Task<IActionResult> SelectableRoles()
        {
            try
            {
                IEnumerable<Role> roles = await _roleRepository.GetSelectable();

                if (roles != null)
                    return Ok(roles);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Role");
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
                Role role = await _roleRepository.Get(id);

                if (role != null)
                    return Ok(role);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Role");
            }

            return NoContent();
        }
    }
}
