using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationService _applicationService;
        private readonly InterviewService _interviewService;
        private readonly RecruitmentsystemMVC.Helpers.RoleHelper _roleHelper;

        public ApplicationController(ApplicationService applicationService, InterviewService interviewService, RecruitmentsystemMVC.Helpers.RoleHelper roleHelper)
        {
            _applicationService = applicationService;
            _interviewService = interviewService;
            _roleHelper = roleHelper;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Unauthorized();

            var apps = await _applicationService.GetApplicationsAsync();
            int pageSize = 10;
            var paginatedApps = PaginatedList<ApplicationDTO>.Create(apps, page, pageSize);
            return View(paginatedApps);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var app = await _applicationService.GetApplicationByIdAsync(id);
            if (app == null) return NotFound();

            if (_roleHelper.IsCandidate())
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
                if (app.CandidateId != userId) return Unauthorized();
            }
            else if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR())
            {
                return Unauthorized();
            }

            ViewBag.Interviews = await _interviewService.GetInterviewsByApplicationAsync(id);
            return View(app);
        }

        private async Task PrepareDropdowns()
        {
            var jobService = HttpContext.RequestServices.GetRequiredService<JobService>();
            var candidateService = HttpContext.RequestServices.GetRequiredService<CandidateService>();
            ViewBag.Jobs = await jobService.GetActiveJobsAsync();
            ViewBag.Candidates = await candidateService.GetCandidatesAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] int jobId, [FromForm] int candidateId, IFormFile resumeFile)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Forbid();

            var result = await _applicationService.ApplyAsync(jobId, candidateId, resumeFile);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            await PrepareDropdowns();
            ModelState.AddModelError("", result.Message);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var app = await _applicationService.GetApplicationByIdAsync(id);
            if (app == null) return NotFound();

            var jobService = HttpContext.RequestServices.GetRequiredService<JobService>();
            ViewBag.Jobs = await jobService.GetJobsAsync();
            return View(app);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ApplicationDTO appDto)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Forbid();

            if (await _applicationService.UpdateStatusAsync(id, appDto.Status))
            {
                TempData["SuccessMessage"] = "Application updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            var jobService = HttpContext.RequestServices.GetRequiredService<JobService>();
            ViewBag.Jobs = await jobService.GetJobsAsync();
            ModelState.AddModelError("", "Failed to update status.");
            return View(appDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Forbid();

            var (success, message) = await _applicationService.DeleteApplicationAsync(id);
            return Json(new { success = success, message = message });
        }

        [HttpPost]
        public async Task<IActionResult> Apply([FromForm] int jobId, [FromForm] int candidateId, IFormFile resumeFile)
        {
            if (candidateId <= 0)
            {
                return Json(new { success = false, message = "Invalid candidate session. Please login again." });
            }

            var result = await _applicationService.ApplyAsync(jobId, candidateId, resumeFile);
            return Json(new { 
                success = result.Success, 
                message = result.Message 
            });
        }

        [HttpPost]
        public async Task<IActionResult> Shortlist(int id)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Forbid();
            var success = await _applicationService.ShortlistAsync(id);
            return Json(new { success = success });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Forbid();
            var success = await _applicationService.RejectAsync(id);
            return Json(new { success = success });
        }

        public async Task<IActionResult> MyApplications()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            
            var apps = await _applicationService.GetMyApplicationsAsync();
            var myApps = apps.OrderByDescending(a => a.AppliedDate).ToList();
            return View(myApps);
        }
    }
}
