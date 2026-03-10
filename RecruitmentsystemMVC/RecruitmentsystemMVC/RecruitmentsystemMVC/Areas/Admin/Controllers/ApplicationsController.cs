using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;
using System.Text.Json;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationsController : BaseAdminController
    {
        private readonly IApiService _apiService;

        public ApplicationsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var apps = await _apiService.GetAsync<List<ApplicationResponseDTO>>("Applications") ?? new List<ApplicationResponseDTO>();
            return View(apps);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var app = await _apiService.GetAsync<ApplicationResponseDTO>($"Applications/{id}");
            if (app == null) return NotFound(new { message = "Application not found" });
            
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return Json(app, options);
        }
    }
}
