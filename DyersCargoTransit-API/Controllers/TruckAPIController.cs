using DyersCargoTransit_API.Data;
using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DyersCargoTransit_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TruckAPIController : ControllerBase
	{

		private readonly ApplicationDbContext _cxt;

		public TruckAPIController(ApplicationDbContext context)
		{
			this._cxt = context;
		}


		//Truck Records
		[HttpGet("Truck")]
		public IActionResult GetTrucks()
		{
			var trucks = _cxt.Trucks.Include(t => t.TruckStatus).ToList();

			if (trucks == null)
			{
				return BadRequest();
			}
			return Ok(trucks);
		}


		//Truck Record Id
		[HttpGet("{id}")]
		public IActionResult GetTruckById(int id)
		{
			var truck = _cxt.Trucks.Include(t => t.TruckStatus).FirstOrDefault(t => t.Id == id);

			if (truck == null)
			{
				return NotFound();
			}
			return Ok(truck);
		}

		//Create Truck Record
		[HttpPost("TruckPost")]
		public IActionResult CreateTruck([FromBody] Truck values)
		{
			_cxt.Trucks.Add(values);
			_cxt.SaveChanges();
			return CreatedAtAction(nameof(GetTruckById), new { id = values.Id }, values);
		}

		//Edit Truck Record
		[HttpPut("TruckPut")]
		public IActionResult UpdateTruck( [FromBody] Truck values)
		{
			
			if (values.Id == null)
			{
				return NotFound();
			}
			_cxt.Trucks.Update(values);
			_cxt.SaveChanges();
			return CreatedAtAction(nameof(GetTruckById), new { id = values.Id }, values);
		}




		//Delete Truck Record Id
		[HttpDelete("{id}")]
		public IActionResult DeleteTruckById(int id)
		{
			var truck = _cxt.Trucks.Include(t => t.TruckStatus).FirstOrDefault(t => t.Id == id);

			if (truck == null)
			{
				return NotFound();
			}
			_cxt.Trucks.Remove(truck);
			_cxt.SaveChanges();
			return Ok(truck);
		}



		//=========================================================================================================================================
		
		//Truck Status
		[HttpGet("TruckStatus")]
		public IActionResult GetTruckStatuses()
		{
			var status = _cxt.TruckStatuses.ToList();

			if (status == null)
			{
				return BadRequest();
			}
			return Ok(status);
		}



		//Truck Status Id
		[HttpGet("TruckStatus/{Id}")]
		public IActionResult GetTruckStatusById(int id)
		{
			var status = _cxt.TruckStatuses.FirstOrDefault(t => t.Id == id);

			if (status == null)
			{
				return NotFound();
			}
			return Ok(status);
		}

		//Create Truck Status
		[HttpPost("TruckStatusPost")]
		public IActionResult CreateTruckStatus([FromBody] TruckStatus values)
		{
			_cxt.TruckStatuses.Add(values);
			_cxt.SaveChanges();
			return CreatedAtAction(nameof(GetTruckStatusById), new { id = values.Id }, values);
		}

		//Edit Truck Status
		[HttpPut("TruckStatusPut")]
		public IActionResult UpdateTruckStatus(int id, [FromBody] TruckStatus values)
		{
			var status = _cxt.TruckStatuses.FirstOrDefault(s => s.Id == id);
			if (status == null)
			{
				return NotFound();
			}
			_cxt.TruckStatuses.Update(values);
			_cxt.SaveChanges();
			return CreatedAtAction(nameof(GetTruckStatusById), new { id = values.Id }, values);
		}
	}
}
