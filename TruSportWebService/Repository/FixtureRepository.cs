using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class FixtureRepository : IOnTrackRepository<Fixture>
    {
        OnTrackContext _context;

        public FixtureRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Fixture> Get(string id)
        {
            return await _context.Fixtures.Include("Field").Include("League").Include("Match").Include("MatchType").Include("Season").Include(e => e.MatchRosters).ThenInclude(e => e.Player).Include(e => e.HomeTeam).ThenInclude(e => e.Coaches).Include(e => e.AwayTeam).ThenInclude(e => e.Coaches).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Fixture>> GetAll()
        {
            //return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.AwayTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7" || e.HomeTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7").ToListAsync();
            return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").ToListAsync();
        }

        public async Task<IEnumerable<Fixture>> GetByTeam(string teamID)
        {
            return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.AwayTeamID == teamID || e.HomeTeamID == teamID).ToListAsync();
        }

        public async Task<IEnumerable<Fixture>> GetByLeague(string leagueID)
        {
            //return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.LeagueID == leagueID && (e.AwayTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7" || e.HomeTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7")).ToListAsync();

            return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.LeagueID == leagueID).ToListAsync();
        }

        public async Task<IEnumerable<spLiveFixtures>> GetLive()
        {
            try
            {
                //var dateTime = DateTime.SpecifyKind(DateTime.Now.ToLocalTime(), DateTimeKind.Utc);
                //string nzTimeZoneKey = "Atlantic/Bermuda";
                //TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(nzTimeZoneKey);
                //DateTime nzDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, nzTimeZone);

                //var fixtures = await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.Date == nzDateTime.Date && (TimeSpan.Parse(e.Time)) < nzDateTime.TimeOfDay && (TimeSpan.Parse(e.Time)) <= nzDateTime.AddMinutes(110).TimeOfDay && (e.AwayTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7" || e.HomeTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7") && !e.IsPostPoned).ToListAsync();

                ////e => e.Date == DateTime.Now.Date && (TimeSpan.Parse(e.Time)) < DateTime.Now.TimeOfDay && (TimeSpan.Parse(e.Time)) <= DateTime.Now.AddMinutes(110).TimeOfDay &&

                //return fixtures;

                //SqlParameter StopID = new SqlParameter("@StopID", stopID);

                string sqlQuery = "EXEC [dbo].[spLiveFixtures] ";

                var fixtures = await _context.Query<spLiveFixtures>().FromSql(sqlQuery).ToListAsync();

                return fixtures;

            }
            catch(Exception ex)
            {

            }

            return null;
        }


        public async Task<IEnumerable<Fixture>> GetUpcoming()
        {
            //return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.Date > DateTime.Now.Date && (TimeSpan.Parse(e.Time)) > DateTime.Now.TimeOfDay && (e.AwayTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7" || e.HomeTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7")).ToListAsync();
            return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.Date > DateTime.Now.Date && (TimeSpan.Parse(e.Time)) > DateTime.Now.TimeOfDay).ToListAsync();
        }

        public async Task<IEnumerable<Fixture>> GetPast()
        {
            return await _context.Fixtures.Include("HomeTeam").Include("AwayTeam").Include("Field").Include("League").Include("Match").Include("MatchType").Include("MatchRosters").Include("Season").Where(e => e.Date < DateTime.Now.Date && (TimeSpan.Parse(e.Time)) < DateTime.Now.TimeOfDay && (e.AwayTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7" || e.HomeTeamID != "584edccc-3930-4f57-9dc3-0be4922ec4a7")).ToListAsync();
        }

        public async Task Insert(Fixture item)
        {
            try
            {
                Season season = await _context.Seasons.FirstOrDefaultAsync(e => e.IsCurrent);

                item.SeasonID = season.ID;

                _context.Fixtures.Add(item);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            { }
        }

        public async Task Update(Fixture item)
        {
            _context.Fixtures.Update(item);

            _context.Matches.Update(item.Match);

            await _context.SaveChangesAsync();
        }
    }
}
