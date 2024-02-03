using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DyersCargoTransit_API.Model
{
    public class CustomerProfile
    {
        

        [Key]
        public int Id { get; set; }

        
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
