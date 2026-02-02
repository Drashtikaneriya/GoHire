using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    public class InterviewsController : BaseHRController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
