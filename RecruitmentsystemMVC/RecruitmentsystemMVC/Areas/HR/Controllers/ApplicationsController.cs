using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    public class ApplicationsController : BaseHRController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
