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
