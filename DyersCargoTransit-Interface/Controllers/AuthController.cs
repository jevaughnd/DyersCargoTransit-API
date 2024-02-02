using DyersCargoTransit_Interface.Models;
using DyersCargoTransit_Interface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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

        const string Session_Auth2 = "LoginSession";
        const string Session_Role = "RoleSession";


        //////Original
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










        ///============
        /// REGISTER MODIFICATION

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

            HttpClient client2 = new HttpClient();
            client2.BaseAddress = new Uri($"{CUS_PROFILE_URL}")
            {

            };

            ///
            client2.DefaultRequestHeaders.Accept.Clear();
            // CustomerProfile customerProfile = new CustomerProfile();
            //List<Parish> parList = new List<Parish>();

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
                HttpClient client1 = new HttpClient();
                TempData["new-customer"] = "New Customer Profile Added";
                client1.BaseAddress = new Uri(AUTH_URL);

                var user = new AppUser()
                {
                    Username = registrationVM.Username,
                    Password = registrationVM.Password,
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

                        // Succes message displayed in index
                        //return RedirectToAction("Index");
                        //return View(customerVm);
                    }


                }
                return View(registrationVM);
            }
            //--------------------------------------------------------------------------------------------------------
            return View(registrationVM);

        }



























        //--------------------------------------

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
                                var lstRoles = responseData["roles"].ToString();

                                if (lstRoles.Contains("Admin"))
                                {
                                    TempData["new-login"] = "Login Successful"; // Succes message displayed in Home Index
                                    HttpContext.Session.SetString(Session_Role, "Admin");
                                    return RedirectToAction("CustomerProfiles", "Auth");
                                }
                                else
                                {
                                    TempData["new-login"] = "Login Successful"; // Succes message displayed in Home Index
                                    HttpContext.Session.SetString(Session_Role, "Customer");
                                    return RedirectToAction("Index", "Home");
                                }

                            }





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












        

        public IActionResult CustomerProfiles()
        {

            var profileList = new List<CustomerProfile>();
            using (HttpClient client1 = new HttpClient())
            {
                client1.BaseAddress = new Uri(CUS_PROFILE_URL);
                client1.DefaultRequestHeaders.Accept.Clear();
                client1.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client1.GetAsync($"{CUS_PROFILE_URL}/CustomerProfiles").Result;


                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    //Deserialise object from Json string
                    profileList = JsonConvert.DeserializeObject<List<CustomerProfile>>(data);
                }


            }
            return View(profileList);
        }












        //========================







        // EDIT FILE:GET
        [HttpGet]
        public IActionResult EditFile(int id)
        {
            CustomerProfile customerProfile = new CustomerProfile();
            
            CustomerProfileDto customerProfileDto = new CustomerProfileDto();
            List<Parish> ParishList = new List<Parish>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{CUS_PROFILE_URL}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Parish
                HttpResponseMessage pResponse = client.GetAsync($"{CUS_PROFILE_URL}/{PARISH_ENDPOINT}").Result;
                if (pResponse.IsSuccessStatusCode)
                {
                    var pData = pResponse.Content.ReadAsStringAsync().Result;
                    ParishList = JsonConvert.DeserializeObject<List<Parish>>(pData)!;
                }

                // CustomerProfile id
                HttpResponseMessage profileRes = client.GetAsync($"{CUS_PROFILE_URL}/{id}").Result; // {id} shows values in form
                if (profileRes.IsSuccessStatusCode)
                {
                    var data = profileRes.Content.ReadAsStringAsync().Result;
                    // Deserialise object from Json string
                    customerProfile = JsonConvert.DeserializeObject<CustomerProfile>(data)!;
                }

                // DDL in VM
                var viewModel = new CustomerProfileVM
                {
                    Id = customerProfile.Id,
                 
                   
                    FullName = customerProfile.FullName,
                    EmailAddress = customerProfile.EmailAddress,
                    PhoneNumber = customerProfile.PhoneNumber,
                    DOB = customerProfile.DOB,
                    Street = customerProfile.Street,
                    Town = customerProfile.Town,
                    SelectedParishId = customerProfile.ParishId,
                    ProfilePictureFile = customerProfileDto.ProfilePictureFile,

                    // Additional properties from your ViewModel or Model as needed

                    // parish
                    ParishList = ParishList.Select(Vclist => new SelectListItem
                    {
                        Text = Vclist.ParishName,
                        Value = Vclist.Id.ToString(),

                    }).ToList(),
                };

                return View(viewModel);
            }
        }

        // EDIT: FILE Post
        [HttpPost]
        public async Task<IActionResult> EditFile(CustomerProfileVM customerProfileVM)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{CUS_PROFILE_URL}");
            client.DefaultRequestHeaders.Accept.Clear();

            // File upload logic

            if (!ModelState.IsValid) return View(customerProfileVM);


            var customer = new CustomerProfileDto
            {
                
                FullName = customerProfileVM.FullName,
                EmailAddress = customerProfileVM.EmailAddress,
                PhoneNumber = customerProfileVM.PhoneNumber,
                DOB = customerProfileVM.DOB,
                Street = customerProfileVM.Street,
                Town = customerProfileVM.Town,
                ParishId = customerProfileVM.SelectedParishId,
                ProfilePictureFile = customerProfileVM.ProfilePictureFile,
            };


            var response = await SendCustomerProfileToAPI(customer, customerProfileVM.Id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("CustomerProfiles");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Update Customer Profile");
                return View(customerProfileVM);
            }
        }

        // EDIT: FILE PUT
        private async Task<HttpResponseMessage> SendCustomerProfileToAPI(CustomerProfileDto customerProfileDto, int id = 0)
        {
           

            using (var httpClient = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    // Set the Content-Type header to "multipart/form-data"
                    formData.Headers.ContentType.MediaType = "multipart/form-data";

                    // Add customer profile data to the request
                    formData.Add(new StringContent(customerProfileDto.FullName), "FullName");
                    formData.Add(new StringContent(customerProfileDto.EmailAddress), "EmailAddress");
                    formData.Add(new StringContent(customerProfileDto.PhoneNumber), "PhoneNumber");
                    formData.Add(new StringContent(customerProfileDto.DOB.ToString()), "DOB");
                    formData.Add(new StringContent(customerProfileDto.Street), "Street");
                    formData.Add(new StringContent(customerProfileDto.Town), "Town");
                    formData.Add(new StringContent(customerProfileDto.ParishId.ToString()), "ParishId");


                    // Additional properties from  ViewModel or Model as needed

                    //profile img, // File upload logic
                    if (customerProfileDto.ProfilePictureFile != null && customerProfileDto.ProfilePictureFile.Length > 0)
                    {

                        formData.Add(new StreamContent(customerProfileDto.ProfilePictureFile.OpenReadStream())
                        {
                            Headers = { ContentLength = customerProfileDto.ProfilePictureFile.Length,
                                        ContentType = new MediaTypeHeaderValue(
                                            customerProfileDto.ProfilePictureFile.ContentType)
                                    }

                        }, "ProfilePictureFile", customerProfileDto.ProfilePictureFile.FileName);

                    }


                    

                    // Send to API Code
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                    // Send the data to the API
                    return await httpClient.PutAsync($"{CUS_PROFILE_URL}/ProfilePut/{id}", formData);
                }
            }
        }




        //DETAIL
        public IActionResult ProfileDetail(int id)
        {

            CustomerProfile customerProfile = new CustomerProfile();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{CUS_PROFILE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{CUS_PROFILE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    customerProfile = JsonConvert.DeserializeObject<CustomerProfile>(data);
                }
            }
            return View(customerProfile);
        }

























        //COPY OF LOGIN, for Backup

        //--------------------------------------

        ////LOGIN
        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    //Retrieve JWT token from local storage
        //    string token = RetrieveTokenFromLocalStorage();
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        //if the token is missing or expired
        //        return RedirectToAction("Login");
        //    }
        //    return View();
        //}

        //[HttpGet] //gets Login Page
        //public IActionResult Login()
        //{
        //    return View();
        //}



        //[HttpPost]
        //public async Task<IActionResult> Login(AppUser user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(AUTH_URL);
        //            string jsonContent = JsonConvert.SerializeObject(user);

        //            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


        //            //send login to API
        //            HttpResponseMessage response = await client.PostAsync($"{AUTH_URL}/login", content);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var responseContent = await response.Content.ReadAsStringAsync();
        //                var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

        //                if (responseData.ContainsKey("status") && responseData["status"].ToString() == "succes")
        //                {
        //                    if (responseData.ContainsKey("data"))
        //                    {
        //                        var token = responseData["data"].ToString();
        //                        HttpContext.Session.SetString(SESSION_AUTH, token);

        //                    }
        //                    TempData["new-login"] = "Login Successful"; // Succes message displayed in Home Index
        //                    return RedirectToAction("Index", "Customer");


        //                }
        //                else
        //                {
        //                    //login faild
        //                    ModelState.AddModelError(string.Empty, "Invalid Username Or Password");
        //                    return View(user);
        //                }
        //            }
        //            else
        //            {
        //                //login faild
        //                ModelState.AddModelError(string.Empty, "Invalid Username Or Password");
        //                return View(user);
        //            }
        //        }
        //    }
        //    return View(user);
        //}

        //// Reference / called in index 
        //private string RetrieveTokenFromLocalStorage()
        //{
        //    string token = HttpContext.Session.GetString(SESSION_AUTH)!;
        //    return token;
        //}
        ////------------------------------------------------------------------




















    }
}
