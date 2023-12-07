using DyersCargoTransit_Interface.Models;
using DyersCargoTransit_Interface.Models.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DyersCargoTransit_Interface.Controllers
{
    public class CustomerShippmentController : Controller
    {
        const string SHIP_URL = "https://localhost:7005/api/CustomerShippmentAPI";


        const string CUSTOMER_ENDPOINT = "Customer";
        const string CARGO_ENDPOINT = "Cargo";
        const string SCHEDULE_ENDPOINT = "ShippingSchedule";
        const string SHIP_STATUS_ENDPOINT = "Customer_ShipmentStatus";

        public IActionResult Index()
        {
            //new shipment Succes Message
            if (TempData.ContainsKey("new-shipment"))
            {
                ViewData["new-shipment"] = TempData["new-shipment"].ToString();
            }//-----------------------------------------------------



            //edited-shipment  //Succes Message

            if (TempData.ContainsKey("edited-shipment"))
            {
                ViewData["edited-shipment"] = TempData["edited-shipment"].ToString();
            }//-----------------------------------------------------



            var shipmentList = new List<Customer_Shipment>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(SHIP_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{SHIP_URL}/CustomerShippment").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    shipmentList = JsonConvert.DeserializeObject<List<Customer_Shipment>>(data);
                }
            }
            return View(shipmentList);
        }



        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            
            Customer_Shipment cusShipment = new Customer_Shipment();

            List<Customer> cuslist = new List<Customer>();
            List<Cargo> cargolist = new List<Cargo>();
            List<ShippingSchedule> schedulelist = new List<ShippingSchedule>();
            List<Customer_ShipmentStatus> statuslist = new List<Customer_ShipmentStatus>();

            //Customer
            //Cargo
            //ShippingSchedule
            //Customer_ShipmentStatus


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{SHIP_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //Customer
                HttpResponseMessage cusResponse = client.GetAsync($"{SHIP_URL}/{CUSTOMER_ENDPOINT}").Result;
                if (cusResponse.IsSuccessStatusCode)
                {
                    var cusData = cusResponse.Content.ReadAsStringAsync().Result;
                    cuslist = JsonConvert.DeserializeObject<List<Customer>>(cusData)!;
                }


                //Cargo
                HttpResponseMessage cargoResponse = client.GetAsync($"{SHIP_URL}/{CARGO_ENDPOINT}").Result;
                if (cargoResponse.IsSuccessStatusCode)
                {
                    var cargoData = cargoResponse.Content.ReadAsStringAsync().Result;
                    cargolist = JsonConvert.DeserializeObject<List<Cargo>>(cargoData)!;
                }

                //Schedule
                HttpResponseMessage scheduleResponse = client.GetAsync($"{SHIP_URL}/{SCHEDULE_ENDPOINT}").Result;
                if (scheduleResponse.IsSuccessStatusCode)
                {
                    var scheduleData = scheduleResponse.Content.ReadAsStringAsync().Result;
                    schedulelist = JsonConvert.DeserializeObject<List<ShippingSchedule>>(scheduleData)!;
                }

                //Customer_ShipmentStatus
                HttpResponseMessage statusResponse = client.GetAsync($"{SHIP_URL}/{SHIP_STATUS_ENDPOINT}").Result;
                if (statusResponse.IsSuccessStatusCode)
                {
                    var statusData = statusResponse.Content.ReadAsStringAsync().Result;
                    statuslist = JsonConvert.DeserializeObject<List<Customer_ShipmentStatus>>(statusData)!;
                }




                //DDL in CusShipmentVM
                var viewModel = new CusShipmentVM
                {
                    //customer list
                    CustomerList = cuslist.Select(cus=> new SelectListItem
                    {
                        Text = cus.FullName,
                        Value = cus.Id.ToString(),
                    }).ToList(),


                    //Cargo Description
                    CargoList = cargolist.Select(cargo => new SelectListItem
                    {
                        Text = cargo.CargoDescription,
                        Value = cargo.Id.ToString(),
                    }).ToList(),


                    //Schedule, arival location Id
                    ShippingScheduleList = schedulelist.Select(schedule => new SelectListItem
                    {
                        Text = schedule.Id.ToString(),
                        Value = schedule.Id.ToString(),
                    }).ToList(),
                    


                    //Customer_ShipmentStatus
                    ShipmentStatusList = statuslist.Select(status => new SelectListItem
                    {
                        Text = status.ShipmentStatusName,
                        Value = status.Id.ToString(),
                    }).ToList(),

                };
                return View(viewModel);


            }
        }





        //CREATE:POST
        [HttpPost]
        public IActionResult Create(CusShipmentVM cusShipmentVm, int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{SHIP_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var shipment = new Customer_Shipment
            {
                Id = id,
                CustomerId = cusShipmentVm.SelectedCustomerId,
                CargoId = cusShipmentVm.SelectedCargoId,
                ShippingScheduleId = cusShipmentVm.SelectedShippingScheduleId,
                Customer_ShipmentStatusId = cusShipmentVm.SelectedShipmentStatusId,
            };


            var json = JsonConvert.SerializeObject(shipment);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            HttpResponseMessage cusResponse = client.PutAsync($"{SHIP_URL}/CustomerShippmentPut", data).Result; //Updates use put request 
            if (cusResponse.IsSuccessStatusCode)
            {

                TempData["new-shipment"] = "New Shipment was Added"; // Succes message displayed in index
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to create Shippment"); 
                return View(cusShipmentVm);
            }
        }






        //-------------------------------------------------------------------------------
        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {

            Customer_Shipment cusShipment = new Customer_Shipment();

            List<Customer> cuslist = new List<Customer>();
            List<Cargo> cargolist = new List<Cargo>();
            List<ShippingSchedule> schedulelist = new List<ShippingSchedule>();
            List<Customer_ShipmentStatus> statuslist = new List<Customer_ShipmentStatus>();

            //Customer
            //Cargo
            //ShippingSchedule
            //Customer_ShipmentStatus


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{SHIP_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //Customer
                HttpResponseMessage cusResponse = client.GetAsync($"{SHIP_URL}/{CUSTOMER_ENDPOINT}").Result;
                if (cusResponse.IsSuccessStatusCode)
                {
                    var cusData = cusResponse.Content.ReadAsStringAsync().Result;
                    cuslist = JsonConvert.DeserializeObject<List<Customer>>(cusData)!;
                }


                //Cargo
                HttpResponseMessage cargoResponse = client.GetAsync($"{SHIP_URL}/{CARGO_ENDPOINT}").Result;
                if (cargoResponse.IsSuccessStatusCode)
                {
                    var cargoData = cargoResponse.Content.ReadAsStringAsync().Result;
                    cargolist = JsonConvert.DeserializeObject<List<Cargo>>(cargoData)!;
                }

                //Schedule
                HttpResponseMessage scheduleResponse = client.GetAsync($"{SHIP_URL}/{SCHEDULE_ENDPOINT}").Result;
                if (scheduleResponse.IsSuccessStatusCode)
                {
                    var scheduleData = scheduleResponse.Content.ReadAsStringAsync().Result;
                    schedulelist = JsonConvert.DeserializeObject<List<ShippingSchedule>>(scheduleData)!;
                }

                //Customer_ShipmentStatus
                HttpResponseMessage statusResponse = client.GetAsync($"{SHIP_URL}/{SHIP_STATUS_ENDPOINT}").Result;
                if (statusResponse.IsSuccessStatusCode)
                {
                    var statusData = statusResponse.Content.ReadAsStringAsync().Result;
                    statuslist = JsonConvert.DeserializeObject<List<Customer_ShipmentStatus>>(statusData)!;
                }


                //Customer_Shipment valus
                HttpResponseMessage shipResponse = client.GetAsync($"{SHIP_URL}/{id}").Result;
                if (shipResponse.IsSuccessStatusCode)
                {
                    var shipData = shipResponse.Content.ReadAsStringAsync().Result;
                    cusShipment = JsonConvert.DeserializeObject<Customer_Shipment>(shipData)!;
                }

                //DDL in CusShipmentVM
                var viewModel = new CusShipmentVM
                {
                    Id = cusShipment.Id,
                    SelectedCustomerId = cusShipment.CustomerId,
                    SelectedCargoId = cusShipment.CargoId,
                    SelectedShippingScheduleId = cusShipment.ShippingScheduleId,
                    SelectedShipmentStatusId = cusShipment.Customer_ShipmentStatusId,

                    

                    //customer list
                    CustomerList = cuslist.Select(cus => new SelectListItem
                    {
                        Text = cus.FullName,
                        Value = cus.Id.ToString(),
                    }).ToList(),


                    //Cargo Description
                    CargoList = cargolist.Select(cargo => new SelectListItem
                    {
                        Text = cargo.CargoDescription,
                        Value = cargo.Id.ToString(),
                    }).ToList(),


                    //Schedule
                    ShippingScheduleList = schedulelist.Select(schedule => new SelectListItem
                    {
                        Text = schedule.Id.ToString(),
                        Value = schedule.Id.ToString(),
                    }).ToList(),



                    //Customer_ShipmentStatus
                    ShipmentStatusList = statuslist.Select(status => new SelectListItem
                    {
                        Text = status.ShipmentStatusName,
                        Value = status.Id.ToString(),
                    }).ToList(),


                    
                };
                return View(viewModel);


            }
        }






        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(CusShipmentVM cusShipmentVm)
        {


            //edited-shipment

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{SHIP_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var shipment = new Customer_Shipment
            {
                Id = cusShipmentVm.Id,
                CustomerId = cusShipmentVm.SelectedCustomerId,
                CargoId = cusShipmentVm.SelectedCargoId,
                ShippingScheduleId = cusShipmentVm.SelectedShippingScheduleId,
                Customer_ShipmentStatusId = cusShipmentVm.SelectedShipmentStatusId,
            };


            var json = JsonConvert.SerializeObject(shipment);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            HttpResponseMessage cusResponse = client.PutAsync($"{SHIP_URL}/CustomerShippmentPut", data).Result; ///Updates use put request 
            if (cusResponse.IsSuccessStatusCode)
            {
                TempData["edited-shipment"] = "Shipment Updated"; // Succes message displayed in index
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to update Shippment");
                return View(cusShipmentVm);
            }
        }



        //DETAIL
        public IActionResult Detail( int id)
        {
            Customer_Shipment shipment = new Customer_Shipment();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{SHIP_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{SHIP_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    shipment = JsonConvert.DeserializeObject<Customer_Shipment>(data);
                }
            }
            return View(shipment);
        }



        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Customer_Shipment shipment = new Customer_Shipment();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{SHIP_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{SHIP_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    shipment = JsonConvert.DeserializeObject<Customer_Shipment>(data);
                }
            }
            return View(shipment);
        }




        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, Customer_Shipment shipment)
        {
            //Customer_Shipment shipment = new Customer_Shipment();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{SHIP_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{SHIP_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    shipment = JsonConvert.DeserializeObject<Customer_Shipment>(data);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
