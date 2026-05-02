using RecruitmentsystemAPI.DTO.Auth;
using RecruitmentsystemAPI.Models;

public interface IAuthService
{
    Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO dto);
    Task<LoginResponseDTO?> RegisterAsync(RegisterRequestDTO dto);
    string GenerateJwtToken(User user);
}