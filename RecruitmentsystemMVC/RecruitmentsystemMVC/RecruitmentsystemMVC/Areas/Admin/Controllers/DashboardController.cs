using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        private readonly IApiService _apiService;

        public DashboardController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersList()
        {
            var data = await _apiService.GetAsync<List<UserDTO>>("User");
            return Json(data ?? new List<UserDTO>());
        }

        [HttpGet]
        public async Task<IActionResult> GetJobsList()
        {
            var data = await _apiService.GetAsync<List<JobPositionModel>>("JobPositions");
            return Json(data ?? new List<JobPositionModel>());
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidatesList()
        {
            var data = await _apiService.GetAsync<List<CandidateDTO>>("Candidates");
            return Json(data ?? new List<CandidateDTO>());
        }

        [HttpGet]
        public async Task<IActionResult> GetApplicationsList()
        {
            var data = await _apiService.GetAsync<List<ApplicationResponseDTO>>("Applications");
            return Json(data ?? new List<ApplicationResponseDTO>());
        }

        [HttpGet]
        public async Task<IActionResult> GetInterviewsList()
        {
            var data = await _apiService.GetAsync<List<InterviewResponseDTO>>("Interviews");
            return Json(data ?? new List<InterviewResponseDTO>());
        }
    }
}
