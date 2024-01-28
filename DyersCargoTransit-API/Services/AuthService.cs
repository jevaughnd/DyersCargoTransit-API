using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DyersCargoTransit_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private ApplicationUser user;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        ////register
        //public async Task<bool> Register(User user)
        //{
        //    var identityUser = new ApplicationUser
        //    {
        //        UserName = user.Username,
        //        Email = user.Username,
        //    };

        //    var result = await userManager.CreateAsync(identityUser, user.Password);
        //    return result.Succeeded;
        //}





        // REGISTER
        public async Task<bool> Register(User user)
        {
            var identityUser = new ApplicationUser
            {
                UserName = user.Username,
                Email = user.Username,
            };

            var result = await userManager.CreateAsync(identityUser, user.Password);

            if (result.Succeeded)
            {
                // Assign default role "Employee"
                var roles = new List<string> { "Employee" };
                await AssignRoles(user.Username, roles);

                // Fetch the registered user to ensure we have the latest data
                var registeredUser = await userManager.Users
                    .Include(u => u.UserProfile)
                    .FirstOrDefaultAsync(u => u.UserName == user.Username);

                if (registeredUser != null)
                {
                    // Check if the UserProfile already exists
                    var existingProfile = registeredUser.UserProfile;

                    if (existingProfile == null)
                    {
                        // Create and associate a new UserProfile
                        var userProfile = new UserProfile
                        {
                            ApplicationUserId = registeredUser.Id,
                            FullName = user.Username, // You can use other relevant information
                            //TRN = user.Username,
                            TRN = user.Username.Substring(0, Math.Min(20, user.Username.Length)),
                            Bio = user.Username,
                            //PhoneNumber = user.Username,
                            PhoneNumber = user.Username.Substring(0, Math.Min(15, user.Username.Length)),
                            Street = user.Username,
                            Town = user.Username,
                            DOB = DateTime.Now, // You might want to set a default value or prompt the user for this information
                            ProfilePicture = user.Username,
                            ParishId = 1

                        };

                        // Associate the UserProfile with the registered user
                        registeredUser.UserProfile = userProfile;

                        // Save changes to the database
                        await userManager.UpdateAsync(registeredUser);

                        
                   


                        // Return true for successful registration
                        return true;
                    }
                    else
                    {
                        // UserProfile already exists (optional: you might want to log this)
                        return true;
                    }
                }
                else
                {
                    // Return false if unable to retrieve registered user
                    return false;
                }
            }

            // Return false for failed registration
            return false;
        }







        //login----------------
        public async Task<bool> Login(User user)
        {
            var identityUser = await userManager.FindByEmailAsync(user.Username);
            if (identityUser == null)
            {
                return false;
            }
            return await userManager.CheckPasswordAsync(identityUser, user.Password);
        }


        //assign roles------------
        public async Task<bool> AssignRoles(string userName, IEnumerable<string> roles)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return false;
            }
            var result = await userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }


        //generate token-------------
        public async Task<string> GenerateToken(User user)
        {

            //--
            var identityUser = await userManager.FindByEmailAsync(user.Username);
            if (identityUser == null)
            {
                return null;
            }

            var userRoles = await userManager.GetRolesAsync(identityUser);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Username),
            };

            if (userRoles.Any())
            {
                claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            }
            //--




            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWTConfig:Key").Value));
            var signingCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    issuer: config.GetSection("JWTConfig: Issuer").Value,
                    audience: config.GetSection("JWTConfig: Audience").Value,
                    signingCredentials: signingCreds
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

       
    }
}
