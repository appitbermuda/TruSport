using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Basketball;

namespace OnTrackWebService.Repository
{
    public class PlayerRepository : IOnTrackRepository<Player>
    {
        OnTrackContext _context;

        public PlayerRepository(OnTrackContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                var player = await _context.Players.FirstOrDefaultAsync(e => e.ID == id);

                _context.Players.Remove(player);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<Player> Get(string id)
        {
            return await _context.Players.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<IEnumerable<Player>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Player>> GetAll()
        {
            try
            {
                var players = await _context.Players
                    .Include(e => e.PlayerSeasons).ThenInclude(e => e.Season)
                    .Include(e => e.PlayerSeasons).ThenInclude(e => e.Team)
                    .ToListAsync();

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPlayersByTeam");
            }

            return null;
        }

        public async Task<IEnumerable<PlayerSeason>> GetPlayers()
        {
            try
            {
                var players = await _context.PlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).Where(e => e.Season.IsCurrent && e.IsActive).ToListAsync();

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPlayersByTeam");
            }

            return null;
        }

        public async Task<IEnumerable<PlayerSeason>> GetPlayersByTeam(string teamID)
        {
            try
            {
                var players = await _context.PlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).Where(e => e.TeamID == teamID && e.Season.IsCurrent && e.IsActive).ToListAsync();

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetPlayersByTeam");
            }

            return null;
        }

        public async Task<PlayerSeason> GetPlayer(string playerID)
        {
            try
            {
                var players = await _context.PlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).FirstOrDefaultAsync(e => e.PlayerID == playerID && e.Season.IsCurrent);

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetPlayer");
            }

            return null;
        }

        public async Task<IEnumerable<BasketballPlayerSeason>> GetBasketballPlayers()
        {
            try
            {
                var players = await _context.BasketballPlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).Where(e => e.Season.IsCurrent && e.IsActive).ToListAsync();

                return players;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBasketballPlayers");
            }

            return null;
        }

        public async Task<IEnumerable<BasketballPlayerSeason>> GetBasketballPlayersByTeam(string teamID)
        {
            try
            {
                var players = await _context.BasketballPlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).Where(e => e.TeamID == teamID && e.Season.IsCurrent && e.IsActive).ToListAsync();

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBasketballPlayersByTeam");
            }

            return null;
        }

        public async Task<BasketballPlayerSeason> GetBasketballPlayer(string playerID)
        {
            try
            {
                var players = await _context.BasketballPlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).FirstOrDefaultAsync(e => e.PlayerID == playerID && e.Season.IsCurrent);

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBasketballPlayer");
            }

            return null;
        }

        public async Task<IEnumerable<CricketPlayerSeason>> GetCricketPlayers()
        {
            try
            {
                var players = await _context.CricketPlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).Where(e => e.Season.IsCurrent && e.IsActive).ToListAsync();

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPlayers");
            }

            return null;
        }

        public async Task<IEnumerable<CricketPlayerSeason>> GetCricketPlayersByTeam(string teamID)
        {
            try
            {
                var players = await _context.CricketPlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).Where(e => e.TeamID == teamID && e.Season.IsCurrent && e.IsActive).ToListAsync();

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPlayersByTeam");
            }

            return null;
        }

        public async Task<CricketPlayerSeason> GetCricketPlayer(string playerID)
        {
            try
            {
                var players = await _context.CricketPlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).FirstOrDefaultAsync(e => e.PlayerID == playerID && e.Season.IsCurrent);

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetCricketPlayer");
            }

            return null;
        }

        public async Task<IEnumerable<BowlingPlayerSeason>> GetBowlingPlayers()
        {
            try
            {
                var players = await _context.BowlingPlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).Where(e => e.Season.IsCurrent && e.IsActive).ToListAsync();

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBowlingPlayers");
            }

            return null;
        }

        public async Task<IEnumerable<BowlingPlayerSeason>> GetBowlingPlayersByTeam(string teamID)
        {
            try
            {
                var players = await _context.BowlingPlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).Where(e => e.TeamID == teamID && e.Season.IsCurrent && e.IsActive).ToListAsync();

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBowlingPlayersByTeam");
            }

            return null;
        }

        public async Task<BowlingPlayerSeason> GetBowlingPlayer(string playerID)
        {
            try
            {
                var players = await _context.BowlingPlayerSeasons.Include(e => e.Team).Include(e => e.Player).Include(e => e.Season).FirstOrDefaultAsync(e => e.PlayerID == playerID && e.Season.IsCurrent);

                return players;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetBowlingPlayer");
            }

            return null;
        }

        public async Task<bool> Insert(Player item)
        {
            try
            {
                _context.Players.Add(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
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

        public async Task Update(CricketPlayerSeason player)
        {
            try
            {
                var _player = player.Player;



                _context.Players.Update(_player);
                await _context.SaveChangesAsync();

                _context.CricketPlayerSeasons.Update(player);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }

        public async Task Update(BowlingPlayerSeason player)
        {
            try
            {
                var _player = player.Player;



                _context.Players.Update(_player);
                await _context.SaveChangesAsync();

                _context.BowlingPlayerSeasons.Update(player);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }

        public async Task UpdateAll(List<BowlingPlayerSeason> players)
        {
            try
            {
                var _players = players.Select(e=> e.Player);



                _context.Players.UpdateRange(_players);
                await _context.SaveChangesAsync();

                _context.BowlingPlayerSeasons.UpdateRange(players);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }

        public async Task RemovePlayer(CricketPlayerSeason player)
        {
            try
            {
                player.TeamID = null;

                _context.CricketPlayerSeasons.Update(player);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> Update(Player item)
        {
            try
            {
                _context.Players.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        Task<IEnumerable<Player>> IOnTrackRepository<Player>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Player>.Insert(Player item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Player>.Update(Player item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Player>.Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
