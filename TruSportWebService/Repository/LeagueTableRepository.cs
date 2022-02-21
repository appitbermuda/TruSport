using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Basketball;
using OnTrackWebService.Models.Imports;
using MatchType = OnTrackWebService.Models.MatchType;

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

        public async Task<IEnumerable<BasketballLeagueStanding>> GetAllBasketball()
        {
            //return await _context.LeagueTables.FromSql("select * from leaguetable").ToListAsync();
            return await _context.BasketballLeagueStandings
                .Include(e => e.League)
                .Include(e => e.Season)
                .Include(e => e.Team).ToListAsync();
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

        public async Task<List<BasketballLeagueStanding>> GetBasketballStandings()
        {
            try
            {
                var basketballLeagueStandings = await _context.BasketballLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.Team.Name != "TBD").ToListAsync();

                //List<BowlingLeagueStanding> makeLeagueTable = new List<BowlingLeagueStanding>();

                //foreach (var standing in bowlingLeagueStandings)
                //{
                //    BowlingLeagueStanding table = new BowlingLeagueStanding
                //    {
                //        TeamID = standing.TeamID,
                //        Team = standing.Team,
                //        LeagueID = standing.LeagueID,
                //        League = standing.League,
                //        SeasonID = standing.SeasonID,
                //        Season = standing.Season,
                //        PointsWon = standing.PointsWon,
                //        PointsLost = standing.PointsLost,
                //        TeamAvg = standing.TeamAvg,
                //        ScratchPins = standing.ScratchPins,
                //        HighGame = standing.HighGame,
                //        HighSers = standing.HighSers,
                //        Week = standing.Week
                //    };

                //    makeLeagueTable.Add(table);
                //}

                var fixtureDates = await _context.BasketballFixtures.Select(e => e.Date).ToListAsync();
                int fixtureCount = fixtureDates.GroupBy(e => e.Date).Count();

                List<BasketballLeagueStanding> leagueTable = new List<BasketballLeagueStanding>();
                var tablePositions = basketballLeagueStandings.OrderByDescending(e => e.Percent).ThenByDescending(e => e.PointsDifference);
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
                Debug.WriteLine(ex.Message, "GetBasketballStandings");
            }

            return null;
        }

        public async Task<bool> SyncBasketballStandings()
        {
            try
            {
                var seasons = await _context.Seasons.Include(e => e.Sport).Where(e => e.Sport.Name == Constants.Basketball).ToListAsync();

                foreach (var season in seasons)
                {
                    var fixtures = await _context.BasketballFixtures
                        .Include(e => e.HomeTeam)
                        .Include(e => e.AwayTeam)
                        .Include(e => e.League)
                        .Include(e => e.Season)
                        .Include(e => e.MatchType)
                        .Where(e => e.SeasonID == season.ID && e.Date <= DateTime.Now.Date && e.MatchType.IsTable && !e.IsPostponed && !e.IsCancelled).ToListAsync();

                    foreach (var fixture in fixtures)
                    {
                        var previousFixtures = await _context.BasketballFixtures.Where(e => (e.HomeTeamID == fixture.HomeTeamID || e.AwayTeamID == fixture.HomeTeamID || e.HomeTeamID == fixture.AwayTeamID || e.AwayTeamID == fixture.AwayTeamID) && e.Date <= DateTime.Now && !e.IsPostponed && !e.IsCancelled).ToListAsync();
                        var teamStandings = await _context.BasketballLeagueStandings.Where(e => e.TeamID == fixture.HomeTeamID || e.TeamID == fixture.AwayTeamID).ToListAsync();
                        var teamScores = await _context.BasketballScores
                            .Include(e => e.BasketballFixture)
                            .Where(e => previousFixtures.Any(d => d.ID == e.BasketballFixtureID) && (e.HomeTeamScore > 0 || e.AwayTeamScore > 0)).ToListAsync();

                        int homeTeamPlayed = previousFixtures.Where(e => e.HomeTeamID == fixture.HomeTeamID || e.AwayTeamID == fixture.HomeTeamID).Count();
                        int awayTeamPlayed = previousFixtures.Where(e => e.HomeTeamID == fixture.AwayTeamID || e.AwayTeamID == fixture.AwayTeamID).Count();

                        var homeTeamStanding = teamStandings.FirstOrDefault(e => e.TeamID == fixture.HomeTeamID);
                        var awayTeamStanding = teamStandings.FirstOrDefault(e => e.TeamID == fixture.AwayTeamID);

                        homeTeamStanding.Played = homeTeamPlayed;
                        awayTeamStanding.Played = awayTeamPlayed;

                        homeTeamStanding.Win = teamScores.Where(e => (e.BasketballFixture.HomeTeamID == fixture.HomeTeamID && e.HomeTeamScore > e.AwayTeamScore) || (e.BasketballFixture.AwayTeamID == fixture.HomeTeamID && e.AwayTeamScore > e.HomeTeamScore)).Count();
                        homeTeamStanding.Loss = teamScores.Where(e => (e.BasketballFixture.HomeTeamID == fixture.HomeTeamID && e.AwayTeamScore > e.HomeTeamScore) || (e.BasketballFixture.AwayTeamID == fixture.HomeTeamID && e.HomeTeamScore > e.AwayTeamScore)).Count();
                        homeTeamStanding.Draw = teamScores.Where(e => (e.BasketballFixture.HomeTeamID == fixture.HomeTeamID && e.AwayTeamScore == e.HomeTeamScore) || (e.BasketballFixture.AwayTeamID == fixture.HomeTeamID && e.HomeTeamScore == e.AwayTeamScore)).Count();

                        awayTeamStanding.Win = teamScores.Where(e => (e.BasketballFixture.AwayTeamID == fixture.AwayTeamID && e.AwayTeamScore > e.HomeTeamScore) || (e.BasketballFixture.HomeTeamID == fixture.AwayTeamID && e.HomeTeamScore > e.AwayTeamScore)).Count();
                        awayTeamStanding.Loss = teamScores.Where(e => (e.BasketballFixture.AwayTeamID == fixture.AwayTeamID && e.HomeTeamScore > e.AwayTeamScore) || (e.BasketballFixture.HomeTeamID == fixture.AwayTeamID && e.AwayTeamScore > e.HomeTeamScore)).Count();
                        awayTeamStanding.Draw = teamScores.Where(e => (e.BasketballFixture.AwayTeamID == fixture.AwayTeamID && e.HomeTeamScore == e.AwayTeamScore) || (e.BasketballFixture.HomeTeamID == fixture.AwayTeamID && e.AwayTeamScore == e.HomeTeamScore)).Count();

                        homeTeamStanding.PointsFor = (teamScores.Where(e => e.BasketballFixture.HomeTeamID == fixture.HomeTeamID).Sum(e => e.HomeTeamScore) ?? 0) + (teamScores.Where(e => e.BasketballFixture.AwayTeamID == fixture.HomeTeamID).Sum(e => e.AwayTeamScore) ?? 0);
                        homeTeamStanding.PointsAgainst = (teamScores.Where(e => e.BasketballFixture.HomeTeamID == fixture.HomeTeamID).Sum(e => e.AwayTeamScore) ?? 0) + (teamScores.Where(e => e.BasketballFixture.AwayTeamID == fixture.HomeTeamID).Sum(e => e.HomeTeamScore) ?? 0);

                        awayTeamStanding.PointsFor = (teamScores.Where(e => e.BasketballFixture.AwayTeamID == fixture.AwayTeamID).Sum(e => e.AwayTeamScore) ?? 0) + (teamScores.Where(e => e.BasketballFixture.HomeTeamID == fixture.AwayTeamID).Sum(e => e.HomeTeamScore) ?? 0);
                        awayTeamStanding.PointsAgainst = (teamScores.Where(e => e.BasketballFixture.AwayTeamID == fixture.AwayTeamID).Sum(e => e.HomeTeamScore) ?? 0) + (teamScores.Where(e => e.BasketballFixture.HomeTeamID == fixture.AwayTeamID).Sum(e => e.AwayTeamScore) ?? 0);

                        _context.BasketballLeagueStandings.Update(homeTeamStanding);
                        _context.BasketballLeagueStandings.Update(awayTeamStanding);

                        await _context.SaveChangesAsync();

                        _context.Database.BeginTransaction();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "SyncBasketballStandings");
            }

            return false;
        }

        public async Task<List<BasketballLeagueStanding>> GetIslandFallBasketballLeagueStanding()
        {
            try
            {
                var basketballLeagueStandings = await _context.BasketballLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.Team.Name != "TBD").ToListAsync();

                //List<BowlingLeagueStanding> makeLeagueTable = new List<BowlingLeagueStanding>();

                //foreach (var standing in bowlingLeagueStandings)
                //{
                //    BowlingLeagueStanding table = new BowlingLeagueStanding
                //    {
                //        TeamID = standing.TeamID,
                //        Team = standing.Team,
                //        LeagueID = standing.LeagueID,
                //        League = standing.League,
                //        SeasonID = standing.SeasonID,
                //        Season = standing.Season,
                //        PointsWon = standing.PointsWon,
                //        PointsLost = standing.PointsLost,
                //        TeamAvg = standing.TeamAvg,
                //        ScratchPins = standing.ScratchPins,
                //        HighGame = standing.HighGame,
                //        HighSers = standing.HighSers,
                //        Week = standing.Week
                //    };

                //    makeLeagueTable.Add(table);
                //}

                var fixtureDates = await _context.BasketballFixtures.Select(e => e.Date).ToListAsync();
                int fixtureCount = fixtureDates.GroupBy(e => e.Date).Count();

                List<BasketballLeagueStanding> leagueTable = new List<BasketballLeagueStanding>();
                var tablePositions = basketballLeagueStandings.OrderByDescending(e => e.Percent).ThenByDescending(e => e.PointsDifference);
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
                Debug.WriteLine(ex.Message, "IslandFallBasketballLeague");
            }

            return null;
        }

        public async Task<List<BowlingLeagueStanding>> GetBowlingStandings()
        {
            try
            {
                var bowlingLeagueStandings = await _context.BowlingLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.Team.Name != "TBD").ToListAsync();

                //List<BowlingLeagueStanding> makeLeagueTable = new List<BowlingLeagueStanding>();

                //foreach (var standing in bowlingLeagueStandings)
                //{
                //    BowlingLeagueStanding table = new BowlingLeagueStanding
                //    {
                //        TeamID = standing.TeamID,
                //        Team = standing.Team,
                //        LeagueID = standing.LeagueID,
                //        League = standing.League,
                //        SeasonID = standing.SeasonID,
                //        Season = standing.Season,
                //        PointsWon = standing.PointsWon,
                //        PointsLost = standing.PointsLost,
                //        TeamAvg = standing.TeamAvg,
                //        ScratchPins = standing.ScratchPins,
                //        HighGame = standing.HighGame,
                //        HighSers = standing.HighSers,
                //        Week = standing.Week
                //    };

                //    makeLeagueTable.Add(table);
                //}

                var fixtureDates = await _context.BowlingFixtures.Select(e => e.Date).ToListAsync();
                int fixtureCount = fixtureDates.GroupBy(e => e.Date).Count();

                List<BowlingLeagueStanding> leagueTable = new List<BowlingLeagueStanding>();
                var tablePositions = bowlingLeagueStandings.OrderByDescending(e => e.PointsWon).ThenByDescending(e => e.TeamAvg);
                int position = 1;
                foreach (var table in tablePositions)
                {
                    table.Position = position;
                    table.WeekUpdated = "Week " + table.Week +" of " + fixtureCount;
                    leagueTable.Add(table);

                    position++;
                }

                return leagueTable;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBowlingStandings");
            }

            return null;
        }

        public async Task<List<BowlingLeagueStanding>> GetBowlingLeagueStandings(string leagueID)
        {
            try
            {
                var bowlingLeagueStandings = await _context.BowlingLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.League.ID == leagueID && e.Team.Name != "TBD").ToListAsync();

                List<BowlingLeagueStanding> makeLeagueTable = new List<BowlingLeagueStanding>();

                foreach (var standing in bowlingLeagueStandings)
                {
                    BowlingLeagueStanding table = new BowlingLeagueStanding
                    {
                        TeamID = standing.TeamID,
                        Team = standing.Team,
                        LeagueID = standing.LeagueID,
                        League = standing.League,
                        SeasonID = standing.SeasonID,
                        Season = standing.Season,
                        PointsWon = standing.PointsWon,
                        PointsLost = standing.PointsLost,
                        TeamAvg = standing.TeamAvg,
                        ScratchPins = standing.ScratchPins,
                        HighGame = standing.HighGame,
                        HighSers = standing.HighSers
                    };

                    makeLeagueTable.Add(table);
                }

                List<BowlingLeagueStanding> leagueTable = new List<BowlingLeagueStanding>();
                var tablePositions = makeLeagueTable.OrderByDescending(e => e.PointsWon).ThenByDescending(e => e.TeamAvg);
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
                Debug.WriteLine(ex.Message, "GetBowlingLeagueStandings");
            }

            return null;
        }

        public async Task<List<BasketballLeagueStanding>> GetBasketballLeagueStandings(string leagueID)
        {
            try
            {
                var basketballLeagueStandings = await _context.BasketballLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.League.ID == leagueID && e.Team.Name != "TBD").ToListAsync();

                //List<BasketballLeagueStanding> makeLeagueTable = new List<BasketballLeagueStanding>();

                //foreach (var standing in basketballLeagueStandings)
                //{
                //    BasketballLeagueStanding table = new BasketballLeagueStanding
                //    {
                //        TeamID = standing.TeamID,
                //        Team = standing.Team,
                //        LeagueID = standing.LeagueID,
                //        League = standing.League,
                //        SeasonID = standing.SeasonID,
                //        Season = standing.Season,
                //        PointsFor = standing.PointsFor,
                //        PointsAgainst = standing.PointsAgainst,
                //        Win = standing.Win,
                //        Loss = standing.Loss,
                //        Draw = standing.Draw,
                //        Played = standing.Played                    
                //    };

                //    makeLeagueTable.Add(table);
                //}

                List<BasketballLeagueStanding> leagueTable = new List<BasketballLeagueStanding>();
                var tablePositions = basketballLeagueStandings.OrderByDescending(e => e.Percent).ThenByDescending(e => e.PointsDifference);
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
                Debug.WriteLine(ex.Message, "GetBasketballLeagueStandings");
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
                    .Where(e => e.League.Name == "Premier Division" && e.Team.Name != "TBD").ToListAsync();

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
                    .Where(e => e.League.Name == "First Division" && e.Team.Name != "TBD").ToListAsync();

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
                    .Where(e => e.League.Name == "Premier Division" && e.Team.Name != "TBD").ToListAsync();

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
                    .Where(e => e.League.Name == "First Division" && e.Team.Name != "TBD").ToListAsync();

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
                        .Include(e => e.League).Where(e => e.LeagueID == currentSeason.LeagueID && e.Team.Name != "TBD").ToListAsync();

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
                    .Where(e => e.LeagueID == currentSeason.LeagueID && e.Team.Name != "TBD").ToListAsync();

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

        public async Task<List<BasketballLeagueStanding>> GetBasketballTableByTeam(string teamID)
        {
            try
            {
                var currentSeason = await _context.TeamSeasons.FirstOrDefaultAsync(e => e.TeamID == teamID && e.Season.IsCurrent);
                var basketballLeagueStanding = await _context.BasketballLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.LeagueID == currentSeason.LeagueID && e.Team.Name != "TBD").ToListAsync();

                basketballLeagueStanding.ForEach(e => e.IsSelectedTeam = e.TeamID == teamID);

                //List<BowlingLeagueStanding> makeLeagueTable = new List<BowlingLeagueStanding>();

                //foreach (var standing in bowlingLeagueStanding)
                //{
                //    BowlingLeagueStanding table = new BowlingLeagueStanding
                //    {
                //        TeamID = standing.TeamID,
                //        Team = standing.Team,
                //        LeagueID = standing.LeagueID,
                //        League = standing.League,
                //        SeasonID = standing.SeasonID,
                //        Season = standing.Season,
                //        PointsWon = standing.PointsWon,
                //        PointsLost = standing.Loss,
                //        Draws = standing.Draws,
                //        Played = standing.Played,
                //        IsSelectedTeam = standing.TeamID == teamID,
                //        Points = standing.Points,
                //        NetRunRate = standing.NetRunRate
                //    };

                //    makeLeagueTable.Add(table);
                //}

                List<BasketballLeagueStanding> leagueTable = new List<BasketballLeagueStanding>();
                var tablePositions = basketballLeagueStanding.OrderByDescending(e => e.Percent).ThenByDescending(e => e.PointsDifference);
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
                Debug.WriteLine(ex.Message, "GetBasketballLeagueTable");
            }

            return null;
        }

        public async Task<List<BowlingLeagueStanding>> GetBowlingTableByTeam(string teamID)
        {
            try
            {
                var currentSeason = await _context.BowlingTeamSeasons.FirstOrDefaultAsync(e => e.BowlingTeamID == teamID && e.Season.IsCurrent);
                var bowlingLeagueStanding = await _context.BowlingLeagueStandings
                    .Include(e => e.Team)
                    .Include(e => e.Season)
                    .Include(e => e.League)
                    .Where(e => e.LeagueID == currentSeason.LeagueID && e.Team.Name != "TBD").ToListAsync();

                bowlingLeagueStanding.ForEach(e => e.IsSelectedTeam = e.TeamID == teamID);

                //List<BowlingLeagueStanding> makeLeagueTable = new List<BowlingLeagueStanding>();

                //foreach (var standing in bowlingLeagueStanding)
                //{
                //    BowlingLeagueStanding table = new BowlingLeagueStanding
                //    {
                //        TeamID = standing.TeamID,
                //        Team = standing.Team,
                //        LeagueID = standing.LeagueID,
                //        League = standing.League,
                //        SeasonID = standing.SeasonID,
                //        Season = standing.Season,
                //        PointsWon = standing.PointsWon,
                //        PointsLost = standing.Loss,
                //        Draws = standing.Draws,
                //        Played = standing.Played,
                //        IsSelectedTeam = standing.TeamID == teamID,
                //        Points = standing.Points,
                //        NetRunRate = standing.NetRunRate
                //    };

                //    makeLeagueTable.Add(table);
                //}

                List<BowlingLeagueStanding> leagueTable = new List<BowlingLeagueStanding>();
                var tablePositions = bowlingLeagueStanding.OrderByDescending(e => e.PointsWon).ThenBy(e => e.PointsLost);
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
                Debug.WriteLine(ex.Message, "GetBowlingLeagueTable");
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
                        .Include(e => e.League).Where(e => e.LeagueID == currentSeason.LeagueID && e.Team.Name != "TBD").ToListAsync();

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
                        .Include(e => e.League).Where(e => e.TeamID == teamID && e.LeagueID == leagueID && e.Team.Name != "TBD").ToListAsync();

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

        public async Task<ImportBowlingLeagueStandings> UploadBowlingLeagueStandings(IFormFile file)
        {
            try
            {
                List<BowlingLeagueStandings> errorStandings = new List<BowlingLeagueStandings>();
                List<BowlingLeagueStanding> standings = new List<BowlingLeagueStanding>();
                List<BowlingLeagueStanding> addStandings = new List<BowlingLeagueStanding>();
                List<BowlingTeam> teams = await _context.BowlingTeams.ToListAsync();
                List<League> leagues = await _context.Leagues.Include(e => e.Sport).Where(e => e.Sport.Name == "Bowling").ToListAsync();
                List<BowlingLeagueStanding> leagueStandings = await _context.BowlingLeagueStandings.AsNoTracking().ToListAsync();
                List<Season> seasons = await _context.Seasons.Include(e => e.Sport).Where(e => e.Sport.Name == "Bowling").ToListAsync();
                League league = null;
                BowlingTeam team = null;
                Season season = null;
                BowlingLeagueStanding leagueStanding = null;

                //Stream reader = file.OpenReadStream();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;
                    var records = csv.GetRecords<BowlingLeagueStandings>();

                    foreach (var record in records)
                    {
                        try
                        {
                            league = leagues.FirstOrDefault(e => e.Name == record.League.Trim());
                            team = teams.FirstOrDefault(e => (e.TeamID == record.TeamID));
                            season = seasons.FirstOrDefault(e => e.IsCurrent);
                            leagueStanding = leagueStandings.FirstOrDefault(e => e.TeamID == team.ID);

                            if (leagueStanding != null)
                            {
                                standings.Add(new BowlingLeagueStanding
                                {
                                    ID = leagueStanding.ID,
                                    TeamID = team.ID,
                                    PointsWon = record.PointsWon,
                                    PointsLost = record.PointsLost,
                                    TeamAvg = record.TeamAvg,
                                    ScratchPins = record.ScratchPins,
                                    HighGame = record.HighGame,
                                    HighSers = record.HighSeries,
                                    LeagueID = league.ID,
                                    SeasonID = season.ID,
                                    Week = record.Week
                                    //SportID = season.SportID
                                });
                            }
                            else
                            {
                                addStandings.Add(new BowlingLeagueStanding
                                {
                                    TeamID = team.ID,
                                    PointsWon = record.PointsWon,
                                    PointsLost = record.PointsLost,
                                    TeamAvg = record.TeamAvg,
                                    ScratchPins = record.ScratchPins,
                                    HighGame = record.HighGame,
                                    HighSers = record.HighSeries,
                                    LeagueID = league.ID,
                                    SeasonID = season.ID,
                                    Week = record.Week
                                    //SportID = season.SportID
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorStandings.Add(record);

                        }
                    }

                    try
                    {
                        if(addStandings.Count > 0)
                            await AddStandings(addStandings);

                        if(standings.Count > 0)
                            await UpdateStandings(standings);
                    }
                    catch (Exception ex)
                    {
                        return new ImportBowlingLeagueStandings
                        {
                            Message = "Error updating standings.",
                            Exception = ex.Message
                        };
                    }
                }

                if (errorStandings != null && errorStandings.Count > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorStandings);
                        streamWriter.Flush();

                        return new ImportBowlingLeagueStandings
                        {
                            Message = "Successfully updated standings with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorStandings,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportBowlingLeagueStandings
                {
                    Message = "Successfully updated standings!"
                };

            }
            catch (Exception ex)
            {
                return new ImportBowlingLeagueStandings
                {
                    Message = "Error updating standings!",
                    Exception = ex.Message
                };
            }


        }

        public async Task<ImportBasketballLeagueStandings> UploadBasketballLeagueStandings(IFormFile file)
        {
            try
            {
                List<BasketballLeagueStandings> errorStandings = new List<BasketballLeagueStandings>();
                List<BasketballLeagueStanding> standings = new List<BasketballLeagueStanding>();
                List<BasketballLeagueStanding> addStandings = new List<BasketballLeagueStanding>();
                List<Team> teams = await _context.Teams.Include(e => e.Sport).Where(e => e.Sport.Name == Constants.Basketball).ToListAsync();
                List<League> leagues = await _context.Leagues.Include(e => e.Sport).Where(e => e.Sport.Name == "Bowling").ToListAsync();
                List<BasketballLeagueStanding> leagueStandings = await _context.BasketballLeagueStandings.AsNoTracking().ToListAsync();
                List<Season> seasons = await _context.Seasons.Include(e => e.Sport).Where(e => e.Sport.Name == "Bowling").ToListAsync();
                League league = null;
                Team team = null;
                Season season = null;
                BasketballLeagueStanding leagueStanding = null;

                //Stream reader = file.OpenReadStream();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;
                    var records = csv.GetRecords<BasketballLeagueStandings>();

                    foreach (var record in records)
                    {
                        try
                        {
                            league = leagues.FirstOrDefault(e => e.Name == record.League.Trim());
                            team = teams.FirstOrDefault(e => (e.Name == record.Team));
                            season = seasons.FirstOrDefault(e => e.IsCurrent);
                            leagueStanding = leagueStandings.FirstOrDefault(e => e.TeamID == team.ID);

                            if (leagueStanding != null)
                            {
                                standings.Add(new BasketballLeagueStanding
                                {
                                    ID = leagueStanding.ID,
                                    TeamID = team.ID,
                                    Win = record.Win,
                                    Loss = record.Loss,
                                    Draw = record.Draw,
                                    //Points = record.Points,
                                    PointsFor = record.PointsFor,
                                    PointsAgainst = record.PointsAgainst,
                                    LeagueID = league.ID,
                                    SeasonID = season.ID,
                                    //SportID = season.SportID
                                });
                            }
                            else
                            {
                                addStandings.Add(new BasketballLeagueStanding
                                {
                                    TeamID = team.ID,
                                    Win = record.Win,
                                    Loss = record.Loss,
                                    Draw = record.Draw,
                                    //Points = record.Points,
                                    PointsFor = record.PointsFor,
                                    PointsAgainst = record.PointsAgainst,
                                    LeagueID = league.ID,
                                    SeasonID = season.ID,
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorStandings.Add(record);

                        }
                    }

                    try
                    {
                        if (addStandings.Count > 0)
                            await AddStandings(addStandings);

                        if (standings.Count > 0)
                            await UpdateStandings(standings);
                    }
                    catch (Exception ex)
                    {
                        return new ImportBasketballLeagueStandings
                        {
                            Message = "Error updating standings.",
                            Exception = ex.Message
                        };
                    }
                }

                if (errorStandings != null && errorStandings.Count > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorStandings);
                        streamWriter.Flush();

                        return new ImportBasketballLeagueStandings
                        {
                            Message = "Successfully updated standings with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorStandings,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportBasketballLeagueStandings
                {
                    Message = "Successfully updated standings!"
                };

            }
            catch (Exception ex)
            {
                return new ImportBasketballLeagueStandings
                {
                    Message = "Error updating standings!",
                    Exception = ex.Message
                };
            }


        }

        public async Task AddStandings(List<BasketballLeagueStanding> items)
        {
            try
            {
                _context.BasketballLeagueStandings.AddRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Basketball Standings");
            }
        }

        public async Task AddStandings(List<BowlingLeagueStanding> items)
        {
            try
            {
                _context.BowlingLeagueStandings.AddRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Bowling Standings");
            }
        }

        public async Task UpdateStandings(List<BasketballLeagueStanding> items)
        {
            try
            {
                _context.BasketballLeagueStandings.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Basketball Standings");
            }
        }

        public async Task UpdateStandings(List<BowlingLeagueStanding> items)
        {
            try
            {
                _context.BowlingLeagueStandings.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Bowling Standings");
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
