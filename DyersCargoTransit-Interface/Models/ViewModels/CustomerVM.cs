using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DyersCargoTransit_Interface.Models.ViewModels
{
    public class CustomerVM
    {
        public int Id { get; set; }
        


        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }


        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string TRN { get; set; }
        public DateTime DOB { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }




        //parish
        [Display(Name = "Parish")]
        public int ParishId { get; set; }


        //Select value from DDL
        public List<SelectListItem>? ParishList { get; set; }

        //Selected DDL value
        public int SelectedParishId { get; set; }
    }
}
