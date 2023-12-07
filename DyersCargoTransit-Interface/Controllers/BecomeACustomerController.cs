using DyersCargoTransit_Interface.Models;
using DyersCargoTransit_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DyersCargoTransit_Interface.Controllers
{
    public class BecomeACustomerController : Controller
    {
        const string Customer_URL = "https://localhost:7005/api/CustomerAPI";

        const string PARISH_ENDPOINT = "Parish";


        public IActionResult Index()
        {
            return View();
        }



        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            Customer customer = new Customer();
            List<Parish> parList = new List<Parish>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Customer_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //parish
                HttpResponseMessage parResponse = client.GetAsync($"{Customer_URL}/{PARISH_ENDPOINT}").Result;
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

            //Succes Message
            if (TempData.ContainsKey("you-are-customer"))
            {
                ViewData["you-are-customer"] = TempData["you-are-customer"].ToString();
            }//-----------------------------------------------------

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{Customer_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();

            var customer = new Customer
            {
                Id = id,
                FullName = customerVm.FullName,
                EmailAddress = customerVm.EmailAddress,
                PhoneNumber = customerVm.PhoneNumber,
                TRN = customerVm.TRN,
                DOB = customerVm.DOB,
                Street = customerVm.Street,
                Town = customerVm.Town,
                ParishId = customerVm.SelectedParishId,
            };

            var json = JsonConvert.SerializeObject(customer);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage cusResponse = client.PutAsync($"{Customer_URL}/CustomerPut", data).Result; //Updates use put request 
            if (cusResponse.IsSuccessStatusCode)
            {
                TempData["you-are-customer"] = "Your Information Was Saved"; // Succes message displayed in create
                //return RedirectToAction("Index");
                return View(customerVm);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to submit information");
                return View(customerVm);
            }
        }
    }
}
