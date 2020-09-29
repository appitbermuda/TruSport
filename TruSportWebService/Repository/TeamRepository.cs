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
        LeagueTableRepository leagueTableRepository;

        public TeamRepository(OnTrackContext context)
        {
            _context = context;
            leagueTableRepository = new LeagueTableRepository(context);
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
            try
            {
                var teams = await _context.Teams
                    .Include(e => e.Sport)
                    .Include(e => e.Field)
                    .Where(e => e.Name != "TBD").ToListAsync();

                return teams;
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Team>> GetByLeague(string leagueID)
        {
            throw new NotImplementedException();
            //return await _context.Teams.Include("Field").Include("Coaches").Include(e => e.Coaches).Where(e => e.LeagueID == leagueID && e.Name != "TBD").ToListAsync();
        }

        public async Task<TeamSeason> GetTeam(string id)
        {
            //return await _context.Teams.Include("Field").Include("Coaches").Include(e => e.Coaches).FirstOrDefaultAsync(e => e.ID == id);
            //return await _context.TeamSeasons.Include("League").Include("Season").Include(e => e.Team.Coaches).Include(e => e.Team.Field).Include(e => e.Team).FirstOrDefaultAsync(e => e.TeamID == id && e.Season.IsCurrent);
            try
            {
                TeamSeason teamSeason = new TeamSeason();

                teamSeason = await _context.TeamSeasons
                    .Include(e => e.League)
                    .Include(e => e.Season)
                    .Include(e => e.Team).ThenInclude(e => e.Coaches)
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .FirstOrDefaultAsync(e => e.TeamID == id && e.Season.IsCurrent);

                return teamSeason;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<TeamSeason> GetFootballTeam(string teamID)
        {
            try
            {
                TeamSeason teamSeason = new TeamSeason();
                List<Fixture> fixtures = new List<Fixture>();
                List<LeagueTable> leagueTables = new List<LeagueTable>();

                fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam).ThenInclude(e => e.Coaches)
                .Include(e => e.AwayTeam).ThenInclude(e => e.Coaches)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.Match)
                .Include(e => e.MatchType)
                .Include(e => e.MatchRosters)
                .Include(e => e.Season)
                .Where(e => e.AwayTeamID == teamID || e.HomeTeamID == teamID).ToListAsync();

                fixtures.ForEach(e => e.SelectedTeamID = teamID);
                fixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == teamID && e.Match.HomeTeamScore > e.Match.AwayTeamScore) || (e.AwayTeamID == teamID && e.Match.AwayTeamScore > e.Match.HomeTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == teamID && e.Match.AwayTeamPenalty > e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == teamID && e.Match.HomeTeamPenalty > e.Match.AwayTeamPenalty) ? "W" : (e.AwayTeamID == teamID && e.Match.AwayTeamScore < e.Match.HomeTeamScore) || (e.HomeTeamID == teamID && e.Match.HomeTeamScore < e.Match.AwayTeamScore) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.AwayTeamID == teamID && e.Match.AwayTeamPenalty < e.Match.HomeTeamPenalty) || ((e.Match.IsPenalties.HasValue && e.Match.IsPenalties.Value) && e.HomeTeamID == teamID && e.Match.HomeTeamPenalty < e.Match.AwayTeamPenalty) ? "L" : (!e.Match.HomeTeamScore.HasValue || !e.Match.AwayTeamScore.HasValue) ? "" : "D");

                leagueTables = await _context.LeagueTable
                        .Include(e => e.Team)
                        .Include(e => e.Season)
                        .Include(e => e.League).ToListAsync();

                leagueTables.ForEach(e => e.IsSelectedTeam = (e.TeamID == teamID));

                teamSeason = await _context.TeamSeasons
                    .Include(e => e.League)
                    .Include(e => e.Season)
                    .Include(e => e.Team).ThenInclude(e => e.Coaches)
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .FirstOrDefaultAsync(e => e.TeamID == teamID && e.Season.IsCurrent);

                teamSeason.Fixtures = fixtures;
                teamSeason.Form = fixtures.Where(e => e.FixtureTime < DateTime.Now && e.Match.HomeTeamScore.HasValue && e.Match.AwayTeamScore.HasValue).OrderByDescending(e => e.Date).Take(6).ToList();
                teamSeason.FootballTable = leagueTables.Where(e => e.LeagueID == teamSeason.LeagueID).ToList();

                return teamSeason;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<TeamSeason> GetCricketTeam(string teamID)
        {
            try
            {
                TeamSeason teamSeason = new TeamSeason();
                List<CricketFixture> fixtures = new List<CricketFixture>();
                List<CricketLeagueTable> leagueTables = new List<CricketLeagueTable>();

                fixtures = await _context.CricketFixtures
                .Include(e => e.HomeTeam).ThenInclude(e => e.Coaches)
                .Include(e => e.AwayTeam).ThenInclude(e => e.Coaches)
                .Include(e => e.Field)
                .Include(e => e.League).ThenInclude(e => e.Sport)
                .Include(e => e.MatchType)
                .Include(e => e.CricketRosters)
                .Include(e => e.Season).ThenInclude(e => e.Sport)
                .Where(e => e.AwayTeamID == teamID || e.HomeTeamID == teamID).ToListAsync();

                fixtures.ForEach(e => e.SelectedTeamID = teamID);
                //fixtures.ForEach(e => e.SelectedTeamResult = (e.HomeTeamID == teamID && (e.CricketScores.Where(x => x.BattingTeamID == teamID).Sum(x => x.Runs) > e.CricketScores.Where(x => x.BattingTeamID != teamID).Sum(x => x.Runs)) || (e.AwayTeamID == teamID && (e.CricketScores.Where(x => x.BattingTeamID == teamID).Sum(x => x.Runs) > e.CricketScores.Where(x => x.BattingTeamID != teamID).Sum(x => x.Runs))) ? "W" : (e.HomeTeamID == teamID && (e.CricketScores.Where(x => x.BattingTeamID == teamID).Sum(x => x.Runs) < e.CricketScores.Where(x => x.BattingTeamID != teamID).Sum(x => x.Runs))) || (e.AwayTeamID == teamID && (e.CricketScores.Where(x => x.BattingTeamID == teamID).Sum(x => x.Runs) < e.CricketScores.Where(x => x.BattingTeamID != teamID).Sum(x => x.Runs))) ? "L" : (e.CricketScores.Sum(x => x.Runs) == 0 || e.CricketScores.Sum(x => x.Runs) == 0) ? "" : "D"));

                //leagueTables = leagueTableRepository.GetCricketTableByTeam(teamID) await _context.CricketLeagueTable
                //        .Include(e => e.Team)
                //        .Include(e => e.Season)
                //        .Include(e => e.League).ToListAsync();

                //leagueTables.ForEach(e => e.IsSelectedTeam = (e.TeamID == teamID));

                teamSeason = await _context.TeamSeasons
                    .Include(e => e.League)
                    .Include(e => e.Season)
                    .Include(e => e.Team).ThenInclude(e => e.Coaches)
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .FirstOrDefaultAsync(e => e.TeamID == teamID && e.Season.IsCurrent);

                //teamSeason.Fixtures = fixtures;
                //teamSeason.Form = fixtures.Where(e => e.FixtureTime < DateTime.Now && e.Match.HomeTeamScore.HasValue && e.Match.AwayTeamScore.HasValue).OrderByDescending(e => e.Date).Take(6).ToList();
                //teamSeason.CricketTable = leagueTables.Where(e => e.LeagueID == teamSeason.LeagueID).ToList();

                return teamSeason;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<TeamSeason>> Get()
        {
            try
            {
                List<TeamSeason> teamSeasons = new List<TeamSeason>();

                teamSeasons = await _context.TeamSeasons
                    .Include(e => e.League)
                    .Include(e => e.Season)
                    .Include(e => e.Team).ThenInclude(e => e.Coaches)
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .Where(e => e.Team.Name != "TBD").ToListAsync();

                return teamSeasons;
            }
            catch (Exception ex)
            {

            }

            return null;
            
        }

        public async Task<IEnumerable<TeamSeason>> GetBySport(string SportID)
        {
            try
            {
                List<TeamSeason> teamSeasons = new List<TeamSeason>();

                teamSeasons = await _context.TeamSeasons
                    .Include(e => e.League)
                    .Include(e => e.Season).ThenInclude(e => e.Sport)
                    .Include(e => e.Team).ThenInclude(e => e.Coaches)
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .Where(e => e.Team.Name != "TBD" && e.Season.SportID == SportID).ToListAsync();

                return teamSeasons;
            }
            catch (Exception ex)
            {

            }

            return null;

        }

        public async Task<IEnumerable<TeamSeason>> GetTeamBySport(string SportType)
        {
            try
            {
                List<TeamSeason> teamSeasons = new List<TeamSeason>();

                teamSeasons = await _context.TeamSeasons
                    .Include(e => e.League)
                    .Include(e => e.Season).ThenInclude(e => e.Sport)
                    .Include(e => e.Team).ThenInclude(e => e.Coaches)
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .Where(e => e.Team.Name != "TBD" && e.Season.Sport.Name == SportType).ToListAsync();

                return teamSeasons;
            }
            catch (Exception ex)
            {

            }

            return null;

        }

        public async Task<List<Team>> GetCricketTeams()
        {
            try
            {
                List<Team> teams = new List<Team>();

                var teamSeason = await _context.TeamSeasons
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .Include(e => e.Team).ThenInclude(e => e.Sport)
                    .Include(e => e.Season).ThenInclude(e => e.Sport)
                    .Include(e => e.League)
                    .Where(e => e.Team.Name != "TBD" && e.Season.Sport.Name == "Cricket" && e.Season.IsCurrent)
                    .ToListAsync();

                teamSeason.ForEach(e => e.Team.League = e.League);
                teamSeason.ForEach(e => e.Team.LeagueID = e.LeagueID);

                return teamSeason.Select(e => e.Team).OrderBy(e => e.Name).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return null;
        }

        public async Task<List<Team>> GetFootballTeams()
        {
            try
            {
                List<Team> teams = new List<Team>();

                var teamSeason = await _context.TeamSeasons
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .Include(e => e.Team).ThenInclude(e => e.Sport)
                    .Include(e => e.Season).ThenInclude(e => e.Sport)
                    .Include(e => e.League)
                    .Where(e => e.Team.Name != "TBD" && e.Season.Sport.Name == "Football" && e.Season.IsCurrent)
                    .ToListAsync();

                teamSeason.ForEach(e => e.Team.League = e.League);
                teamSeason.ForEach(e => e.Team.LeagueID = e.LeagueID);

                teams = teamSeason.Select(e => e.Team).ToList();
                teams.ForEach(e => e.TeamSeasons = teamSeason.Where(d => d.TeamID == e.ID).ToList());

                //return teamSeason.Select(e => e.Team).OrderBy(e => e.Name).ToList();
                return teams;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Team");
            }

            return null;
        }

        public async Task<Team> GetCricketProfile(string teamID)
        {
            Team team = new Team();
            List<CricketFixture> fixtures = new List<CricketFixture>();
            List<CricketLeagueTable> cricketLeagueTables = new List<CricketLeagueTable>();
            List<Transfer> transfers = new List<Transfer>();

            try
            {
                var teamSeason = await _context.TeamSeasons
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .Include(e => e.Team).ThenInclude(e => e.Sport)
                    .Include(e => e.Season).ThenInclude(e => e.Sport)
                    .Include(e => e.League)
                    .Where(e => e.TeamID == teamID && e.Season.IsCurrent)
                    .ToListAsync();

                teamSeason.ForEach(e => e.Team.League = e.League);
                teamSeason.ForEach(e => e.Team.LeagueID = e.LeagueID);

                team = teamSeason.FirstOrDefault(e => e.Season.IsCurrent).Team;

                cricketLeagueTables = await _context.CricketLeagueTable
                        .Include(e => e.Team)
                        .Include(e => e.Season)
                        .Include(e => e.League).ToListAsync();

                cricketLeagueTables.ForEach(e => e.IsSelectedTeam = (e.TeamID == teamID));

                team.CricketTable = cricketLeagueTables.FirstOrDefault(e => e.Season.IsCurrent && e.LeagueID == team.League.ID && e.TeamID == teamID);

                teamSeason.ForEach(e => e.CricketTable = cricketLeagueTables.Where(f => f.LeagueID == e.LeagueID).ToList());

                teamSeason.ForEach(e => e.Transfers = transfers.Where(t => t.SeasonID == e.SeasonID).ToList());

                team.TeamSeasons = teamSeason;

            }
            catch (Exception ex)
            {

            }

            return team;
        }

        public async Task<Team> GetFootballProfile(string teamID)
        {
            Team team = new Team();
            List<Fixture> fixtures = new List<Fixture>();
            List<LeagueTable> footballLeagueTables = new List<LeagueTable>();
            List<Transfer> transfers = new List<Transfer>();

            try
            {
                if (!String.IsNullOrEmpty(teamID))
                {
                    var teamSeason = await _context.TeamSeasons
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .Include(e => e.Team).ThenInclude(e => e.Sport)
                    .Include(e => e.Season).ThenInclude(e => e.Sport)
                    .Include(e => e.League)
                    .Where(e => e.TeamID == teamID && e.Season.IsCurrent)
                    .ToListAsync();

                    teamSeason.ForEach(e => e.Team.League = e.League);
                    teamSeason.ForEach(e => e.Team.LeagueID = e.LeagueID);

                    team = teamSeason.FirstOrDefault(e => e.Season.IsCurrent).Team;

                    transfers = await _context.Transfers
                        .Include(e => e.Season)
                        .Include(e => e.Sport)
                        .Include(e => e.NewTeam)
                        .Where(e => e.NewTeamID == teamID).ToListAsync();

                    footballLeagueTables = await _context.LeagueTable
                            .Include(e => e.Team)
                            .Include(e => e.Season)
                            .Include(e => e.League).ToListAsync();

                    footballLeagueTables.ForEach(e => e.IsSelectedTeam = (e.TeamID == teamID));

                    team.FootballTable = footballLeagueTables.FirstOrDefault(e => e.Season.IsCurrent && e.LeagueID == team.League.ID && e.TeamID == teamID);

                    teamSeason.ForEach(e => e.FootballTable = footballLeagueTables.Where(f => f.LeagueID == e.LeagueID).ToList());

                    teamSeason.ForEach(e => e.Transfers = transfers.Where(t => t.SeasonID == e.SeasonID).ToList());

                    team.TeamSeasons = teamSeason;
                }
            }
            catch (Exception ex)
            {

            }

            return team;
        }

        //Deprecated
        //[Obsolete]
        public async Task<IEnumerable<TeamSeason>> GetTeams()
        {
            try
            {
                List<TeamSeason> teamSeasons = new List<TeamSeason>();

                teamSeasons = await _context.TeamSeasons
                    .Include(e => e.League)
                    .Include(e => e.Season).ThenInclude(e => e.Sport)
                    .Include(e => e.Team).ThenInclude(e => e.Coaches)
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .Where(e => e.Team.Name != "TBD" && e.Season.Sport.Name == "Football").ToListAsync();

                return teamSeasons;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<TeamSeason>> GetTeamsByLeague(string leagueID)
        {
            try
            {
                List<TeamSeason> teamSeasons = new List<TeamSeason>();

                teamSeasons = await _context.TeamSeasons
                    .Include(e => e.League)
                    .Include(e => e.Season).ThenInclude(e => e.Sport)
                    .Include(e => e.Team).ThenInclude(e => e.Coaches)
                    .Include(e => e.Team).ThenInclude(e => e.Field)
                    .Where(e => e.LeagueID == leagueID && e.Team.Name != "TBD").ToListAsync();

                return teamSeasons;
            }
            catch (Exception ex)
            {

            }

            return null;
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
