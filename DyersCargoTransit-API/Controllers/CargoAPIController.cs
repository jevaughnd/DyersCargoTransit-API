using DyersCargoTransit_API.Data;
using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace DyersCargoTransit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _cxt;
        
        public CargoAPIController (ApplicationDbContext cxt)
        {
            this._cxt = cxt;
        }

        //cargo
        [HttpGet("Cargo")]
        public IActionResult GetCargoes()
        {
            var items = _cxt.Cargos.Include(i => i.CargoType).Include(i => i.CargoStatus).ToList();

            if(items == null)
            {
                return BadRequest();
            }
            return Ok(items);
        }



        //individual cargo
        [HttpGet("{id}")]
        public IActionResult GetCargoById(int id)
        {
            var item = _cxt.Cargos.Include(i => i.CargoType).Include(i => i.CargoStatus).FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }


        //create cargo
        [HttpPost("CargoPost")]
        public IActionResult CreateCargo([FromBody] Cargo values)
        {
            _cxt.Cargos.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCargoById), new {id = values.Id}, values);
        }



        //Edit Cargo
        [HttpPut("CargoPut")]
        public IActionResult UpdateCargo([FromBody] Cargo values)
        {
            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.Cargos.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCargoById), new { id = values.Id }, values);
        }



        //Delete Cargo
        [HttpDelete("{id}")]
        public IActionResult DeleteCargoById(int id)
        {
            var item = _cxt.Cargos.Include(i => i.CargoType).Include(i => i.CargoStatus).FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }
            _cxt.Cargos.Remove(item);
            _cxt.SaveChanges();
            return Ok(item);

        }



        //==========================================================================================================================

        //Cargo Type
        [HttpGet("CargoType")]
        public IActionResult GetCargoType()
        {
            var cargoType = _cxt.CargoTypes.ToList();
            if (cargoType == null)
            {
                return BadRequest();
            }
            return Ok(cargoType);
        }


        //Individual Cargo Type
        [HttpGet]
        [Route("CargoType/{Id}")]
        public IActionResult GetCargoTypeById(int id)
        {
            var cargoType = _cxt.CargoStatuses.FirstOrDefault(x => x.Id == id); //gets individual type by Id
            if (cargoType == null)
            {
                return NotFound();
            }
            return Ok(cargoType);
        }


        //Create 
        [HttpPost]
        [Route("CargoTypePost")]
        public IActionResult CreateCargoType([FromBody] CargoType values)
        {
            _cxt.CargoTypes.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCargoTypeById), new { id = values.Id }, values);
        }


        //edit
        [HttpPut]
        [Route("CargoTypePut")]
        public IActionResult UpdateCargoType(int id, [FromBody] CargoType values)
        {
            var cargoType = _cxt.CargoTypes.FirstOrDefault(x => x.Id == id);
            if (cargoType == null)
            {
                return NotFound();
            }
            _cxt.CargoTypes.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCargoTypeById), new { id = values.Id }, values);
        }






        //==========================================================================================================================

        //Cargo Status
        [HttpGet("CargoStatus")]
        public IActionResult GetCargoStatuses()
        {
            var cargoStatus =_cxt.CargoStatuses.ToList();
            if (cargoStatus == null)
            {
                return BadRequest();
            }
            return Ok(cargoStatus);
        }


        //Individual Cargo Status
        [HttpGet]
        [Route("CargoStatus/{Id}")]
        public IActionResult GetCargoStatusById(int id)
        {
            var cargoStatus = _cxt.CargoStatuses.FirstOrDefault(x => x.Id == id); //gets individual status by Id
            if (cargoStatus == null)
            {
                return NotFound();
            }
            return Ok(cargoStatus);
        }



        //Create 
        [HttpPost]
        [Route("CargoStatusPost")]
        public IActionResult CreateCargoStatus([FromBody] CargoStatus values)
        {
            _cxt.CargoStatuses.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCargoStatusById), new { id = values.Id }, values);
        }



        //edit
        [HttpPut]
        [Route("CargoStatusPut")]
        public IActionResult UpdateCargoStatus(int id, [FromBody] CargoStatus values)
        {
            var cargoStatus = _cxt.CargoStatuses.FirstOrDefault(x => x.Id == id);
            if (cargoStatus == null)
            {
                return NotFound();
            }
            _cxt.CargoStatuses.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCargoStatusById), new { id = values.Id }, values);
        }


    }
}
