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
                List<BowlingGame> games = new List<BowlingGame>();
                List<BowlingTeam> teams = await teamRepository.GetBowlingTeams();
                List<BowlingFixture> fixtures = await fixtureRepository.GetBowlingFixtures();
                List<BowlingRoster> bowlingRosters = await bowlingRosterRepository.GetAll();
                BowlingTeam team = null;
                Season season = null;
                BowlingFixture fixture = null;
                BowlingRoster bowlingRoster = null;

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
                            bowlingRoster = bowlingRosters.FirstOrDefault(e => e.BowlingPlayerSeason.Player.FirstName == record.FirstName && e.BowlingPlayerSeason.Player.LastName == record.LastName);

                            games.Add(new BowlingGame
                            {
                                //Date = record.Date,
                                //Time = record.Time.AddHours(4).ToString("HH:mm:ss"),
                                BowlingRosterID = bowlingRoster.ID,
                                Score = record.Score,
                                Game = record.Game,
                                Points = record.Points
                            });
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorGames.Add(record);

                        }
                    }

                    try
                    {
                        await AddGames(games);
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

                if (errorGames != null && errorGames.Count > 0)
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
    }
}
