using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;

        public UserController(UserService userService, RoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var users = await _userService.GetUsersAsync();
            int pageSize = 5;
            var paginatedUsers = PaginatedList<UserDTO>.Create(users, page, pageSize);
            return View(paginatedUsers);
        }
 
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _roleService.GetRolesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _roleService.GetRolesAsync();
                return View(dto);
            }

            var (success, message) = await _userService.CreateUserAsync(dto);
            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            
            ModelState.AddModelError(string.Empty, message);
            ViewBag.Roles = await _roleService.GetRolesAsync();
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user != null)
            {
                ViewBag.Roles = await _roleService.GetRolesAsync();
                return View(user);
            }
            
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _roleService.GetRolesAsync();
                return View(dto);
            }

            var (success, message) = await _userService.UpdateUserAsync(id, dto);
            if (success)
            {
                TempData["SuccessMessage"] = message;
                return RedirectToAction(nameof(Index));
            }
            
            ModelState.AddModelError(string.Empty, message);
            ViewBag.Roles = await _roleService.GetRolesAsync();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _userService.DeleteUserAsync(id);
            
            if (!success && (message.Contains("associated") || message.Contains("linked") || message.Contains("constraint") || message.Contains("foreign key") || message.Contains("conflict")))
            {
                // Fetch dependency details for a professional message
                var candidateService = HttpContext.RequestServices.GetRequiredService<CandidateService>();
                var applicationService = HttpContext.RequestServices.GetRequiredService<ApplicationService>();
                var interviewService = HttpContext.RequestServices.GetRequiredService<InterviewService>();

                // Check if they are a candidate
                var allCandidates = await candidateService.GetCandidatesAsync();
                var hasCandidateProfile = allCandidates.Any(c => c.Id == id);
                
                // Check for applications
                var allApps = await applicationService.GetApplicationsAsync();
                var appCount = allApps.Count(a => a.CandidateId == id);
                
                // Check for interviews
                var allInterviews = await interviewService.GetInterviewsAsync();
                var interviewCount = allInterviews.Count(i => i.CandidateId == id || i.InterviewerUserId == id);

                var detailedMessage = "<div class='text-start'><p class='mb-2'>This user cannot be deleted because they are associated with the following records:</p><ul class='list-group list-group-flush border-top border-bottom mb-3'>";
                
                bool hasDep = false;
                if (hasCandidateProfile) { detailedMessage += "<li class='list-group-item px-0 py-2 d-flex justify-content-between'><span>Candidate Profile</span> <span class='badge bg-secondary rounded-pill'>1</span></li>"; hasDep = true; }
                if (appCount > 0) { detailedMessage += $"<li class='list-group-item px-0 py-2 d-flex justify-content-between'><span>Job Applications</span> <span class='badge bg-secondary rounded-pill'>{appCount}</span></li>"; hasDep = true; }
                if (interviewCount > 0) { detailedMessage += $"<li class='list-group-item px-0 py-2 d-flex justify-content-between'><span>Interview Sessions</span> <span class='badge bg-secondary rounded-pill'>{interviewCount}</span></li>"; hasDep = true; }
                
                if (!hasDep) detailedMessage += "<li class='list-group-item px-0 py-2'>Other System References</li>";
                
                detailedMessage += "</ul><p class='small text-muted mb-0'>Please remove or deactivate these associated records before deleting the user.</p></div>";
                
                return Json(new { success = false, message = detailedMessage });
            }

            return Json(new { success = success, message = message });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var success = await _userService.ToggleUserStatusAsync(id);
            return Json(new { success = success, message = success ? "User status updated." : "Failed to update user status." });
        }
    }
}
