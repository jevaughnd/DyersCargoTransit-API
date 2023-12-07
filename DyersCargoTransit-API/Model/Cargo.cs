using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DyersCargoTransit_API.Model
{
    public class Cargo
    {
        [Key]
        public int Id { get; set; }
       
        //cargo type
        public int CargoTypeId { get; set; }
        [ForeignKey("CargoTypeId")]
        public virtual CargoType? CargoType { get; set; }


        [Column(TypeName = "varchar(200)")]
        public string CargoDescription { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string CargoWeight { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string CargoDimentions { get; set;}


        [Column(TypeName = "varchar(50)")]
        public string CargoDestination { get; set; }



        //cargo status
        public int CargoStatusId { get; set; }
        [ForeignKey("CargoStatusId")]
        public virtual CargoStatus? CargoStatus { get; set; }

    }
}
