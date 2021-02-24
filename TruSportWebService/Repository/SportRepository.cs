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
    public class SportRepository : IOnTrackRepository<Sport>
    {
        OnTrackContext _context;

        public SportRepository(OnTrackContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var sport = await _context.Sports.FirstOrDefaultAsync(e => e.ID == id);

                _context.Sports.Remove(sport);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public Task<Sport> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Sport>> GetAll()
        {
            try
            {
                var sports = await _context.Sports.ToListAsync();

                return sports;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Sport");
            }

            return null;
        }

        public Task<IEnumerable<Sport>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Insert(Sport item)
        {
            try
            {
                _context.Sports.Add(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> Update(Sport item)
        {
            try
            {
                _context.Sports.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        Task IOnTrackRepository<Sport>.Delete(string id)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Sport>.Insert(Sport item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Sport>.Update(Sport item)
        {
            throw new NotImplementedException();
        }
    }
}
