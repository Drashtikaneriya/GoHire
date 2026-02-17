using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobPositionsController : Controller
    {
        private readonly IApiService _apiService;

        public JobPositionsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _apiService.GetAsync<List<JobPositionModel>>("JobPositions");
            return Json(new { data = data ?? new List<JobPositionModel>() });
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var job = await _apiService.GetAsync<JobPositionModel>($"JobPositions/{id}");
            return Json(job);
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _apiService.GetAsync<List<CompanyModel>>("Companies");
            return Json(companies);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(JobPositionModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data" });

            // Handle CreatedBy: Preserve original on Edit, Set from Session on Create
            if (model.JobPositionId > 0)
            {
                var existingJob = await _apiService.GetAsync<JobPositionModel>($"JobPositions/{model.JobPositionId}");
                if (existingJob != null)
                {
                    model.CreatedBy = existingJob.CreatedBy;
                }
            }
            else
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId.HasValue) model.CreatedBy = userId.Value;
            }

            // Fetch Company Name if possible
            if(model.CompanyId > 0)
            {
                 var company = await _apiService.GetAsync<CompanyModel>($"Companies/{model.CompanyId}");
                 if(company != null) model.CompanyName = company.CompanyName;
            }

            ApiResponse response;
            if (model.JobPositionId == 0)
                response = await _apiService.PostAsync("JobPositions", model);
            else
                response = await _apiService.PutAsync($"JobPositions/{model.JobPositionId}", model);

            if (response.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = response.Message ?? "Error while saving" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync($"JobPositions/{id}");
            if (response.IsSuccess)
                return Json(new { success = true });

            return Json(new { success = false, message = response.Message ?? "Error deleting job" });
        }
    }
}
