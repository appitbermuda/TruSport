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
    public class CustomerRepository : IDisposable
    {
        OnTrackContext _context;
        PasswordHasher _passwordHasher;
        EmailRepository emailRepository;

        private readonly AppSettings _appSettings;

        //public CustomerRepository(IOptions<AppSettings> appSettings)
        //{
        //    _appSettings = appSettings.Value;
        //}

        public CustomerRepository(OnTrackContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _passwordHasher = new PasswordHasher();
            emailRepository = new EmailRepository(context);
            _appSettings = appSettings.Value;
        }

        public CustomerRepository()
        {
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> Get(string id)
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return false;
        }

        public async Task<Customer> SignIn(UserAuthentication customerAuthentication)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == customerAuthentication.email && e.IsValidated);

                if (customer != null)
                {
                    PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(customer.Password, customerAuthentication.password);

                    if(passwordVerificationResult == PasswordVerificationResult.Success)
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

        public async Task<bool> Validate(string email)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == email && !e.IsValidated);

                if (customer != null)
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

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }

            return false;
        }

        public async Task<bool> ForgotPassword(ForgotPassword forgotPassword)
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

        public async Task<string> ResetPassword(PasswordReset passwordReset)
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

        public async Task<IEnumerable<Customer>> GetAll()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();

                customers.ForEach(e => e.Password = null);
                customers.ForEach(e => e.TemporaryPassword = null);
                customers.ForEach(e => e.Token = null);

                return customers;
            }
            catch(Exception ex)
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
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.Team)
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

        public async Task<List<Customer>> GetCustomersByTeam(string TeamID)
        {
            try
            {
                var orders = await _context.OrderDetails
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.Team)
                    .Where(e => e.FixtureProduct.Product.TeamID == TeamID).ToListAsync();

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

        public async Task<List<Customer>> GetCustomersByDateByTeam(DateTime date, string TeamID)
        {
            try
            {
                var orders = await _context.OrderDetails
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Fixture)
                    .Include(e => e.FixtureProduct).ThenInclude(e => e.Product).ThenInclude(e => e.Team)
                    .Where(e => e.Order.Date == date && e.FixtureProduct.Product.TeamID == TeamID).ToListAsync();

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

        public Task<IEnumerable<Customer>> GetByTeam(string teamID)
        {
            throw new NotImplementedException();
        }

        public Task Insert(Customer item)
        {
            throw new NotImplementedException();
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, "Customer");
            }
        }

        public async Task<Customer> Authenticate(string customername, string password)
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
