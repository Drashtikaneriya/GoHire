using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO.Role;
using RecruitmentsystemAPI.Models;

namespace RecruitmentsystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public RolesController(AppDbContext db) => _db = db;

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _db.Roles
                .Select(r => new RoleResponse
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Created = r.Created,
                    Modified = r.Modified
                })
                .ToListAsync();

            return Ok(roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _db.Roles
                .Where(r => r.RoleId == id)
                .Select(r => new RoleResponse
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Created = r.Created,
                    Modified = r.Modified
                })
                .FirstOrDefaultAsync();

            if (role == null)
                return NotFound(new { message = "Role not found" });

            return Ok(role);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateDTO dto)
        {
            var exists = await _db.Roles.AnyAsync(r => r.RoleName == dto.RoleName);
            if (exists)
                return Conflict(new { message = "A role with this name already exists." });

            var role = new Role
            {
                RoleName = dto.RoleName,
                Created = DateTime.Now
            };

            _db.Roles.Add(role);
            await _db.SaveChangesAsync();

            return Ok(new RoleResponse
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Created = role.Created,
                Modified = role.Modified
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoleUpdateDTO dto)
        {
            var role = await _db.Roles.FindAsync(id);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            var duplicate = await _db.Roles.AnyAsync(r => r.RoleName == dto.RoleName && r.RoleId != id);
            if (duplicate)
                return Conflict(new { message = "Another role with this name already exists." });

            role.RoleName = dto.RoleName;
            role.Modified = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new RoleResponse
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Created = role.Created,
                Modified = role.Modified
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _db.Roles.FindAsync(id);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Role deleted successfully" });
        }
    }
}
