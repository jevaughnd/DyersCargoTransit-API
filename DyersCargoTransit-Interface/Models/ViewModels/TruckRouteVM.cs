using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DyersCargoTransit_Interface.Models.ViewModels
{
    public class TruckRouteVM
    {
        public int Id { get; set; }


        //shipping Schedule
        [Display(Name = "Select Schedule")]
        public int ShippingScheduleId { get; set; }
        public int SelectedShippingScheduleId { get; set; }
        public List<SelectListItem>? ShippingScheduleList { get; set; }



        //truck
        [Display(Name = "Select Truck")]
        public int TruckId { get; set; }
        public int SelectedTruckId { get; set; }
        public List<SelectListItem>? TruckList { get; set; }




        [Display(Name = "Way Points")]
        public string Waypoints { get; set; }


        [Display(Name = "Distance (miles)")]
        public string Distance { get; set; }


        [Display(Name = "Estimated Time (hrs)")]
        public string EstimatedTime { get; set; }

    }
}
