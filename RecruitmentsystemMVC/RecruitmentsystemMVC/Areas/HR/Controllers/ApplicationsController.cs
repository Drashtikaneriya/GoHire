using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var applications = await _apiService.GetAsync<List<ApplicationDTO>>("Applications");
            return Json(new { data = applications ?? new List<ApplicationDTO>() });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var application = await _apiService.GetAsync<ApplicationDTO>($"Applications/{id}");
            if (application == null) return NotFound();
            return View(application);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int applicationId, string status, string hrNotes)
        {
            var updateDto = new ApplicationUpdateStatusDTO
            {
                ApplicationId = applicationId,
                Status = status,
                HRNotes = hrNotes
            };

            var response = await _apiService.PutAsync("Applications/UpdateStatus", updateDto);
            
            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Status and notes updated successfully" });
            }
            return Json(new { success = false, message = response.Message });
        }
    }
}
