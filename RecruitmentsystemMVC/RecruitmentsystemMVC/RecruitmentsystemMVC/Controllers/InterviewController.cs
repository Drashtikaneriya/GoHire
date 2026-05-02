using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class InterviewController : Controller
    {
        private readonly InterviewService _interviewService;
        private readonly UserService _userService;
        private readonly RecruitmentsystemMVC.Helpers.RoleHelper _roleHelper;

        public InterviewController(InterviewService interviewService, UserService userService, RecruitmentsystemMVC.Helpers.RoleHelper roleHelper)
        {
            _interviewService = interviewService;
            _userService = userService;
            _roleHelper = roleHelper;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (_roleHelper.IsInterviewer())
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
                var myInterviews = await _interviewService.GetInterviewerInterviewsAsync(userId);
                var paginated = PaginatedList<InterviewDTO>.Create(myInterviews, page, 10);
                return View(paginated);
            }
            
            if (!_roleHelper.CanManageInterviews()) return Unauthorized();

            var interviews = await _interviewService.GetInterviewsAsync();
            int pageSize = 10;
            var paginatedInterviews = PaginatedList<InterviewDTO>.Create(interviews, page, pageSize);
            return View(paginatedInterviews);
        }

        public async Task<IActionResult> InterviewerSchedule()
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            var interviews = await _interviewService.GetInterviewerInterviewsAsync(userId);
            return View(interviews);
        }

        public async Task<IActionResult> MyInterviews()
        {
            var interviews = await _interviewService.GetMyInterviewsAsync();
            return View(interviews);
        }

        [HttpGet]
        public async Task<IActionResult> Schedule()
        {
            if (!_roleHelper.CanManageInterviews()) return Unauthorized();

            var applicationService = HttpContext.RequestServices.GetRequiredService<ApplicationService>();
            var roundService = HttpContext.RequestServices.GetRequiredService<InterviewRoundService>();
            var interviewerService = HttpContext.RequestServices.GetRequiredService<UserService>(); // Interviewers are users
            
            ViewBag.Applications = await applicationService.GetApplicationsAsync();
            ViewBag.Rounds = await roundService.GetInterviewRoundsAsync();
            var allUsers = await interviewerService.GetUsersAsync();
            ViewBag.Interviewers = allUsers.ToList();
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Schedule(ScheduleInterviewDTO interviewDto)
        {
            if (!_roleHelper.CanManageInterviews()) return Forbid();
            var success = await _interviewService.ScheduleInterviewAsync(interviewDto);
            return Json(new { success = success, message = success ? "Interview scheduled successfully." : "Failed to schedule interview." });
        }

        [HttpPost]
        public async Task<IActionResult> Feedback(int id, FeedbackDTO feedbackDto)
        {
            var success = await _interviewService.AddFeedbackAsync(id, feedbackDto);
            return Json(new { success = success, message = success ? "Feedback added successfully." : "Failed to add feedback." });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitResult(int id, string result)
        {
            var success = await _interviewService.SubmitResultAsync(id, result);
            return Json(new { success = success, message = success ? "Result submitted successfully." : "Failed to submit result." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_roleHelper.CanManageInterviews()) return Forbid(); // Admin and HR can delete
            var (success, message) = await _interviewService.DeleteInterviewAsync(id);
            return Json(new { success = success, message = message });
        }
    }
}
