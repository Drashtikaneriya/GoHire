using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;
using System.Text.Json;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompaniesController : Controller
    {
        private readonly IApiService _apiService;

        public CompaniesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _apiService.GetAsync<List<CompanyModel>>("Companies");
            // FORCE CamelCase serialization
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return Json(new { data = data ?? new List<CompanyModel>() }, options);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest("Invalid Company ID");

            var company = await _apiService.GetAsync<CompanyModel>($"Companies/{id}");
            
            if (company == null) return NotFound(new { message = "Company not found" });

            // FORCE CamelCase serialization
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return Json(company, options);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(CompanyModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Validation Failed", errors = errors });
            }

            // PRESERVE CreatedBy logic
            if (model.CompanyId > 0)
            {
                var existing = await _apiService.GetAsync<CompanyModel>($"Companies/{model.CompanyId}");
                if (existing != null)
                {
                    model.CreatedBy = existing.CreatedBy;
                }
            }
            else
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                model.CreatedBy = userId ?? 1; // Default to 1 if session missing
            }

            ApiResponse response;
            
            // LOGIC: If ID > 0 -> UPDATE (PUT), Else -> CREATE (POST)
            if (model.CompanyId > 0)
            {
                response = await _apiService.PutAsync($"Companies/{model.CompanyId}", model);
            }
            else
            {
                response = await _apiService.PostAsync("Companies", model);
            }

            if (response != null && response.IsSuccess)
            {
                return Json(new { success = true, message = model.CompanyId > 0 ? "Company updated successfully" : "Company created successfully" });
            }

            return Json(new { success = false, message = response?.Message ?? "Error occurred while saving." });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return Json(new { success = false, message = "Invalid ID" });

            var response = await _apiService.DeleteAsync($"Companies/{id}");

            if (response != null && response.IsSuccess)
            {
                return Json(new { success = true, message = "Company deleted successfully" });
            }

            return Json(new { success = false, message = response?.Message ?? "Error deleting company" });
        }
    }
}
