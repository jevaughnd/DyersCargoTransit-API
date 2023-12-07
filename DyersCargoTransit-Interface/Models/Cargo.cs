using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace DyersCargoTransit_Interface.Models
{
    public class Cargo
    {
        public int Id { get; set; }

        //cargo type
        [Display(Name = "Cargo Type")]
        public int CargoTypeId { get; set; }
        [ForeignKey("CargoTypeId")]
        public virtual CargoType? CargoType { get; set; }


        [Display(Name = "Cargo Description")]
        public string CargoDescription { get; set; }



        [Display(Name = "Weight (Lbs)")]
        public string CargoWeight { get; set; }



        [Display(Name = "Dimentions")]
        public string CargoDimentions { get; set; }



        [Display(Name = "Cargo Destination")]
        public string CargoDestination { get; set; }



        //cargo status
        [Display(Name = "Cargo Status")]
        public int CargoStatusId { get; set; }
        [ForeignKey("CargoStatusId")]
        public virtual CargoStatus? CargoStatus { get; set; }

    }
}
