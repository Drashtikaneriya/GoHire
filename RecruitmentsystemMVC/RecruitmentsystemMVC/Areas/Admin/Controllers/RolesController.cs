using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Services;

//namespace RecruitmentsystemMVC.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class RolesController : Controller
//    {
//        private readonly IApiService _apiService;

//        public RolesController(IApiService apiService)
//        {
//            _apiService = apiService;
//        }

//        // ================= INDEX =================
//        public IActionResult Index()
//        {
//            return View();
//        }

//        // ================= GET ALL =================
//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var roles = await _apiService.GetAsync<List<RoleModel>>("Roles");
//            return Json(new { data = roles ?? new List<RoleModel>() });
//        }

//        // ================= DELETE =================
//        [HttpDelete]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var success = await _apiService.DeleteAsync($"Roles/{id}");
//            return Json(new
//            {
//                success,
//                message = success ? "Role deleted successfully" : "Delete failed"
//            });
//        }

//        // ================= UPSERT GET =================
//        [HttpGet]
//        public async Task<IActionResult> Upsert(int? id)
//        {
//            if (id == null || id == 0)
//                return View(new RoleModel());

//            var role = await _apiService.GetAsync<RoleModel>($"Roles/{id}");
//            if (role == null) return NotFound();

//            return View(role);
//        }

//        // ================= UPSERT POST =================
//        [HttpPost]
//        public async Task<IActionResult> Upsert(RoleModel model)
//        {
//            if (model.RoleId == 0)
//            {
//                await _apiService.PostAsync("Roles", model);
//            }
//            else
//            {
//                await _apiService.PutAsync($"Roles/{model.RoleId}", model);
//            }

//            return RedirectToAction(nameof(Index));
//        }

//    }
//}
using RecruitmentsystemMVC.Models;

[Area("Admin")]
public class RolesController : Controller
{
    private readonly IApiService _apiService;

    public RolesController(IApiService apiService)
    {
        _apiService = apiService;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _apiService.GetAsync<List<RoleModel>>("Roles");
        return Json(new { data = roles ?? new List<RoleModel>() });
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var role = await _apiService.GetAsync<RoleModel>($"Roles/{id}");
        return Json(role);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(RoleModel model)
    {
        if (!ModelState.IsValid)
            return Json(new { success = false, message = "Invalid data" });

        if (model.RoleId == 0)
            await _apiService.PostAsync("Roles", model);
        else
            await _apiService.PutAsync($"Roles/{model.RoleId}", model);

        return Json(new { success = true });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _apiService.DeleteAsync($"Roles/{id}");
        if (response.IsSuccess)
            return Json(new { success = true });

        return Json(new { success = false, message = response.Message ?? "Error deleting role" });
    }
}

