using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTOs;
using RecruitmentsystemAPI.Models;

namespace RecruitmentsystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UserController(AppDbContext db)
        {
            _db = db;
        }

        // ===================== GET ALL =====================
        [Authorize(Roles = "Admin")]

        [HttpGet]
       
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _db.Users
                    .Include(u => u.Role) // ✅ JOIN Role table
                    .Select(u => new UserResponseDTO
                    {
                        UserId = u.UserId,
                        UserName = u.UserName,
                        Email = u.Email,
                        RoleId = u.RoleId,
                        RoleName = u.Role != null ? u.Role.RoleName : "", // ✅
                        CreatedDate = u.CreatedDate,
                        ModifiedDate = u.ModifiedDate
                    })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error getting users", error = ex.Message });
            }
        }

        // ===================== GET BY ID =====================

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _db.Users
                    .Where(u => u.UserId == id)
                    .Select(u => new UserResponseDTO
                    {
                        UserId = u.UserId,
                        UserName = u.UserName,
                        Email = u.Email,
                        RoleId = u.RoleId,
                        CreatedDate = u.CreatedDate,
                        ModifiedDate = u.ModifiedDate
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                    return NotFound(new { message = "User not found" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error getting user", error = ex.Message });
            }
        }

        // ===================== CREATE =====================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = new User
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    Password = dto.Password, // later hash
                    RoleId = dto.RoleId,
                    CreatedDate = DateTime.Now
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                var response = new UserResponseDTO
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    CreatedDate = user.CreatedDate
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating user", error = ex.Message });
            }
        }

        // ===================== UPDATE =====================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _db.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                user.UserName = dto.UserName;
                user.Email = dto.Email;
                user.RoleId = dto.RoleId;
                user.ModifiedDate = DateTime.Now;

                await _db.SaveChangesAsync();

                var response = new UserResponseDTO
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    CreatedDate = user.CreatedDate,
                    ModifiedDate = user.ModifiedDate
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating user", error = ex.Message });
            }
        }

        // ===================== DELETE =====================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting user", error = ex.Message });
            }
        }
    }
}
