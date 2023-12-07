using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace DyersCargoTransit_Interface.Models
{
    public class Customer_Shipment
    {

        public int Id { get; set; }



        //Customer
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }



        //Cargo
        [Display(Name = "Cargo")]
        public int CargoId { get; set; }
        [ForeignKey("CargoId")]
        public virtual Cargo? Cargo { get; set; }




        //Schedule
        [Display(Name = "Scheduled (Id)")]
        public int ShippingScheduleId { get; set; }
        [ForeignKey("ShippingScheduleId")]
        public virtual ShippingSchedule? ShippingSchedule { get; set; }



        //Customer_ShipmentStatus
        [Display(Name = "Shipment Status")]
        public int Customer_ShipmentStatusId { get; set; }
        [ForeignKey("Customer_ShipmentStatusId")]
        public virtual Customer_ShipmentStatus? Customer_ShipmentStatus { get; set; }
    }
}
