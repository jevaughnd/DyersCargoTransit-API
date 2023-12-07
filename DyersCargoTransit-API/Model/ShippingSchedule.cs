using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_API.Model
{
    public class ShippingSchedule
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string DepartureLocation { get; set; }
        


        [Column(TypeName = "varchar(50)")]
        public string ArrivalLocation { get; set; }


        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set;}
    }
}
