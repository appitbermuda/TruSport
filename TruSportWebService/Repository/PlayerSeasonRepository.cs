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
    public class PlayerSeasonRepository : IOnTrackRepository<PlayerSeason>
    {
        OnTrackContext _context;
        TeamRepository teamRepository;
        SeasonRepository seasonRepository;
        PlayerRepository playerRepository;

        public PlayerSeasonRepository(OnTrackContext context)
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

        public async Task<PlayerSeason> Get(string id)
        {
            return await _context.PlayerSeasons.Include("Player").FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<PlayerSeason>> GetAll()
        {
            return await _context.PlayerSeasons.Include("Player").ToListAsync();
        }

        public async Task<List<BowlingPlayerSeason>> GetCurrentBowlingPlayerSeason()
        {
            try
            {
                var players = await _context.BowlingPlayerSeasons
                    .Include(e => e.Player)
                    .Include(e => e.Season)
                    .Include(e => e.Team).ThenInclude(e => e.League)
                    .Where(e => e.Season.IsCurrent).ToListAsync();

                return players;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCurrentBowlingPlayerSeason");
            }

            return null;
        }

        public async Task<ImportBowlingPlayers> UploadBowlingPlayers(IFormFile file)
        {
            try
            {
                List<BowlingPlayerSeasons> errorPlayers = new List<BowlingPlayerSeasons>();
                List<BowlingPlayerSeason> playerSeasons = new List<BowlingPlayerSeason>();
                List<BowlingTeam> teams = await teamRepository.GetBowlingTeams();
                List<Player> players = await playerRepository.GetAll();
                List<Season> seasons = await seasonRepository.GetBowlingSeason();
                BowlingTeam team = null;
                Player player = null;
                Season season = seasons.FirstOrDefault(e => e.IsCurrent);

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;
                    var records = csv.GetRecords<BowlingPlayerSeasons>();

                    foreach (var record in records)
                    {
                        try
                        {
                            team = teams.FirstOrDefault(e => e.TeamID == record.TeamID);
                            player = players.FirstOrDefault(e => e.FirstName == record.FirstName && e.LastName == record.LastName);

                            if(player == null)
                            {
                                player = new Player();
                                player.FirstName = record.FirstName;
                                player.LastName = record.LastName;                                
                                _context.Players.Add(player);
                                await _context.SaveChangesAsync();
                            }

                            playerSeasons.Add(new BowlingPlayerSeason
                            {
                                TeamID = team.ID,
                                PlayerID = player.ID,
                                SeasonID = season.ID,
                                GamesPlayed = record.Games ?? 0,
                                Pins = record.Pins ?? 0,
                                Average = record.Average ?? 0,
                                PointsWon = record.PointsWon ?? 0.0m,
                                IsActive = true
                            });
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorPlayers.Add(record);

                        }
                    }

                    try
                    {
                        await AddBowlingPlayers(playerSeasons);
                    }
                    catch (Exception ex)
                    {
                        return new ImportBowlingPlayers
                        {
                            Message = "Error importing players to database.",
                            Exception = ex.Message
                        };
                    }
                }

                if (errorPlayers != null && errorPlayers.Count > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorPlayers);
                        streamWriter.Flush();

                        return new ImportBowlingPlayers
                        {
                            Message = "Successfully imported teams with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorPlayers,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportBowlingPlayers
                {
                    Message = "Successfully imported players!"
                };

            }
            catch (Exception ex)
            {
                return new ImportBowlingPlayers
                {
                    Message = "Error importing players!",
                    Exception = ex.Message
                };
            }


        }

        public async Task AddBowlingPlayers(List<BowlingPlayerSeason> items)
        {
            try
            {
                await _context.BowlingPlayerSeasons.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Bowling Players");
            }
        }

        public async Task<IEnumerable<PlayerSeason>> GetBySeason(int season)
        {
            throw new NotImplementedException();
        }

        public Task Insert(PlayerSeason item)
        {
            throw new NotImplementedException();
        }

        public Task Update(PlayerSeason item)
        {
            throw new NotImplementedException();
        }
    }
}
