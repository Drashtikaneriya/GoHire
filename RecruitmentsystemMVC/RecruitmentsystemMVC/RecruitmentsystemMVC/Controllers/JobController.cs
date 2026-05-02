using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class JobController : Controller
    {
        private readonly JobService _jobService;
        private readonly CompanyService _companyService;
        private readonly RecruitmentsystemMVC.Helpers.RoleHelper _roleHelper;

        public JobController(JobService jobService, CompanyService companyService, RecruitmentsystemMVC.Helpers.RoleHelper roleHelper)
        {
            _jobService = jobService;
            _companyService = companyService;
            _roleHelper = roleHelper;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Unauthorized();

            var jobs = await _jobService.GetJobsAsync();
            int pageSize = 10;
            var paginatedJobs = PaginatedList<JobDTO>.Create(jobs, page, pageSize);
            ViewBag.Companies = await _companyService.GetCompaniesAsync();
            return View(paginatedJobs);
        }

        public async Task<IActionResult> Browse()
        {
            var jobs = await _jobService.GetActiveJobsAsync();
            return View(jobs);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound();
            return View(job);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Unauthorized();
            ViewBag.Companies = await _companyService.GetCompaniesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobDTO jobDto)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Forbid();

            if (!ModelState.IsValid)
            {
                ViewBag.Companies = await _companyService.GetCompaniesAsync();
                return View(jobDto);
            }

            if (int.TryParse(HttpContext.Session.GetString("UserId"), out int userId))
            {
                jobDto.CreatedByUserId = userId;
            }

            var result = await _jobService.CreateJobAsync(jobDto);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Job created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Companies = await _companyService.GetCompaniesAsync();
            ModelState.AddModelError("", $"Failed to create job: {result.Message}");
            return View(jobDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Unauthorized();

            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound();
            
            ViewBag.Companies = await _companyService.GetCompaniesAsync();
            return View(job);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, JobDTO jobDto)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Forbid();

            if (!ModelState.IsValid)
            {
                ViewBag.Companies = await _companyService.GetCompaniesAsync();
                return View(jobDto);
            }

            var result = await _jobService.UpdateJobAsync(id, jobDto);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Job updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Companies = await _companyService.GetCompaniesAsync();
            ModelState.AddModelError("", $"Failed to update job: {result.Message}");
            return View(jobDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_roleHelper.IsAdmin() && !_roleHelper.IsHR()) return Forbid();

            var (success, message) = await _jobService.DeleteJobAsync(id);
            return Json(new { success = success, message = message });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var success = await _jobService.ToggleJobStatusAsync(id);
            return Json(new { success = success, message = success ? "Job status updated." : "Failed to update job status." });
        }
    }
}
