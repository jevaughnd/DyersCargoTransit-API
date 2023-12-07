using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_API.Model
{
    public class Customer_Shipment
    {
        [Key]
        public int Id { get; set; }



        //customer
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }



        //cargo
        public int CargoId { get; set; }
        [ForeignKey("CargoId")]
        public virtual Cargo? Cargo { get; set; }




        //schedule
        public int ShippingScheduleId { get; set;}
        [ForeignKey("ShippingScheduleId")]
        public virtual ShippingSchedule? ShippingSchedule { get; set; }



        //Customer_ShipmentStatus
        public int Customer_ShipmentStatusId { get; set; }
        [ForeignKey("Customer_ShipmentStatusId")]
        public virtual Customer_ShipmentStatus? Customer_ShipmentStatus { get; set; }




    }
}
