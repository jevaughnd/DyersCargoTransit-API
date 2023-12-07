using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_Interface.Models
{
    public class Customer_ShipmentStatus
    {
        public int Id { get; set; }

        public string ShipmentStatusName { get; set; }
    }
}
