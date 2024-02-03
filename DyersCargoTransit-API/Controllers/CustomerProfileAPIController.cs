using DyersCargoTransit_API.Data;
using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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




    //CUSTOMER END POINTS

    [HttpGet("CustomerProfiles")]
    public IActionResult GetCustomerProfiles()
    {
        var persons = _cxt.CustomerProfiles.Include(p => p.Parish).ToList();

        if (persons == null)
        {
            return BadRequest();
        }

        var baseUrl = "https://localhost:7005/Images/";
        foreach(var img in persons) 
        {
            img.ProfilePicture = baseUrl + img.ProfilePicture;
        }
        return Ok(persons);
    }





    //Finds Individual Record By Id
    [HttpGet("{id}")]
    public IActionResult GetCustomerProfileById(int id)
    {
        var person = _cxt.CustomerProfiles.Include(p => p.Parish).FirstOrDefault(x => x.Id == id);

        if (person == null)
        {
            return NotFound();
        }

        var baseUrl = "https://localhost:7005/Images/";
        person.ProfilePicture = baseUrl + person.ProfilePicture;

        return Ok(person);
    }



    //Create Record
    //[Authorize(Roles = "Admin, Customer")]
    [HttpPost("CustomerProfilePost")]
    public IActionResult CreateCustomerProfile([FromBody] CustomerProfile values)
    {
        _cxt.CustomerProfiles.Add(values);
        _cxt.SaveChanges();
        return CreatedAtAction(nameof(GetCustomerProfileById), new { id = values.Id }, values);
    }


    //Edit Record
    //[Authorize(Roles = "Admin")]
    [HttpPut("CustomerProfilePut")]
    public IActionResult UpdateCustomer([FromBody] CustomerProfile values)
    {
        if (values.Id == null)
        {
            return NotFound();
        }
        _cxt.CustomerProfiles.Update(values);
        _cxt.SaveChanges();
        return CreatedAtAction(nameof(GetCustomerProfileById), new { id = values.Id }, values);
    }




    //Delete Individual Record By Id
    [HttpDelete("{id}")]
    public IActionResult DeleteProfileById(int id)
    {
        var person = _cxt.CustomerProfiles.Include(p => p.Parish).FirstOrDefault(x => x.Id == id);

        if (person == null)
        {
            return NotFound();
        }
        _cxt.CustomerProfiles.Remove(person);
        _cxt.SaveChanges();
        return Ok(person);
    }




   //UPDATE FILE
    [HttpPut("ProfilePut/{id}")]
    public async Task<IActionResult> EditFile(int id, [FromForm] CustomerProfileDto model)
    {

        


        if (ModelState.IsValid)
        {
            var existingCustomerProfile = await _cxt.CustomerProfiles.FindAsync(id);

            if (existingCustomerProfile == null)
            {
                return NotFound();
            }



            // Update the properties of the CustomerProfile entity based on the model
            
            existingCustomerProfile.FullName = model.FullName;
            existingCustomerProfile.EmailAddress = model.EmailAddress;
            existingCustomerProfile.PhoneNumber = model.PhoneNumber;
            existingCustomerProfile.DOB = model.DOB;
            existingCustomerProfile.Street = model.Street;
            existingCustomerProfile.Town = model.Town;
            existingCustomerProfile.ParishId = model.ParishId;


            // Check if a new image is provided
            if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
            {
                // Generate a unique file name for the  image
                var uniqueFileName = Guid.NewGuid() + "_" + model.ProfilePictureFile.FileName;

                // Define the final file path on the server
                var apiFilePath = Path.Combine("api", "server", "Updated-Imgs", uniqueFileName);

                // Save the new image to the server, overwriting the existing image
                using (var stream = new FileStream(apiFilePath, FileMode.Create))
                {
                    await model.ProfilePictureFile.CopyToAsync(stream);
                }

                // Update the  image file path in the database
                existingCustomerProfile.ProfilePicture = apiFilePath;
            }

            //mod to see picture beside form
            var baseUrl = "https://localhost:7005/Images/";
            existingCustomerProfile.ProfilePicture = baseUrl + existingCustomerProfile.ProfilePicture;
            //---

            // Save the changes to the database
            _cxt.CustomerProfiles.Update(existingCustomerProfile);
            _cxt.SaveChanges();

            return CreatedAtAction(nameof(GetCustomerProfileById), new { id = existingCustomerProfile.Id }, existingCustomerProfile);
        }

        return BadRequest(ModelState);
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



    //=========================================================================================================================//
    //PARISH END POINTS

    //-----------------------
    [HttpGet]
    [Route("Parish")]
    public IActionResult GetParishes()
    {
        var parItem = _cxt.Parishes.ToList();
        if (parItem == null)
        {
            return BadRequest();
        }
        return Ok(parItem);
    }


    //-----------------------
    //Finds Record where id is = to the result of the firstOrDefault query
    [HttpGet]
    [Route("Parish/{Id}")]
    public IActionResult GetParishById(int id)
    {
        var parItem = _cxt.Parishes.FirstOrDefault(x => x.Id == id); //gets individual parish by Id
        if (parItem == null)
        {
            return NotFound();
        }
        return Ok(parItem);
    }


    //-----------------------
    [HttpPost]
    [Route("ParishPost")]
    public IActionResult CreateParish([FromBody] Parish values)
    {
        _cxt.Parishes.Add(values);
        _cxt.SaveChanges();
        return CreatedAtAction(nameof(GetParishById), new { id = values.Id }, values);
    }


    //-----------------------
    [HttpPut]
    [Route("ParishPut")]
    public IActionResult UpdateParish(int id, [FromBody] Parish values)
    {
        var parItem = _cxt.Parishes.FirstOrDefault(x => x.Id == id);
        if (parItem == null)
        {
            return NotFound();
        }
        _cxt.Parishes.Update(values);
        _cxt.SaveChanges();
        return CreatedAtAction(nameof(GetParishById), new { id = values.Id }, values);
    }








}
