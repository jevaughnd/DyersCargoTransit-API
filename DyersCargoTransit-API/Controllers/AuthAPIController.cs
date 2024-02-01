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

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> Register(User user)
        //{
        //    if (await _authService.RegisterUser(user))
        //    {
        //        //assign default role Customer
        //        var roles = new List<string> { "Customer" };
        //        await _authService.AssignRoles(user.Username, roles);

        //        return Ok(new { status = "succes", message = "registration succesful" });
        //    }
        //    return BadRequest(new { status = "fail", message = "registration failed" });
        //}


        //Suposed to Poulate CustomerProfile Feilds When Customer Registers
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(User user, CustomerProfile customerProfile)
        {
            if (await _authService.RegisterUser(user))
            {
                // Assign default role Customer
                var roles = new List<string> { "Customer" };
                await _authService.AssignRoles(user.Username, roles);

                // Retrieve the authenticated user
                var identityUser = await _userManager.FindByEmailAsync(user.Username);

                if (identityUser != null)
                {
                    // Check if the customer profile already exists
                    var existingCustomerProfile = _cxt.CustomerProfiles.FirstOrDefault(x => x.UserId == identityUser.Id);

                    if (existingCustomerProfile != null)
                    {
                        // Update existing customer profile
                        existingCustomerProfile.FullName = customerProfile.FullName;
                        existingCustomerProfile.EmailAddress = customerProfile.EmailAddress;
                        // Update other fields as needed...

                        _cxt.SaveChanges();
                    }
                    else
                    {
                        // Create a new customer profile
                        var newCustomerProfile = new CustomerProfile
                        {
                            UserId = identityUser.Id,
                            FullName = customerProfile.FullName,
                            EmailAddress = customerProfile.EmailAddress,
                            // Set other fields as needed...
                        };

                        _cxt.CustomerProfiles.Add(newCustomerProfile);
                        _cxt.SaveChanges();
                    }
                }

                return Ok(new { status = "success", message = "registration successful" });
            }

            return BadRequest(new { status = "fail", message = "registration failed" });
        }







        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(User user)
        {
            var result = await _authService.LoginUser(user);

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
