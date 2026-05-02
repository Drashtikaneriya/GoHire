using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO.User;
using RecruitmentsystemAPI.Models;

namespace RecruitmentsystemAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UserController(AppDbContext db) => _db = db;

        // GET /users — Admin, HR Manager, Interviewer
        [Authorize(Roles = "Admin,HR Manager,Interviewer")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _db.Users
                .Include(u => u.Role)
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    RoleId = u.RoleId,
                    RoleName = u.Role != null ? u.Role.RoleName : string.Empty,
                    IsActive = u.IsActive,
                    Created = u.Created,
                    Modified = u.Modified
                })
                .ToListAsync();

            return Ok(users);
        }

        // GET /users/{id} — Admin, HR Manager, Interviewer
        [Authorize(Roles = "Admin,HR Manager,Interviewer")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _db.Users
                .Include(u => u.Role)
                .Where(u => u.UserId == id)
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    RoleId = u.RoleId,
                    RoleName = u.Role != null ? u.Role.RoleName : string.Empty,
                    IsActive = u.IsActive,
                    Created = u.Created,
                    Modified = u.Modified
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        // POST /users — Admin creates HR Managers and Interviewers
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDTO dto)
        {
            var emailExists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailExists)
                return Conflict(new { message = "Email already in use." });

            var roleExists = await _db.Roles.AnyAsync(r => r.RoleId == dto.RoleId);
            if (!roleExists)
                return BadRequest(new { message = "Invalid RoleId." });

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password, // Hash in production!
                Phone = dto.Phone,
                RoleId = dto.RoleId,
                IsActive = true,
                Created = DateTime.Now
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.UserId },
                new { message = "User created successfully.", userId = user.UserId });
        }

        // PUT /users/{id} — Admin only
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDTO dto)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            var emailTaken = await _db.Users.AnyAsync(u => u.Email == dto.Email && u.UserId != id);
            if (emailTaken)
                return Conflict(new { message = "Email already in use by another user." });

            var roleExists = await _db.Roles.AnyAsync(r => r.RoleId == dto.RoleId);
            if (!roleExists)
                return BadRequest(new { message = "Invalid RoleId." });

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.RoleId = dto.RoleId;
            user.IsActive = dto.IsActive;
            user.Modified = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = "User updated successfully." });
        }

        // DELETE /users/{id} — Admin only
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            try
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

                return Ok(new { message = "User deleted successfully." });
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Cannot delete this user because they have associated records (e.g., Candidates, Applications, or Interviews)." });
            }
        }

        // PATCH /users/{id}/status — Admin toggles active/inactive
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> ToggleStatus(int id, [FromBody] UserStatusDTO dto)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            user.IsActive = dto.IsActive;
            user.Modified = DateTime.Now;

            await _db.SaveChangesAsync();

            return Ok(new { message = $"User status set to {(dto.IsActive ? "active" : "inactive")}." });
        }
    }
}
