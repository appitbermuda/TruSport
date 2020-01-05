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
    public class PlayerRepository : IOnTrackRepository<Player>
    {
        OnTrackContext _context;

        public PlayerRepository(OnTrackContext context)
        {
            _context = context;
        }
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Player> Get(string id)
        {
            return await _context.Players.Include("Team").FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Player>> GetAll()
        {
            return await _context.Players.Include("Team").ToListAsync();
        }

        public async Task<IEnumerable<Player>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PlayerSeason>> GetPlayers()
        {
            return await _context.PlayerSeasons.Include("Team").Include("Player").Include("Season").Where(e => e.Season.IsCurrent && e.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<PlayerSeason>> GetPlayersByTeam(string teamID)
        {
            return await _context.PlayerSeasons.Include("Team").Include("Player").Include("Season").Where(e => e.TeamID == teamID && e.Season.IsCurrent && e.IsActive).ToListAsync();
        }

        public async Task<PlayerSeason> GetPlayer(string playerID)
        {
            return await _context.PlayerSeasons.Include("Team").Include("Player").Include("Season").FirstOrDefaultAsync(e => e.PlayerID == playerID && e.Season.IsCurrent);
        }

        public Task Insert(Player item)
        {
            throw new NotImplementedException();
        }

        public async Task Update(PlayerSeason player)
        {
            try
            {
                    var _player = player.Player;



                    _context.Players.Update(_player);
                    await _context.SaveChangesAsync();

                    _context.PlayerSeasons.Update(player);
                    await _context.SaveChangesAsync();
                
            }
            catch(Exception ex)
            {

            }
        }

        public async Task RemovePlayer(PlayerSeason player)
        {
            try
            {
                player.TeamID = null;

                _context.PlayerSeasons.Update(player);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }

        public Task Update(Player item)
        {
            throw new NotImplementedException();
        }
    }
}
