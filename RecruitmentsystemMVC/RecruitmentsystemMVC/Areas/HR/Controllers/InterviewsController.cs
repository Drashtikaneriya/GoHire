using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class InterviewsController : BaseHRController
    {
        private readonly IApiService _apiService;

        public InterviewsController(IApiService apiService)
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
            var interviews = await _apiService.GetAsync<List<InterviewDTO>>("Interviews");
            return Json(new { data = interviews ?? new List<InterviewDTO>() });
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            // Populate Applications Dropdown
            var apps = await _apiService.GetAsync<List<ApplicationDTO>>("Applications");
            ViewBag.Applications = new SelectList(apps ?? new List<ApplicationDTO>(), "ApplicationId", "CandidateName"); // Showing Candidate Name mainly

            if (id == null || id == 0)
            {
                return View(new InterviewDTO());
            }

            // Edit - fetch details
            var interview = await _apiService.GetAsync<InterviewDTO>($"Interviews/{id}");
            if (interview == null) return NotFound();
            
            // Map to CreateDTO if needed or just use InterviewDTO which inherits
            return View(interview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(InterviewDTO model)
        {
            if (ModelState.IsValid)
            {
                ApiResponse response;
                if (model.InterviewId == 0)
                {
                    response = await _apiService.PostAsync("Interviews", model);
                }
                else
                {
                    response = await _apiService.PutAsync("Interviews", model);
                }

                if (response.IsSuccess)
                {
                    TempData["Success"] = "Interview saved successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", response.Message);
                }
            }
            
            // Reload dropdown on failure
            var apps = await _apiService.GetAsync<List<ApplicationDTO>>("Applications");
            ViewBag.Applications = new SelectList(apps ?? new List<ApplicationDTO>(), "ApplicationId", "CandidateName");
            return View(model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync($"Interviews/{id}");
            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Delete successful" });
            }
            return Json(new { success = false, message = response.Message });
        }
    }
}
