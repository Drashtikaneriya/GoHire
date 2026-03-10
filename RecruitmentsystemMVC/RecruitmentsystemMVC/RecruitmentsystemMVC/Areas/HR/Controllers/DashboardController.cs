using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    public class DashboardController : BaseHRController
    {
        private readonly IApiService _apiService;

        public DashboardController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            var stats = await _apiService.GetAsync<RecruitmentsystemMVC.Models.DTOs.DashboardStatsDTO>("dashboard/stats");
            return View(stats ?? new RecruitmentsystemMVC.Models.DTOs.DashboardStatsDTO());
        }
    }
}
