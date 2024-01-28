using DyersCargoTransit_API.Data;
using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DyersCargoTransit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileAPIController : Controller
    {
        private readonly ApplicationDbContext cxt;

        public ProfileAPIController(ApplicationDbContext context)
        { cxt = context; }





        //private readonly UserManager<ApplicationUser> userManager;

        //public ProfileAPIController(UserManager<ApplicationUser> userManager)
        //{
        //    this.userManager = userManager;
        //}


        //Get Profile
        [HttpGet("User-Profile/{userId}")]
        public async Task<IActionResult> UserProfile(string userId)
        {
            // Properly encode the userId to ensure it is URL-safe
            string encodedUserId = Uri.EscapeDataString(userId);

            // Retrieve the user by ID
            var user = await cxt.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.Id == encodedUserId);

            if (user == null)
            {
                return NotFound($"User with ID {userId} not found");
            }

            var userProfile = user.UserProfile;

            if (userProfile != null)
            {
                var result = new
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    UserProfile = new
                    {
                        FullName = userProfile.FullName,
                        Bio = userProfile.Bio,
                        PhoneNumber = userProfile.PhoneNumber,
                        TRN = userProfile.TRN,
                        DOB = userProfile.DOB,
                        Street = userProfile.Street,
                        Town = userProfile.Town,
                        Parish = userProfile.Parish,
                        ProfilePicture = userProfile.ProfilePicture
                    }
                };

                //Gets Saved Images by Id, to show on profile page
                //construct img url
                var baseUrl = "https://localhost:7005/Images/";
                userProfile.ProfilePicture = baseUrl + userProfile.ProfilePicture;

                return Ok(result);
            }
            else
            {
                return NotFound($"UserProfile for user with ID {userId} not found");
            }
        }


        //-------------------------------------
        [HttpPut("Update-Profile/{userId}")]
        public async Task<IActionResult> UpdateProfile(string userId, [FromForm] UserProfileDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the user profile using the provided userId
            var userProfile = await cxt.UserProfiles
                .Include(u => u.User) // Include the related User entity
                .FirstOrDefaultAsync(u => u.User.Id == userId);

            if (userProfile == null)
            {
                return NotFound($"UserProfile for user with ID {userId} not found");
            }

            // Update user profile fields
            userProfile.FullName = model.FullName;
            userProfile.Bio = model.Bio;
            userProfile.PhoneNumber = model.PhoneNumber;
            userProfile.TRN = model.TRN;
            userProfile.DOB = model.DOB;
            userProfile.Street = model.Street;
            userProfile.Town = model.Town;
            userProfile.Parish = model.Parish;

            // Handle file upload for ProfilePicture
            if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
            {
                // Generate a unique file name
                var uniqueProfileImgName = Guid.NewGuid() + "_" + model.ProfilePictureFile.FileName;

                // Define the final file path on the server
                var apiProImgFilePath = Path.Combine("api", "server", "Updated-Imgs", uniqueProfileImgName);

                // Save the new image to the server, overwriting the existing image
                using (var stream = new FileStream(apiProImgFilePath, FileMode.Create))
                {
                    await model.ProfilePictureFile.CopyToAsync(stream);
                }

                // Update the image file path in the database
                userProfile.ProfilePicture = apiProImgFilePath;
            }

            // Save changes to the database
            await cxt.SaveChangesAsync();

            return Ok("Profile updated successfully");
        }






        //-----------------------------------
        //Retrieve a link to the uploaded file
        [HttpGet("files/{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            // Construct the full path to the file based on the provided 'fileName'
            string filePath = Path.Combine("api", "server", "Updated-Imgs", fileName);


            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Or handle the case where the file doesn't exist
            }

            // Determine the content type based on the file's extension
            string contentType = GetContentType(fileName);


            // Return the image file as a FileStreamResult with the appropriate content type
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, contentType); // Adjust the content type as needed

        }
        private string GetContentType(string fileName)
        {
            // Determine the content type based on the file's extension
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream"; // Default to binary data
            }
        }







    }
}
