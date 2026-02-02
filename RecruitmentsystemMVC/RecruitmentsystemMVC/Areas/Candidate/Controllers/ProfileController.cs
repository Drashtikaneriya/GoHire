using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Candidate.Controllers
{
    public class ProfileController : BaseCandidateController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
