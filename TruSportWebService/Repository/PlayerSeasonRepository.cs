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
    public class PlayerSeasonRepository : IOnTrackRepository<PlayerSeason>
    {
        OnTrackContext _context;

        public PlayerSeasonRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PlayerSeason> Get(string id)
        {
            return await _context.PlayerSeasons.Include("Player").FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<PlayerSeason>> GetAll()
        {
            return await _context.PlayerSeasons.Include("Player").ToListAsync();
        }

        public async Task<IEnumerable<PlayerSeason>> GetBySeason(int season)
        {
            throw new NotImplementedException();
        }

        public Task Insert(PlayerSeason item)
        {
            throw new NotImplementedException();
        }

        public Task Update(PlayerSeason item)
        {
            throw new NotImplementedException();
        }
    }
}
