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
    public class LeagueTableRepository : IOnTrackRepository<LTable>
    {
        OnTrackContext _context;

        public LeagueTableRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<LeagueTable> Get(string id)
        {
            throw new NotImplementedException();
            //return await _context.LeagueTables.Include("Team").FirstOrDefaultAsync(e => e.TeamID == id);
        }

        public async Task<IEnumerable<LeagueTable>> GetAll()
        {
            //return await _context.LeagueTables.FromSql("select * from leaguetable").ToListAsync();
            return await _context.LeagueTable.ToListAsync();
        }

        public async Task<IEnumerable<PremierLeagueTable>> GetPremierLeagueTable()
        {
            //return await _context.LeagueTables.FromSql("select * from leaguetable").ToListAsync();
            return await _context.PremierLeagueTable.ToListAsync();
        }

        public async Task<IEnumerable<FirstDivisionTable>> GetFirstDivisionTable()
        {
            //return await _context.LeagueTables.FromSql("select * from leaguetable").ToListAsync();
            return await _context.FirstDivisionTable.ToListAsync();
        }

        public async Task<IEnumerable<CoronaLeagueTable>> GetCoronaLeagueTable()
        {
            //return await _context.LeagueTables.FromSql("select * from leaguetable").ToListAsync();
            return await _context.CoronaLeagueTable.ToListAsync();
        }

        public async Task<PremierLeagueTable> GetPremierLeagueTable(string teamID)
        {
            return await _context.PremierLeagueTable.FirstOrDefaultAsync(e => e.TeamID == teamID);
        }

        public async Task<FirstDivisionTable> GetFirstDivisionTable(string teamID)
        {
            return await _context.FirstDivisionTable.FirstOrDefaultAsync(e => e.TeamID == teamID);
        }

        //public async Task<IEnumerable<LeagueTable>> GetCoronaLeagueTable()
        //{
        //    return await _context.LeagueTable.Include("Team").Include("Season").Where(e => e.LeagueName.Contains("Corona")).ToListAsync();
        //}


        public async Task<IEnumerable<LeagueTable>> GetByTeam(string teamID, string leagueID)
        {
            //League league = _context.Leagues.FirstOrDefaultAsync(e => e.ID == leagueID)
            return await _context.LeagueTable.Include("Team").Include("Season").Include("League").Where(e => e.TeamID == teamID).ToListAsync();
        }

        public async Task<IEnumerable<LeagueTable>> GetByLeague(string leagueID)
        {
            try
            {
                var leagueTables = await _context.LeagueTable.Include("Team").Include("Season").Include("League").Where(e => e.LeagueID == leagueID).ToListAsync();

                return leagueTables;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "League Table");
                return null;
            }
        }

        public Task Insert(LeagueTable item)
        {
            throw new NotImplementedException();
        }

        public Task Update(LeagueTable item)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<LTable>> IOnTrackRepository<LTable>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<LTable> IOnTrackRepository<LTable>.Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task Insert(LTable item)
        {
            throw new NotImplementedException();
        }

        public Task Update(LTable item)
        {
            throw new NotImplementedException();
        }
    }
}
