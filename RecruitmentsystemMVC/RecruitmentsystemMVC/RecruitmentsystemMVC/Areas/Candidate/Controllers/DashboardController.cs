using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Areas.Candidate.Controllers
{
    public class DashboardController : BaseCandidateController
    {
        private readonly IApiService _apiService;

        public DashboardController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            var jobsResponse = await _apiService.GetAsync<List<JobPositionDTO>>("JobPositions") ?? new List<JobPositionDTO>();
            var appsResponse = await _apiService.GetAsync<List<ApplicationResponseDTO>>("Applications") ?? new List<ApplicationResponseDTO>();
            var interviewsResponse = await _apiService.GetAsync<List<InterviewResponseDTO>>("Interviews") ?? new List<InterviewResponseDTO>();

            var userId = HttpContext.Session.GetInt32("UserId");
            var candidateName = HttpContext.Session.GetString("UserName");

            var myApps = appsResponse.Where(a => a.CandidateId == userId).ToList();
            var myInterviews = interviewsResponse.Where(i => i.CandidateName == candidateName && (i.Result == "Pending" || i.Result == "Scheduled")).ToList();

            var vm = new CandidateDashboardViewModel
            {
                TotalJobs = jobsResponse.Count,
                AppliedJobs = myApps.Count,
                UpcomingInterviews = myInterviews.Count,
                RecommendedJobs = jobsResponse.Take(5).ToList(),
                RecentApplications = myApps.OrderByDescending(a => a.AppliedOn).Take(5).ToList()
            };

            return View(vm);
        }
    }
}
