using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_API.Model
{
    public class CargoStatus
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string CargoStatusName { get; set; }
    }
}
