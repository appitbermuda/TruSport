using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class AwardRepository : IOnTrackRepository<Award>
    {
        OnTrackContext _context;

        public AwardRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Award> Get(string id)
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).FirstOrDefaultAsync(e => e.ID == id);
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetAll()
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetBySport(string SportID)
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.SportID == SportID).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetBySportType(string SportType)
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name == SportType).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetCricketAwards()
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "cricket" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetFootballAwards()
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "football" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetCricketPlayer(string playerid)
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport)
                    .Where(e => e.Sport.Name.ToLower() == "cricket" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date && e.PlayerID == playerid).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetFootballPlayer(string playerid)
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport)
                    .Where(e => e.Sport.Name.ToLower() == "football" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date && e.PlayerID == playerid).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetCricketPlayerOfTheWeek()
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "cricket" && e.AwardType.Name == "Week" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetFootballPlayerOfTheWeek()
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "football" && e.AwardType.Name == "Week" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetCricketPlayerOfTheMonth()
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "cricket" && e.AwardType.Name == "Month" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetFootballPlayerOfTheMonth()
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "football" && e.AwardType.Name == "Month" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetCricketPlayerOfTheYear()
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "cricket" && e.AwardType.Name == "Season" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetFootballPlayerOfTheYear()
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "football" && e.AwardType.Name == "Season" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task Insert(Award playerAward)
        {
            try
            {
                _context.Awards.Add(playerAward);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }

        public async Task Update(Award playerAward)
        {
            try
            {
                    _context.Awards.Update(playerAward);
                    await _context.SaveChangesAsync();
                
            }
            catch(Exception ex)
            {

            }
        }
    }
}
