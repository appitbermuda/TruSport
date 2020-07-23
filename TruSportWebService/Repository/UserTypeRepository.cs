using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class UserTypeRepository : IOnTrackRepository<UserType>
    {
        OnTrackContext _context;

        public UserTypeRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserType> Get(string id)
        {
            return await _context.UserTypes.FirstOrDefaultAsync(e => e.ID == id && e.IsSelectable);
        }

        public async Task<IEnumerable<UserType>> GetAll()
        {
            return await _context.UserTypes.ToListAsync();
        }

        public async Task<IEnumerable<UserType>> GetSelectable()
        {
            return await _context.UserTypes.Where(e => e.IsSelectable).ToListAsync();
        }

        public Task<IEnumerable<UserType>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(UserType item)
        {
            throw new NotImplementedException();
        }

        public Task Update(UserType item)
        {
            throw new NotImplementedException();
        }
    }
}
