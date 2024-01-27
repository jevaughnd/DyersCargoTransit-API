using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace DyersCargoTransit_Interface.Models
{
    public class TruckRoute

    {
        public int Id { get; set; }


        //shipping Schedule
        [Display(Name = "Shipping Schedule")]
        public int ShippingScheduleId { get; set; }
        [ForeignKey("ShippingScheduleId")]
        public virtual ShippingSchedule? ShippingSchedule { get; set; }

        

        //truck
        [Display(Name = "Truck")]
        public int TruckId { get; set; }
        [ForeignKey("TruckId")]
        public virtual Truck? Truck { get; set; }



        [Display(Name = "Way Points")]
        public string Waypoints { get; set; }


        public string Distance { get; set; }



        [Display(Name = "Estimated Time")]
        public string EstimatedTime { get; set; }


    }
}
