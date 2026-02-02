using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Candidate.Controllers
{
    public class ApplyJobController : BaseCandidateController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
