using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Ad;

namespace OnTrackWebService.Repository
{
    public class ImpressionRepository : IOnTrackRepository<Impression>
    {
        OnTrackContext _context;

        public ImpressionRepository(OnTrackContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                var impression = await _context.Impressions.FirstOrDefaultAsync(e => e.ID == id);

                _context.Impressions.Remove(impression);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<Impression> Get(string id)
        {
            try
            {
                var impression = await _context.Impressions.FirstOrDefaultAsync(e => e.ID == id);

                return impression;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Impression>> GetAll()
        {
            try
            {
                var ads = await _context.Impressions.ToListAsync();

                return ads;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<bool> Insert(Impression item)
        {
            try
            {
                _context.Impressions.Add(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> Update(Impression item)
        {
            try
            {
                _context.Impressions.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        Task IOnTrackRepository<Impression>.Insert(Impression item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Impression>.Update(Impression item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Impression>.Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
