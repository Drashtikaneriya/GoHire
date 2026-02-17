using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Candidate.Controllers
{
    public class DashboardController : BaseCandidateController
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
