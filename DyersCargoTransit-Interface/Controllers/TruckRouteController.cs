using DyersCargoTransit_Interface.Models;
using DyersCargoTransit_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DyersCargoTransit_Interface.Controllers
{
    public class TruckRouteController : Controller
    {

        const string T_ROUTE_URL = "https://localhost:7005/api/TruckRouteAPI";
                                  

        const string SCHEDULE_ENDPOINT = "ShippingSchedule";
        const string TRUCK_ENDPOINT = "Truck";
            
    

        public IActionResult Index()
        {
            var routeList = new List<TruckRoute>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(T_ROUTE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{T_ROUTE_URL}/TruckRoute").Result; 
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    routeList = JsonConvert.DeserializeObject<List<TruckRoute>>(data);
                }
            }

            return View(routeList);
        }





        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            TruckRoute route = new TruckRoute();
            List<Truck> truckList= new List<Truck>();
            List<ShippingSchedule> scheduleList = new List<ShippingSchedule>();
          
            


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{T_ROUTE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));



                //truck
                HttpResponseMessage truckResponse = client.GetAsync($"{T_ROUTE_URL}/{TRUCK_ENDPOINT}").Result;
                if (truckResponse.IsSuccessStatusCode)
                {
                    var truckData = truckResponse.Content.ReadAsStringAsync().Result;
                    truckList = JsonConvert.DeserializeObject<List<Truck>>(truckData)!;
                }



                //scedule
                HttpResponseMessage schResponse = client.GetAsync($"{T_ROUTE_URL}/{SCHEDULE_ENDPOINT}").Result;
                if (schResponse.IsSuccessStatusCode)
                {
                    var schData = schResponse.Content.ReadAsStringAsync().Result;
                    scheduleList = JsonConvert.DeserializeObject<List<ShippingSchedule>>(schData)!;
                }





                var viewModel = new TruckRouteVM
                {
                    //truck
                    TruckList = truckList.Select(list => new SelectListItem
                    {
                        Text = list.LicencePlate,
                        Value = list.Id.ToString(),
                    }).ToList(),


                    //schedule, Id
                    ShippingScheduleList = scheduleList.Select(list => new SelectListItem
                    {
                        Text = list.Id.ToString(),
                        Value = list.Id.ToString(),

                    }).ToList(),
                };
                return View(viewModel);


            }
        }


        //CREATE:POST
        [HttpPost]
        public IActionResult Create(TruckRouteVM truckRouteVM, int id)
        {


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{T_ROUTE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var truckRoute = new TruckRoute
            {
                Id = id,
                TruckId = truckRouteVM.SelectedTruckId,
                ShippingScheduleId = truckRouteVM.SelectedShippingScheduleId,
                Waypoints = truckRouteVM.Waypoints,
                Distance = truckRouteVM.Distance,
                EstimatedTime = truckRouteVM.EstimatedTime,
            };


            var json = JsonConvert.SerializeObject(truckRoute);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            HttpResponseMessage truckRouteResponse = client.PostAsync($"{T_ROUTE_URL}/TruckRoutePost", data).Result; //Creates use post request 
            if (truckRouteResponse.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to create Truck Route");
                return View(truckRouteVM);
            }
        }



        //---------------------------------
        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            TruckRoute route = new TruckRoute();
            List<Truck> truckList = new List<Truck>();
            List<ShippingSchedule> scheduleList = new List<ShippingSchedule>();




            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{T_ROUTE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //truck list
                HttpResponseMessage truckResponse = client.GetAsync($"{T_ROUTE_URL}/{TRUCK_ENDPOINT}").Result;
                if (truckResponse.IsSuccessStatusCode)
                {
                    var truckData = truckResponse.Content.ReadAsStringAsync().Result;
                    truckList = JsonConvert.DeserializeObject<List<Truck>>(truckData)!;
                }

                //scedule list
                HttpResponseMessage schResponse = client.GetAsync($"{T_ROUTE_URL}/{SCHEDULE_ENDPOINT}").Result;
                if (schResponse.IsSuccessStatusCode)
                {
                    var schData = schResponse.Content.ReadAsStringAsync().Result;
                    scheduleList = JsonConvert.DeserializeObject<List<ShippingSchedule>>(schData)!;
                }



				//get truck route values
				HttpResponseMessage routeResponse = client.GetAsync($"{T_ROUTE_URL}/{id}").Result;
				if (routeResponse.IsSuccessStatusCode)
				{
					var routeData = routeResponse.Content.ReadAsStringAsync().Result;
					route = JsonConvert.DeserializeObject<TruckRoute>(routeData)!;
				}




				var routeVM = new TruckRouteVM
                {
                    Id = route.Id,
                    SelectedShippingScheduleId = route.ShippingScheduleId,
                    SelectedTruckId = route.TruckId,
                    Waypoints = route.Waypoints,
                    Distance = route.Distance,
                    EstimatedTime = route.EstimatedTime,




                    //truck
                    TruckList = truckList.Select(list => new SelectListItem
                    {
                        Text = list.LicencePlate,
                        Value = list.Id.ToString(),
                    }).ToList(),


                    //schedule, Id
                    ShippingScheduleList = scheduleList.Select(list2 => new SelectListItem
                    {
                        Text = list2.Id.ToString(),
                        Value = list2.Id.ToString(),

                    }).ToList(),
                };
                return View(routeVM);


            }
        }





        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(TruckRouteVM truckRouteVM)
        {


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{T_ROUTE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var truckRoute = new TruckRoute
            {
                Id = truckRouteVM.Id,
                ShippingScheduleId = truckRouteVM.SelectedShippingScheduleId,
                TruckId = truckRouteVM.SelectedTruckId,
               
                Waypoints = truckRouteVM.Waypoints,
                Distance = truckRouteVM.Distance,
                EstimatedTime = truckRouteVM.EstimatedTime,
            };


            var json = JsonConvert.SerializeObject(truckRoute);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            HttpResponseMessage truckRouteResponse = client.PutAsync($"{T_ROUTE_URL}/TruckRoutePut", data).Result; //edits use put request 
            if (truckRouteResponse.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to update Truck Route");
                return View(truckRouteVM);
            }
        }






        //DETAIL
        public IActionResult Detail(int id)
        {
            TruckRoute truckRoute = new TruckRoute();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{T_ROUTE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{T_ROUTE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    truckRoute = JsonConvert.DeserializeObject<TruckRoute>(data);
                }
            }
            return View(truckRoute);
        }




        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            TruckRoute truckRoute = new TruckRoute();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{T_ROUTE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{T_ROUTE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    truckRoute = JsonConvert.DeserializeObject<TruckRoute>(data);
                }
            }
            return View(truckRoute);
        }




        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, TruckRoute truckRoute)
        {
            //TruckRoute truckRoute = new TruckRoute();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{T_ROUTE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{T_ROUTE_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    truckRoute = JsonConvert.DeserializeObject<TruckRoute>(data);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
