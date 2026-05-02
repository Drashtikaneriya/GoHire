using Microsoft.AspNetCore.Mvc;
using RecruitmentsystemMVC.Models;
using RecruitmentsystemMVC.Models.DTOs;
using RecruitmentsystemMVC.Services;

namespace RecruitmentsystemMVC.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var roles = await _roleService.GetRolesAsync();
            int pageSize = 5;
            var paginatedRoles = PaginatedList<RoleDTO>.Create(roles, page, pageSize);
            return View(paginatedRoles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleDTO roleDto)
        {
            if (await _roleService.CreateRoleAsync(roleDto))
                return Json(new { success = true });
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, RoleDTO roleDto)
        {
            if (await _roleService.UpdateRoleAsync(id, roleDto))
                return Json(new { success = true });
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _roleService.DeleteRoleAsync(id))
                return Json(new { success = true });
            return Json(new { success = false });
        }
    }
}
