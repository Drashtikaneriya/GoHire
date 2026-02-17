using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class JobPositionsController : BaseHRController
    {
        private readonly IApiService _apiService;

        public JobPositionsController(IApiService apiService)
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
            var jobs = await _apiService.GetAsync<List<JobPositionModel>>("JobPositions");
            var companies = await _apiService.GetAsync<List<CompanyModel>>("Companies");

            jobs = jobs ?? new List<JobPositionModel>();
            companies = companies ?? new List<CompanyModel>();

            foreach (var job in jobs)
            {
                var comp = companies.FirstOrDefault(c => c.CompanyId == job.CompanyId);
                job.CompanyName = comp?.CompanyName ?? "Not Assigned";
            }

            return Json(new { data = jobs });
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            await LoadCompanies();

            if (id == null || id == 0)
            {
                return View(new JobPositionModel());
            }

            var job = await _apiService.GetAsync<JobPositionModel>($"JobPositions/{id}");
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(JobPositionModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = HttpContext.Session.GetInt32("UserId") ?? 0;

                ApiResponse response;
                if (model.JobPositionId == 0)
                {
                    response = await _apiService.PostAsync("JobPositions", model);
                }
                else
                {
                    response = await _apiService.PutAsync("JobPositions", model);
                }

                if (response.IsSuccess)
                {
                    TempData["Success"] = "Job Position saved successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", response.Message);
                }
            }

            await LoadCompanies();
            return View(model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync($"JobPositions/{id}");
            if (response.IsSuccess)
            {
                return Json(new { success = true, message = "Delete successful" });
            }
            return Json(new { success = false, message = response.Message });
        }

        private async Task LoadCompanies()
        {
            var companies = await _apiService.GetAsync<List<CompanyModel>>("Companies");
            ViewBag.Companies = new SelectList(companies ?? new List<CompanyModel>(), "CompanyId", "CompanyName");
        }
    }
}
