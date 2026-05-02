using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly AppDbContext _db;

    public AuthController(IAuthService authService, AppDbContext db)
    {
        _authService = authService;
        _db = db;
    }

    /// <summary>
    /// Register a new Candidate account. Role is auto-assigned as "Candidate".
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO dto)
    {
        var emailExists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailExists)
            return Conflict(new { message = "Email is already registered." });

        var result = await _authService.RegisterAsync(dto);

        if (result == null)
            return StatusCode(500, new { message = "Registration failed. The 'Candidate' role may not be seeded in the database." });

        return Ok(result);
    }

    /// <summary>
    /// Login for all roles: ADMIN, HR MANAGER, INTERVIEWER, CANDIDATE.
    /// Authenticates by Email + Password.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result == null)
            return Unauthorized(new { message = "Invalid credentials or account is inactive." });

        return Ok(result);
    }
}