using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;

namespace OnTrackWebService.Repository
{
    public class TicketConfigurationRepository : IOnTrackRepository<TicketConfiguration>
    {
        OnTrackContext _context;

        public TicketConfigurationRepository(OnTrackContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketConfiguration>> All()
        {
            return await _context.TicketConfigurations.Include(e => e.TicketCompany).ToListAsync();
        }

        public async Task<TicketConfiguration> TeamConfiguration(ClaimsPrincipal claimsUser)
        {
            try
            {
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                      .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var config = await _context.TicketConfigurations.Include(e => e.TicketCompany).FirstOrDefaultAsync(e => e.TicketCompanyID == companyUser.TicketCompanyID);
                return config;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketConfiguration");
            }

            return null;
        }

        public async Task<TicketConfiguration> Get(string id)
        {
            try
            {
                var config = await _context.TicketConfigurations.Include(e => e.TicketCompany).FirstOrDefaultAsync(e => e.ID == id);
                return config;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketConfiguration");
            }

            return null;
        }

        public async Task Update(TicketConfiguration item)
        {
            try
            {
                _context.TicketConfigurations.Update(item);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketConfiguration");
            }
        }

        public async Task<IEnumerable<TicketConfiguration>> GetAll()
        {
            try
            {
                var ticketconfigurations = await _context.TicketConfigurations.Include(e => e.TicketCompany).ToListAsync();

                return ticketconfigurations;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketConfiguration");
            }

            return null;
        }

        public async Task Insert(TicketConfiguration item)
        {
            try
            {
                _context.TicketConfigurations.Add(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "TicketConfiguration");
            }
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
