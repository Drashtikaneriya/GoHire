using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;

namespace RecruitmentsystemMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Home/Error/{statusCode?}")]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode == 404)
            {
                ViewData["ErrorTitle"] = "404 - Page Not Found";
                ViewData["ErrorMessage"] = "Sorry, the page you are looking for does not exist.";
            }
            else
            {
                ViewData["ErrorTitle"] = "An error occurred";
                ViewData["ErrorMessage"] = "Something went wrong. Please try again later.";
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
