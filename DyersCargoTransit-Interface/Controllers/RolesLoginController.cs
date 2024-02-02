using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using DyersCargoTransit_Interface.Models;

namespace DyersCargoTransit_Interface.Controllers
{
    public class RolesLoginController : Controller
    {
        //const string AUTH_URL = "https://localhost:7005/api/AuthAPI";

        const string SESSION_AUTH = "DyersCargoTransit-API";

        //private readonly HttpClient _client;
        //public RolesLoginController(IHttpClientFactory factoryClient)
        //{
        //    _client = factoryClient.CreateClient("LoginAPI");
        //}




        private readonly HttpClient _client;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesLoginController(IHttpClientFactory factoryClient, UserManager<IdentityUser> userManager)
        {
            _client = factoryClient.CreateClient("LoginAPI");
            _userManager = userManager;
        }







        [HttpGet]
        public IActionResult Index()
        {
            AppUser user = new AppUser();
            return View(user);
        }



        //[HttpPost]
        //public async Task<IActionResult> Index(User user)
        //{
        //    var identityUser = new IdentityUser();
        //    var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        //    HttpResponseMessage response = await _client.PostAsync($"{_client.BaseAddress}/login", content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = await response.Content.ReadAsStringAsync();
        //        var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        //        if (responseData.ContainsKey("status") && responseData["status"].ToString() == "succes")
        //        {
        //            if (responseData.ContainsKey("data") && responseData["data"] is JObject jObject)
        //            {

        //                //var token = responseData["data"].ToString();
        //                var token = jObject.GetValue("result").ToString();

        //                //identityUser = JsonConvert.DeserializeObject<IdentityUser>(responseData["user"].ToString());
        //                HttpContext.Session.SetString(SESSION_AUTH, token);

        //                var returnUrl = HttpContext.Session.GetString("returnUrl")!;
        //                if (returnUrl == null)
        //                {
        //                    return RedirectToAction("Index", "Home");
        //                }
        //                return Redirect(returnUrl);
        //                //return RedirectToAction("Index", "Customer");
        //            }
        //        }
        //        //TempData["new-login"] = "Login Successful"; // Succes message displayed in Home Index
        //        //return RedirectToAction("Index", "Customer");

        //    }
        //    return View(user);

        //}










        //TRY OUT

        //=======================================================

        [HttpPost]
        public async Task<IActionResult> Index(AppUser user)
        {
            var identityUser = new IdentityUser();
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync($"{_client.BaseAddress}/login", content);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

                if (responseData.ContainsKey("status") && responseData["status"].ToString() == "succes")
                {
                    if (responseData.ContainsKey("data") && responseData["data"] is JObject jObject)
                    {
                        var token = jObject.GetValue("result").ToString();

                        // Assuming UserManager<IdentityUser> is injected into your controller
                        var userFromManager = await _userManager.FindByNameAsync(user.Username);

                        // Store user roles in a list
                        var userRoles = await _userManager.GetRolesAsync(userFromManager);

                        HttpContext.Session.SetString(SESSION_AUTH, token);

                        var returnUrl = HttpContext.Session.GetString("returnUrl")!;
                        if (returnUrl == null)
                        {
                            // Check if the user has the "Admin" role
                            if (userRoles.Contains("Admin"))
                            {
                                return RedirectToAction("Index", "Customer");
                            }

                            return RedirectToAction("Index", "Home");
                        }
                        return Redirect(returnUrl);
                    }
                }
            }

            // Handle unsuccessful login
            return View(user);
        }









    }
}
