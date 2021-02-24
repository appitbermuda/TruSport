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
    public class LeagueRepository : IOnTrackRepository<League>
    {
        OnTrackContext _context;

        public LeagueRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<League> Get(string id)
        {
            return await _context.Leagues.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<League>> GetAll()
        {
            return await _context.Leagues.ToListAsync();
        }

        public async Task<IEnumerable<League>> Get()
        {
            try
            {
                var leagues = await _context.Leagues.Include(e => e.Sport).ToListAsync();

                return leagues;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<List<League>> GetBowlingLeagues()
        {
            try
            {
                var leagues = await _context.Leagues.Include(e => e.Sport).Where(e => e.Sport.Name == "Bowling").ToListAsync();

                return leagues;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<List<League>> GetCricketLeagues()
        {
            try
            {
                //var sport = await _context.Sports.FirstOrDefaultAsync(e => e.Name == "Cricket");

                var leagues = await _context.Leagues.Include(e => e.Sport).Where(e => e.Sport.Name == "Cricket").ToListAsync();

                return leagues;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<List<League>> GetFootballLeagues()
        {
            try
            {
                //var sport = await _context.Sports.FirstOrDefaultAsync(e => e.Name == "Football");

                var leagues = await _context.Leagues.Include(e => e.Sport).Where(e => e.Sport.Name == "Football").ToListAsync();

                return leagues;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<League>> GetBySportType(string SportType)
        {
            try
            {
                var leagues = await _context.Leagues.Include(e => e.Sport).Where(e => e.Sport.Name == SportType).ToListAsync();

                return leagues;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<IEnumerable<League>> GetBySport(string SportID)
        {
            try
            {
                var leagues = await _context.Leagues.Where(e => e.SportID == SportID).ToListAsync();

                return leagues;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public Task<IEnumerable<League>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(League item)
        {
            throw new NotImplementedException();
        }

        public Task Update(League item)
        {
            throw new NotImplementedException();
        }
    }
}
