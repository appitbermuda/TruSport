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
    public class MatchStatRepository : IOnTrackRepository<MatchStat>
    {
        OnTrackContext _context;

        public MatchStatRepository(OnTrackContext context)
        {
            _context = context;
        }
        public async Task Delete(string id)
        {
            var matchStat = _context.MatchStats.FirstOrDefault(e => e.ID == id);

            _context.MatchStats.Remove(matchStat);

            await _context.SaveChangesAsync();
        }

        public async Task<MatchStat> Get(string id)
        {
            return await _context.MatchStats.Include("MatchRoster").Include("AssistPlayer").FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<List<MatchStat>> GetByMatchRosterID(string matchRosterID)
        {
            return await _context.MatchStats.Include("MatchRoster").Include("AssistPlayer").Where(e => e.MatchRosterID == matchRosterID).ToListAsync();
        }

        public async Task<IEnumerable<MatchStat>> GetAll()
        {
            return await _context.MatchStats.Include("MatchRoster").Include("AssistPlayer").ToListAsync();
        }

        public async Task Insert(MatchStat item)
        {
            _context.MatchStats.Add(item);

            await _context.SaveChangesAsync();
        }

        public async Task InsertAll(List<MatchStat> items)
        {
            foreach (var item in items)
            {
                _context.MatchStats.Add(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(MatchStat item)
        {
            _context.MatchStats.Update(item);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAll(List<MatchStat> items)
        {
            foreach (var item in items)
            {
                _context.MatchStats.Update(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
