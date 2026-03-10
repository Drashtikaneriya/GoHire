using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;
using System.IO;

namespace RecruitmentsystemMVC.Areas.Candidate.Controllers
{
    public class ProfileController : BaseCandidateController
    {
        private readonly IApiService _apiService;
        private readonly IWebHostEnvironment _env;

        public ProfileController(IApiService apiService, IWebHostEnvironment env)
        {
            _apiService = apiService;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account", new { area = "" });

            int candidateId = userId.Value;
            var candidatesList = await _apiService.GetAsync<List<CandidateDTO>>("Candidates") ?? new List<CandidateDTO>();
            var candidate = candidatesList.FirstOrDefault(c => c.CandidateId == candidateId);

            if (candidate == null)
            {
                candidate = new CandidateDTO
                {
                    CandidateId = candidateId,
                    FullName = HttpContext.Session.GetString("UserName"),
                    Email = HttpContext.Session.GetString("UserName")?.ToLower() + "@example.com"
                };
            }

            return View(candidate);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CandidateDTO model, IFormFile? ResumeFile)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account", new { area = "" });

            int candidateId = userId.Value;
            model.CandidateId = candidateId;

            // Retain original resume URL if no new file is uploaded
            if (string.IsNullOrEmpty(model.ResumeUrl))
            {
                var candidatesList = await _apiService.GetAsync<List<CandidateDTO>>("Candidates") ?? new List<CandidateDTO>();
                var current = candidatesList.FirstOrDefault(c => c.CandidateId == candidateId);
                if (current != null)
                {
                    model.ResumeUrl = current.ResumeUrl;
                }
            }

            if (ResumeFile != null && ResumeFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "resumes");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + ResumeFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ResumeFile.CopyToAsync(fileStream);
                }
                model.ResumeUrl = "/resumes/" + uniqueFileName;
            }

            var response = await _apiService.PutAsync($"Candidates/{candidateId}", model);
            
            if (response.IsSuccess)
            {
                TempData["Success"] = "Profile updated successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to update profile: " + response.Message;
            }
            
            return RedirectToAction("Index");
        }
    }
}
