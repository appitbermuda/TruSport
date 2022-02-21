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
    public class SeasonRepository : IOnTrackRepository<Season>
    {
        OnTrackContext _context;

        public SeasonRepository(OnTrackContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                var season = await _context.Seasons.FirstOrDefaultAsync(e => e.ID == id);

                _context.Seasons.Remove(season);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<Season> Get(string id)
        {
            return await _context.Seasons.Include(e => e.Sport).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<List<Season>> GetCricketSeason()
        {
            return await _context.Seasons.Include(e => e.Sport).Where(e => e.Sport.Name == Constants.Cricket).ToListAsync();
        }

        public async Task<List<Season>> GetFootballSeason()
        {
            return await _context.Seasons.Include(e => e.Sport).Where(e => e.Sport.Name == Constants.Football).ToListAsync();
        }

        public async Task<List<Season>> GetBasketballSeason()
        {
            return await _context.Seasons.Include(e => e.Sport).Where(e => e.Sport.Name == Constants.Basketball).ToListAsync();
        }

        public async Task<List<Season>> GetBowlingSeason()
        {
            return await _context.Seasons.Include(e => e.Sport).Where(e => e.Sport.Name == Constants.Bowling).ToListAsync();
        }

        public async Task<IEnumerable<Season>> GetAll()
        {
            return await _context.Seasons.Include(e => e.Sport).ToListAsync();
        }

        public Task<IEnumerable<Season>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Insert(Season item)
        {
            try
            {
                _context.Seasons.Add(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> Update(Season item)
        {
            try
            {
                _context.Seasons.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        Task IOnTrackRepository<Season>.Insert(Season item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Season>.Update(Season item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Season>.Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
