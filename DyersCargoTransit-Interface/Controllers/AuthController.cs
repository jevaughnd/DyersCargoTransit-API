using DyersCargoTransit_Interface.Models;
using DyersCargoTransit_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace DyersCargoTransit_Interface.Controllers
{
    public class AuthController : Controller
    {
        const string AUTH_URL = "https://localhost:7005/api/AuthAPI";

        const string CUS_PROFILE_URL = "https://localhost:7005/api/CustomerProfile";

        const string PARISH_ENDPOINT = "Parish";

        // https://localhost:7005/api/CustomerProfile/CustomerProfiles

        const string SESSION_AUTH = "DyersCargoTransit-API";


        ////Original
        ////REGISTER
        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Register(AppUser user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(AUTH_URL);
        //            string jsonContent = JsonConvert.SerializeObject(user);

        //            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //            // Send registration data to API
        //            HttpResponseMessage response = await client.PostAsync($"{AUTH_URL}/register", content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var responseContent = await response.Content.ReadAsStringAsync();
        //                var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

        //                if (responseData.ContainsKey("status") && responseData["status"].ToString() == "success")
        //                {
        //                    TempData["registration-success"] = "Registration Was Successful";
        //                    return RedirectToAction("Auth", "Login");
        //                }
        //                else
        //                {
        //                    // Registration failed
        //                    if (responseData.ContainsKey("message"))
        //                    {
        //                        ModelState.AddModelError(string.Empty, responseData["message"].ToString());
        //                    }
        //                    else
        //                    {
        //                        ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
        //                    }

        //                    return View(user);
        //                }
        //            }
        //            else
        //            {
        //                // Registration failed
        //                ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
        //                return View(user);
        //            }
        //        }
        //    }

        //    return View(user);
        //}
        //////--------------------------------------------------------------------------------------------------------






        //============
        //NEW REGISTER


        //REGISTER
        [HttpGet]
        public IActionResult RegisterUser()
        {
            var nUser = new RegistrationVM();
            return View(nUser);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegistrationVM registrationVM, int id)
        {


            using (HttpClient client1 = new HttpClient())
            {

                client1.BaseAddress = new Uri(AUTH_URL);

                var user = new AppUser()
                {
                    Username = registrationVM.Username,
                    Password = registrationVM.Password,
                    Roles =
                        {
                            "Customer"
                        }
                };

                string jsonContent = JsonConvert.SerializeObject(user);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send registration data to API
                HttpResponseMessage response = await client1.PostAsync($"{AUTH_URL}/register", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);




                    if (responseData.ContainsKey("status") && responseData["status"].ToString() == "success")
                    {
                        TempData["registration-success"] = "Registration Was Successful";
                        return RedirectToAction("Auth", "Login");
                    }
                    else
                    {
                        // Registration failed
                        if (responseData.ContainsKey("message"))
                        {
                            ModelState.AddModelError(string.Empty, responseData["message"].ToString());
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                        }

                        return View(registrationVM);
                    }






                }
                else
                {
                    // Registration failed
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    return View(registrationVM);
                }



            }


            //if (ModelState.IsValid)
            //{
                


            //}


            HttpClient client2 = new HttpClient();
            client2.BaseAddress = new Uri($"{CUS_PROFILE_URL}")
            {

            };

            ///
            client2.DefaultRequestHeaders.Accept.Clear();
            // CustomerProfile customerProfile = new CustomerProfile();
            List<Parish> parList = new List<Parish>();

            //Customer Profile
            var customerPro = new CustomerProfile
            {
                Id = 0,
                FullName = registrationVM.FullName,
                EmailAddress = registrationVM.EmailAddress,
                PhoneNumber = registrationVM.PhoneNumber,
                DOB = registrationVM.DOB,
                Street = registrationVM.Street,
                Town = registrationVM.Town,
                ParishId = registrationVM.ParishId,
            };

            var json = JsonConvert.SerializeObject(customerPro);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage cusResponse = client2.PostAsync($"{CUS_PROFILE_URL}/CustomerProfilePost", data).Result; //
            if (cusResponse.IsSuccessStatusCode)
            {
                TempData["new-customer"] = "New Customer Added"; // Succes message displayed in index
                                                                 //return RedirectToAction("Index");
                                                                 //return View(customerVm);
            }






            return View(registrationVM);
        }
        //--------------------------------------------------------------------------------------------------------




















        //LOGIN
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Retrieve JWT token from local storage
            string token = RetrieveTokenFromLocalStorage();
            if (string.IsNullOrEmpty(token))
            {
                //if the token is missing or expired
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet] //gets Login Page
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(AppUser user)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AUTH_URL);
                    string jsonContent = JsonConvert.SerializeObject(user);

                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                    //send login to API
                    HttpResponseMessage response = await client.PostAsync($"{AUTH_URL}/login", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

                        if (responseData.ContainsKey("status") && responseData["status"].ToString() == "succes")
                        {
                            if (responseData.ContainsKey("data"))
                            {
                                var token = responseData["data"].ToString();
                                HttpContext.Session.SetString(SESSION_AUTH, token);

                            }
                            TempData["new-login"] = "Login Successful"; // Succes message displayed in Home Index
                            return RedirectToAction("Index", "Customer");


                        }
                        else
                        {
                            //login faild
                            ModelState.AddModelError(string.Empty, "Invalid Username Or Password");
                            return View(user);
                        }
                    }
                    else
                    {
                        //login faild
                        ModelState.AddModelError(string.Empty, "Invalid Username Or Password");
                        return View(user);
                    }
                }
            }
            return View(user);
        }

        // Reference / called in index 
        private string RetrieveTokenFromLocalStorage()
        {
            string token = HttpContext.Session.GetString(SESSION_AUTH)!;
            return token;
        }
        //------------------------------------------------------------------



        

    }
}
