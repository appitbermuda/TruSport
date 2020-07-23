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
    public class vTransfersRepository : IOnTrackRepository<vTransfers>
    {
        OnTrackContext _context;

        public vTransfersRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<vTransfers> Get(string id)
        {
            throw new NotImplementedException();
            //return await _context.vTransfers.Include("Team").FirstOrDefaultAsync(e => e.TeamID == id);
        }

        public async Task<IEnumerable<vTransfers>> GetAll()
        {
            //return await _context.vTransfers.FromSql("select * from leaguetable").ToListAsync();
            return await _context.vTransfers.ToListAsync();
        }

        public async Task<IEnumerable<vTransfers>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
            //return await _context.vTransfers..Where(e => e.TeamID == teamID).ToListAsync();
        }

        public async Task<IEnumerable<vTransfers>> GetByLeague(string leagueID)
        {
            throw new NotImplementedException();
            //return await _context.vTransfers.Where(e => e.LeagueID == leagueID).ToListAsync();
        }

        public Task Insert(vTransfers item)
        {
            throw new NotImplementedException();
        }

        public Task Update(vTransfers item)
        {
            throw new NotImplementedException();
        }
    }
}
