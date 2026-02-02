using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    public class JobPositionsController : BaseHRController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
