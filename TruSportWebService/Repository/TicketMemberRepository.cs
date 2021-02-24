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
    public class TicketMemberRepository : IOnTrackRepository<TicketMember>
    {
        OnTrackContext _context;
        EmailRepository emailRepository;
        TeamRepository teamRepository;

        public TicketMemberRepository(OnTrackContext context)
        {
            _context = context;
            emailRepository = new EmailRepository(context);
            teamRepository = new TeamRepository(context);
        }
        public async Task Delete(string id)
        {
            try
            {
                var ticketMember = await _context.TicketMembers.FirstOrDefaultAsync(e => e.ID == id);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Ticket Member");
            }
        }

        public async Task<TicketMember> AddMember(string CustomerEmail, ClaimsPrincipal claimsUser)
        {
            TicketMember ticketMember = new TicketMember();

            try
            {
                // Get the claims values
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == CustomerEmail && e.IsValidated);
                var ticketCompany = await _context.TicketCompanys.FirstOrDefaultAsync(e => e.ID == companyUser.TicketCompanyID);

                if (ticketCompany != null && customer != null)
                {
                    ticketMember = new TicketMember
                    {
                        CustomerID = customer.ID,
                        TicketCompanyID = ticketCompany.ID
                    };

                    _context.TicketMembers.Add(ticketMember);
                    await _context.SaveChangesAsync();
                }

                customer.Password = null;
                customer.TemporaryPassword = null;

                ticketMember.Customer = customer;
                ticketMember.TicketCompany = ticketCompany;

                return ticketMember;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<bool> RemoveMember(string CustomerEmail, ClaimsPrincipal claimsUser)
        {
            //TicketMember ticketMember = new TicketMember();

            try
            {
                // Get the claims values
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == CustomerEmail && e.IsValidated);
                var ticketCompany = await _context.TicketCompanys.FirstOrDefaultAsync(e => e.ID == companyUser.TicketCompanyID);

                var ticketMember = await _context.TicketMembers.Include(e => e.Customer).Include(e => e.TicketCompany).FirstOrDefaultAsync(e => e.Customer.Email == CustomerEmail && e.TicketCompany.ID == companyUser.TicketCompanyID);

                if(ticketMember != null)
                {
                    _context.TicketMembers.Remove(ticketMember);

                    await _context.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<TicketMember> Get(string email)
        {
            TicketMember ticketMember = new TicketMember();

            try
            {
                ticketMember = await _context.TicketMembers
                    .Include(e => e.TicketCompany)
                    .Include(e => e.Customer)
                    .FirstOrDefaultAsync(e => e.Customer.Email == email);

            }
            catch (Exception ex)
            { }

            return ticketMember;
        }

        public async Task<IEnumerable<TicketMember>> GetAll(ClaimsPrincipal user)
        {
            List<TicketMember> ticketMembers = new List<TicketMember>();

            try
            {
                // Get the claims values
                var email = user.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var companyUser = await _context.TicketCompanyUsers.FirstOrDefaultAsync(e => e.User.Email == email);

                ticketMembers = await _context.TicketMembers
                    .Include(e => e.TicketCompany)
                    .Include(e => e.Customer)
                    .Where(e => e.TicketCompanyID == companyUser.TicketCompanyID).ToListAsync();
            }
            catch(Exception ex)
            { }

            return ticketMembers;
        }
        
        public async Task<ImportTicketMembers> UploadTicketMembers(IFormFile file)
        {
            try
            {
                List<TicketMembers> errorTicketMembers = new List<TicketMembers>();
                List<TicketMember> ticketMembers = new List<TicketMember>();
                List<TicketCompany> companies = _context.TicketCompanys.ToList();
                List<Customer> customers = _context.Customers.ToList();
                TicketCompany company = null;
                Customer customer = null;

                //Stream reader = file.OpenReadStream();

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    csv.Configuration.IgnoreBlankLines = true;
                    csv.Configuration.TrimOptions = TrimOptions.Trim;
                    var records = csv.GetRecords<TicketMembers>();

                    foreach (var record in records)
                    {
                        try
                        {
                            company = companies.FirstOrDefault(e => e.Name.ToLower() == record.Team.ToLower() || e.Alias.ToLower() == record.Team.ToLower());
                            customer = customers.FirstOrDefault(e => e.Email == record.Email);

                            ticketMembers.Add(new TicketMember
                            {
                                TicketCompanyID = company.ID,                                
                                CustomerID = customer.ID
                            });
                        }
                        catch (Exception ex)
                        {
                            record.Exception = ex.Message;
                            errorTicketMembers.Add(record);
                        }
                    }

                    try
                    {
                        await Insert(ticketMembers);
                    }
                    catch (Exception ex)
                    {
                        return new ImportTicketMembers
                        {
                            Message = "Error importing ticket members to database.",
                            Exception = ex.Message
                        };
                    }
                }

                if (errorTicketMembers != null && errorTicketMembers.Count > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream))
                    using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(errorTicketMembers);
                        streamWriter.Flush();

                        return new ImportTicketMembers
                        {
                            Message = "Successfully imported ticket members with errors, please verify the following rows are correctly configured.",
                            ErrorRows = errorTicketMembers,
                            ErrorFile = memoryStream.ToArray()
                        };
                    }
                }

                return new ImportTicketMembers
                {
                    Message = "Successfully imported ticket members!"
                };

            }
            catch (Exception ex)
            {
                return new ImportTicketMembers
                {
                    Message = "Error importing ticket members!",
                    Exception = ex.Message
                };
            }


        }

        public Task<IEnumerable<TicketMember>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task Insert(TicketMember item)
        {
            try
            {
                _context.TicketMembers.Add(item);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Insert Ticket Member");
            }


        }

        public async Task Insert(List<TicketMember> items)
        {
            try
            {
                await _context.TicketMembers.AddRangeAsync(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add Ticket Member");
            }
        }

        public async Task Update(TicketMember item)
        {
            try
            {
                _context.TicketMembers.Update(item);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Ticket Member");
            }
        }



        public async Task Update(List<TicketMember> items)
        {
            try
            {
                _context.TicketMembers.UpdateRange(items);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update Fixtures");
            }
        }
    }
}
