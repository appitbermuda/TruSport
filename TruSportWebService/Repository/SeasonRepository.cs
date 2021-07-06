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
    public class SeasonRepository : IOnTrackRepository<Season>
    {
        OnTrackContext _context;

        public SeasonRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Season> Get(string id)
        {
            return await _context.Seasons.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<List<Season>> GetCricketSeason()
        {
            return await _context.Seasons.Where(e => e.Sport.Name == "Cricket").ToListAsync();
        }

        public async Task<List<Season>> GetFootballSeason()
        {
            return await _context.Seasons.Where(e => e.Sport.Name == "Football").ToListAsync();
        }

        public async Task<List<Season>> GetBowlingSeason()
        {
            return await _context.Seasons.Where(e => e.Sport.Name == "Bowling").ToListAsync();
        }

        public async Task<IEnumerable<Season>> GetAll()
        {
            return await _context.Seasons.ToListAsync();
        }

        public Task<IEnumerable<Season>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(Season item)
        {
            throw new NotImplementedException();
        }

        public Task Update(Season item)
        {
            throw new NotImplementedException();
        }
    }
}
