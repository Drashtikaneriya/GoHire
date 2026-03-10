using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InterviewsController : BaseAdminController
    {
        private readonly IApiService _apiService;

        public InterviewsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
             // Uses plural to call API
            var interviews = await _apiService.GetAsync<List<InterviewResponseDTO>>("Interviews") ?? new List<InterviewResponseDTO>();
            return View(interviews);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            var interview = await _apiService.GetAsync<InterviewResponseDTO>($"Interviews/{id}");
            if (interview == null) return Json(new { success = false, message = "Interview not found" });
            return Json(new { success = true, data = interview });
        }
    }
}
