using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnTrackWebService.Data;
using OnTrackWebService.Interfaces;
using OnTrackWebService.Models;
using OnTrackWebService.Models.Shop;

namespace OnTrackWebService.Repository
{
    public class AuthenticationRepository : IDisposable
    {
        OnTrackContext _context;
        PasswordHasher _passwordHasher;
        EmailRepository emailRepository;

        private readonly AppSettings _appSettings;

        //public AuthenticationRepository(IOptions<AppSettings> appSettings)
        //{
        //    _appSettings = appSettings.Value;
        //}

        public AuthenticationRepository(OnTrackContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _passwordHasher = new PasswordHasher();
            emailRepository = new EmailRepository(context);
            _appSettings = appSettings.Value;
        }

        public AuthenticationRepository()
        {
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUser(string id)
        {
            var user = await _context.Users.Include(e => e.Role).FirstOrDefaultAsync(e => e.ID == id);

            user.Password = null;

            return user;
        }

        public async Task<bool> IsActive(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.IsActive);

                if (user != null)
                    return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return false;
        }

        public async Task<bool> UserExists(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email);

                if (user != null)
                    return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return false;
        }

        public async Task<User> SignInUser(UserAuthentication userAuthentication)
        {
            try
            {
                var user = await _context.Users.Include(e => e.Role).ThenInclude(e => e.Sport)
                    .Include(e => e.UserRoles)
                    .Include(e => e.UserTeams).ThenInclude(e => e.Team)
                    .Include(e => e.TicketCompanyUsers).ThenInclude(e => e.TicketCompany)
                    .FirstOrDefaultAsync(e => e.Email == userAuthentication.email && e.IsValidated && ((!String.IsNullOrEmpty(userAuthentication.sport) && userAuthentication.sport.ToLower() == e.Role.Sport.Name.ToLower()) || String.IsNullOrEmpty(userAuthentication.sport)));
                //var userRoles = await _context.UserRoles.Where(e => e.UserID == user.ID).ToListAsync();

                if (user != null)
                {
                    //if(user.UserRoles != null)
                    //{
                    //    if (user.Role.Name.Contains("Team") && user.UserRoles.Any(e => !String.IsNullOrEmpty(e.AdminIdentifier)))
                    //    {
                    //        var team = await _context.UserTeams.Where(e => e.UserID == user.ID).ToListAsync();
                    //        user.Team = team;
                    //    }

                    //    if (user.Role.Name.Contains("League") && user.UserRoles.Any(e => !String.IsNullOrEmpty(e.AdminIdentifier)))
                    //    {
                    //        var league = await _context.Leagues.FirstOrDefaultAsync(e => user.UserRoles.Any(d => d.AdminIdentifier == e.ID));
                    //        user.League = league;
                    //    }

                    //    if (user.Role.Name.Contains("Ticket") && user.UserRoles.Any(e => !String.IsNullOrEmpty(e.AdminIdentifier)))
                    //    {
                    //        var companys = await _context.TicketCompanyUsers.Where(e => e.UserID == user.ID).ToListAsync();
                    //        user.TicketCompanyUsers = companys;
                    //    }
                    //}

                    PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user.Password, userAuthentication.password);

                    if (passwordVerificationResult == PasswordVerificationResult.Success)
                    {
                        // authentication successful so generate jwt token
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                            new Claim(ClaimTypes.Name, user.Email.ToString()),
                            new Claim(ClaimTypes.Role, user.Role.Name)
                            }),
                            Expires = DateTime.UtcNow.AddYears(100),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        user.Token = tokenHandler.WriteToken(token);

                        // remove password before returning
                        user.Password = null;

                        return user;

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return null;
        }

        public async Task<UserResponse> SignUp(UserRequest user)
        {
            TicketCompanyUser ticketCompanyUser = null;
            UserTeam userTeam = null;

            try
            {
                if (Regex.IsMatch(user.Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                {
                    if (user.Password.Length >= 8)
                    {
                        var userExists = await _context.Users.AnyAsync(e => e.Email == user.Email);
                        if (!userExists)
                        {

                            _context.Database.BeginTransaction();
                            //encrypt password
                            user.Password = _passwordHasher.HashPassword(user.Password);

                            var role = await _context.Roles.FirstOrDefaultAsync(e => e.ID == user.RoleID);

                            User newUser = new User
                            {
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email,
                                Password = user.Password,
                                RoleID = user.RoleID
                            };

                            _context.Users.Add(newUser);
                            await _context.SaveChangesAsync();

                            if (UserInRole.Role(role.Name, Roles.TicketAdmin))
                            {
                                ticketCompanyUser = new TicketCompanyUser();
                                ticketCompanyUser.TicketCompanyID = user.TicketCompanyID;

                                ticketCompanyUser.UserID = user.ID;

                                _context.TicketCompanyUsers.Add(ticketCompanyUser);
                                await _context.SaveChangesAsync();
                            }

                            if (UserInRole.Role(role.Name, Roles.TeamAdmin))
                            {
                                userTeam = new UserTeam();
                                userTeam.TeamID = user.TeamID;

                                userTeam.UserID = user.ID;

                                _context.UserTeams.Add(userTeam);
                                await _context.SaveChangesAsync();
                            }

                            _context.Database.CommitTransaction();

                            //SEND EMAIL
                            try
                            {
                                newUser.Role = role;
                                await emailRepository.Welcome(user.Email);
                                await emailRepository.SendSignUpEmail(newUser, userTeam, ticketCompanyUser);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message, "Welcome Email");
                            }

                            List<UserTeam> userTeams = new List<UserTeam>();
                            userTeams.Add(userTeam);

                            List<TicketCompanyUser> ticketCompanyUsers = new List<TicketCompanyUser>();
                            ticketCompanyUsers.Add(ticketCompanyUser);

                            return new UserResponse
                            {
                                ID = user.ID,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Role = role,
                                RoleID = user.RoleID,
                                Email = user.Email,
                                UserTeams = userTeams,
                                TicketCompanyUsers = ticketCompanyUsers
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                Debug.WriteLine(ex.Message, "User");
            }

            return null;
        }

        public async Task<UserResponse> SignUpUser(User user)
        {
            try
            {
                if (Regex.IsMatch(user.Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                {
                    if (user.Password.Length >= 8)
                    {
                        var userExists = await _context.Users.AnyAsync(e => e.Email == user.Email);
                        if (!userExists)
                        {
                            //encrypt password
                            user.Password = _passwordHasher.HashPassword(user.Password);

                            _context.Users.Add(user);
                            await _context.SaveChangesAsync();

                            user = await _context.Users.Include(e => e.Role).FirstOrDefaultAsync(e => e.ID == user.ID);

                            if (!String.IsNullOrEmpty(user.TeamID))
                            {
                                var team = await _context.Teams.FirstOrDefaultAsync(e => e.ID == user.TeamID);
                                user.Team = team;
                            }

                            //SEND EMAIL
                            try
                            {
                                await emailRepository.Welcome(user.Email);
                                await emailRepository.SendSignUpEmail(user.FirstName + " " + user.LastName, user.Email, user.Role.Name, user.Team != null ? user.Team.Name : null);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message, "Welcome Email");
                            }

                            return new UserResponse
                            {
                                ID = user.ID,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Role = user.Role,
                                RoleID = user.RoleID,
                                Email = user.Email,
                                TeamID = user.TeamID,
                                Team = user.Team
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return null;
        }

        public async Task<bool> ValidateUser(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && !e.IsValidated);

                if (user != null)
                {
                    user.IsValidated = true;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    try
                    {
                        await emailRepository.UserValidated(email);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message, "User Validate");
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

        public async Task<bool> ForgotUserPassword(ForgotPassword forgotPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == forgotPassword.Email);

                if (user != null)
                {
                    //genereate pw
                    string temporaryPassword = RandomPassword();

                    //encrypt password
                    user.TemporaryPassword = _passwordHasher.HashPassword(temporaryPassword);

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    //SEND EMAIL
                    await emailRepository.SendPasswordResetEmail(user.FirstName, user.Email, temporaryPassword);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return false;
        }

        public async Task<string> ResetUserPassword(PasswordReset passwordReset)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == passwordReset.Email);

                PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user.TemporaryPassword, passwordReset.TemporaryPassword);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    
                    //encrypt password
                    user.Password = _passwordHasher.HashPassword(passwordReset.Password);
                    user.TemporaryPassword = null;

                    _context.Users.Update(user);

                    await _context.SaveChangesAsync();

                    //user = await _context.Users.Include(e => e.UserType).Include(e => e.Team).FirstOrDefaultAsync(e => e.ID == user.ID);

                    //SEND EMAIL
                    //await emailRepository.SendSignUpEmail(user.FirstName + " " + user.LastName, user.Email, user.UserType.Name, user.Team != null ? user.Team.Name : null);

                    return "Password reset successfully!";
                }

                return "Temporary password is incorrect or is expired.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }

            return "There was an error resetting your password.";
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

                if (Roles.TicketOwner.Contains(role))
                {
                    User owner = await _context.Users.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == email && e.Role.Name == role);


                    List<TicketScanner> scanners = await _context.Users.Include(e => e.Role).Where(e => e.Role.Name == Constants.TicketingAdmin && e.TeamID == owner.TeamID)
                        .Select(e => new TicketScanner
                        {
                            Name = e.FirstName + " " + e.LastName,
                            Email = e.Email,
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

        public async Task<IEnumerable<AllUsers>> GetAllUsers()
        {
            return await _context.AllUsers.Include(e => e.UserType).Include(e => e.Team).ToListAsync();
        }

        public Task<IEnumerable<User>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(User item)
        {
            throw new NotImplementedException();
        }

        public async Task Update(List<TicketScanner> scanners, ClaimsPrincipal claimsUser)
        {
            List<User> updateScanners = new List<User>();
            try
            {
                // Get the claims values
                var email = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();

                var role = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Role)
                                   .Select(c => c.Value).SingleOrDefault();

                User owner = await _context.Users.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == email && e.Role.Name == role);

                var users = await _context.Users.Where(e => e.TeamID == owner.TeamID).ToListAsync();

                foreach(var scanner in scanners)
                {
                    User user = users.FirstOrDefault(e => e.Email == scanner.Email);

                    if (user.IsActive != scanner.IsActive)
                    {
                        user.IsActive = scanner.IsActive;

                        updateScanners.Add(user);
                    }
                }

                if (updateScanners.Count > 0)
                {
                    _context.UpdateRange(updateScanners);

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }
        }

        public async Task Update(User item)
        {
            try
            {
                var dbUser = await _context.Users.FirstOrDefaultAsync(e => e.ID == item.ID);

                dbUser.FirstName = item.FirstName;
                dbUser.LastName = item.LastName;
                dbUser.Email = item.Email;
                dbUser.RoleID = item.RoleID;
                dbUser.TeamID = item.TeamID;
                dbUser.IsValidated = item.IsValidated;

                _context.Update(dbUser);

                await _context.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "User");
            }
        }

        public async Task<User> AuthenticateUser(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public async Task<Customer> GetCustomer(string id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == id);

            customer.Password = null;

            return customer;
        }

        public async Task<bool> CustomerExists(string email)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == email);

                if (customer != null)
                    return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return false;
        }

        public async Task<Customer> SignInCustomer(UserAuthentication customerAuthentication)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == customerAuthentication.email && e.IsValidated);

                if (customer != null)
                {
                    PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(customer.Password, customerAuthentication.password);

                    if (passwordVerificationResult == PasswordVerificationResult.Success)
                    {
                        // authentication successful so generate jwt token
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, customer.Email.ToString()),
                                new Claim(ClaimTypes.Role, "TicketBuyer")
                            }),
                            Expires = DateTime.UtcNow.AddYears(100),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        customer.Token = tokenHandler.WriteToken(token);

                        // remove password before returning
                        customer.Password = null;

                        return customer;

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return null;
        }

        public async Task<CustomerResponse> SignUp(Customer customer)
        {
            try
            {
                if (Regex.IsMatch(customer.Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$"))
                {
                    if (customer.Password.Length >= 8)
                    {
                        var customerExists = await _context.Customers.AnyAsync(e => e.Email == customer.Email);
                        if (!customerExists)
                        {
                            //encrypt password
                            customer.Password = _passwordHasher.HashPassword(customer.Password);

                            _context.Customers.Add(customer);
                            await _context.SaveChangesAsync();

                            customer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == customer.ID);

                            //SEND EMAIL
                            try
                            {
                                await emailRepository.WelcomeCustomer(customer.Email);
                                await emailRepository.SendCustomerSignUpEmail(customer);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message, "Welcome Email");
                            }

                            return new CustomerResponse
                            {
                                ID = customer.ID,
                                FirstName = customer.FirstName,
                                LastName = customer.LastName,
                                Email = customer.Email,
                                Phone = customer.Phone
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return null;
        }

        public async Task<string> ValidateCustomer(string email)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == email);

                if (customer != null)
                {
                    if (!customer.IsValidated)
                    {
                        customer.IsValidated = true;

                        _context.Customers.Update(customer);
                        await _context.SaveChangesAsync();

                        //try
                        //{
                        //    await emailRepository.CustomerValidated(email);
                        //}
                        //catch (Exception ex)
                        //{
                        //    Debug.WriteLine(ex.Message, "Customer Validate");
                        //}

                        return "Account validated successfully!";
                    }
                    else
                        return "Account already validated.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return "Account cannot be validated at this time.";
        }

        public async Task<bool> ForgotCustomerPassword(ForgotPassword forgotPassword)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == forgotPassword.Email);

                if (customer != null)
                {
                    //genereate pw
                    string temporaryPassword = RandomPassword();

                    //encrypt password
                    customer.TemporaryPassword = _passwordHasher.HashPassword(temporaryPassword);

                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();

                    //SEND EMAIL
                    await emailRepository.SendPasswordResetEmail(customer.FirstName, customer.Email, temporaryPassword);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return false;
        }

        public async Task<bool> CustomerHasTemporaryPassword(string email)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == email && e.TemporaryPassword != null);

                if (customer != null)
                    return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Has Temporary Password");
            }

            return false;
        }

        public async Task<bool> UserHasTemporaryPassword(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.TemporaryPassword != null);

                if (user != null)
                    return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Has Temporary Password");
            }

            return false;
        }

        public async Task<string> ResetCustomerPassword(PasswordReset passwordReset)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == passwordReset.Email);

                PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(customer.TemporaryPassword, passwordReset.TemporaryPassword);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {

                    //encrypt password
                    customer.Password = _passwordHasher.HashPassword(passwordReset.Password);
                    customer.TemporaryPassword = null;

                    _context.Customers.Update(customer);

                    await _context.SaveChangesAsync();

                    //customer = await _context.Customers.Include(e => e.CustomerType).Include(e => e.Team).FirstOrDefaultAsync(e => e.ID == customer.ID);

                    //SEND EMAIL
                    //await emailRepository.SendSignUpEmail(customer.FirstName + " " + customer.LastName, customer.Email, customer.CustomerType.Name, customer.Team != null ? customer.Team.Name : null);

                    return "Password reset successfully!";
                }

                return "Temporary password is incorrect or is expired.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return "There was an error resetting your password.";
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();

                customers.ForEach(e => e.Password = null);
                customers.ForEach(e => e.TemporaryPassword = null);
                customers.ForEach(e => e.Token = null);

                return customers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get All");
            }

            return null;
        }

        public async Task<List<Customer>> GetCustomersByDate(DateTime date)
        {
            try
            {
                var orders = await _context.OrderDetails
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.Order.Date == date).ToListAsync();

                orders.ForEach(e => e.Order.Customer.Password = null);
                orders.ForEach(e => e.Order.Customer.TemporaryPassword = null);
                orders.ForEach(e => e.Order.Customer.Token = null);

                orders.ForEach(e => e.Order.Customer.Orders.Add(e.Order));

                var customers = orders.Select(e => e.Order.Customer).Distinct().ToList();

                return customers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get All");
            }

            return null;
        }

        public async Task<List<Customer>> GetCustomersByTeam(string TicketCompanyID)
        {
            try
            {
                var orders = await _context.OrderDetails
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.FixtureProduct.Product.TicketCompanyID == TicketCompanyID).ToListAsync();

                orders.ForEach(e => e.Order.Customer.Password = null);
                orders.ForEach(e => e.Order.Customer.TemporaryPassword = null);
                orders.ForEach(e => e.Order.Customer.Token = null);

                orders.ForEach(e => e.Order.Customer.Orders.Add(e.Order));

                var customers = orders.Select(e => e.Order.Customer).Distinct().ToList();

                return customers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get All");
            }

            return null;
        }

        public async Task<List<Customer>> GetCustomersByDateByTeam(DateTime date, string TicketCompanyID)
        {
            try
            {
                var orders = await _context.OrderDetails
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.TicketCompany)
                    .Where(e => e.Order.Date == date && e.FixtureProduct.Product.TicketCompanyID == TicketCompanyID).ToListAsync();

                orders.ForEach(e => e.Order.Customer.Password = null);
                orders.ForEach(e => e.Order.Customer.TemporaryPassword = null);
                orders.ForEach(e => e.Order.Customer.Token = null);

                orders.ForEach(e => e.Order.Customer.Orders.Add(e.Order));

                var customers = orders.Select(e => e.Order.Customer).Distinct().ToList();

                return customers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Get All");
            }

            return null;
        }

        public async Task Update(Customer item)
        {
            try
            {
                var dbCustomer = await _context.Customers.FirstOrDefaultAsync(e => e.ID == item.ID);

                dbCustomer.FirstName = item.FirstName;
                dbCustomer.LastName = item.LastName;
                dbCustomer.Email = item.Email;
                dbCustomer.Phone = item.Phone;
                dbCustomer.IsValidated = item.IsValidated;

                _context.Update(dbCustomer);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }
        }

        public async Task<Customer> AuthenticateCustomer(string customername, string password)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Email == customername && x.Password == password);

            // return null if customer not found
            if (customer == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, customer.Email.ToString()),
                    new Claim(ClaimTypes.Role, "TicketBuyer")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            customer.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            customer.Password = null;

            return customer;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        // Generate a random password of a given length (optional)  
        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        // Generate a random number between two numbers    
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size and case.   
        // If second parameter is true, the return string is lowercase  
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}
