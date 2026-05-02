using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly UserService _userService;
        private readonly JobService _jobService;
        private readonly ApplicationService _applicationService;
        private readonly InterviewService _interviewService;
        private readonly CompanyService _companyService;

        public ReportController(
            UserService userService, 
            JobService jobService, 
            ApplicationService applicationService, 
            InterviewService interviewService,
            CompanyService companyService)
        {
            _userService = userService;
            _jobService = jobService;
            _applicationService = applicationService;
            _interviewService = interviewService;
            _companyService = companyService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUsersAsync();
            var jobs = await _jobService.GetJobsAsync();
            var applications = await _applicationService.GetApplicationsAsync();
            var interviews = await _interviewService.GetInterviewsAsync();
            var companies = await _companyService.GetCompaniesAsync();

            ViewBag.TotalUsers = users.Count;
            ViewBag.ActiveJobs = jobs.Count(j => j.Status == "Active");
            ViewBag.TotalApplications = applications.Count;
            ViewBag.TotalInterviews = interviews.Count;
            ViewBag.TotalCompanies = companies.Count;

            // Application Status Counts
            ViewBag.PendingApps = applications.Count(a => a.Status == "Applied" || a.Status == "Pending");
            ViewBag.AcceptedApps = applications.Count(a => a.Status == "Accepted" || a.Status == "Selected");
            ViewBag.RejectedApps = applications.Count(a => a.Status == "Rejected");

            // Job Type Distribution
            var jobTypes = jobs.GroupBy(j => j.EmploymentType).Select(g => new { Type = g.Key, Count = g.Count() }).ToList();
            ViewBag.JobTypeLabels = jobTypes.Select(j => j.Type).ToArray();
            ViewBag.JobTypeCounts = jobTypes.Select(j => j.Count).ToArray();

            // Mock Trend Data (Current Month)
            ViewBag.MonthlyAppLabels = new string[] { "Week 1", "Week 2", "Week 3", "Week 4" };
            ViewBag.MonthlyAppData = new int[] { applications.Count / 4, applications.Count / 3, applications.Count / 2, applications.Count };

            return View();
        }
    }
}
