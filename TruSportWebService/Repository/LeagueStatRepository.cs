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
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Imports;

namespace OnTrackWebService.Repository
{
    public class LeagueStatRepository : IOnTrackRepository<LeagueStat>
    {
        OnTrackContext _context;
        TeamRepository teamRepository;
        SeasonRepository seasonRepository;
        PlayerRepository playerRepository;

        public LeagueStatRepository(OnTrackContext context)
        {
            _context = context;
            teamRepository = new TeamRepository(context);
            seasonRepository = new SeasonRepository(context);
            playerRepository = new PlayerRepository(context);
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
            try
            {
                //return await _context.LeagueStat.FromSql("select * from leaguetable").ToListAsync();
                var season = await _context.Seasons.FirstOrDefaultAsync(e => e.IsCurrent && e.Sport.Name == "Football");

                var goalsConceded = await _context.GoalsConcededByTeam.Where(e => e.SeasonDate == season.Date).OrderByDescending(e => e.Goals).ToListAsync();

                return goalsConceded;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<TopScoresByTeam>> GetTopScoresByTeam()
        {
            try
            {
                //return await _context.LeagueStat.FromSql("select * from leaguetable").ToListAsync();
                var season = await _context.Seasons.FirstOrDefaultAsync(e => e.IsCurrent && e.Sport.Name == "Bowling");

                //var topScores = await _context.TopScoresByTeam.Where(e => e.SeasonDate == season.Date).OrderByDescending(e => e.Goals).ToListAsync();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<GoalsScoredByTeam>> GetGoalsScoredByTeam()
        {
            try
            {
                //return await _context.LeagueStat.FromSql("select * from leaguetable").ToListAsync();
                var season = await _context.Seasons.FirstOrDefaultAsync(e => e.IsCurrent && e.Sport.Name == "Football");

                var goalsScored = await _context.GoalsScoredByTeam.Where(e => e.SeasonDate == season.Date).OrderByDescending(e => e.Goals).ToListAsync();

                return goalsScored;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<GoalsScoredByPlayer>> GetGoalsScoredByPlayer()
        {
            try
            {
                
                //var season = await _context.Seasons.FirstOrDefaultAsync(e => e.IsCurrent);

                //var goalsScored = await _context.GoalsScoredByPlayer.Where(e => e.SeasonDate == season.Date).OrderByDescending(e => e.Goals).ToListAsync();
                var season = await _context.Seasons.FirstOrDefaultAsync(e => e.IsCurrent && e.Sport.Name == "Football");

                var goalsScored = await _context.GoalsScoredByPlayer.Where(e => e.SeasonDate == season.Date).OrderByDescending(e => e.Goals).ToListAsync();

                return goalsScored;
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
                var runsStats = await _context.RunsByPlayer.ToListAsync();
                return runsStats.OrderByDescending(e => e.Stat).ThenByDescending(e => e.Rate).ToList();
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
                var wicketsStats = await _context.WicketsByPlayer.ToListAsync();
                return wicketsStats.OrderByDescending(e => e.Stat).ThenBy(e => e.Rate).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<BowlingSeasonHG>> GetBowlingSeasonHG()
        {
            try
            {
                var bowlingHGs = await _context.BowlingSeasonHG.ToListAsync();
                return bowlingHGs.OrderByDescending(e => e.Stat).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<BowlingSeasonHS>> GetBowlingSeasonHS()
        {
            try
            {
                var bowlingHSs = await _context.BowlingSeasonHS.ToListAsync();
                return bowlingHSs.OrderByDescending(e => e.Stat).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<BowlingSeasonHG>> GetBowlingSeasonHGByTeam(string TeamID)
        {
            try
            {
                var bowlingHGs = await _context.BowlingSeasonHG.Where(e => e.TeamID == TeamID).ToListAsync();
                return bowlingHGs.OrderByDescending(e => e.Stat).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<BowlingSeasonHS>> GetBowlingSeasonHSByTeam(string TeamID)
        {
            try
            {
                var bowlingHSs = await _context.BowlingSeasonHS.Where(e => e.TeamID == TeamID).ToListAsync();
                return bowlingHSs.OrderByDescending(e => e.Stat).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<BowlingSeasonTeamHG>> GetBowlingSeasonTeamHG()
        {
            try
            {
                var bowlingHGs = await _context.BowlingSeasonTeamHG.ToListAsync();
                return bowlingHGs.OrderByDescending(e => e.Stat).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<BowlingSeasonTeamHS>> GetBowlingSeasonTeamHS()
        {
            try
            {
                var bowlingHSs = await _context.BowlingSeasonTeamHS.ToListAsync();
                return bowlingHSs.OrderByDescending(e => e.Stat).ToList();
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

        public async Task<ImportGoalStats> UploadGoalStats(IFormFile file)
        {
            try
            {
                List<GoalStats> errorGoals = new List<GoalStats>();
                List<GoalStats> errorRosters = new List<GoalStats>();
                List<MatchRoster> rosters = new List<MatchRoster>();
                List<MatchStat> matchStats = new List<MatchStat>();
                List<PlayerSeason> footballPlayers = new List<PlayerSeason>();
                List<MatchRoster> matchRosters = await _context.MatchRosters.Include(e => e.Player).ToListAsync();
                List<Team> teams = await teamRepository.GetFootballTeams();
                List<Season> seasons = await seasonRepository.GetFootballSeason();
                List<Player> players = await _context.Players.ToListAsync();
                List<League> leagues = await _context.Leagues.Include(e => e.Sport).Where(e => e.Sport.Name == "Football").ToListAsync();
                List<Fixture> fixtures = await _context.Fixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .ToListAsync();
                List<PlayerSeason> playerSeasons = await _context.PlayerSeasons.Include(e => e.Player).Include(e => e.Season).Where(e => e.Season.IsCurrent).ToListAsync();
                Team team = null;
                Season season = seasons.FirstOrDefault(e => e.IsCurrent);
                Fixture fixture = null;
                MatchRoster matchRoster = null;
                PlayerSeason footballPlayer = null;
                Player player = null;
                League league = null;
                List<Player> newPlayers = new List<Player>();
                List<PlayerSeason> updatePlayers = new List<PlayerSeason>();
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;
                    var records = csv.GetRecords<GoalStats>();

                    foreach (var record in records)
                    {
                        try
                        {
                            league = leagues.FirstOrDefault(e => e.Name.Contains(record.League));
                            team = teams.FirstOrDefault(e => (e.Name.Replace("'", "") == record.Team.Replace("'", "") || e.Alias == record.Team) && e.LeagueID == league.ID);
                            var homeTeam = teams.FirstOrDefault(e => (e.Name.Replace("'", "") == record.HomeTeam.Replace("'","") || e.Alias == record.HomeTeam) && e.LeagueID == league.ID);
                            var awayTeam = teams.FirstOrDefault(e => (e.Name.Replace("'", "") == record.AwayTeam.Replace("'", "") || e.Alias == record.AwayTeam) && e.LeagueID == league.ID);
                            fixture = fixtures.FirstOrDefault(e => (e.HomeTeamID == homeTeam.ID && e.AwayTeamID == awayTeam.ID) && e.SeasonID == season.ID && e.LeagueID == league.ID);

                            player = players.FirstOrDefault(e => (e.FirstName + " " + e.LastName) == record.Name);

                            if (player == null && newPlayers.Count > 0)
                            {
                                player = newPlayers.FirstOrDefault(e => (e.FirstName + " " + e.LastName) == record.Name);
                            }

                            if (player == null && ((newPlayers.Count > 0 && !newPlayers.Any(e => (e.FirstName + " " + e.LastName) == record.Name)) || newPlayers == null || newPlayers.Count == 0))
                            {
                                var playerArray = record.Name.Split(' ');

                                player = new Player
                                {
                                    FirstName = playerArray.Count() > 2 ? playerArray[0] + " " + playerArray[1] : playerArray[0],
                                    LastName = playerArray.Count() > 2 ? playerArray[2] : playerArray.Count() == 1 ? string.Empty : playerArray[1],
                                };

                                newPlayers.Add(player);

                                _context.Players.Add(player);
                                await _context.SaveChangesAsync();

                                footballPlayer = playerSeasons.FirstOrDefault(e => e.Player.FirstName == player.FirstName && e.Player.LastName == player.LastName);

                                //bowlingPlayer = bowlingPlayerSeasons.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());

                                if (footballPlayer == null)
                                {
                                    footballPlayer = footballPlayers.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());
                                }
                            }
                            else
                            {
                                footballPlayer = playerSeasons.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());

                                if (footballPlayer == null)
                                {
                                    footballPlayer = footballPlayers.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());
                                }
                            }

                            if (player.Name != "Own Goal")
                            {
                                if (footballPlayer == null)
                                {
                                    footballPlayer = new PlayerSeason
                                    {
                                        PlayerID = player.ID,
                                        TeamID = team.ID,
                                        SeasonID = season.ID,
                                        GamesPlayed = 1,
                                        Goals = record.Goals,
                                        IsActive = true
                                    };

                                    footballPlayers.Add(footballPlayer);

                                    _context.PlayerSeasons.Add(footballPlayer);
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    footballPlayer.TeamID = team.ID;
                                    footballPlayer.Goals += record.Goals;
                                    footballPlayer.GamesPlayed += 1;

                                    updatePlayers.Add(footballPlayer);
                                }
                            }

                            if (fixture != null)
                            {
                                var thisRoster = matchRosters.FirstOrDefault(e => e.FixtureID == fixture.ID && record.Name == (e.Player.FirstName + " " + e.Player.LastName));

                                if (thisRoster == null)
                                    matchRoster = rosters.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName) == record.Name && e.FixtureID == fixture.ID);
                                else
                                    matchRoster = thisRoster;

                                if (matchRoster == null)
                                {
                                    //if (player.Name != "Own Goal")
                                    //{
                                        footballPlayer = playerSeasons.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());

                                        if (footballPlayer == null)
                                        {
                                            footballPlayer = footballPlayers.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());
                                        }

                                        matchRoster = new MatchRoster
                                        {
                                            Player = footballPlayer != null ? footballPlayer.Player : player,
                                            PlayerID = footballPlayer != null ? footballPlayer.ID : player.ID,
                                            FixtureID = fixture.ID,
                                            TeamID = team.ID,
                                            IsStarter = true
                                        };
                                    //}
                                    //else
                                    //{
                                    //    matchRoster = new MatchRoster
                                    //    {
                                    //        Player = player,
                                    //        PlayerID = player.ID,
                                    //        FixtureID = fixture.ID,
                                    //        TeamID = team.ID,
                                    //        IsStarter = true
                                    //    };
                                    //}

                                    rosters.Add(matchRoster);
                                    _context.MatchRosters.Add(matchRoster);
                                    await _context.SaveChangesAsync();
                                }

                                try
                                {

                                    for (var i = 0; i < record.Goals; i++)
                                    {
                                        var matchStat = new MatchStat
                                        {
                                            MatchRosterID = matchRoster.ID,
                                            Goal = 1,
                                            GoalTime = record.GoalTime ?? -1,
                                            IsOwnGoal = player.Name == "Own Goal" ? true : false
                                        };

                                        matchStats.Add(matchStat);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    record.Exception = ex.Message;
                                    errorRosters.Add(record);

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorGoals.Add(record);
                        }
                    }

                    try
                    {
                        if (updatePlayers.Count > 0)
                        {
                            await UpdatePlayers(updatePlayers);
                        }

                        if (matchStats.Count > 0)
                        {
                            await AddMatchStats(matchStats);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new ImportGoalStats
                        {
                            Message = "Error importing games to database.",
                            Exception = ex.Message
                        };
                    }
                }

                if (errorGoals != null && errorGoals.Count > 0 && errorRosters != null && errorRosters.Count > 0)
                {

                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorGoals);
                        streamWriter.Flush();

                        return new ImportGoalStats
                        {
                            Message = "Successfully imported games with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorGoals,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportGoalStats
                {
                    Message = "Successfully imported games!"
                };

            }
            catch (Exception ex)
            {
                return new ImportGoalStats
                {
                    Message = "Error importing games!",
                    Exception = ex.Message
                };
            }
        }

        public async Task<ImportRunStats> UploadRunStats(IFormFile file)
        {
            try
            {
                List<RunStats> errorStats = new List<RunStats>();
                List<CricketPlayerSeason> cricketPlayerSeasons = new List<CricketPlayerSeason>();
                List<CricketPlayerSeason> updateCricketPlayerSeasons = new List<CricketPlayerSeason>();
                List<Team> teams = await teamRepository.GetCricketTeams();
                List<Player> players = await playerRepository.GetAll();
                Sport sport = await _context.Sports.FirstOrDefaultAsync(e => e.Name == "Cricket");
                List<Season> seasons = await seasonRepository.GetCricketSeason();
                string newTeamName = String.Empty;
                Team team = null;
                Season season = seasons.FirstOrDefault(e => e.IsCurrent);
                Player player = null;

                //Stream reader = file.OpenReadStream();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;

                    var records = csv.GetRecords<RunStats>();

                    foreach (var record in records)
                    {
                        try
                        {
                            //newTeamName =
                            team = teams.FirstOrDefault(e => (e.Name.Replace("'", "").Replace("-","") == record.Team.Replace("'", "").Replace("-", "").Trim() || e.Alias == record.Team.Trim()));
                            //team = teams.FirstOrDefault(e => e.Name == record.Team || e.Alias == record.Team);
                            player = players.FirstOrDefault(e => e.FirstName.ToLower() == record.Firstname.ToLower() && e.LastName.ToLower() == record.Lastname.ToLower());

                            if(player == null)
                            {
                                if(team != null)
                                {
                                    Player newPlayer = new Player
                                    {
                                        FirstName = record.Firstname,
                                        LastName = record.Lastname                                        
                                    };

                                    _context.Players.Add(newPlayer);
                                    _context.SaveChanges();

                                    CricketPlayerSeason newCricketPlayerSeason = new CricketPlayerSeason
                                    {
                                        PlayerID = newPlayer.ID,
                                        TeamID = team.ID,
                                        SeasonID = season.ID,
                                        GamesPlayed = record.GamesPlayed,
                                        IsActive = true,
                                        Runs = record.Runs,
                                        BallsFaced = record.BallsFaced
                                    };

                                    cricketPlayerSeasons.Add(newCricketPlayerSeason);
                                }
                                else
                                {
                                    errorStats.Add(record);
                                }
                            }
                            else
                            {
                                CricketPlayerSeason playerSeason = await _context.CricketPlayerSeasons.FirstOrDefaultAsync(e => e.PlayerID == player.ID);

                                if(playerSeason == null)
                                {
                                    CricketPlayerSeason newCricketPlayerSeason = new CricketPlayerSeason
                                    {
                                        PlayerID = player.ID,
                                        TeamID = team.ID,
                                        SeasonID = season.ID,
                                        GamesPlayed = record.GamesPlayed,
                                        IsActive = true,
                                        Runs = record.Runs,
                                        BallsFaced = record.BallsFaced
                                    };

                                    //_context.CricketPlayerSeasons.Add(newCricketPlayerSeason);
                                    //_context.SaveChanges();

                                    cricketPlayerSeasons.Add(newCricketPlayerSeason);
                                }
                                else
                                {
                                    playerSeason.GamesPlayed = record.GamesPlayed;
                                    playerSeason.Runs = record.Runs;
                                    playerSeason.BallsFaced = record.BallsFaced;

                                    //_context.CricketPlayerSeasons.Update(playerSeason);
                                    //_context.SaveChanges();

                                    updateCricketPlayerSeasons.Add(playerSeason);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorStats.Add(record);

                        }
                    }

                    if (cricketPlayerSeasons != null && cricketPlayerSeasons.Count > 0)
                    {
                        try
                        {
                            await InsertStats(cricketPlayerSeasons);
                        }
                        catch (Exception ex)
                        {
                            return new ImportRunStats
                            {
                                Message = "Error importing run stats to database.",
                                Exception = ex.Message
                            };
                        }
                    }

                    if (updateCricketPlayerSeasons != null && updateCricketPlayerSeasons.Count > 0)
                    {
                        try
                        {
                            await UpdateStats(updateCricketPlayerSeasons);
                        }
                        catch (Exception ex)
                        {
                            return new ImportRunStats
                            {
                                Message = "Error updating run stats in database.",
                                Exception = ex.Message
                            };
                        }
                    }
                }

                if (errorStats != null && errorStats.Count > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorStats);
                        streamWriter.Flush();

                        return new ImportRunStats
                        {
                            Message = "Successfully imported run stats with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorStats,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportRunStats
                {
                    Message = "Successfully imported run stats!"
                };

            }
            catch (Exception ex)
            {
                return new ImportRunStats
                {
                    Message = "Error importing stats!",
                    Exception = ex.Message
                };
            }
        }

        public async Task<ImportWicketStats> UploadWicketStats(IFormFile file)
        {
            try
            {
                List<WicketStats> errorStats = new List<WicketStats>();
                List<CricketPlayerSeason> cricketPlayerSeasons = new List<CricketPlayerSeason>();
                List<CricketPlayerSeason> updateCricketPlayerSeasons = new List<CricketPlayerSeason>();
                List<Team> teams = await teamRepository.GetCricketTeams();
                List<Player> players = await playerRepository.GetAll();
                Sport sport = await _context.Sports.FirstOrDefaultAsync(e => e.Name == "Cricket");
                List<Season> seasons = await seasonRepository.GetCricketSeason();
                string newTeamName = String.Empty;
                Team team = null;
                Season season = seasons.FirstOrDefault(e => e.IsCurrent);
                Player player = null;

                //Stream reader = file.OpenReadStream();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;

                    var records = csv.GetRecords<WicketStats>();

                    foreach (var record in records)
                    {
                        try
                        {
                            //newTeamName =
                            team = teams.FirstOrDefault(e => (e.Name.Replace("'", "").Replace("-", "") == record.Team.Replace("'", "").Replace("-", "").Trim() || e.Alias == record.Team.Trim()));
                            //team = teams.FirstOrDefault(e => e.Name == record.Team || e.Alias == record.Team);
                            player = players.FirstOrDefault(e => e.FirstName.ToLower().Trim() == record.Firstname.ToLower() && e.LastName.ToLower().Trim() == record.Lastname.ToLower());

                            if (player == null)
                            {
                                if (team != null)
                                {
                                    Player newPlayer = new Player
                                    {
                                        FirstName = record.Firstname,
                                        LastName = record.Lastname
                                    };

                                    _context.Players.Add(newPlayer);
                                    _context.SaveChanges();

                                    CricketPlayerSeason newCricketPlayerSeason = new CricketPlayerSeason
                                    {
                                        PlayerID = newPlayer.ID,
                                        TeamID = team.ID,
                                        SeasonID = season.ID,
                                        GamesPlayed = record.GamesPlayed,
                                        IsActive = true,
                                        Wickets = record.Wickets,
                                        RunsConceded = record.RunsConceded
                                    };

                                    cricketPlayerSeasons.Add(newCricketPlayerSeason);
                                }
                                else
                                {
                                    errorStats.Add(record);
                                }
                            }
                            else
                            {
                                CricketPlayerSeason playerSeason = await _context.CricketPlayerSeasons.FirstOrDefaultAsync(e => e.PlayerID == player.ID);

                                if (playerSeason == null)
                                {
                                    CricketPlayerSeason newCricketPlayerSeason = new CricketPlayerSeason
                                    {
                                        PlayerID = player.ID,
                                        TeamID = team.ID,
                                        SeasonID = season.ID,
                                        GamesPlayed = record.GamesPlayed,
                                        IsActive = true,
                                        Wickets = record.Wickets,
                                        RunsConceded = record.RunsConceded
                                    };

                                    //_context.CricketPlayerSeasons.Add(newCricketPlayerSeason);
                                    //_context.SaveChanges();

                                    cricketPlayerSeasons.Add(newCricketPlayerSeason);
                                }
                                else
                                {
                                    playerSeason.GamesPlayed = record.GamesPlayed;
                                    playerSeason.Wickets = record.Wickets;
                                    playerSeason.RunsConceded = record.RunsConceded;

                                    //_context.CricketPlayerSeasons.Update(playerSeason);
                                    //_context.SaveChanges();

                                    updateCricketPlayerSeasons.Add(playerSeason);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorStats.Add(record);

                        }
                    }

                    if (cricketPlayerSeasons != null && cricketPlayerSeasons.Count > 0)
                    {
                        try
                        {
                            await InsertStats(cricketPlayerSeasons);
                        }
                        catch (Exception ex)
                        {
                            return new ImportWicketStats
                            {
                                Message = "Error importing wicket stats to database.",
                                Exception = ex.Message
                            };
                        }
                    }

                    if (updateCricketPlayerSeasons != null && updateCricketPlayerSeasons.Count > 0)
                    {
                        try
                        {
                            await UpdateStats(updateCricketPlayerSeasons);
                        }
                        catch (Exception ex)
                        {
                            return new ImportWicketStats
                            {
                                Message = "Error updating wicket stats in database.",
                                Exception = ex.Message
                            };
                        }
                    }
                }

                if (errorStats != null && errorStats.Count > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorStats);
                        streamWriter.Flush();

                        return new ImportWicketStats
                        {
                            Message = "Successfully imported wicket stats with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorStats,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportWicketStats
                {
                    Message = "Successfully imported wicket stats!"
                };

            }
            catch (Exception ex)
            {
                return new ImportWicketStats
                {
                    Message = "Error importing stats!",
                    Exception = ex.Message
                };
            }
        }

        public async Task InsertStats(List<CricketPlayerSeason> items)
        {
            try
            {
                await _context.CricketPlayerSeasons.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Stats");
            }
        }

        public async Task UpdateStats(List<CricketPlayerSeason> items)
        {
            try
            {
                _context.CricketPlayerSeasons.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Stats");
            }
        }

        public async Task AddMatchStats(List<MatchStat> items)
        {
            try
            {
                _context.MatchStats.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Stats");
            }
        }

        public async Task UpdatePlayers(List<PlayerSeason> items)
        {
            try
            {
                _context.PlayerSeasons.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Stats");
            }
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
