using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RecruitmentsystemMVC.Areas.HR.Controllers
{
    [Area("HR")]
    public class BaseHRController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var token = HttpContext.Session.GetString("JWToken");

            // Standardize session validation across all HR controllers
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(role) || role != "HR")
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
            }

            base.OnActionExecuting(context);
        }
    }
}
