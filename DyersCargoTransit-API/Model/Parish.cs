using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DyersCargoTransit_API.Model
{
    public class Parish
    {
        [Key]
        public int Id { get; set; }

  
        [Column (TypeName ="varchar(50)")]
        public string ParishName { get; set; }
    }
}
