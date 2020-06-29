using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class RoleRepository : IOnTrackRepository<Role>
    {
        OnTrackContext _context;

        public RoleRepository(OnTrackContext context)
        {
            _context = context;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> Get(string id)
        {
            try
            {
                return await _context.Roles.FirstOrDefaultAsync(e => e.ID == id && e.IsSelectable);
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            try
            {
                return await _context.Roles.Where(e => e.IsSelectable).ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public Task<IEnumerable<Role>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(Role item)
        {
            throw new NotImplementedException();
        }

        public Task Update(Role item)
        {
            throw new NotImplementedException();
        }
    }
}
