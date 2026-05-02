using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecruitmentsystemAPI.Data;
using RecruitmentsystemAPI.DTO.Auth;
using RecruitmentsystemAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO dto)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u =>
                u.Email == dto.Email &&
                u.Password == dto.Password &&
                u.IsActive);

        if (user == null)
            return null;

        var token = GenerateJwtToken(user);

        return new LoginResponseDTO
        {
            Token = token,
            UserId = user.UserId,
            UserName = user.FullName,
            Role = user.Role!.RoleName
        };
    }

    public async Task<LoginResponseDTO?> RegisterAsync(RegisterRequestDTO dto)
    {
        // Find the Candidate role
        var candidateRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.RoleName == "Candidate");

        if (candidateRole == null)
            return null; // Candidate role must exist in DB

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Password = dto.Password, // Hash in production!
            Phone = dto.Phone,
            RoleId = candidateRole.RoleId,
            IsActive = true,
            Created = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Auto-create an empty candidate profile
        var candidate = new Candidate
        {
            UserId = user.UserId,
            Created = DateTime.Now
        };

        _context.Candidates.Add(candidate);
        await _context.SaveChangesAsync();

        // Reload user with role for JWT generation
        await _context.Entry(user).Reference(u => u.Role).LoadAsync();

        var token = GenerateJwtToken(user);

        return new LoginResponseDTO
        {
            Token = token,
            UserId = user.UserId,
            UserName = user.FullName,
            Role = user.Role!.RoleName
        };
    }

    public string GenerateJwtToken(User user)
    {
        var jwt = _config.GetSection("Jwt");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role!.RoleName),
            new Claim("UserId", user.UserId.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                Convert.ToDouble(jwt["TokenExpiryMinutes"])
            ),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}