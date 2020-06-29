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
    public class LeagueStatRepository : IOnTrackRepository<LeagueStat>
    {
        OnTrackContext _context;

        public LeagueStatRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<LeagueStat> Get(string id)
        {
            throw new NotImplementedException();
            //return await _context.LeagueStat.Include("Team").FirstOrDefaultAsync(e => e.TeamID == id);
        }

        public async Task<IEnumerable<LeagueStat>> GetAll()
        {
            //return await _context.LeagueTables.FromSql("select * from leaguetable").ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GoalsConcededByTeam>> GetGoalsConcededByTeam()
        {
            //return await _context.LeagueStat.FromSql("select * from leaguetable").ToListAsync();
            return await _context.GoalsConcededByTeam.OrderByDescending(e => e.Goals).ToListAsync();
        }

        public async Task<IEnumerable<GoalsScoredByTeam>> GetGoalsScoredByTeam()
        {
            //return await _context.LeagueStat.FromSql("select * from leaguetable").ToListAsync();
            return await _context.GoalsScoredByTeam.OrderByDescending(e => e.Goals).ToListAsync();
        }

        public async Task<IEnumerable<GoalsScoredByPlayer>> GetGoalsScoredByPlayer()
        {
            try
            {
                //return await _context.LeagueStat.FromSql("select * from leaguetable").ToListAsync();
                return await _context.GoalsScoredByPlayer.OrderByDescending(e => e.Goals).ToListAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<RunsByPlayer>> GetRunsByPlayer()
        {
            try
            {
                //return await _context.LeagueStat.FromSql("select * from leaguetable").ToListAsync();
                return await _context.RunsByPlayer.OrderByDescending(e => e.Stat).ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<WicketsByPlayer>> GetWicketsByPlayer()
        {
            try
            {
                //return await _context.LeagueStat.FromSql("select * from leaguetable").ToListAsync();
                return await _context.WicketsByPlayer.OrderByDescending(e => e.Stat).ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<GoalsConcededByTeam>> GetGoalsConcededByTeamByLeague(string leagueID)
        {
            
            return await _context.GoalsConcededByTeam.Where(e => e.LeagueID == leagueID).OrderByDescending(e => e.Goals).ToListAsync();
        }

        public async Task<IEnumerable<GoalsScoredByTeam>> GetGoalsScoredByTeamByLeague(string leagueID)
        {
            //return await _context.LeagueStat.FromSql("select * from leaguetable").ToListAsync();
            return await _context.GoalsScoredByTeam.Where(e => e.LeagueID == leagueID).OrderByDescending(e => e.Goals).ToListAsync();
        }

        public async Task<IEnumerable<GoalsScoredByPlayer>> GetGoalsScoredByPlayerByTeam(string teamID)
        {
            //return await _context.LeagueStat.FromSql("select * from leaguetable").ToListAsync();
            return await _context.GoalsScoredByPlayer.Where(e => e.TeamID == teamID).OrderByDescending(e => e.Goals).ToListAsync();
        }

        public Task Insert(LeagueStat item)
        {
            throw new NotImplementedException();
        }

        public Task Update(LeagueStat item)
        {
            throw new NotImplementedException();
        }
    }
}
