using DyersCargoTransit_API.Data;
using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class CustomerProfileController : ControllerBase
{
    private readonly ApplicationDbContext _cxt;

    public CustomerProfileController(ApplicationDbContext context)
    {
        this._cxt = context;
    }

    [Authorize(Roles = "Admin,Customer")]
    [HttpGet("Profile")]
    public IActionResult GetCustomerProfile()
    {
        // Retrieve the current user's ID (you might need to adjust this based on your authentication setup)
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Query the customer profile using the user ID
        var customerProfile = _cxt.CustomerProfiles.FirstOrDefault(x => x.UserId == userId);

        if (customerProfile == null)
        {
            return NotFound();
        }

        return Ok(customerProfile);
    }




    [Authorize(Roles = "Admin,Customer")]
    [HttpPut("Profile")]
    public IActionResult UpdateCustomerProfile([FromBody] CustomerProfile profile)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var existingProfile = _cxt.CustomerProfiles.FirstOrDefault(x => x.UserId == userId);

        if (existingProfile == null)
        {
            return NotFound();
        }

        existingProfile.FullName = profile.FullName;
        existingProfile.EmailAddress = profile.EmailAddress;
        existingProfile.PhoneNumber = profile.PhoneNumber;
        existingProfile.DOB = profile.DOB;
        existingProfile.Street = profile.Street;
        existingProfile.Town = profile.Town;
        existingProfile.ParishId = profile.ParishId;
        existingProfile.ProfilePicture = profile.ProfilePicture;

        _cxt.SaveChanges();

        return Ok(existingProfile);
    }

}
