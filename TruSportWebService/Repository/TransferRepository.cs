using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Imports;

namespace OnTrackWebService.Repository
{
    public class TransferRepository : IOnTrackRepository<Transfer>
    {
        OnTrackContext _context;
        TeamRepository teamRepository;
        SeasonRepository seasonRepository;
        LeagueRepository leagueRepository;

        public TransferRepository(OnTrackContext context)
        {
            _context = context;
            teamRepository = new TeamRepository(context);
            seasonRepository = new SeasonRepository(context);
            leagueRepository = new LeagueRepository(context);
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Transfer> Get(string id)
        {
            throw new NotImplementedException();
            //return await _context.Transfers.Include("Team").FirstOrDefaultAsync(e => e.TeamID == id);
        }

        public async Task<IEnumerable<Transfer>> GetAll()
        {
            //return await _context.Transfers.FromSql("select * from leaguetable").ToListAsync();
            return await _context.Transfers.ToListAsync();
        }

        public async Task<IEnumerable<Transfer>> Football()
        {
            try
            {
                var transfers = await _context.Transfers.Include(e => e.Sport).Include(e => e.NewTeam).Include(e => e.Season).ToListAsync();

                transfers.Where(e => e.Sport.Name.ToLower() == "football");

                return transfers;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Football Transfer");
            }

            return null;
        }

        public async Task<IEnumerable<Transfer>> Cricket()
        {
            try
            {
                var transfers = await _context.Transfers.Include(e => e.Sport).Include(e => e.NewTeam).Include(e => e.Season).ToListAsync();

                transfers.Where(e => e.Sport.Name.ToLower() == "cricket");

                return transfers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Cricket Transfer");
            }

            return null;
        }

        public async Task<IEnumerable<Transfer>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
            //return await _context.Transfers..Where(e => e.TeamID == teamID).ToListAsync();
        }

        public async Task<IEnumerable<Transfer>> GetByLeague(string leagueID)
        {
            throw new NotImplementedException();
            //return await _context.Transfers.Where(e => e.LeagueID == leagueID).ToListAsync();
        }

        public async Task<ImportTransfers> UploadFootballTransfers(IFormFile file)
        {
            try
            {
                List<Transfers> errorTransfers = new List<Transfers>();
                List<Transfer> transfers = new List<Transfer>();
                List<Team> teams = await teamRepository.GetFootballTeams();
                Sport sport = await _context.Sports.FirstOrDefaultAsync(e => e.Name == "Football");
                List<League> leagues = await leagueRepository.GetFootballLeagues();
                List<Season> seasons = await seasonRepository.GetFootballSeason();
                string newTeamName = String.Empty;
                Team newTeam = null;
                Season season = null;
                League league = null;

                //Stream reader = file.OpenReadStream();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Transfers>();

                    foreach (var record in records)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(record.League))
                            {
                                league = leagues.FirstOrDefault(e => e.Name == record.League.Trim());
                                newTeam = teams.FirstOrDefault(e => (e.Name.Replace("'", "") == record.NewTeam.Replace("'", "").Trim() || e.Alias == record.NewTeam.Trim()) && e.LeagueID == league.ID);
                            }
                            else
                            {
                                newTeam = teams.FirstOrDefault(e => (e.Name.Replace("'", "") == record.NewTeam.Replace("'", "").Trim() || e.Alias == record.NewTeam.Trim()) && (e.League.Name == "Premier Division" || e.League.Name == "First Division"));
                            }

                            season = seasons.FirstOrDefault(e => e.Key == record.Season);

                            transfers.Add(new Transfer
                            {
                                PlayerName = record.Name,
                                Date = season.Date,
                                PreviousTeam = record.PreviousTeam,
                                NewTeamID = newTeam.ID,
                                SeasonID = season.ID,
                                SportID = sport.ID,
                                IsLateTransfer = Convert.ToBoolean(record.IsLateTransfer)
                            });
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorTransfers.Add(record);

                        }
                    }

                    try
                    {
                        await Insert(transfers);
                    }
                    catch (Exception ex)
                    {
                        return new ImportTransfers
                        {
                            Message = "Error importing transfers to database.",
                            Exception = ex.Message
                        };
                    }
                }

                if (errorTransfers != null && errorTransfers.Count > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorTransfers);
                        streamWriter.Flush();

                        return new ImportTransfers
                        {
                            Message = "Successfully imported transfers with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorTransfers,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportTransfers
                {
                    Message = "Successfully imported transfers!"
                };

            }
            catch (Exception ex)
            {
                return new ImportTransfers
                {
                    Message = "Error importing transfers!",
                    Exception = ex.Message
                };
            }


        }

        public async Task Insert(List<Transfer> items)
        {
            try
            {
                await _context.Transfers.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Transfers");
            }
        }

        public Task Insert(Transfer item)
        {
            throw new NotImplementedException();
        }

        public Task Update(Transfer item)
        {
            throw new NotImplementedException();
        }
    }
}
