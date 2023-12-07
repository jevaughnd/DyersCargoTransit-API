using DyersCargoTransit_API.Data;
using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DyersCargoTransit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingScheduleAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _cxt;
        public ShippingScheduleAPIController(ApplicationDbContext context)
        {
            this._cxt = context;
        }


        //ShippingSchedule Records
        [HttpGet("ShippingSchedule")]
        public IActionResult GetShippingSchedules()
        {
            var schedules = _cxt.ShippingSchedules.ToList();
            if (schedules == null)
            {
                return BadRequest();
            }
            return Ok(schedules);
        }


        //Individual ShippingSchedule Record
        [HttpGet("{id}")]
        public IActionResult GetShippingScheduleById(int id)
        {
            var schedule = _cxt.ShippingSchedules.FirstOrDefault(i => i.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }


        //Create ShippingSchedule Record
        [HttpPost("ShippingSchedulePost")]
        public IActionResult CreateShippingSchedule([FromBody] ShippingSchedule values)
        {
            _cxt.ShippingSchedules.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetShippingScheduleById), new { id = values.Id }, values);
        }


        //Edit ShippingSchedule Record
        [HttpPut("ShippingSchedulePut")]
        public IActionResult UpdateShippingSchedule( [FromBody] ShippingSchedule values)
        {
            
            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.ShippingSchedules.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetShippingScheduleById), new { id = values.Id }, values);
        }



        //Delete ShippingSchedule Record
        [HttpDelete("{id}")]
        public IActionResult DeleteShippingScheduleById(int id)
        {
            var schedule = _cxt.ShippingSchedules.FirstOrDefault(i => i.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }
            _cxt.ShippingSchedules.Remove(schedule);
            _cxt.SaveChanges();
            return Ok(schedule);
        }

    }
}
