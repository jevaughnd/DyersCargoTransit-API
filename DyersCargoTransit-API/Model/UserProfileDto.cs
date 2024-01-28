using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_API.Model
{
    public class UserProfileDto
    {
        
        public string FullName { get; set; }


        
        public string Bio { get; set; }




        public string PhoneNumber { get; set; }


        
        public string TRN { get; set; }


        public DateTime DOB { get; set; }


        //Address
       
        public string Street { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string Town { get; set; }


        public int ParishId { get; set; }
        [ForeignKey("ParishId")]
        public virtual Parish? Parish { get; set; }

        public IFormFile? ProfilePictureFile { get; set; }

    }
}
