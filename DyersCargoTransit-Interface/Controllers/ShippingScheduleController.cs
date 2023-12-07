using DyersCargoTransit_Interface.Models;
using DyersCargoTransit_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DyersCargoTransit_Interface.Controllers
{
    public class ShippingScheduleController : Controller
    {
        const string SCHEDULE_URL = "https://localhost:7005/api/ShippingScheduleAPI";

        //ShippingSchedule


        public IActionResult Index()
        {
            var scheduleList = new List<ShippingSchedule>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(SCHEDULE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{SCHEDULE_URL}/ShippingSchedule").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    scheduleList = JsonConvert.DeserializeObject<List<ShippingSchedule>>(data);
                }
            }
            return View(scheduleList);
        }


        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            ShippingSchedule shippingSchedule = new ShippingSchedule();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{SCHEDULE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));



                return View(shippingSchedule);
            }
        }





        //CREATE:POST
        [HttpPost]
        public IActionResult Create(ShippingSchedule shippingSchedule, int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{SCHEDULE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var schedule = new ShippingSchedule
            {
                Id = id,
                DepartureLocation = shippingSchedule.DepartureLocation,
                DepartureTime = shippingSchedule.DepartureTime,
                ArrivalLocation = shippingSchedule.ArrivalLocation,
                ArrivalTime = shippingSchedule.ArrivalTime,
            };

            var json = JsonConvert.SerializeObject(schedule);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage schResponse = client.PutAsync($"{SCHEDULE_URL}/ShippingSchedulePut", data).Result; //Updates use put request 
            if (schResponse.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to create Schedule");
                return View(shippingSchedule);
            }
        }




        //--------------------------------------------

        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ShippingSchedule shippingSchedule = new ShippingSchedule();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{SCHEDULE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));



                //schedule values
                HttpResponseMessage schResponse = client.GetAsync($"{SCHEDULE_URL}/{id}").Result;
                if (schResponse.IsSuccessStatusCode)
                {
                    var schData = schResponse.Content.ReadAsStringAsync().Result;
                    //deserialize object
                    shippingSchedule = JsonConvert.DeserializeObject<ShippingSchedule>(schData)!;
                }
                var schedule = new ShippingSchedule
                {
                    Id = shippingSchedule.Id,
                    DepartureLocation = shippingSchedule.DepartureLocation,
                    DepartureTime = shippingSchedule.DepartureTime,
                    ArrivalLocation = shippingSchedule.ArrivalLocation,
                    ArrivalTime = shippingSchedule.ArrivalTime,
                };

                return View(schedule);
            }
        }





        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(ShippingSchedule shippingSchedule)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{SCHEDULE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var schedule = new ShippingSchedule
            {
                Id = shippingSchedule.Id,
                DepartureLocation = shippingSchedule.DepartureLocation,
                DepartureTime = shippingSchedule.DepartureTime,
                ArrivalLocation = shippingSchedule.ArrivalLocation,
                ArrivalTime = shippingSchedule.ArrivalTime,
            };

            var json = JsonConvert.SerializeObject(schedule);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage schResponse = client.PutAsync($"{SCHEDULE_URL}/ShippingSchedulePut", data).Result; //Updates use put request 
            if (schResponse.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to update Schedule");
                return View(shippingSchedule);
            }
        }



        //DETAIL
        public IActionResult Detail(int id)
        {
            ShippingSchedule schedule = new ShippingSchedule();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{SCHEDULE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
               
                HttpResponseMessage response = client.GetAsync($"{SCHEDULE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    schedule = JsonConvert.DeserializeObject<ShippingSchedule>(data);
                }
            }
            return View(schedule);
        }



        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            ShippingSchedule schedule = new ShippingSchedule();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{SCHEDULE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{SCHEDULE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    schedule = JsonConvert.DeserializeObject<ShippingSchedule>(data);
                }
            }
            return View(schedule);
        }

        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, ShippingSchedule schedule)
        {
            //ShippingSchedule schedule = new ShippingSchedule();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{SCHEDULE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{SCHEDULE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    schedule = JsonConvert.DeserializeObject<ShippingSchedule>(data);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
