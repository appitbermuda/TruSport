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
    public class TransferRepository : IOnTrackRepository<Transfers>
    {
        OnTrackContext _context;

        public TransferRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Transfers> Get(string id)
        {
            throw new NotImplementedException();
            //return await _context.Transfers.Include("Team").FirstOrDefaultAsync(e => e.TeamID == id);
        }

        public async Task<IEnumerable<Transfers>> GetAll()
        {
            //return await _context.Transfers.FromSql("select * from leaguetable").ToListAsync();
            return await _context.Transfers.ToListAsync();
        }

        public async Task<IEnumerable<Transfers>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
            //return await _context.Transfers..Where(e => e.TeamID == teamID).ToListAsync();
        }

        public async Task<IEnumerable<Transfers>> GetByLeague(string leagueID)
        {
            throw new NotImplementedException();
            //return await _context.Transfers.Where(e => e.LeagueID == leagueID).ToListAsync();
        }

        public Task Insert(Transfers item)
        {
            throw new NotImplementedException();
        }

        public Task Update(Transfers item)
        {
            throw new NotImplementedException();
        }
    }
}
