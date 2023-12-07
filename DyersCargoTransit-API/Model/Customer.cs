using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DyersCargoTransit_API.Model
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string FullName { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string EmailAddress { get; set; }


        [Column(TypeName = "varchar(15)")]
        public string PhoneNumber { get; set; }


        [Column(TypeName = "varchar(10)")]
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
    }
}
