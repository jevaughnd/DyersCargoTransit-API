using System.ComponentModel.DataAnnotations;

namespace DyersCargoTransit_API.Model
{
    public class TruckStatus
    {
        [Key]
        public int Id { get; set; }

        public string TruckStatusName { get; set; }

    }
}
