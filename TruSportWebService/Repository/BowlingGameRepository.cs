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
using MatchType = OnTrackWebService.Models.MatchType;

namespace OnTrackWebService.Repository
{
    public class BowlingGameRepository : IOnTrackRepository<BowlingGame>
    {
        OnTrackContext _context;
        TeamRepository teamRepository;
        LeagueRepository leagueRepository;
        SeasonRepository seasonRepository;
        SettingRepository settingRepository;
        FixtureRepository fixtureRepository;
        BowlingRosterRepository bowlingRosterRepository;        

        public BowlingGameRepository(OnTrackContext context)
        {
            _context = context;
            teamRepository = new TeamRepository(context);
            leagueRepository = new LeagueRepository(context);
            seasonRepository = new SeasonRepository(context);
            settingRepository = new SettingRepository(context);
            fixtureRepository = new FixtureRepository(context);
            bowlingRosterRepository = new BowlingRosterRepository(context);
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<BowlingGame> Get(string id)
        {
            try
            {
                var bowlingGames = await _context.BowlingGames
                    .Include(e => e.BowlingRoster)
                    .FirstOrDefaultAsync(e => e.ID == id);

                return bowlingGames;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get Bowling Game");
            }

            return null;
        }

        public async Task<IEnumerable<BowlingGame>> GetAll()
        {
            try
            {
                var bowlingGames = await _context.BowlingGames
                    .Include(e => e.BowlingRoster)
                    .ToListAsync();

                return bowlingGames;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get Bowling Games");
            }

            return null;
        }

        public async Task<BowlingGame> GetByFixture(string fixtureID)
        {
            try
            {
                var bowlingGames = await _context.BowlingGames
                    .Include(e => e.BowlingRoster)
                    .FirstOrDefaultAsync(e => e.BowlingRoster.BowlingFixtureID == fixtureID);

                return bowlingGames;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get Bowling Fixture Games");
            }

            return null;
        }

        public Task Insert(BowlingGame item)
        {
            throw new NotImplementedException();
        }

        public async Task Update(BowlingGame item)
        {
            try
            {
                _context.BowlingGames.Update(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "BowlingGame");
            }
        }


        public async Task<ImportBowlingGames> UploadBowlingGames(IFormFile file)
        {
            try
            {
                List<BowlingGames> errorGames = new List<BowlingGames>();
                List<BowlingGames> errorRosters = new List<BowlingGames>();
                List<BowlingRoster> rosters = new List<BowlingRoster>();
                List<BowlingGame> games = new List<BowlingGame>();
                List<BowlingScore> scores = new List<BowlingScore>();
                List<BowlingPlayerSeason> bowlingPlayers = new List<BowlingPlayerSeason>();
                List<BowlingTeam> teams = await teamRepository.GetBowlingTeams();
                List<Season> seasons = await seasonRepository.GetBowlingSeason();
                List<BowlingGame> bowlingGames = await _context.BowlingGames.ToListAsync();
                List<Player> players = await _context.Players.ToListAsync();
                List<BowlingFixture> fixtures = await _context.BowlingFixtures
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Field)
                .Include(e => e.League)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingPlayerSeason).ThenInclude(e => e.Player)
                .Include(e => e.BowlingRosters).ThenInclude(e => e.BowlingGames)
                .Include(e => e.BowlingScore)
                .Include(e => e.MatchType)
                .Include(e => e.Season)
                .ToListAsync();
                List<BowlingRoster> bowlingRosters = await bowlingRosterRepository.GetAll();
                List<BowlingPlayerSeason> bowlingPlayerSeasons = await _context.BowlingPlayerSeasons.Include(e => e.Player).Include(e => e.Season).Where(e => e.Season.IsCurrent).ToListAsync();
                BowlingTeam team = null;
                Season season = seasons.FirstOrDefault(e => e.IsCurrent);
                BowlingFixture fixture = null;
                BowlingRoster bowlingRoster = null;
                BowlingPlayerSeason bowlingPlayer = null;
                Player player = null;
                List<Player> newPlayers = new List<Player>();
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;
                    var records = csv.GetRecords<BowlingGames>();

                    foreach (var record in records)
                    {
                        try
                        {
                            team = teams.FirstOrDefault(e => (e.TeamID == record.TeamID));
                            fixture = fixtures.FirstOrDefault(e => e.Date == record.Date && (e.HomeTeamID == team.ID || e.AwayTeamID == team.ID));
                            
                            //bowlingRoster = bowlingRosters.FirstOrDefault(e => (e.BowlingPlayerSeason.Player.FirstName + " " + e.BowlingPlayerSeason.Player.LastName) == record.Name);

                            if (record.Name.ToLower() == "bye")
                                player = players.FirstOrDefault(e => e.FirstName.ToLower() == record.Name.ToLower() && e.LastName == NumberToWords(record.Position));
                            else
                                player = players.FirstOrDefault(e => (e.FirstName + " " + e.LastName) == record.Name);

                            if(player == null && newPlayers.Count > 0)
                            {
                                if (record.Name.ToLower() == "bye")
                                    player = newPlayers.FirstOrDefault(e => e.FirstName.ToLower() == record.Name.ToLower() && e.LastName == NumberToWords(record.Position));
                                else
                                    player = newPlayers.FirstOrDefault(e => (e.FirstName + " " + e.LastName) == record.Name);
                            }

                            if (player == null && ((newPlayers.Count > 0 && !newPlayers.Any(e => (e.FirstName.ToLower() == record.Name.ToLower() && e.LastName == NumberToWords(record.Position)) || (e.FirstName + " " + e.LastName) == record.Name)) || newPlayers == null || newPlayers.Count == 0))
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

                                bowlingPlayer = bowlingPlayerSeasons.FirstOrDefault(e => e.Player.FirstName == player.FirstName && e.Player.LastName == player.LastName);

                                if (record.Name.ToLower() == "bye")
                                    bowlingPlayer = bowlingPlayerSeasons.FirstOrDefault(e => e.Player.FirstName.ToLower() == record.Name.ToLower() && e.Player.LastName == NumberToWords(record.Position));
                                else
                                    bowlingPlayer = bowlingPlayerSeasons.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());

                                if (bowlingPlayer == null)
                                {
                                    if (record.Name.ToLower() == "bye")
                                        bowlingPlayer = bowlingPlayers.FirstOrDefault(e => e.Player.FirstName.ToLower() == record.Name.ToLower() && e.Player.LastName == NumberToWords(record.Position));
                                    else
                                        bowlingPlayer = bowlingPlayers.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());
                                }
                            }
                            else
                            {
                                if (record.Name.ToLower() == "bye")
                                    bowlingPlayer = bowlingPlayerSeasons.FirstOrDefault(e => e.Player.FirstName.ToLower() == record.Name.ToLower() && e.Player.LastName == NumberToWords(record.Position));
                                else
                                    bowlingPlayer = bowlingPlayerSeasons.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());

                                if (bowlingPlayer == null)
                                {
                                    if (record.Name.ToLower() == "bye")
                                        bowlingPlayer = bowlingPlayers.FirstOrDefault(e => e.Player.FirstName.ToLower() == record.Name.ToLower() && e.Player.LastName == NumberToWords(record.Position));
                                    else
                                        bowlingPlayer = bowlingPlayers.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());
                                }
                            }

                            if (bowlingPlayer == null)
                            {
                                bowlingPlayer = new BowlingPlayerSeason
                                {
                                    PlayerID = player.ID,
                                    TeamID = team.ID,
                                    SeasonID = season.ID,
                                    GamesPlayed = 0,
                                    Pins = 0,
                                    Average = 0,
                                    PointsWon = 0,
                                    IsActive = true
                                };

                                bowlingPlayers.Add(bowlingPlayer);

                                _context.BowlingPlayerSeasons.Add(bowlingPlayer);
                                await _context.SaveChangesAsync();
                            }

                            if (record.Name.ToLower() == "bye")
                                bowlingRoster = bowlingRosters.FirstOrDefault(e => e.BowlingPlayerSeason.Player.FirstName.ToLower() == record.Name.ToLower() && e.BowlingPlayerSeason.Player.LastName == NumberToWords(record.Position));
                            else
                                bowlingRoster = bowlingRosters.FirstOrDefault(e => (e.BowlingPlayerSeason.Player.FirstName + " " + e.BowlingPlayerSeason.Player.LastName) == record.Name);

                            if (bowlingRoster == null)
                            {
                                if (record.Name.ToLower() == "bye")
                                    bowlingPlayer = bowlingPlayerSeasons.FirstOrDefault(e => e.Player.FirstName.ToLower() == record.Name.ToLower() && e.Player.LastName == NumberToWords(record.Position));
                                else
                                    bowlingPlayer = bowlingPlayerSeasons.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());

                                if(bowlingPlayer == null)
                                {
                                    if (record.Name.ToLower() == "bye")
                                        bowlingPlayer = bowlingPlayers.FirstOrDefault(e => e.Player.FirstName.ToLower() == record.Name.ToLower() && e.Player.LastName == NumberToWords(record.Position));
                                    else
                                        bowlingPlayer = bowlingPlayers.FirstOrDefault(e => (e.Player.FirstName + " " + e.Player.LastName).Trim() == record.Name.Trim());
                                }

                                bowlingRoster = new BowlingRoster
                                {
                                    BowlingPlayerSeasonID = bowlingPlayer.ID,
                                    BowlingFixtureID = fixture.ID,
                                    TeamID = team.ID,
                                    Position = record.Position
                                };

                                rosters.Add(bowlingRoster);
                                _context.BowlingRosters.Add(bowlingRoster);
                                await _context.SaveChangesAsync();
                            }

                            var bowlingGame = bowlingGames.FirstOrDefault(e => e.BowlingRosterID == bowlingRoster.ID);

                            if ((bowlingGame != null && bowlingGame.Game != 1) || bowlingGame == null)
                            {
                                games.Add(new BowlingGame
                                {
                                    BowlingRosterID = bowlingRoster.ID,
                                    Score = record.Game1,
                                    Game = 1,
                                    Points = null
                                });
                            }

                            if ((bowlingGame != null && bowlingGame.Game != 2) || bowlingGame == null)
                            {
                                games.Add(new BowlingGame
                                {
                                    BowlingRosterID = bowlingRoster.ID,
                                    Score = record.Game2,
                                    Game = 2,
                                    Points = null
                                });
                            }

                            if ((bowlingGame != null && bowlingGame.Game != 3) || bowlingGame == null)
                            {
                                games.Add(new BowlingGame
                                {
                                    BowlingRosterID = bowlingRoster.ID,
                                    Score = record.Game3,
                                    Game = 3,
                                    Points = null
                                });
                            }

                            if (fixture.HomeTeamID == team.ID)
                            {
                                if (scores.Count > 0 && scores.Any(e => e.BowlingFixtureID == fixture.ID))
                                {
                                    scores.FirstOrDefault(e => e.BowlingFixtureID == fixture.ID).HomeTeamPoints = record.TeamPoints;
                                    scores.FirstOrDefault(e => e.BowlingFixtureID == fixture.ID).HomeTeamMatchPoints = record.MatchPoints;
                                }
                                else
                                {
                                    scores.Add(new BowlingScore
                                    {
                                        BowlingFixtureID = fixture.ID,
                                        HomeTeamMatchPoints = record.MatchPoints,
                                        HomeTeamPoints = record.TeamPoints,
                                    });
                                }
                            }
                            else
                            {
                                if (scores.Count > 0 && scores.Any(e => e.BowlingFixtureID == fixture.ID))
                                {
                                    scores.FirstOrDefault(e => e.BowlingFixtureID == fixture.ID).AwayTeamPoints = record.TeamPoints;
                                    scores.FirstOrDefault(e => e.BowlingFixtureID == fixture.ID).AwayTeamMatchPoints = record.MatchPoints;
                                }
                                else
                                {
                                    scores.Add(new BowlingScore
                                    {
                                        BowlingFixtureID = fixture.ID,
                                        AwayTeamMatchPoints = record.MatchPoints,
                                        AwayTeamPoints = record.TeamPoints,
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorGames.Add(record);

                        }
                    }

                    try
                    {
                        if (scores.Count > 0)
                        {
                            await AddScores(scores);
                        }

                        if (games.Count > 0)
                        {
                            await AddGames(games);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new ImportBowlingGames
                        {
                            Message = "Error importing games to database.",
                            Exception = ex.Message
                        };
                    }
                }

                if (errorGames != null && errorGames.Count > 0 && errorRosters != null && errorRosters.Count > 0)
                {

                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorGames);
                        streamWriter.Flush();

                        return new ImportBowlingGames
                        {
                            Message = "Successfully imported games with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorGames,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportBowlingGames
                {
                    Message = "Successfully imported games!"
                };

            }
            catch (Exception ex)
            {
                return new ImportBowlingGames
                {
                    Message = "Error importing games!",
                    Exception = ex.Message
                };
            }
        }

        public async Task AddGames(List<BowlingGame> items)
        {
            try
            {
                await _context.BowlingGames.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Bowling Games");
            }
        }

        public async Task AddScores(List<BowlingScore> items)
        {
            try
            {
                await _context.BowlingScores.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Bowling Scores");
            }
        }

        public async Task AddRosters(List<BowlingRoster> items)
        {
            try
            {
                await _context.BowlingRosters.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Bowling Scores");
            }
        }

        private static String NumberToWords(int Number)
        {
            String name = "";
            switch (Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
    }
}
