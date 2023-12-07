using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace DyersCargoTransit_Interface.Models
{
    public class ShippingSchedule
    {
        public int Id { get; set; }


        [Display(Name = "Departure Location")]
        public string DepartureLocation { get; set; }



        [Display(Name = "Arrival Location")]
        public string ArrivalLocation { get; set; }



        [Display(Name = "Departure Time")]
        [DisplayFormat(DataFormatString = "({0:hh:mm tt}) , {0:d-MMM-yyyy}")]
        //[DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}, [ Departure Time - {0:h:mm tt} ]")]
        public DateTime DepartureTime { get; set; }




        [Display(Name = "Arrival Time")]
        [DisplayFormat(DataFormatString = "({0:hh:mm tt}) , {0:d-MMM-yyyy}")]
        public DateTime ArrivalTime { get; set; }

    }
}
