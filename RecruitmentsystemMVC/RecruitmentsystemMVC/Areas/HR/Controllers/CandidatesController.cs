using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    public class CandidatesController : BaseHRController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
