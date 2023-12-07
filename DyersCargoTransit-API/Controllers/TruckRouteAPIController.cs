using DyersCargoTransit_API.Data;
using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DyersCargoTransit_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TruckRouteAPIController : ControllerBase
	{
		private readonly ApplicationDbContext _cxt;

		public TruckRouteAPIController(ApplicationDbContext context)
		{
			this._cxt = context;
		}


		//TruckRoute
		[HttpGet("TruckRoute")]
		public IActionResult GetTruckRoutes()
		{
			var routs = _cxt.TruckRoutes.Include(r => r.ShippingSchedule).Include(r => r.Truck).ToList();
			
			if (routs == null )
			{
				return BadRequest();
			}
			return Ok(routs);
		}


		//TruckRoute record id
		[HttpGet("{id}")]
		public IActionResult GetTruckRouteById(int id)
		{
			var route = _cxt.TruckRoutes.Include(r => r.ShippingSchedule).Include(r => r.Truck)
				                                                         .FirstOrDefault( r => r.Id == id);
			
			if (route == null)
			{
				return NotFound();
			}
			return Ok(route);
		}

		//Create TruckRoute Record
		[HttpPost("TruckRoutePost")]
		public IActionResult CreateTruckRoute([FromBody] TruckRoute values)
		{
			_cxt.TruckRoutes.Add(values);
			_cxt.SaveChanges();
			return CreatedAtAction(nameof(GetTruckRouteById), new { id = values.Id }, values);
		}

		//Edit TruckRoute Record
		[HttpPut("TruckRoutePut")]
		public IActionResult UpdateTruckRoute([FromBody] TruckRoute values)
		{
			if (values.Id == null)
			{
				return NotFound();
			}

			_cxt.TruckRoutes.Update(values);
			_cxt.SaveChanges();
			return CreatedAtAction(nameof(GetTruckRouteById), new { id = values.Id }, values);
		}

		//Delete TruckRoute record by id
		[HttpDelete("{id}")]
		public IActionResult DeleteTruckRouteById(int id)
		{
			var route = _cxt.TruckRoutes.Include(r => r.ShippingSchedule).Include(r => r.Truck)
																		 .FirstOrDefault(r => r.Id == id);

			if (route == null)
			{
				return NotFound();
			}
			_cxt.TruckRoutes.Remove(route);
			_cxt.SaveChanges();
			return Ok(route);
		}






		//=========================================================================================================================================
		//ShippingSchedule 

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



		//individual ShippingSchedule
		[HttpGet("ShippingSchedule/{Id}")]
		public IActionResult GetShippingScheduleById(int id)
		{
			var schedule = _cxt.ShippingSchedules.FirstOrDefault(i => i.Id == id);
			if (schedule == null)
			{
				return NotFound();
			}
			return Ok(schedule);
		}


		//create ShippingSchedule
		[HttpPost("ShippingSchedulePost")]
		public IActionResult CreateShippingSchedule([FromBody] ShippingSchedule values)
		{
			_cxt.ShippingSchedules.Add(values);
			_cxt.SaveChanges();
			return CreatedAtAction(nameof(GetShippingScheduleById), new { id = values.Id }, values);
		}



		//Edit ShippingSchedule
		[HttpPut("ShippingSchedulePut")]
		public IActionResult UpdateShippingSchedule(int id, [FromBody] ShippingSchedule values)
		{
			var Schedule = _cxt.ShippingSchedules.FirstOrDefault(i => i.Id == id);
			if (Schedule == null)
			{
				return NotFound();
			}
			_cxt.ShippingSchedules.Update(values);
			_cxt.SaveChanges();
			return CreatedAtAction(nameof(GetShippingScheduleById), new { id = values.Id }, values);
		}





		//=========================================================================================================================================
		//Truck


		[HttpGet("Truck")]
		public IActionResult GetTrucks()
		{
			var trucks = _cxt.Trucks.ToList();

			if (trucks == null)
			{
				return BadRequest();
			}
			return Ok(trucks);
		}



		//Truck Id
		[HttpGet("Truck/{Id}")]
		public IActionResult GetTruckById(int id)
		{
			var truck = _cxt.Trucks.FirstOrDefault(r => r.Id == id);

			if (truck == null)
			{
				return NotFound();
			}
			return Ok(truck);
		}



		//Create Truck
		[HttpPost("TruckPost")]
		public IActionResult CreateTruck([FromBody] Truck values)
		{
			_cxt.Trucks.Add(values);
			_cxt.SaveChanges();
			return CreatedAtAction(nameof(GetTruckById), new { id = values.Id }, values);
		}




		//Edit Truck
		[HttpPut("TruckPut")]
		public IActionResult UpdateTruck( int id, [FromBody] Truck values)
		{
			var truck = _cxt.Trucks.FirstOrDefault(r => r.Id == id);
			if (truck == null)
			{
				return NotFound();
			}

			_cxt.Trucks.Update(values);
			_cxt.SaveChanges();
			return CreatedAtAction(nameof(GetTruckById), new { id = values.Id }, values);
		}

	}
}
