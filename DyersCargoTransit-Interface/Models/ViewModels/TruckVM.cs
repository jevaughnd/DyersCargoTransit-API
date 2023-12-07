using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DyersCargoTransit_Interface.Models.ViewModels
{
    public class TruckVM
    {

        public int Id { get; set; }


        [Display(Name = "Licence Plate")]
        public string LicencePlate { get; set; }



        [Display(Name = "Truck Capacity (lbs)")]
        public string TruckCapacity { get; set; }



        //truck status
        [Display(Name = "Truck Status")]
        public int TruckStatusId { get; set; }
        public List<SelectListItem>? TruckStatusList { get; set; }
        public int SelectedTruckStatusId { get; set; }
        

    }
}
