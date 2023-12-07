using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_API.Model
{
    public class TruckRoute
    {
        [Key]
        public int Id { get; set; }


        //Shipping Schedule
        public int ShippingScheduleId { get; set; }
        [ForeignKey("ShippingScheduleId")]
        public virtual ShippingSchedule? ShippingSchedule { get;set; }



        //Truck
        public int TruckId { get; set; }
        [ForeignKey("TruckId")]
        public virtual Truck? Truck { get; set; }



        [Column(TypeName ="varchar(100)")]
        public string Waypoints { get; set;}



        [Column(TypeName ="varchar(100)")]
        public string Distance { get;set; }



        [Column(TypeName ="varchar(50)")]
        public string EstimatedTime { get; set; }


    }
}
