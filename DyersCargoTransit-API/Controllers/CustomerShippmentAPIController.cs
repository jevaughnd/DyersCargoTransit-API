using DyersCargoTransit_API.Data;
using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Implementation;

namespace DyersCargoTransit_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerShippmentAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _cxt;

        public CustomerShippmentAPIController(ApplicationDbContext context)
        {
            this._cxt = context;
        }


        //Customer Shippment
        [HttpGet("CustomerShippment")]
        public IActionResult GetCustomerShippments()
        {
            var shipments = _cxt.CustomerShipments.Include(s => s.Customer)
                                                  .Include(s => s.Cargo)
                                                  .Include( s => s.ShippingSchedule)
                                                  .Include(s => s.Customer_ShipmentStatus)
                                                  .ToList();
            if (shipments == null)
            {
                return BadRequest();
            }
            return Ok(shipments);
        }





        //Finds Individual Record By Id
        [HttpGet("{id}")]
        public IActionResult GetCustomerShippmentById(int id)
        {
            var shipment = _cxt.CustomerShipments.Include(s => s.Customer)
                                                  .Include(s => s.Cargo)
                                                  .Include(s => s.ShippingSchedule)
                                                  .Include(s => s.Customer_ShipmentStatus)
                                                  .FirstOrDefault(s => s.Id == id);

            if (shipment == null)
            {
                return NotFound();
            }
            return Ok(shipment);
        }




        //Create Record
        [HttpPost("CustomerShippmentPost")]
        public IActionResult CreateCustomerShippment([FromBody] Customer_Shipment values)
        {
            _cxt.CustomerShipments.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCustomerShippmentById), new { id = values.Id }, values);
        }



        //Edit Record
        [HttpPut("CustomerShippmentPut")]
        public IActionResult UpdateCustomerShippment([FromBody] Customer_Shipment values)
        {
            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.CustomerShipments.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCustomerShippmentById), new { id = values.Id }, values);
        }




        //Delete Individual Record By Id
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomerShippmentById(int id)
        {
            var shipment = _cxt.CustomerShipments.Include(s => s.Customer)
                                                  .Include(s => s.Cargo)
                                                  .Include(s => s.ShippingSchedule)
                                                  .Include(s => s.Customer_ShipmentStatus)
                                                  .FirstOrDefault(s => s.Id == id);

            if (shipment == null)
            {
                return NotFound();
            }
            _cxt.CustomerShipments.Remove(shipment);
            _cxt.SaveChanges();
            return Ok(shipment);
        }





        //=========================================================================================================================================
        //Customer

        [HttpGet("Customer")]
        public IActionResult GetCustomers()
        {
            var persons = _cxt.Customers.ToList();

            if (persons == null)
            {
                return BadRequest();
            }
            return Ok(persons);
        }


        //Finds Individual customer By Id
        [HttpGet("Customer/{Id}")]
        public IActionResult GetCustomerById(int id)
        {
            var person = _cxt.Customers.FirstOrDefault(x => x.Id == id);

            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }


        //Create customer
        [HttpPost("CustomerPost")]
        public IActionResult CreateCustomer([FromBody] Customer values)
        {
            _cxt.Customers.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCustomerById), new { id = values.Id }, values);
        }


        //Edit customer 
        [HttpPut("CustomerPut")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer values)
        {
            var person = _cxt.Customers.FirstOrDefault(x => x.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            _cxt.Customers.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCustomerById), new { id = values.Id }, values);
        }




        //=========================================================================================================================================
        //Cargo
        
        [HttpGet("Cargo")]
        public IActionResult GetCargoes()
        {
            var item = _cxt.Cargos.ToList();
            if (item == null)
            {
                return BadRequest();
            }
            return Ok(item);
        }


        //individual cargo
        [HttpGet("Cargo/{Id}")]
        public IActionResult GetCargoById(int id)
        {
            var item = _cxt.Cargos.FirstOrDefault(i => i.Id == id);
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
            return CreatedAtAction(nameof(GetCargoById), new { id = values.Id }, values);
        }



        //Edit Cargo
        [HttpPut("CargoPut")]
        public IActionResult UpdateCargo(int id, [FromBody] Cargo values)
        {
            var item = _cxt.Cargos.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            _cxt.Cargos.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCargoById), new { id = values.Id }, values);
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
        //Customer_ShipmentStatus

        [HttpGet("Customer_ShipmentStatus")]
        public IActionResult GetCustomer_ShipmentStatuses()
        {
            var statuses = _cxt.CustomerShipmentStatuses.ToList();
            if (statuses == null)
            {
                return BadRequest();
            }
            return Ok(statuses);
        }

        //individual Customer_ShipmentStatus
        [HttpGet("Customer_ShipmentStatus/{Id}")]
        public IActionResult GetCustomer_ShipmentStatusById(int id)
        {
            var status = _cxt.CustomerShipmentStatuses.FirstOrDefault(i => i.Id == id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        //create Customer_ShipmentStatus
        [HttpPost("Customer_ShipmentStatusPost")]
        public IActionResult CreateCustomer_ShipmentStatus([FromBody] Customer_ShipmentStatus values)
        {
            _cxt.CustomerShipmentStatuses.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetCargoById), new { id = values.Id }, values);
        }




        //Edit Customer_ShipmentStatus
        [HttpPut("Customer_ShipmentStatusPut")]
        public IActionResult UpdateCustomer_ShipmentStatus(int id, [FromBody] Customer_ShipmentStatus values)
        {
            var status = _cxt.CustomerShipmentStatuses.FirstOrDefault(i => i.Id == id);
            if (status == null)
            {
                return NotFound();
            }
            _cxt.CustomerShipmentStatuses.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetShippingScheduleById), new { id = values.Id }, values);
        }

    }
}
