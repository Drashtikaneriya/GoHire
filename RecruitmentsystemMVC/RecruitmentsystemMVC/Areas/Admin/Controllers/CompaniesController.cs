using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    public class CompaniesController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
