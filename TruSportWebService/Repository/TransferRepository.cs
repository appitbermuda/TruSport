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
    public class TransferRepository : IOnTrackRepository<Transfer>
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

        public async Task<Transfer> Get(string id)
        {
            throw new NotImplementedException();
            //return await _context.Transfers.Include("Team").FirstOrDefaultAsync(e => e.TeamID == id);
        }

        public async Task<IEnumerable<Transfer>> GetAll()
        {
            //return await _context.Transfers.FromSql("select * from leaguetable").ToListAsync();
            return await _context.Transfers.ToListAsync();
        }

        public async Task<IEnumerable<Transfer>> Football()
        {
            try
            {
                var transfers = await _context.Transfers.Include(e => e.Sport).Include(e => e.NewTeam).Include(e => e.Season).ToListAsync();

                transfers.Where(e => e.Sport.Name.ToLower() == "football");

                return transfers;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Football Transfer");
            }

            return null;
        }

        public async Task<IEnumerable<Transfer>> Cricket()
        {
            try
            {
                var transfers = await _context.Transfers.Include(e => e.Sport).Include(e => e.NewTeam).Include(e => e.Season).ToListAsync();

                transfers.Where(e => e.Sport.Name.ToLower() == "cricket");

                return transfers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Cricket Transfer");
            }

            return null;
        }

        public async Task<IEnumerable<Transfer>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
            //return await _context.Transfers..Where(e => e.TeamID == teamID).ToListAsync();
        }

        public async Task<IEnumerable<Transfer>> GetByLeague(string leagueID)
        {
            throw new NotImplementedException();
            //return await _context.Transfers.Where(e => e.LeagueID == leagueID).ToListAsync();
        }

        public Task Insert(Transfer item)
        {
            throw new NotImplementedException();
        }

        public Task Update(Transfer item)
        {
            throw new NotImplementedException();
        }
    }
}
