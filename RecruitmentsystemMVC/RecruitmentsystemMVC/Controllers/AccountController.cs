using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using System.Text;
using System.Text.Json;

namespace RecruitmentsystemMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        public AccountController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
            _baseUrl = "https://localhost:7272/api/"; // Base URL as per requirement
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("JWToken") != null)
            {
                var role = HttpContext.Session.GetString("UserRole");
                return RedirectToDashboard(role);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _clientFactory.CreateClient();

            var requestDto = new LoginRequestDTO
            {
                UserName = model.UserName,
                Password = model.Password
            };

            var json = JsonSerializer.Serialize(requestDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(_baseUrl + "auth/login", content);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Invalid username or password";
                    return View(model);
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponseDTO>(responseJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (loginResponse != null)
                {
                    // 🔐 Store in Session
                    HttpContext.Session.SetString("JWToken", loginResponse.Token);
                    HttpContext.Session.SetString("UserRole", loginResponse.Role);
                    HttpContext.Session.SetString("UserName", loginResponse.UserName);
                    HttpContext.Session.SetInt32("UserId", loginResponse.UserId);

                    // 🚀 Role Wise Redirect
                    return RedirectToDashboard(loginResponse.Role);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "API Connection Error: " + ex.Message;
            }

            return View(model);
        }

        private IActionResult RedirectToDashboard(string role)
        {
            return role switch
            {
                "Admin" => RedirectToAction("Index", "Dashboard", new { area = "Admin" }),
                "HR" => RedirectToAction("Index", "Dashboard", new { area = "HR" }),
                "Candidate" => RedirectToAction("Index", "Dashboard", new { area = "Candidate" }),
                _ => RedirectToAction("Login")
            };
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

