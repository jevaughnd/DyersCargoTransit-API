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
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration config;
       

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        //register
        public async Task<bool> RegisterUser(AppUser user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.Username,
                Email = user.Username,
            };

            var result = await userManager.CreateAsync(identityUser, user.Password);
            return result.Succeeded;
        }






        //login----------------
        public async Task<bool> LoginUser(AppUser user)
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




        //Generate Token-------------
        public async Task<string> GenerateToken(AppUser user)
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
