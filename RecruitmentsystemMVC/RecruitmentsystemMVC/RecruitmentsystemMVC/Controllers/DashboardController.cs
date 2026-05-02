using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserService _userService;
        private readonly JobService _jobService;
        private readonly ApplicationService _applicationService;
        private readonly CompanyService _companyService;
        private readonly InterviewService _interviewService;
        private readonly CandidateService _candidateService;

        public DashboardController(
            UserService userService,
            JobService jobService,
            ApplicationService applicationService,
            CompanyService companyService,
            InterviewService interviewService,
            CandidateService candidateService)
        {
            _userService = userService;
            _jobService = jobService;
            _applicationService = applicationService;
            _companyService = companyService;
            _interviewService = interviewService;
            _candidateService = candidateService;
        }

        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdStr);
            ViewBag.Role = role;

            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                var users = await _userService.GetUsersAsync();
                var jobs = await _jobService.GetJobsAsync();
                var apps = await _applicationService.GetApplicationsAsync();
                var companies = await _companyService.GetCompaniesAsync();
                var interviews = await _interviewService.GetInterviewsAsync();
                var candidates = await _candidateService.GetCandidatesAsync();
                var upcomingInterviews = await _interviewService.GetUpcomingInterviewsAsync();

                ViewBag.TotalUsers = users.Count;
                ViewBag.TotalJobs = jobs.Count(j => j.IsActive);
                ViewBag.TotalApplications = apps.Count;
                ViewBag.TotalCompanies = companies.Count;
                ViewBag.TotalInterviews = interviews.Count;
                ViewBag.CandidateCount = candidates.Count;
                ViewBag.PendingReview = apps.Count(a => a.Status == "Applied" || a.Status == "Pending" || a.Status == "Submitted");
                ViewBag.UpcomingInterviewsCount = upcomingInterviews.Count;
                
                // Detailed data for lists
                ViewBag.UpcomingInterviewsList = upcomingInterviews.OrderBy(i => i.InterviewDate).Take(5).ToList();
                ViewBag.RecentApplications = apps.OrderByDescending(a => a.AppliedDate).Take(5).ToList();
            }
            else if (string.Equals(role, "HR Manager", StringComparison.OrdinalIgnoreCase) || 
                     string.Equals(role, "HR_MANAGER", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(role, "HR", StringComparison.OrdinalIgnoreCase))
            {
                var jobs = await _jobService.GetJobsAsync();
                var apps = await _applicationService.GetApplicationsAsync();
                ViewBag.TotalJobs = jobs.Count;
                ViewBag.TotalApplications = apps.Count;
            }
            else if (string.Equals(role, "Interviewer", StringComparison.OrdinalIgnoreCase))
            {
                var interviews = await _interviewService.GetInterviewerInterviewsAsync(userId);
                ViewBag.TotalInterviews = interviews.Count;
                ViewBag.UpcomingInterviews = interviews.Count(i => i.InterviewDate >= DateTime.Now);
            }
            else if (string.Equals(role, "Candidate", StringComparison.OrdinalIgnoreCase))
            {
                var myApps = await _applicationService.GetMyApplicationsAsync();
                var myInterviews = await _interviewService.GetMyInterviewsAsync();
                
                ViewBag.TotalMyApplications = myApps.Count;
                ViewBag.PendingApps = myApps.Count(a => a.Status == "Applied" || a.Status == "Pending" || a.Status == "Submitted");
                ViewBag.UpcomingInterviewsCount = myInterviews.Count(i => i.InterviewDate >= DateTime.Now);
                ViewBag.MyInterviewsList = myInterviews.Where(i => i.InterviewDate >= DateTime.Now).OrderBy(i => i.InterviewDate).Take(3).ToList();
            }

            return View();
        }
    }
}
