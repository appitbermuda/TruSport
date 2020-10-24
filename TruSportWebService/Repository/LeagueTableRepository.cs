using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public async Task<IEnumerable<CricketLeagueTable>> GetAllCricket()
        {
            //return await _context.LeagueTables.FromSql("select * from leaguetable").ToListAsync();
            return await _context.CricketLeagueTable.ToListAsync();
        }

        public async Task<IEnumerable<PremierLeagueTable>> GetPremierLeagueTable()
        {
            try
            {
                return await _context.PremierLeagueTable.ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<FirstDivisionTable>> GetFirstDivisionTable()
        {
            try
            {
                return await _context.FirstDivisionTable.ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<CoronaLeagueTable>> GetCoronaLeagueTable()
        {
            try
            {
                return await _context.CoronaLeagueTable.ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<CricketLeagueTable>> GetCricketPremierLeagueTable()
        {
            try
            {
                var cricketLeagueStanding = await _context.CricketLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.League.Name == "Premier Division").ToListAsync();

                List<CricketLeagueTable> makeLeagueTable = new List<CricketLeagueTable>();

                foreach (var standing in cricketLeagueStanding)
                {
                    CricketLeagueTable table = new CricketLeagueTable
                    {
                        TeamID = standing.TeamID,
                        Team = standing.Team,
                        TeamLogo = standing.Team.TeamLogo,
                        Name = standing.Team.Name,
                        LeagueID = standing.LeagueID,
                        League = standing.League,
                        LeagueName = standing.League.Name,
                        SeasonID = standing.SeasonID,
                        Season = standing.Season,
                        SeasonDate = standing.Season.Date,
                        Wins = standing.Wins,
                        Loss = standing.Loss,
                        Draws = standing.Draws,
                        Played = standing.Played,
                        Points = standing.Points,
                        NetRunRate = standing.NetRunRate
                    };

                    makeLeagueTable.Add(table);
                }

                List<CricketLeagueTable> leagueTable = new List<CricketLeagueTable>();
                var tablePositions = makeLeagueTable.OrderByDescending(e => e.Points).ThenByDescending(e => e.NetRunRate);
                int position = 1;
                foreach (var table in tablePositions)
                {
                    table.Position = position;
                    leagueTable.Add(table);

                    position++;
                }

                return leagueTable;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPremierLeagueTable");
            }

            return null;
        }

        public async Task<IEnumerable<CricketLeagueTable>> GetCricketFirstDivisionTable()
        {
            try
            {
                var cricketLeagueStanding = await _context.CricketLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.League.Name == "First Division").ToListAsync();

                List<CricketLeagueTable> makeLeagueTable = new List<CricketLeagueTable>();

                foreach (var standing in cricketLeagueStanding)
                {
                    CricketLeagueTable table = new CricketLeagueTable
                    {
                        TeamID = standing.TeamID,
                        Team = standing.Team,
                        TeamLogo = standing.Team.TeamLogo,
                        Name = standing.Team.Name,
                        LeagueID = standing.LeagueID,
                        League = standing.League,
                        LeagueName = standing.League.Name,
                        SeasonID = standing.SeasonID,
                        Season = standing.Season,
                        SeasonDate = standing.Season.Date,
                        Wins = standing.Wins,
                        Loss = standing.Loss,
                        Draws = standing.Draws,
                        Played = standing.Played,
                        Points = standing.Points,
                        NetRunRate = standing.NetRunRate
                    };

                    makeLeagueTable.Add(table);
                }

                List<CricketLeagueTable> leagueTable = new List<CricketLeagueTable>();
                var tablePositions = makeLeagueTable.OrderByDescending(e => e.Points).ThenByDescending(e => e.NetRunRate);
                int position = 1;
                foreach (var table in tablePositions)
                {
                    table.Position = position;
                    leagueTable.Add(table);

                    position++;
                }

                return leagueTable;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPremierLeagueTable");
            }

            return null;
        }

        public async Task<PremierLeagueTable> GetPremierLeagueTable(string teamID)
        {
            return await _context.PremierLeagueTable.FirstOrDefaultAsync(e => e.TeamID == teamID);
        }

        public async Task<FirstDivisionTable> GetFirstDivisionTable(string teamID)
        {
            return await _context.FirstDivisionTable.FirstOrDefaultAsync(e => e.TeamID == teamID);
        }

        public async Task<CricketLeagueTable> GetCricketPremierLeagueTable(string teamID)
        {
            try
            {
                var cricketLeagueStanding = await _context.CricketLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.League.Name == "Premier Division").ToListAsync();

                List<CricketLeagueTable> makeLeagueTable = new List<CricketLeagueTable>();

                foreach(var standing in cricketLeagueStanding)
                {
                    CricketLeagueTable table = new CricketLeagueTable
                    {
                        TeamID = standing.TeamID,
                        Team = standing.Team,
                        TeamLogo = standing.Team.TeamLogo,
                        Name = standing.Team.Name,
                        LeagueID = standing.LeagueID,
                        League = standing.League,
                        LeagueName = standing.League.Name,
                        SeasonID = standing.SeasonID,
                        Season = standing.Season,
                        SeasonDate = standing.Season.Date,
                        Wins = standing.Wins,
                        Loss = standing.Loss,
                        Draws = standing.Draws,
                        Played = standing.Played,
                        IsSelectedTeam = standing.TeamID == teamID,
                        Points = standing.Points,
                        NetRunRate = standing.NetRunRate
                    };

                    makeLeagueTable.Add(table);
                }

                List<CricketLeagueTable> leagueTable = new List<CricketLeagueTable>();
                var tablePositions = makeLeagueTable.OrderByDescending(e => e.Points).ThenByDescending(e => e.NetRunRate);
                int position = 1;
                foreach(var table in tablePositions)
                {
                    table.Position = position;
                    leagueTable.Add(table);

                    position++;
                }

                return leagueTable.FirstOrDefault(e => e.TeamID == teamID);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPremierLeagueTable");
            }

            return null;
        }

        public async Task<CricketLeagueTable> GetCricketFirstDivisionTable(string teamID)
        {
            try
            {
                var cricketLeagueStanding = await _context.CricketLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.League.Name == "First Division").ToListAsync();

                List<CricketLeagueTable> makeLeagueTable = new List<CricketLeagueTable>();

                foreach (var standing in cricketLeagueStanding)
                {
                    CricketLeagueTable table = new CricketLeagueTable
                    {
                        TeamID = standing.TeamID,
                        Team = standing.Team,
                        TeamLogo = standing.Team.TeamLogo,
                        Name = standing.Team.Name,
                        LeagueID = standing.LeagueID,
                        League = standing.League,
                        LeagueName = standing.League.Name,
                        SeasonID = standing.SeasonID,
                        Season = standing.Season,
                        SeasonDate = standing.Season.Date,
                        Wins = standing.Wins,
                        Loss = standing.Loss,
                        Draws = standing.Draws,
                        Played = standing.Played,
                        IsSelectedTeam = standing.TeamID == teamID,
                        Points = standing.Points,
                        NetRunRate = standing.NetRunRate
                    };

                    makeLeagueTable.Add(table);
                }

                List<CricketLeagueTable> leagueTable = new List<CricketLeagueTable>();
                var tablePositions = makeLeagueTable.OrderByDescending(e => e.Points).ThenByDescending(e => e.NetRunRate);
                int position = 1;
                foreach (var table in tablePositions)
                {
                    table.Position = position;
                    leagueTable.Add(table);

                    position++;
                }

                return leagueTable.FirstOrDefault(e => e.TeamID == teamID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPremierLeagueTable");
            }

            return null;
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

        public async Task<List<LeagueTable>> GetTableByTeam(string teamID)
        {
            try
            {
                var currentSeason = await _context.TeamSeasons.FirstOrDefaultAsync(e => e.TeamID == teamID && e.Season.IsCurrent);

                try
                {
                    var league = await _context.Leagues.Include(e => e.Sport).FirstOrDefaultAsync();

                    var leagueTables = await _context.LeagueTable
                        .Include(e => e.Team)
                        .Include(e => e.Season)
                        .Include(e => e.League).Where(e => e.LeagueID == currentSeason.LeagueID).ToListAsync();

                    leagueTables.ForEach(e => e.IsSelectedTeam = e.TeamID == teamID);

                    return leagueTables;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "League Table");
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League Table");
            }

            return null;
        }

        public async Task<List<CricketLeagueTable>> GetCricketTableByTeam(string teamID)
        {
            try
            {
                var currentSeason = await _context.TeamSeasons.FirstOrDefaultAsync(e => e.TeamID == teamID && e.Season.IsCurrent);
                var cricketLeagueStanding = await _context.CricketLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.LeagueID == currentSeason.LeagueID).ToListAsync();

                List<CricketLeagueTable> makeLeagueTable = new List<CricketLeagueTable>();

                foreach (var standing in cricketLeagueStanding)
                {
                    CricketLeagueTable table = new CricketLeagueTable
                    {
                        TeamID = standing.TeamID,
                        Team = standing.Team,
                        TeamLogo = standing.Team.TeamLogo,
                        Name = standing.Team.Name,
                        LeagueID = standing.LeagueID,
                        League = standing.League,
                        LeagueName = standing.League.Name,
                        SeasonID = standing.SeasonID,
                        Season = standing.Season,
                        SeasonDate = standing.Season.Date,
                        Wins = standing.Wins,
                        Loss = standing.Loss,
                        Draws = standing.Draws,
                        Played = standing.Played,
                        IsSelectedTeam = standing.TeamID == teamID,
                        Points = standing.Points,
                        NetRunRate = standing.NetRunRate
                    };

                    makeLeagueTable.Add(table);
                }

                List<CricketLeagueTable> leagueTable = new List<CricketLeagueTable>();
                var tablePositions = makeLeagueTable.OrderByDescending(e => e.Points).ThenByDescending(e => e.NetRunRate);
                int position = 1;
                foreach (var table in tablePositions)
                {
                    table.Position = position;
                    leagueTable.Add(table);

                    position++;
                }

                return leagueTable.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketLeagueTable");
            }

            return null;
        }

        public async Task<IEnumerable<LeagueTable>> GetTablesByTeam(string teamID)
        {
            var currentSeason = await _context.TeamSeasons.FirstOrDefaultAsync(e => e.TeamID == teamID && e.Season.IsCurrent);

            try
            {
                var league = await _context.Leagues.Include(e => e.Sport).FirstOrDefaultAsync();

                    var leagueTables = await _context.LeagueTable
                        .Include(e => e.Team)
                        .Include(e => e.Season)
                        .Include(e => e.League).Where(e => e.LeagueID == currentSeason.LeagueID).ToListAsync();

                leagueTables.ForEach(e => e.IsSelectedTeam = e.TeamID == teamID);

                    return leagueTables;
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League Table");
            }

            return null;
        }

        public async Task<List<CricketLeagueTable>> GetCricketTablesByTeam(string teamID, string leagueID)
        {
            try
            {
                var league = await _context.Leagues.Include(e => e.Sport).FirstOrDefaultAsync();

                var leagueTables = await _context.CricketLeagueTable
                        .Include(e => e.Team)
                        .Include(e => e.Season)
                        .Include(e => e.League).Where(e => e.TeamID == teamID && e.LeagueID == leagueID).ToListAsync();

                return leagueTables;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League Table");
            }

            return null;
        }

        //Deprecated
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

        public async Task<List<LeagueTable>> GetFootballTableByLeague(string leagueID)
        {
            try
            {
                var leagueTables = await _context.LeagueTable
                        .Include(e => e.Team)
                        .Include(e => e.Season)
                        .Include(e => e.League).Where(e => e.LeagueID == leagueID).ToListAsync();

                return leagueTables;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "League Table");
                return null;
            }
        }

        public async Task<List<CricketLeagueTable>> GetCricketTableByLeague(string leagueID)
        {
            try
            {
                var cricketLeagueStanding = await _context.CricketLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.LeagueID == leagueID).ToListAsync();

                List<CricketLeagueTable> makeLeagueTable = new List<CricketLeagueTable>();

                foreach (var standing in cricketLeagueStanding)
                {
                    CricketLeagueTable table = new CricketLeagueTable
                    {
                        TeamID = standing.TeamID,
                        Team = standing.Team,
                        TeamLogo = standing.Team.TeamLogo,
                        Name = standing.Team.Name,
                        LeagueID = standing.LeagueID,
                        League = standing.League,
                        LeagueName = standing.League.Name,
                        SeasonID = standing.SeasonID,
                        Season = standing.Season,
                        SeasonDate = standing.Season.Date,
                        Wins = standing.Wins,
                        Loss = standing.Loss,
                        Draws = standing.Draws,
                        Played = standing.Played,
                        Points = standing.Points,
                        NetRunRate = standing.NetRunRate
                    };

                    makeLeagueTable.Add(table);
                }

                List<CricketLeagueTable> leagueTable = new List<CricketLeagueTable>();
                var tablePositions = makeLeagueTable.OrderByDescending(e => e.Points).ThenByDescending(e => e.NetRunRate);
                int position = 1;
                foreach (var table in tablePositions)
                {
                    table.Position = position;
                    leagueTable.Add(table);

                    position++;
                }

                return leagueTable;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPremierLeagueTable");
            }

            return null;
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
