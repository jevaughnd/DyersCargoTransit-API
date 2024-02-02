using DyersCargoTransit_API.Data;
using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
