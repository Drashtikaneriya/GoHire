using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Candidate.Controllers
{
    public class MyApplicationsController : BaseCandidateController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
