using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DyersCargoTransit_Interface.Models.ViewModels
{
    public class CusShipmentVM
    {

        public int Id { get; set; }



        //Customer
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public int SelectedCustomerId { get; set; }
        public List<SelectListItem>? CustomerList { get; set; }



        //Cargo
        [Display(Name = "Cargo")]
        public int CargoId { get; set; }
        public int SelectedCargoId { get; set; }
        public List<SelectListItem>? CargoList { get; set; }
       


        //Schedule
        [Display(Name = "Schedule (Id)")]
        public int ShippingScheduleId { get; set; }
        public int SelectedShippingScheduleId { get; set; }
        public List<SelectListItem>? ShippingScheduleList { get; set; }
        

        //Customer_ShipmentStatus
        [Display(Name = "Shipment Status")]
        public int Customer_ShipmentStatusId { get; set; }
        public int SelectedShipmentStatusId { get; set; }
        public List<SelectListItem>? ShipmentStatusList { get; set; }

    }
}
