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




        //Suposed to Populate the CustomerProfile Feilds When Customer Registers

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel registrationModel)
        {
            if (await _authService.RegisterUser(registrationModel.User))
            {
                var roles = new List<string> { "Customer" };
                await _authService.AssignRoles(registrationModel.User.Username, roles);

                var identityUser = await _userManager.FindByEmailAsync(registrationModel.User.Username);

                if (identityUser != null)
                {
                    var existingCustomerProfile = _cxt.CustomerProfiles.FirstOrDefault(x => x.UserId == identityUser.Id);

                    if (existingCustomerProfile != null)
                    {
                        existingCustomerProfile.FullName = registrationModel.CustomerProfile.FullName;
                        existingCustomerProfile.EmailAddress = registrationModel.CustomerProfile.EmailAddress;
                        existingCustomerProfile.PhoneNumber = registrationModel.CustomerProfile.PhoneNumber;
                        existingCustomerProfile.DOB = registrationModel.CustomerProfile.DOB;
                        existingCustomerProfile.Street = registrationModel.CustomerProfile.Street;
                        existingCustomerProfile.Town = registrationModel.CustomerProfile.Town;
                        existingCustomerProfile.ParishId = registrationModel.CustomerProfile.ParishId;
                        existingCustomerProfile.ProfilePicture = registrationModel.CustomerProfile.ProfilePicture;

                        _cxt.SaveChanges();
                    }
                    else
                    {
                        var newCustomerProfile = new CustomerProfile
                        {
                            UserId = identityUser.Id,
                            FullName = registrationModel.CustomerProfile.FullName,
                            EmailAddress = registrationModel.CustomerProfile.EmailAddress,
                            PhoneNumber = registrationModel.CustomerProfile.PhoneNumber,
                            DOB = registrationModel.CustomerProfile.DOB,
                            Street = registrationModel.CustomerProfile.Street,
                            Town = registrationModel.CustomerProfile.Town,
                            ParishId = registrationModel.CustomerProfile.ParishId,
                            ProfilePicture = registrationModel.CustomerProfile.ProfilePicture,
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
