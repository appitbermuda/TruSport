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
    public class AdRepository : IOnTrackRepository<Ad>
    {
        OnTrackContext _context;

        public AdRepository(OnTrackContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                var ad = await _context.Ads.FirstOrDefaultAsync(e => e.ID == id);

                _context.Ads.Remove(ad);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<Ad> Get(string id)
        {
            try
            {
                var ad = await _context.Ads.FirstOrDefaultAsync(e => e.ID == id);

                return ad;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Ad>> GetAll()
        {
            try
            {
                var ads = await _context.Ads.ToListAsync();

                return ads;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Ad>> Retreive()
        {
            try
            {
                var ads = await _context.Ads.Where(e => e.StartDate <= DateTime.Now && ((e.EndDate.HasValue && e.EndDate >= DateTime.Now) || !e.EndDate.HasValue)).ToListAsync();

                ads.ForEach(e => e.Image = Constants.ImageEndpoint + e.Image);

                return ads;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task Impression(string adID)
        {
            try
            {
                var impression = await _context.Impressions.FirstOrDefaultAsync(e => e.AdID == adID);

                if (impression == null)
                {
                    impression = new Impression();
                    impression.AdID = adID;
                    impression.Count = 1;

                    _context.Impressions.Add(impression);
                }
                else
                {
                    impression.Count = impression.Count + 1;

                    _context.Impressions.Update(impression);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<bool> Insert(Ad item)
        {
            try
            {
                _context.Ads.Add(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> Update(Ad item)
        {
            try
            {
                _context.Ads.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        Task IOnTrackRepository<Ad>.Insert(Ad item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Ad>.Update(Ad item)
        {
            throw new NotImplementedException();
        }

        Task IOnTrackRepository<Ad>.Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
