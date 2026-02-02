using Microsoft.AspNetCore.Mvc;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    public class JobPositionsController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
