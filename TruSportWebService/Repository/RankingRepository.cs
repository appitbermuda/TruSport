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
    public class RankingRepository : IOnTrackRepository<TennisRanking>
    {
        OnTrackContext _context;
        TeamRepository teamRepository;
        SeasonRepository seasonRepository;
        PlayerRepository playerRepository;

        public RankingRepository(OnTrackContext context)
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

        public async Task<TennisRanking> Get(string id)
        {
            throw new NotImplementedException();
            //return await _context.Ranking.Include("Team").FirstOrDefaultAsync(e => e.TeamID == id);
        }

        public async Task<IEnumerable<TennisRanking>> GetAll()
        {
            //return await _context.LeagueTables.FromSql("select * from leaguetable").ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TennisRanking>> GetMensCurrentTennisRanking()
        {
            try
            {
                var ranking = await _context.TennisRankings
                    .Include(e => e.RankingType)
                    .Include(e => e.TennisPlayerSeason).ThenInclude(e => e.Player)
                    .Include(e => e.TennisPlayerSeason).ThenInclude(e => e.Season)
                    .Where(e => e.RankingType.Name == "Men" && e.TennisPlayerSeason.Season.IsCurrent).OrderByDescending(e => e.Points).ToListAsync();

                int i = 1;
                TennisRanking lastRanking = new TennisRanking();
                foreach(var rank in ranking.OrderByDescending(e => e.Points))
                {
                    if (lastRanking == null)
                    {
                        rank.Rank = i;
                    }
                    else
                    {
                        if (lastRanking.Points == rank.Points)
                        {
                            rank.Rank = lastRanking.Rank;
                        }
                        else
                        {
                            rank.Rank = i;
                        }

                        lastRanking = rank;
                    }

                    i++;
                }

                return ranking;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<TennisRanking>> GetWomensCurrentTennisRanking()
        {
            try
            {
                var ranking = await _context.TennisRankings
                    .Include(e => e.RankingType)
                    .Include(e => e.TennisPlayerSeason).ThenInclude(e => e.Player)
                    .Include(e => e.TennisPlayerSeason).ThenInclude(e => e.Season)
                    .Where(e => e.RankingType.Name == "Women" && e.TennisPlayerSeason.Season.IsCurrent).OrderByDescending(e => e.Points).ToListAsync();

                return ranking;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<ImportGoalStats> UploadRankings(IFormFile file)
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
                            //await UpdatePlayers(updatePlayers);
                        }

                        if (matchStats.Count > 0)
                        {
                            //await AddMatchStats(matchStats);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new ImportGoalStats
                        {
                            Message = "Error importing ranking to database.",
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

        public async Task InsertStats(List<TennisPlayerSeason> items)
        {
            try
            {
                await _context.TennisPlayerSeasons.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Stats");
            }
        }

        public async Task UpdateStats(List<TennisPlayerSeason> items)
        {
            try
            {
                _context.TennisPlayerSeasons.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Stats");
            }
        }

        public Task Insert(TennisRanking item)
        {
            throw new NotImplementedException();
        }

        public Task Update(TennisRanking item)
        {
            throw new NotImplementedException();
        }
    }
}
