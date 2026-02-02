using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    public class ReportsController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
