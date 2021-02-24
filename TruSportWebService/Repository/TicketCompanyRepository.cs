using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Imports;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Repository
{
    public class TicketCompanyRepository : IOnTrackRepository<TicketCompany>
    {
        OnTrackContext _context;
        EmailRepository emailRepository;
        TeamRepository teamRepository;

        public TicketCompanyRepository(OnTrackContext context)
        {
            _context = context;
            emailRepository = new EmailRepository(context);
            teamRepository = new TeamRepository(context);
        }
        public async Task Delete(string id)
        {
            try
            {
                var ticketCompany = await _context.TicketCompanys.FirstOrDefaultAsync(e => e.ID == id);

                _context.TicketCompanys.Remove(ticketCompany);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Ticket Team");
            }
        }

        public async Task<TicketCompany> Get(string id)
        {
            TicketCompany ticketCompany = new TicketCompany();

            try
            {
                ticketCompany = await _context.TicketCompanys
                    .Include(e => e.Sport)
                    .FirstOrDefaultAsync(e => e.ID == id);

            }
            catch (Exception ex)
            { }

            return ticketCompany;
        }

        public async Task<IEnumerable<TicketCompany>> GetAll()
        {
            List<TicketCompany> ticketCompanys = new List<TicketCompany>();

            try
            {
                ticketCompanys = await _context.TicketCompanys
                    .Include(e => e.Sport).ToListAsync();
            }
            catch(Exception ex)
            { }

            return ticketCompanys;
        }

        public async Task<IEnumerable<TicketScanner>> GetScanners(ClaimsPrincipal claimsUser)
        {
            try
            {
                // Get the claims values
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                if (UserInRole.Role(role, Roles.TicketOwner))
                {
                    var ticketOwner = await _context.TicketCompanyUsers
                        .Include(e => e.User).ThenInclude(e => e.Role)
                        .Include(e => e.TicketCompany)
                        .FirstOrDefaultAsync(e => e.User.Email == email && e.User.Role.Name == role);

                    var text = Roles.TicketScanner.Split(',');

                    List<TicketScanner> scanners = await _context.TicketCompanyUsers
                        .Include(e => e.User).ThenInclude(e => e.Role)
                        .Include(e => e.TicketCompany)
                        .Where(e => e.TicketCompanyID == ticketOwner.TicketCompanyID && text.Contains(e.User.Role.Name))
                        .Select(e => new TicketScanner
                        {
                            Name = e.User.Name,
                            Email = e.User.Email,
                            IsActive = e.IsActive
                        }).ToListAsync();

                    return scanners;
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<bool> UpdateScanners(List<TicketScanner> scanners, ClaimsPrincipal claimsUser)
        {
            List<TicketCompanyUser> updateScanners = new List<TicketCompanyUser>();
            try
            {
                // Get the claims values
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                var ticketOwner = await _context.TicketCompanyUsers
                    .Include(e => e.User).ThenInclude(e => e.Role)
                    .Include(e => e.TicketCompany)
                    .Where(e => e.User.Email == email && e.User.Role.Name == role).ToListAsync();

                if (ticketOwner != null && ticketOwner.Count > 0)
                {
                    var users = _context.TicketCompanyUsers.Include(e => e.User).AsEnumerable()
                        .Where(e => ticketOwner.Any(d => d.TicketCompanyID == e.TicketCompanyID)).ToList();

                    foreach (var scanner in scanners)
                    {
                        TicketCompanyUser companyUser = users.FirstOrDefault(e => e.User.Email == scanner.Email);

                        if (companyUser.IsActive != scanner.IsActive)
                        {
                            companyUser.IsActive = scanner.IsActive;

                            updateScanners.Add(companyUser);
                        }
                    }

                    if (updateScanners.Count > 0)
                    {
                        _context.UpdateRange(updateScanners);

                        await _context.SaveChangesAsync();
                    }

                    return true;
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return false;
        }

        public async Task Insert(TicketCompany item)
        {
            try
            {
                _context.TicketCompanys.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Ticket Team");
            }
        }

        public async Task Insert(List<TicketCompany> items)
        {
            try
            {
                await _context.TicketCompanys.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Ticket Team");
            }
        }

        public async Task Update(TicketCompany item)
        {
            try
            {
                _context.TicketCompanys.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Ticket Team");
            }
        }



        public async Task Update(List<TicketCompany> items)
        {
            try
            {
                _context.TicketCompanys.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Fixtures");
            }
        }
    }
}
