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
        private readonly UserManager<ApplicationUser> _userManager;


        public AuthAPIController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            this._authService = authService;
            this._userManager = userManager;
        }


        //[HttpPost]
        //[Route("register")]
        ////public async Task<IActionResult> Register(User user)
        //public async Task<IActionResult> Register(User user)
        //{
        //    if (await _authService.Register(user))
        //    {
        //        //assign default role employee
        //        var roles = new List<string> { "Employee" };
        //        await _authService.AssignRoles(user.Username, roles);

        //        return Ok(new { status = "succes", message = "registration succesful" });
        //    }
        //    return BadRequest(new { status = "fail", message = "registration failed" });
        //}


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _authService.Register(user))
            {
                // No need to explicitly assign roles here as it's already done in AuthService.Register
                return Ok(new { status = "success", message = "registration successful" });
            }

            return BadRequest(new { status = "fail", message = "registration failed" });
        }










        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(User user)
        {
            var result = await _authService.Login(user);

            //user
            var identityUser = await _userManager.FindByEmailAsync(user.Username);
            

            if (result == true)
            {
                var token = _authService.GenerateToken(user);

                return Ok(new { status = "succes", message = "login succesful", data = token, user = identityUser });
            }
            return BadRequest(new { status = "failed", message = "login failed" });
        }


        //-------------------------------------------------------------------------------------------------










    }
}
