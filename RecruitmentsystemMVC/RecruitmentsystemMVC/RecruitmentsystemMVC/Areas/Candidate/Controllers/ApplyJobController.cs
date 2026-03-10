using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;
using System.IO;

namespace RecruitmentsystemMVC.Areas.Candidate.Controllers
{
    public class ApplyJobController : BaseCandidateController
    {
        private readonly IApiService _apiService;
        private readonly IWebHostEnvironment _env;

        public ApplyJobController(IApiService apiService, IWebHostEnvironment env)
        {
            _apiService = apiService;
            _env = env;
        }

        public async Task<IActionResult> Index(string keyword)
        {
            var jobs = await _apiService.GetAsync<List<JobPositionDTO>>("JobPositions") ?? new List<JobPositionDTO>();

            if (!string.IsNullOrEmpty(keyword))
            {
                jobs = jobs.Where(j => j.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) || 
                                     (j.Location != null && j.Location.Contains(keyword, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            return View(jobs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var job = await _apiService.GetAsync<JobPositionDTO>($"JobPositions/{id}");
            if (job == null) return NotFound();
            
            var vm = new JobApplicationViewModel
            {
                JobId = job.JobPositionId,
                Job = job
            };
            
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Apply(JobApplicationViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account", new { area = "" });

            int candidateId = userId.Value;

            // Prevent duplicate application
            var existingApps = await _apiService.GetAsync<List<ApplicationResponseDTO>>("Applications") ?? new List<ApplicationResponseDTO>();
            if (existingApps.Any(a => a.CandidateId == candidateId && a.JobId == model.JobId))
            {
                TempData["Error"] = "You have already applied for this job.";
                return RedirectToAction("Details", new { id = model.JobId });
            }

            if (model.Resume != null && model.Resume.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "resumes");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Resume.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Resume.CopyToAsync(fileStream);
                }

                // Update Candidate Resume URL
                var candidatesList = await _apiService.GetAsync<List<CandidateDTO>>("Candidates") ?? new List<CandidateDTO>();
                var candidate = candidatesList.FirstOrDefault(c => c.CandidateId == candidateId);
                // Note: If no Candidates API Put exists, this might silently fail, but URL is saved locally.
                if (candidate != null)
                {
                    candidate.ResumeUrl = "/resumes/" + uniqueFileName;
                    await _apiService.PutAsync($"Candidates/{candidateId}", candidate);
                }
            }
            else
            {
                TempData["Error"] = "Please upload your resume (PDF/DOC).";
                return RedirectToAction("Details", new { id = model.JobId });
            }

            var application = new ApplicationModel
            {
                JobId = model.JobId,
                CandidateId = candidateId,
                AppliedOn = DateTime.UtcNow,
                Status = "Applied"
            };

            var response = await _apiService.PostAsync("Applications", application);

            if (response.IsSuccess)
            {
                TempData["Success"] = "Application submitted successfully!";
                return RedirectToAction("Index", "MyApplications", new { area = "Candidate" });
            }
            else
            {
                TempData["Error"] = response.Message ?? "Failed to submit application. Note: You may have already applied for this job.";
                return RedirectToAction("Details", new { id = model.JobId });
            }
        }
    }
}
