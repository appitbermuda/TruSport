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
    public class TournamentRepository : IOnTrackRepository<TennisTournament>
    {
        OnTrackContext _context;

        public TournamentRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<TennisTournament> Get(string id)
        {
            return await _context.TennisTournaments.Include(e => e.Field).FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<TennisTournament>> GetAll()
        {
            try
            {
                var tournaments = await _context.TennisTournaments.Include(e => e.Field).ToListAsync();

                return tournaments;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        //public async Task<IEnumerable<TennisTournament>> GetBySportType(string SportType)
        //{
        //    try
        //    {
        //        var leagues = await _context.TennisTournaments.Include(e => e.Sport).Where(e => e.Sport.Name == SportType).ToListAsync();

        //        return leagues;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return null;
        //}

        //public async Task<IEnumerable<TennisTournament>> GetBySport(string SportID)
        //{
        //    try
        //    {
        //        var leagues = await _context.TennisTournaments.Where(e => e.SportID == SportID).ToListAsync();

        //        return leagues;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return null;
        //}

        public Task<IEnumerable<TennisTournament>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public async Task Insert(TennisTournament item)
        {
            try
            {
                _context.TennisTournaments.Add(item);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }

        public async Task Update(TennisTournament item)
        {
            try
            {
                _context.TennisTournaments.Update(item);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
