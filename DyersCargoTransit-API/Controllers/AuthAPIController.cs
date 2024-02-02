using DyersCargoTransit_API.Data;
using DyersCargoTransit_API.Model;
using DyersCargoTransit_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DyersCargoTransit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;

     


        //public AuthAPIController(IAuthService authService, UserManager<IdentityUser> userManager)
        //{
        //    this._authService = authService;
        //    this._userManager = userManager;
        //}

        private readonly ApplicationDbContext _cxt;

        public AuthAPIController(IAuthService authService, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            this._authService = authService;
            this._userManager = userManager;
            this._cxt = context;
        }



        ////Original Register Works

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(AppUser user)
        {
            if (await _authService.RegisterUser(user))
            {
                //assign default role Customer
                var roles = new List<string> { "Customer" };
                await _authService.AssignRoles(user.Username, roles);

                return Ok(new { status = "succes", message = "registration succesful" });
            }
            return BadRequest(new { status = "fail", message = "registration failed" });
        }













        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(AppUser user)
        {
            var result = await _authService.LoginUser(user);

            //user
            var identityUser = await _userManager.FindByEmailAsync(user.Username);

            var lstRoles = GetRoles(user);



            if (result == true)
            {
                var token = _authService.GenerateToken(user);

                return Ok(new { status = "succes", message = "login succesful", data = token, user = identityUser, roles = lstRoles});
            }
            return BadRequest(new { status = "failed", message = "login failed" });
        }


        //-------------------------------------------------------------------------------------------------


        [HttpPost]
        public List<string> GetRoles(AppUser user)
        {
            var lstRole = new List<string>();
            var lUser = _userManager.FindByEmailAsync(user.Username).Result;
            if (lUser != null)
                lstRole = ((_userManager.GetRolesAsync(lUser))).Result.ToList();
            return lstRole;
        }







    }
}
