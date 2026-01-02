using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTOs;
using YourProjectName.Models;
using RecruitmentsystemAPI.DTO;
using RecruitmentsystemAPI.DTO.Role;
namespace RecruitmentsystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public RolesController(AppDbContext db)
        {
            _db = db;
        }

        // ===================== GET ALL =====================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error getting roles", error = ex.Message });
            }
        }

        // ===================== GET BY ID =====================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error getting role", error = ex.Message });
            }
        }

        // ===================== CREATE =====================
        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var role = new Role
                {
                    RoleName = dto.RoleName,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };

                _db.Roles.Add(role);
                await _db.SaveChangesAsync();

                var response = new RoleResponse
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Created = role.Created,
                    Modified = role.Modified
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating role", error = ex.Message });
            }
        }

        // ===================== UPDATE =====================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoleUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _db.Roles.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Role not found" });

                existing.RoleName = dto.RoleName;
                existing.Modified = DateTime.Now;

                await _db.SaveChangesAsync();

                var response = new 
                {
                    RoleId = existing.RoleId,
                    RoleName = existing.RoleName,
                    Created = existing.Created,
                    Modified = existing.Modified
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating role", error = ex.Message });
            }
        }

        // ===================== DELETE =====================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var role = await _db.Roles.FindAsync(id);
                if (role == null)
                    return NotFound(new { message = "Role not found" });

                _db.Roles.Remove(role);
                await _db.SaveChangesAsync();

                return Ok(new { message = "Role deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting role", error = ex.Message });
            }
        }
    }
}
