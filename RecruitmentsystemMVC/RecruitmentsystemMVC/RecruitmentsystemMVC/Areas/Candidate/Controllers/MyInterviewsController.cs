using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentsystemMVC.Areas.Candidate.Controllers
{
    [Area("Candidate")]
    public class MyInterviewsController : BaseCandidateController
    {
        private readonly IApiService _apiService;

        public MyInterviewsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // API Plural call for Interviews
            var interviews = await _apiService.GetAsync<List<InterviewResponseDTO>>("Interviews") ?? new List<InterviewResponseDTO>();
            
            var candidateName = HttpContext.Session.GetString("UserName");
            var myInterviews = interviews.Where(i => i.CandidateName == candidateName).ToList();

            return View(myInterviews);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            var interview = await _apiService.GetAsync<InterviewResponseDTO>($"Interviews/{id}");
            if (interview == null) return Json(new { success = false, message = "Interview not found" });
            return Json(new { success = true, data = interview });
        }
    }
}
