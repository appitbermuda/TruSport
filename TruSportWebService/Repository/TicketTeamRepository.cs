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
    public class TicketTeamRepository : IOnTrackRepository<TicketTeam>
    {
        OnTrackContext _context;
        EmailRepository emailRepository;
        TeamRepository teamRepository;

        public TicketTeamRepository(OnTrackContext context)
        {
            _context = context;
            emailRepository = new EmailRepository(context);
            teamRepository = new TeamRepository(context);
        }
        public async Task Delete(string id)
        {
            try
            {
                var ticketTeam = await _context.TicketTeams.FirstOrDefaultAsync(e => e.ID == id);

                _context.TicketTeams.Remove(ticketTeam);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Ticket Team");
            }
        }

        public async Task<TicketTeam> Get(string id)
        {
            TicketTeam ticketTeam = new TicketTeam();

            try
            {
                ticketTeam = await _context.TicketTeams
                    .Include(e => e.Team)
                    .FirstOrDefaultAsync(e => e.ID == id);

            }
            catch (Exception ex)
            { }

            return ticketTeam;
        }

        public async Task<IEnumerable<TicketTeam>> GetAll()
        {
            List<TicketTeam> ticketTeams = new List<TicketTeam>();

            try
            {
                ticketTeams = await _context.TicketTeams
                    .Include(e => e.Team).ToListAsync();
            }
            catch(Exception ex)
            { }

            return ticketTeams;
        }

        public async Task Insert(TicketTeam item)
        {
            try
            {
                _context.TicketTeams.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Ticket Team");
            }
        }

        public async Task Insert(List<TicketTeam> items)
        {
            try
            {
                await _context.TicketTeams.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Ticket Team");
            }
        }

        public async Task Update(TicketTeam item)
        {
            try
            {
                _context.TicketTeams.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Ticket Team");
            }
        }



        public async Task Update(List<TicketTeam> items)
        {
            try
            {
                _context.TicketTeams.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Fixtures");
            }
        }
    }
}
