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
        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Ad> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ad>> GetAll()
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

        public Task Insert(Ad item)
        {
            throw new NotImplementedException();
        }

        public Task Update(Ad item)
        {
            throw new NotImplementedException();
        }
    }
}
