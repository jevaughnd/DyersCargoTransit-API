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
