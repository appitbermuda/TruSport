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
    public class CoachRepository : IOnTrackRepository<Coach>
    {
        OnTrackContext _context;

        public CoachRepository(OnTrackContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                var coach = await _context.Coaches.FirstOrDefaultAsync(e => e.ID == id);

                _context.Coaches.Remove(coach);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<Coach> Get(string id)
        {
            return await _context.Coaches.Include(e => e.Team).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Coach>> GetAll()
        {
            return await _context.Coaches.Include(e => e.Team).ToListAsync();
        }

        public async Task<IEnumerable<Coach>> GetByTeam(string teamID)
        {
            return await _context.Coaches.Include(e => e.Team).Where(e => e.TeamID == teamID).ToListAsync();
        }

        public async Task<bool> Insert(Coach item)
        {
            try
            {
                _context.Coaches.Add(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> Update(Coach item)
        {
            try
            {
                _context.Coaches.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        Task IOnTrackRepository<Coach>.Delete(string id)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Coach>.Insert(Coach item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Coach>.Update(Coach item)
        {
            throw new NotImplementedException();
        }
    }
}
