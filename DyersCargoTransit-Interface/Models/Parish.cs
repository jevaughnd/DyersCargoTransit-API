using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace DyersCargoTransit_Interface.Models
{
    public class Parish
    {
        public int Id { get; set; }


        [Display(Name = "Parish")]
        public string ParishName { get; set; }
    }
}
