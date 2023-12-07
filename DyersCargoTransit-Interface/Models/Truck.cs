using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace DyersCargoTransit_Interface.Models
{
    public class Truck
    {
        public int Id { get; set; }


        [Display(Name = "Licence Plate")]
        public string LicencePlate { get; set; }



        [Display(Name = "Truck Capacity (lbs)")]
        public string TruckCapacity { get; set; }



        //truck status
        [Display(Name = "Truck Status")]
        public int TruckStatusId { get; set; }
        [ForeignKey("TruckStatusId")]
        public virtual TruckStatus? TruckStatus { get; set; }
    }
}
