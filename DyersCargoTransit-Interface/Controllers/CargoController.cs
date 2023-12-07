using DyersCargoTransit_Interface.Models;
using DyersCargoTransit_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DyersCargoTransit_Interface.Controllers
{
    public class CargoController : Controller
    {
        const string CARGO_URL = "https://localhost:7005/api/CargoAPI";


        const string CARGO_TYPE_ENDPOINT = "CargoType";
        const string CARGO_STATUS_ENDPOINT = "CargoStatus";




        public IActionResult Index()
        {
            var cargoList = new List<Cargo>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(CARGO_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{CARGO_URL}/Cargo").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    cargoList = JsonConvert.DeserializeObject<List<Cargo>>(data);
                }
            }
            return View(cargoList);
        }



        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            Cargo cargo = new Cargo();
           
            List <CargoType> typeList = new List<CargoType>();
            List <CargoStatus> statuslist = new List<CargoStatus>();


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{CARGO_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //cargo type
                HttpResponseMessage typeResponse = client.GetAsync($"{CARGO_URL}/{CARGO_TYPE_ENDPOINT}").Result;
                if (typeResponse.IsSuccessStatusCode)
                {
                    var typeData = typeResponse.Content.ReadAsStringAsync().Result;
                    typeList = JsonConvert.DeserializeObject<List<CargoType>>(typeData)!;
                }
                
                
                //cargo status
                HttpResponseMessage statusResponse = client.GetAsync($"{CARGO_URL}/{CARGO_STATUS_ENDPOINT}").Result;
                if (statusResponse.IsSuccessStatusCode)
                {
                    var statusData = statusResponse.Content.ReadAsStringAsync().Result;
                    statuslist = JsonConvert.DeserializeObject<List<CargoStatus>>(statusData)!;
                }




                //DDL in CargoVM
                var viewModel = new CargoVM
                {
                    //type
                    CargoTypeList = typeList.Select(type => new SelectListItem
                    {
                        Text = type.CargoTypeName,
                        Value = type.Id.ToString(),
                    }).ToList(),


                    //status
                    CargoStatusList = statuslist.Select(status => new SelectListItem
                    {
                        Text = status.CargoStatusName,
                        Value = status.Id.ToString(),
                    }).ToList(),

                };
                return View(viewModel);


            }
        }





        //CREATE:POST
        [HttpPost]
        public IActionResult Create(CargoVM cargoVm, int id)
        {

            //Succes Message
            if (TempData.ContainsKey("new-cargo"))
            {
                ViewData["new-cargo"] = TempData["new-cargo"].ToString();
            }//-----------------------------------------------------

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{CARGO_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var cargo = new Cargo
            {
                Id = id,
                CargoTypeId = cargoVm.SelectedCargoTypeId,
                CargoDescription = cargoVm.CargoDescription,
                CargoWeight = cargoVm.CargoWeight,
                CargoDimentions= cargoVm.CargoDimentions,
                CargoDestination= cargoVm.CargoDestination,
                CargoStatusId = cargoVm.SelectedCargoStatusId,
            };


            var json = JsonConvert.SerializeObject(cargo);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            HttpResponseMessage cusResponse = client.PutAsync($"{CARGO_URL}/CargoPut", data).Result; //Updates use put request 
            if (cusResponse.IsSuccessStatusCode)
            {
                TempData["new-cargo"] = "New Cargo Added"; // Succes message displayed in create
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to create Cargo");
                return View(cargoVm);
            }
        }


        //------------------------------------------------



        //EDIT:GET
        [HttpGet]
        public IActionResult Edit( int id)
        {
            Cargo cargo = new Cargo();

            List<CargoType> typeList = new List<CargoType>();
            List<CargoStatus> statuslist = new List<CargoStatus>();


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{CARGO_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //cargo type
                HttpResponseMessage typeResponse = client.GetAsync($"{CARGO_URL}/{CARGO_TYPE_ENDPOINT}").Result;
                if (typeResponse.IsSuccessStatusCode)
                {
                    var typeData = typeResponse.Content.ReadAsStringAsync().Result;
                    typeList = JsonConvert.DeserializeObject<List<CargoType>>(typeData)!;
                }


                //cargo status
                HttpResponseMessage statusResponse = client.GetAsync($"{CARGO_URL}/{CARGO_STATUS_ENDPOINT}").Result;
                if (statusResponse.IsSuccessStatusCode)
                {
                    var statusData = statusResponse.Content.ReadAsStringAsync().Result;
                    statuslist = JsonConvert.DeserializeObject<List<CargoStatus>>(statusData)!;
                }


                //gets cargo values
                HttpResponseMessage cargoResponse = client.GetAsync($"{CARGO_URL}/{id}").Result;
                if (cargoResponse.IsSuccessStatusCode)
                {
                    var cargoData = cargoResponse.Content.ReadAsStringAsync().Result;
                    cargo = JsonConvert.DeserializeObject<Cargo>(cargoData)!;
                }


                //DDL in CargoVM
                var viewModel = new CargoVM
                {
                    Id = cargo.Id,
                    SelectedCargoTypeId = cargo.CargoTypeId,
                    CargoDescription = cargo.CargoDescription,
                    CargoWeight = cargo.CargoWeight,
                    CargoDimentions = cargo.CargoDimentions,
                    CargoDestination = cargo.CargoDestination,
                    SelectedCargoStatusId = cargo.CargoStatusId,




                    //type
                    CargoTypeList = typeList.Select(type => new SelectListItem
                    {
                        Text = type.CargoTypeName,
                        Value = type.Id.ToString(),
                    }).ToList(),


                    //status
                    CargoStatusList = statuslist.Select(status => new SelectListItem
                    {
                        Text = status.CargoStatusName,
                        Value = status.Id.ToString(),
                    }).ToList(),

                };
                return View(viewModel);


            }
        }





        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(CargoVM cargoVm)
        {

            //Succes Message
            if (TempData.ContainsKey("new-cargo"))
            {
                ViewData["new-cargo"] = TempData["new-cargo"].ToString();
            }//-----------------------------------------------------

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{CARGO_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var cargo = new Cargo
            {
                Id = cargoVm.Id,
                CargoTypeId = cargoVm.SelectedCargoTypeId,
                CargoDescription = cargoVm.CargoDescription,
                CargoWeight = cargoVm.CargoWeight,
                CargoDimentions = cargoVm.CargoDimentions,
                CargoDestination = cargoVm.CargoDestination,
                CargoStatusId = cargoVm.SelectedCargoStatusId,
            };


            var json = JsonConvert.SerializeObject(cargo);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");


            HttpResponseMessage cusResponse = client.PutAsync($"{CARGO_URL}/CargoPut", data).Result; //Updates use put request 
            if (cusResponse.IsSuccessStatusCode)
            {
                TempData["new-cargo"] = "New Cargo Added"; // Succes message 
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to update Cargo");
                return View(cargoVm);
            }
        }



        //DETAIL
        public IActionResult Detail(int id)
        {
            Cargo cargo = new Cargo();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{CARGO_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{CARGO_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    cargo = JsonConvert.DeserializeObject<Cargo>(data);
                }
            }
            return View(cargo);
        }




        //---------------------------------
        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Cargo cargo = new Cargo();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{CARGO_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{CARGO_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    cargo = JsonConvert.DeserializeObject<Cargo>(data);
                }
            }
            return View(cargo);
        }




        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, Cargo cargo)
        {
            //Cargo cargo = new Cargo();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{CARGO_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
               
                HttpResponseMessage response = client.DeleteAsync($"{CARGO_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    cargo = JsonConvert.DeserializeObject<Cargo>(data);
                }
            }
            return RedirectToAction("Index");
        }

    }
}
