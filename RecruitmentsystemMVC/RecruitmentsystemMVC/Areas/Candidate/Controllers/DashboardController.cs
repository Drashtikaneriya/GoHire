using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Candidate.Controllers
{
    public class DashboardController : BaseCandidateController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
