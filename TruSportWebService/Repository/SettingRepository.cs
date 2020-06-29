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
    public class SettingRepository : ISettingRepository<Setting>
    {
        OnTrackContext _context;

        public SettingRepository(OnTrackContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Setting>> All()
        {
            return await _context.Settings.ToListAsync();
        }

        public async Task<SportFeature> HomeMobileFeatureImages()
        {
            try
            {
                var settings = await _context.Settings.Where(e => e.Key.Contains("HomeMobileFeature")).ToListAsync();

                SportFeature sportFeature = new SportFeature
                {
                    HomeFootballImage = settings.FirstOrDefault(e => e.Key.Contains("Football")).Value,
                    HomeCricketImage = settings.FirstOrDefault(e => e.Key.Contains("Cricket")).Value,
                    //HomeTennisImage = settings.FirstOrDefault(e => e.Key.Contains("Tennis")).Value,
                    //HomeTrackFieldImage = settings.FirstOrDefault(e => e.Key.Contains("TrackField")).Value,
                    //HomeSwimmingImage = settings.FirstOrDefault(e => e.Key.Contains("Swimming")).Value,
                    //HomeRugbyImage = settings.FirstOrDefault(e => e.Key.Contains("Rugby")).Value,
                    //HomeBasketballImage = settings.FirstOrDefault(e => e.Key.Contains("Basketball")).Value,
                    //HomeFieldHockeyImage = settings.FirstOrDefault(e => e.Key.Contains("FieldHockey")).Value,
                    //HomeGolfImage = settings.FirstOrDefault(e => e.Key.Contains("Golf")).Value,
                    //HomeCyclingImage = settings.FirstOrDefault(e => e.Key.Contains("Cycling")).Value,
                };

                return sportFeature;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Setting");
            }

            return null;
        }

        public async Task<bool> DoesItemExist(string id)
        {
            return await _context.Settings.AnyAsync(e => e.ID == id);
        }

        public async Task<Setting> Get(string id)
        {
            return await _context.Settings.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<string> Insert(Setting item)
        {
            _context.Settings.Add(item);
            await _context.SaveChangesAsync();

            return item.ID;
        }

        public async Task Update(Setting item)
        {
            _context.Settings.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
