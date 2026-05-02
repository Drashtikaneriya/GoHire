using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class CandidateController : Controller
    {
        private readonly CandidateService _candidateService;
        private readonly RoleService _roleService;

        public CandidateController(CandidateService candidateService, RoleService roleService)
        {
            _candidateService = candidateService;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var candidates = await _candidateService.GetCandidatesAsync();
            int pageSize = 5;
            var paginatedCandidates = PaginatedList<UserDTO>.Create(candidates, page, pageSize);
            return View(paginatedCandidates);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var profile = await _candidateService.GetProfileAsync(id);
            if (profile == null) return NotFound();
            return View(profile);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecruitmentsystemMVC.Models.DTOs.CreateUserDTO candidateDto)
        {
            var roles = await _roleService.GetRolesAsync();
            var candidateRole = roles.FirstOrDefault(r => r.Name == "Candidate");
            candidateDto.RoleId = candidateRole?.Id ?? 0;

            var result = await _candidateService.CreateCandidateAsync(candidateDto);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Candidate added successfully.";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", result.Message);
            return View(candidateDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var profile = await _candidateService.GetProfileAsync(id);
            if (profile == null) return NotFound();
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RecruitmentsystemMVC.Models.DTOs.UserDTO candidateDto)
        {
            var result = await _candidateService.UpdateCandidateAsync(id, candidateDto);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Candidate details updated.";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", result.Message);
            return View(candidateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _candidateService.DeleteCandidateAsync(id);
            return Json(new { success = success, message = message });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            if (await _candidateService.ToggleCandidateStatusAsync(id))
                return Json(new { success = true });
            return Json(new { success = false });
        }

        public async Task<IActionResult> Profile()
        {
            var profile = await _candidateService.GetMyProfileAsync();
            if (profile == null) return NotFound();
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> UploadResume(IFormFile resume)
        {
            if (resume == null || resume.Length == 0) return Json(new { success = false, message = "No file selected." });

            var userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            var success = await _candidateService.UploadResumeAsync(userId, resume);

            if (success) return Json(new { success = true, message = "Resume uploaded successfully!" });
            return Json(new { success = false, message = "Failed to upload resume." });
        }
    }
}
