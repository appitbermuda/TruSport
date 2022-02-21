using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Repository
{
    public class WalletRepository : ITicketRepository<Wallet>
    {
        OnTrackContext _context;

        public WalletRepository(OnTrackContext context)
        {
            _context = context;
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var card = await _context.Wallets.FirstOrDefaultAsync(e => e.ID == id);

                _context.Wallets.Remove(card);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<Wallet> Get(ClaimsPrincipal claimsUser, string id)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var card = await _context.Wallets
                    .Include(e => e.Customer)
                    .FirstOrDefaultAsync(e => e.ID == id && e.Customer.Email == email);

                return card;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Wallet>> GetAll()
        {
            try
            {
                var wallet = await _context.Wallets
                    .Include(e => e.Customer)
                    .ToListAsync();

                return wallet;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Wallet>> GetWallet(ClaimsPrincipal claimsUser)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var wallet = await _context.Wallets
                    .Include(e => e.Customer)
                    .Where(e => e.Customer.Email == email)
                    .ToListAsync();

                return wallet;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<bool> Insert(Wallet item)
        {
            try
            {
                _context.Wallets.Add(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> Update(Wallet item)
        {
            try
            {
                _context.Wallets.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
