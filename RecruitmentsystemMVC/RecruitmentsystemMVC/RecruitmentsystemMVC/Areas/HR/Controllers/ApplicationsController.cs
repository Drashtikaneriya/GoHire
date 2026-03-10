using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;
using System.Text.Json;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class ApplicationsController : BaseHRController
    {
        private readonly IApiService _apiService;

        public ApplicationsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // Initial load - Fetch from API to pass to server-side rendered table
            var applications = await _apiService.GetAsync<List<ApplicationResponseDTO>>("Applications") ?? new List<ApplicationResponseDTO>();
            return View(applications);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // API already handles joins for JobTitle and CandidateName
                var applications = await _apiService.GetAsync<List<ApplicationResponseDTO>>("Applications") ?? new List<ApplicationResponseDTO>();
                
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                return Json(new { data = applications }, options);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<object>(), error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var app = await _apiService.GetAsync<ApplicationResponseDTO>($"Applications/{id}");
                if (app == null) return NotFound(new { message = "Application not found" });

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                return Json(app, options);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching details", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(ApplicationUpdateDTO dto)
        {
            try
            {
                if (dto.ApplicationId <= 0) return Json(new { success = false, message = "Invalid Application ID" });

                // Call the API PUT endpoint: api/Applications/{id}
                var response = await _apiService.PutAsync($"Applications/{dto.ApplicationId}", dto);
                
                if (response != null && response.IsSuccess)
                {
                    return Json(new { success = true, message = "Application status updated successfully!" });
                }
                return Json(new { success = false, message = response?.Message ?? "Could not update status." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Server error", error = ex.Message });
            }
        }
    }
}
