using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_API.Model
{
    public class UserProfile
    {


        [Key]
        
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string FullName { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string? Bio { get; set; }

     
        
        [Column(TypeName = "varchar(50)")]
        public string PhoneNumber { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string TRN { get; set; }


        public DateTime DOB { get; set; }


        //Address
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
