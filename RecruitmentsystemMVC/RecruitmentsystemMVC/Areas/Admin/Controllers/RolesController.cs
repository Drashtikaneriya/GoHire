using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    public class RolesController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
