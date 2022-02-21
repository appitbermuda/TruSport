using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                var award = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).FirstOrDefaultAsync(e => e.ID == id);

                
                return award;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetAward");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetAll()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBySportType");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetBySport(string SportID)
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.SportID == SportID).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBySportType");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetBySportType(string SportType)
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name == SportType).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBySportType");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetBasketballAwards()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name == Constants.Basketball && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBasketballAwards");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetCricketAwards()
        {
            try
            {
                    var awards = await _context.Awards
                        .Include(e => e.Player)
                        .Include(e => e.AwardType)
                        .Include(e => e.Season)
                        .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "cricket" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                    if (awards != null && awards.Count > 0)
                        return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketAwards");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetFootballAwards()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "football" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetFootballAwards");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetBasketballPlayer(string playerid)
        {
            try
            {
                return await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport)
                    .Where(e => e.Sport.Name == Constants.Basketball && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date && e.PlayerID == playerid).ToListAsync();
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

        public async Task<IEnumerable<Award>> GetBasketballPlayerOfTheWeek()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name == Constants.Basketball && e.AwardType.Name == "Week" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBasketballPlayerOfTheWeek");
            }


            return null;
        }

        public async Task<IEnumerable<Award>> GetCricketPlayerOfTheWeek()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "cricket" && e.AwardType.Name == "Week" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPlayerOfTheWeek");
            }


            return null;
        }

        public async Task<IEnumerable<Award>> GetFootballPlayerOfTheWeek()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "football" && e.AwardType.Name == "Week" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetFootballPlayerOfTheWeek");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetBasketballPlayerOfTheMonth()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name == Constants.Basketball && e.AwardType.Name == "Month" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBasketballPlayerOfTheMonth");
            }


            return null;
        }

        public async Task<IEnumerable<Award>> GetCricketPlayerOfTheMonth()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "cricket" && e.AwardType.Name == "Month" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPlayerOfTheMonth");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetFootballPlayerOfTheMonth()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "football" && e.AwardType.Name == "Month" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetFootballPlayerOfTheMonth");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetBasketballPlayerOfTheYear()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name == Constants.Basketball && e.AwardType.Name == "Season" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBasketballPlayerOfTheYear");
            }


            return null;
        }

        public async Task<IEnumerable<Award>> GetCricketPlayerOfTheYear()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "cricket" && e.AwardType.Name == "Season" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPlayerOfTheYear");
            }

            return null;
        }

        public async Task<IEnumerable<Award>> GetFootballPlayerOfTheYear()
        {
            try
            {
                var awards = await _context.Awards
                    .Include(e => e.Player)
                    .Include(e => e.AwardType)
                    .Include(e => e.Season)
                    .Include(e => e.Sport).Where(e => e.Sport.Name.ToLower() == "football" && e.AwardType.Name == "Season" && e.StartDate < DateTime.Now && e.ExpiryDate > DateTime.Now.Date).ToListAsync();

                if (awards != null && awards.Count > 0)
                    return awards;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetFootballPlayerOfTheYear");
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
