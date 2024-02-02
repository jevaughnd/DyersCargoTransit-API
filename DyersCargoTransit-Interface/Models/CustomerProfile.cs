using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_Interface.Models
{
    public class CustomerProfile
    {
        [Key]
        public int Id { get; set; }





        // Other properties for the customer profile...
        [Column(TypeName = "varchar(50)")]
        public string FullName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string EmailAddress { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string PhoneNumber { get; set; }

        public DateTime DOB { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Street { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Town { get; set; }

        public int ParishId { get; set; }
        [ForeignKey("ParishId")]
        public virtual Parish? Parish { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? ProfilePicture { get; set; } = String.Empty;
    }
}
