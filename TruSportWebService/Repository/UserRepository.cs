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

namespace OnTrackWebService.Repository
{
    public class UserRepository : IDisposable
    {
        OnTrackContext _context;
        PasswordHasher _passwordHasher;
        EmailRepository emailRepository;

        private readonly AppSettings _appSettings;

        //public UserRepository(IOptions<AppSettings> appSettings)
        //{
        //    _appSettings = appSettings.Value;
        //}

        public UserRepository(OnTrackContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _passwordHasher = new PasswordHasher();
            emailRepository = new EmailRepository(context);
            _appSettings = appSettings.Value;
        }

        public UserRepository()
        {
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Get(string id)
        {
            var user = await _context.Users.Include(e => e.Role).FirstOrDefaultAsync(e => e.ID == id);

            user.Password = null;

            return user;
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

        public async Task<User> SignIn(UserAuthentication userAuthentication)
        {
            try
            {
                var user = await _context.Users.Include(e => e.Role).Include(e => e.Team).FirstOrDefaultAsync(e => e.Email == userAuthentication.email && e.IsValidated);

                if (user != null)
                {
                    PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user.Password, userAuthentication.password);

                    if(passwordVerificationResult == PasswordVerificationResult.Success)
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

        public async Task<UserResponse> SignUp(User user)
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

                            user = await _context.Users.Include(e => e.Role).Include(e => e.Team).FirstOrDefaultAsync(e => e.ID == user.ID);

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

        public async Task<bool> Validate(string email)
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

        public async Task<bool> ForgotPassword(ForgotPassword forgotPassword)
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

        public async Task<string> ResetPassword(PasswordReset passwordReset)
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

        public async Task<IEnumerable<AllUsers>> GetAll()
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

        public async Task<User> Authenticate(string username, string password)
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
