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
    public class TeamRepository : IOnTrackRepository<Team>
    {
        OnTrackContext _context;

        public TeamRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Team> Get(string id)
        {
            return await _context.Teams.Include("Field").Include("Coaches").Include(e => e.Coaches).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Team>> GetAll()
        {
            return await _context.Teams.Include("League").Include("Field").Include("Coaches").Include(e => e.Coaches).Where(e => e.Name != "TBD").ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetByLeague(string leagueID)
        {
            throw new NotImplementedException();
            //return await _context.Teams.Include("Field").Include("Coaches").Include(e => e.Coaches).Where(e => e.LeagueID == leagueID && e.Name != "TBD").ToListAsync();
        }

        public async Task<TeamSeason> GetTeam(string id)
        {
            //return await _context.Teams.Include("Field").Include("Coaches").Include(e => e.Coaches).FirstOrDefaultAsync(e => e.ID == id);
            return await _context.TeamSeasons.Include("League").Include("Season").Include(e => e.Team.Coaches).Include(e => e.Team.Field).Include(e => e.Team).FirstOrDefaultAsync(e => e.TeamID == id && e.Season.IsCurrent);
        }


        public async Task<IEnumerable<TeamSeason>> GetTeams()
        {
            return await _context.TeamSeasons.Include("League").Include("Season").Include(e => e.Team.Coaches).Include(e => e.Team.Field).Include(e => e.Team).Where(e => e.Team.Name != "TBD").ToListAsync();
        }

        public async Task<IEnumerable<TeamSeason>> GetTeamsByLeague(string leagueID)
        {
            return await _context.TeamSeasons.Include("League").Include("Season").Include(e => e.Team.Coaches).Include(e => e.Team.Field).Include(e => e.Team).Where(e => e.LeagueID == leagueID && e.Team.Name != "TBD").ToListAsync();
        }

        public async Task<IEnumerable<TeamSeason>> GetTeamsBySeason(int season)
        {
            return await _context.TeamSeasons.Include("League").Include("Season").Include(e => e.Team.Coaches).Include(e => e.Team.Field).Include(e => e.Team).Where(e => e.Season.Key == season && e.Team.Name != "TBD").ToListAsync();
        }

        public Task Insert(Team item)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Team item)
        {
            try
            {
                var teamSeason = await _context.TeamSeasons.FirstOrDefaultAsync(e => e.TeamID == item.ID && e.Season.IsCurrent);

                if (teamSeason.LeagueID != item.LeagueID)
                {
                    teamSeason.LeagueID = item.LeagueID;

                    _context.TeamSeasons.Update(teamSeason);
                }

                _context.Teams.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }
        }
    }
}
