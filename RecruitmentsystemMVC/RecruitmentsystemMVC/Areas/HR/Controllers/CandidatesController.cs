using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class CandidatesController : BaseHRController
    {
        private readonly IApiService _apiService;

        public CandidatesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var candidates = await _apiService.GetAsync<List<CandidateDTO>>("Candidates");
            return Json(new { data = candidates ?? new List<CandidateDTO>() });
        }

        public async Task<IActionResult> Details(int id)
        {
            var candidate = await _apiService.GetAsync<CandidateDTO>($"Candidates/{id}");
            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }
    }
}
