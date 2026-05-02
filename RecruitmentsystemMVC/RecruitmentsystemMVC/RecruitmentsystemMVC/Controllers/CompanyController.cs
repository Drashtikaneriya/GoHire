using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class CompanyController : Controller
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var companies = await _companyService.GetCompaniesAsync();
            int pageSize = 5;
            var paginatedCompanies = PaginatedList<CompanyDTO>.Create(companies, page, pageSize);
            return View(paginatedCompanies);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null) return NotFound();
            return View(company);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var success = await _companyService.CreateCompanyAsync(dto);
            if (success)
            {
                TempData["SuccessMessage"] = "Company created successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            ModelState.AddModelError(string.Empty, "API Error: Could not create the company.");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company != null)
            {
                return View(company);
            }
            
            TempData["ErrorMessage"] = "Company not found.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CompanyDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var success = await _companyService.UpdateCompanyAsync(id, dto);
            if (success)
            {
                TempData["SuccessMessage"] = "Company updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            ModelState.AddModelError(string.Empty, "API Error: Could not update the company.");
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _companyService.DeleteCompanyAsync(id);
            
            if (!success && (message.Contains("associated") || message.Contains("linked") || message.Contains("constraint")))
            {
                var jobService = HttpContext.RequestServices.GetRequiredService<JobService>();
                var allJobs = await jobService.GetJobsAsync();
                var companyJobs = allJobs.Count(j => j.CompanyId == id);
                
                if (companyJobs > 0)
                {
                    var detailedMessage = $"This company cannot be deleted because it has <b>{companyJobs} active/past job postings</b>. Please delete or reassign those jobs first.";
                    return Json(new { success = false, message = detailedMessage, isHtml = true });
                }
            }

            return Json(new { success = success, message = message });
        }
    }
}
