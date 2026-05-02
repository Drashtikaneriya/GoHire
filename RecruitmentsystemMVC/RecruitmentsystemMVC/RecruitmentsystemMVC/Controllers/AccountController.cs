using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("JWTToken") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid) return View(loginDto);

            var response = await _authService.LoginAsync(loginDto);
            if (response != null)
            {
                HttpContext.Session.SetString("JWTToken", response.Token ?? "");
                
                // Fetch full user details from /auth/me
                var me = await _authService.GetMeAsync();
                if (me != null)
                {
                    HttpContext.Session.SetString("UserRole", me.Role ?? "");
                    HttpContext.Session.SetString("UserId", me.UserId.ToString());
                    HttpContext.Session.SetString("UserName", me.FullName ?? me.Name ?? me.UserName ?? "User");
                    HttpContext.Session.SetString("UserEmail", me.Email ?? "");
                }
                else
                {
                    // Fallback to login response data
                    HttpContext.Session.SetString("UserRole", response.Role ?? "");
                    HttpContext.Session.SetString("UserId", response.UserId.ToString());
                    HttpContext.Session.SetString("UserName", response.FullName ?? response.Name ?? response.UserName ?? "User");
                }

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(loginDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            if (!ModelState.IsValid) return View(registerDto);

            var success = await _authService.RegisterAsync(registerDto);
            if (success)
            {
                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Registration failed. Email might already exist.");
            return View(registerDto);
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login");
        }
    }
}
