using DyersCargoTransit_Interface.Models;
using DyersCargoTransit_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DyersCargoTransit_Interface.Controllers
{
    public class TruckController : Controller
    {

        const string TRUCK_URL = "https://localhost:7005/api/TruckAPI";
        const string STATUS_ENDPOINT = "TruckStatus";


        public IActionResult Index()
        {
            var truckList = new List<Truck>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(TRUCK_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{TRUCK_URL}/Truck").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    truckList = JsonConvert.DeserializeObject<List<Truck>>(data);
                }
            }
            return View(truckList);
        }


        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
           
            Truck truck = new Truck();
            List<TruckStatus> truckStatusList = new List<TruckStatus>();

         
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{TRUCK_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

               
                //truck status
                HttpResponseMessage statusResponse = client.GetAsync($"{TRUCK_URL}/{STATUS_ENDPOINT}").Result;
                if (statusResponse.IsSuccessStatusCode)
                {
                    var statusData = statusResponse.Content.ReadAsStringAsync().Result;
                    truckStatusList = JsonConvert.DeserializeObject<List<TruckStatus>>(statusData)!;
                }

                var viewModel = new TruckVM
                {
                    //status
                    TruckStatusList = truckStatusList.Select(status => new SelectListItem
                    {
                        Text = status.TruckStatusName,
                        Value = status.Id.ToString(),
                    }).ToList(),

                };
                return View(viewModel);
            }
        }



        //CREATE:POST
        [HttpPost]
        public IActionResult Create(TruckVM truckVM, int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{TRUCK_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var truck = new Truck
            {
                Id = id,
                LicencePlate = truckVM.LicencePlate,
                TruckCapacity = truckVM.TruckCapacity,
                TruckStatusId = truckVM.SelectedTruckStatusId
            };


            var json = JsonConvert.SerializeObject(truck);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            HttpResponseMessage truckResponse = client.PostAsync($"{TRUCK_URL}/TruckPost", data).Result; //Creates use post request 
            if (truckResponse.IsSuccessStatusCode)
            {
                
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to create Truck");
                return View(truckVM);
            }
        }




        //---------------------------------------------
        //EDIT:GET
        [HttpGet]
        public IActionResult Edit( int id)
        {
            Truck truck = new Truck();
            List<TruckStatus> truckStatusList = new List<TruckStatus>();


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{TRUCK_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //get truck values
                HttpResponseMessage truckResponse = client.GetAsync($"{TRUCK_URL}/{id}").Result;
                if (truckResponse.IsSuccessStatusCode)
                {
                    var truckData = truckResponse.Content.ReadAsStringAsync().Result;
                    //deserialiaze object
                    truck = JsonConvert.DeserializeObject<Truck>(truckData)!;
                }



                //truck status
                HttpResponseMessage statusResponse = client.GetAsync($"{TRUCK_URL}/{STATUS_ENDPOINT}").Result;
                if (statusResponse.IsSuccessStatusCode)
                {
                    var statusData = statusResponse.Content.ReadAsStringAsync().Result;
                    truckStatusList = JsonConvert.DeserializeObject<List<TruckStatus>>(statusData)!;
                }


                var viewModel = new TruckVM
                {

                    Id = truck.Id,
                    LicencePlate = truck.LicencePlate,
                    TruckCapacity = truck.TruckCapacity,
                    SelectedTruckStatusId = truck.TruckStatusId,



                    //status
                    TruckStatusList = truckStatusList.Select(status => new SelectListItem
                    {
                        Text = status.TruckStatusName,
                        Value = status.Id.ToString(),
                    }).ToList(),

                };
                return View(viewModel);


            }
        }





        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(TruckVM truckVM)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{TRUCK_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var truck = new Truck
            {
                Id = truckVM.Id,
                LicencePlate = truckVM.LicencePlate,
                TruckCapacity = truckVM.TruckCapacity,
                TruckStatusId = truckVM.SelectedTruckStatusId
            };


            var json = JsonConvert.SerializeObject(truck);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            HttpResponseMessage truckResponse = client.PutAsync($"{TRUCK_URL}/TruckPut", data).Result; //Updates use put request 
            if (truckResponse.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to update Truck");
                return View(truckVM);
            }
        }


        //DETAIL
        public IActionResult Detail(int id)
        {
            Truck truck = new Truck();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{TRUCK_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{TRUCK_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    truck = JsonConvert.DeserializeObject<Truck>(data);
                }
            }
            return View(truck);
        }





        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Truck truck = new Truck();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{TRUCK_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{TRUCK_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    truck = JsonConvert.DeserializeObject<Truck>(data);
                }
            }
            return View(truck);
        }


        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, Truck truck)
        {
            //Truck truck = new Truck();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{TRUCK_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{TRUCK_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    truck = JsonConvert.DeserializeObject<Truck>(data);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
