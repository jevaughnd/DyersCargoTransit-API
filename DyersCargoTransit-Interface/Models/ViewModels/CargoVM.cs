using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DyersCargoTransit_Interface.Models.ViewModels
{
    public class CargoVM
    {
        public int Id { get; set; }

        //cargo type
        [Display(Name = "Cargo Type")]
        public int CargoTypeId { get; set; }

        //ddl
        public List<SelectListItem>? CargoTypeList { get; set; }

        //selected value
        public int SelectedCargoTypeId { get; set; }




        [Display(Name = "Cargo Description")]
        public string CargoDescription { get; set; }



        [Display(Name = "Cargo Weight (Lbs)")]
        public string CargoWeight { get; set; }



        [Display(Name = "Cargo Dimentions")]
        public string CargoDimentions { get; set; }



        [Display(Name = "Cargo Destination")]
        public string CargoDestination { get; set; }





        //cargo status
        [Display(Name = "Cargo Status")]
        public int CargoStatusId { get; set; }
        
        //ddl
        public List<SelectListItem>? CargoStatusList { get; set; }

        //selected value
        public int SelectedCargoStatusId { get; set; }


    }
}
