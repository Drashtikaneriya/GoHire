using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class InterviewRoundController : Controller
    {
        private readonly InterviewRoundService _roundService;
        private readonly RecruitmentsystemMVC.Helpers.RoleHelper _roleHelper;

        public InterviewRoundController(InterviewRoundService roundService, RecruitmentsystemMVC.Helpers.RoleHelper roleHelper)
        {
            _roundService = roundService;
            _roleHelper = roleHelper;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (!_roleHelper.CanViewInterviewRounds()) return Unauthorized();

            var rounds = await _roundService.GetInterviewRoundsAsync();
            int pageSize = 10;
            // Sorting by ID as sequence is removed from API
            var paginatedRounds = PaginatedList<InterviewRoundDTO>.Create(rounds.OrderBy(r => r.Id).ToList(), page, pageSize);
            return View(paginatedRounds);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInterviewRoundDTO roundDto)
        {
            if (!_roleHelper.CanManageInterviewRounds()) return Forbid();
            if (!ModelState.IsValid) return Json(new { success = false, message = "Validation failed" });

            var success = await _roundService.CreateInterviewRoundAsync(roundDto);
            return Json(new { success = success, message = success ? "Interview round created." : "Failed to create round." });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateInterviewRoundDTO roundDto)
        {
            if (!_roleHelper.CanManageInterviewRounds()) return Forbid();
            if (!ModelState.IsValid) return Json(new { success = false, message = "Validation failed" });

            var success = await _roundService.UpdateInterviewRoundAsync(id, roundDto);
            return Json(new { success = success, message = success ? "Interview round updated." : "Failed to update round." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_roleHelper.CanManageInterviewRounds()) return Forbid();
            var success = await _roundService.DeleteInterviewRoundAsync(id);
            return Json(new { success = success, message = success ? "Delete successful" : "Failed to delete" });
        }
    }
}
