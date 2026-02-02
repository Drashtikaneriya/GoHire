using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    public class DashboardController : BaseHRController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
