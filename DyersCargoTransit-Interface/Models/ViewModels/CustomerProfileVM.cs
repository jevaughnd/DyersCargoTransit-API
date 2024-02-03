using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DyersCargoTransit_Interface.Models.ViewModels
{
    public class CustomerProfileVM
    {
        public int Id { get; set; }

        public string FullName { get; set; }


        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

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


        public IFormFile? ProfilePictureFile { get; set; }


        //mod
        public string? ProfilePicture { get; set; } = String.Empty;




    }
}
