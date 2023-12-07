using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_API.Model
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "varchar(10)")]
        public string LicencePlate { get; set; }



        [Column(TypeName ="varchar(100)")]
        public string TruckCapacity { get; set; }



        //truck status
        public int TruckStatusId { get; set; }
        [ForeignKey("TruckStatusId")]
        public virtual TruckStatus? TruckStatus { get; set; }


    }
}
