using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
