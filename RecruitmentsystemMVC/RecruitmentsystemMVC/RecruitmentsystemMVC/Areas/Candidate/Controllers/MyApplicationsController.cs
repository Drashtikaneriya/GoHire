using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;
using System.Text.Json;

namespace RecruitmentsystemMVC.Areas.Candidate.Controllers
{
    [Area("Candidate")]
    public class MyApplicationsController : BaseCandidateController
    {
        private readonly IApiService _apiService;

        public MyApplicationsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var appsResponse = await _apiService.GetAsync<List<ApplicationResponseDTO>>("Applications") ?? new List<ApplicationResponseDTO>();
            var jobsResponse = await _apiService.GetAsync<List<JobPositionDTO>>("JobPositions") ?? new List<JobPositionDTO>();
            var companiesResponse = await _apiService.GetAsync<List<RecruitmentsystemMVC.Models.CompanyModel>>("Companies") ?? new List<RecruitmentsystemMVC.Models.CompanyModel>();
            
            var userId = HttpContext.Session.GetInt32("UserId");
            var myApps = appsResponse.Where(a => a.CandidateId == userId).ToList();

            var viewModelList = myApps.Select(a => {
                var job = jobsResponse.FirstOrDefault(j => j.JobPositionId == a.JobId);
                var company = companiesResponse.FirstOrDefault(c => c.CompanyId == job?.CompanyId);
                return new RecruitmentsystemMVC.Models.CandidateApplicationItemViewModel
                {
                    ApplicationId = a.ApplicationId,
                    JobId = a.JobId,
                    JobTitle = a.JobTitle ?? job?.Title ?? "N/A",
                    CompanyName = company?.CompanyName ?? "Unknown Company",
                    Status = a.Status ?? "Applied",
                    AppliedOn = a.AppliedOn,
                    CandidateName = a.CandidateName ?? "Unknown",
                    HRNotes = a.HRNotes ?? ""
                };
            }).ToList();

            return View(viewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var app = await _apiService.GetAsync<ApplicationResponseDTO>($"Applications/{id}");
            if (app == null) return NotFound(new { message = "Application not found" });
            
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return Json(app, options);
        }
    }
}
