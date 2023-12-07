using DyersCargoTransit_Interface.Models;
using DyersCargoTransit_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DyersCargoTransit_Interface.Controllers
{
    public class CustomerController : Controller
    {
      
        const string BASE_URL = "https://localhost:7005/api/CustomerAPI";


        const string PARISH_ENDPOINT = "Parish";

        //token
        private string RetrieveTokenFromSession()
        {
            string token = HttpContext.Session.GetString("DyersCargoTransit-API")!;
            return token;
        }

        //client - token
        private readonly HttpClient _client;
        public CustomerController(IHttpClientFactory factoryClient)
        {
            _client = factoryClient.CreateClient("LoginAPI");
        }

       

        //CUSTOMER:INDEX
        public IActionResult Index()
        {

            var returnUrl = Request.GetDisplayUrl().ToString();
            string token = RetrieveTokenFromSession();




            //Succes Message
            if (TempData.ContainsKey("new-customer"))
            {
                ViewData["new-customer"] = TempData["new-customer"].ToString();
            }//-----------------------------------------------------


            //Logged In Succes Message, From Login Controller
            if (TempData.ContainsKey("new-login"))
            {
                ViewData["new-login"] = TempData["new-login"].ToString();
            }//-------------


            var cusList = new List<Customer>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/Customer").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    cusList = JsonConvert.DeserializeObject<List<Customer>>(data);
                }
            }
            return View(cusList);
        }






        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            Customer customer = new Customer();
            List<Parish> parList = new List<Parish>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //parish
                HttpResponseMessage parResponse = client.GetAsync($"{BASE_URL}/{PARISH_ENDPOINT}").Result;
                if (parResponse.IsSuccessStatusCode)
                {
                    var parData = parResponse.Content.ReadAsStringAsync().Result;
                    parList = JsonConvert.DeserializeObject<List<Parish>>(parData)!;
                }

                //DDL in CustomerVM
                var viewModel = new CustomerVM
                {
                    //parish
                    ParishList = parList.Select(par => new SelectListItem
                    {
                        Text = par.ParishName,
                        Value = par.Id.ToString(),
                    }).ToList(),
                };
                return View(viewModel);
            }
        }





        //CREATE:POST
        [HttpPost]
        public IActionResult Create(CustomerVM customerVm, int id)
        {

            

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var customer = new Customer
            {
                Id = id,
                FullName = customerVm.FullName,
                EmailAddress = customerVm.EmailAddress,
                PhoneNumber = customerVm.PhoneNumber,
                TRN = customerVm.TRN,
                DOB= customerVm.DOB,
                Street = customerVm.Street,   
                Town = customerVm.Town,
                ParishId = customerVm.SelectedParishId,
            };

            var json = JsonConvert.SerializeObject(customer);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage cusResponse = client.PutAsync($"{BASE_URL}/CustomerPut", data).Result; //Updates use put request 
            if (cusResponse.IsSuccessStatusCode)
            {
                TempData["new-customer"] = "New Customer Added"; // Succes message displayed in index
                return RedirectToAction("Index");
                //return View(customerVm);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "You do not have permission to create Customer");
                return View(customerVm);
            }
        }





        //-----------------------------------------------------------------
        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Customer customer = new Customer();
            List<Parish> parList = new List<Parish>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //parish
                HttpResponseMessage parResponse = client.GetAsync($"{BASE_URL}/{PARISH_ENDPOINT}").Result;
                if (parResponse.IsSuccessStatusCode)
                {
                    var parData = parResponse.Content.ReadAsStringAsync().Result;
                    parList = JsonConvert.DeserializeObject<List<Parish>>(parData)!;
                }



                //CUSTOMER - //This gets & showns the customer details in the fields
                HttpResponseMessage cusResponse = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (cusResponse.IsSuccessStatusCode)
                {
                    var cusData = cusResponse.Content.ReadAsStringAsync().Result;
                    //deserialiaze object
                    customer = JsonConvert.DeserializeObject<Customer>(cusData)!;
                }


                //DDL in CustomerVM
                var viewModel = new CustomerVM
                {
                    Id = customer.Id,
                    FullName = customer.FullName,
                    EmailAddress = customer.EmailAddress,
                    PhoneNumber = customer.PhoneNumber,
                    TRN = customer.TRN,
                    DOB = customer.DOB,
                    Street = customer.Street,
                    Town = customer.Town,
                    SelectedParishId = customer.ParishId,

                    //parish
                    ParishList = parList.Select(par => new SelectListItem
                    {
                        Text = par.ParishName,
                        Value = par.Id.ToString(),
                    }).ToList(),

                };
                return View(viewModel);
            }
        }




        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(CustomerVM customerVm)
        {
            ////session token
            //var returnUrl = Request.GetDisplayUrl().ToString();
            //string token = RetrieveTokenFromSession();
           
            //if(string.IsNullOrEmpty(token))
            //{
            //    return RedirectToAction("Index", "RolesLogin");
            //}
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //if (customerVm == null)
            //{
            //    return View("ERROR");
            //}
           


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var cus = new Customer
            {
                Id = customerVm.Id,
                FullName = customerVm.FullName,
                EmailAddress = customerVm.EmailAddress,
                PhoneNumber = customerVm.PhoneNumber,
                TRN = customerVm.TRN,
                DOB = customerVm.DOB,
                Street = customerVm.Street,
                Town = customerVm.Town,
                ParishId = customerVm.SelectedParishId,
            };


            var json = JsonConvert.SerializeObject(cus);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage cusResponse = client.PutAsync($"{BASE_URL}/CustomerPut", data).Result; //Updates use put request 

            if (cusResponse.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "You do not have permission to Update Customer");
                return View(customerVm);
            }

        }


        //DETAIL
        public IActionResult Detail(int id)
        {

            Customer customer = new Customer();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    customer = JsonConvert.DeserializeObject<Customer>(data);
                }
            }
            return View(customer);
        }







        //--------------------------------------------------
        //DELETE: GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Customer customer = new Customer();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    customer = JsonConvert.DeserializeObject<Customer>(data);
                }
            }
            return View(customer);
        }



        //DELETE: POST
        [HttpPost]
        public IActionResult Delete(int id, Customer customer)
        {

            //Customer customer = new Customer();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    customer = JsonConvert.DeserializeObject<Customer>(data);
                }
            }
            return RedirectToAction("Index");
        }




    }
}
