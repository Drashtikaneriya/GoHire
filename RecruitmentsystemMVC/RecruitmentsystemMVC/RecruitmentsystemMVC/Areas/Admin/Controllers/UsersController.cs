using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IApiService _apiService;

        public UsersController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _apiService.GetAsync<List<UserDTO>>("User");
            return Json(new { data = data ?? new List<UserDTO>() });
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _apiService.GetAsync<UserDTO>($"User/{id}");
            return Json(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _apiService.GetAsync<List<RoleModel>>("Roles");
            return Json(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(UserDTO model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data" });

            // Fetch Role Name because API requires it
            var role = await _apiService.GetAsync<RoleModel>($"Roles/{model.RoleId}");
            if (role != null)
            {
                model.RoleName = role.RoleName;
            }
            else
            {
                 return Json(new { success = false, message = "Selected Role not found." });
            }

            ApiResponse response;
            if (model.UserId == 0)
                response = await _apiService.PostAsync("User", model);
            else
                response = await _apiService.PutAsync($"User/{model.UserId}", model);

            if (response.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = response.Message ?? "Error while saving" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync($"User/{id}");
            if (response.IsSuccess)
                return Json(new { success = true });

            return Json(new { success = false, message = response.Message ?? "Error deleting user" });
        }
    }
}
