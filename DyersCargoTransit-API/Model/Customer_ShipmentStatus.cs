using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_API.Model
{
    public class Customer_ShipmentStatus
    {
        [Key]
        public int Id { get; set; }


        [Column (TypeName = "varchar(50)")]
        public string ShipmentStatusName { get; set; }
    }
}
