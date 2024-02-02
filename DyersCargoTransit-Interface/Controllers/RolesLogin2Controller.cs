using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DyersCargoTransit_Interface.Models;

namespace DyersCargoTransit_Interface.Controllers
{
    public class RolesLogin2Controller : Controller
    {
        const string SESSION_AUTH = "DyersCargoTransit-API";

        private readonly HttpClient _client;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesLogin2Controller(IHttpClientFactory factoryClient, UserManager<IdentityUser> userManager)
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

                        if (userFromManager != null)
                        {
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
            }

            // Handle unsuccessful login
            return View(user);
        }
    }
}
