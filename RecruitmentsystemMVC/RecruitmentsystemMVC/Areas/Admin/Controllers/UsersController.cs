using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
