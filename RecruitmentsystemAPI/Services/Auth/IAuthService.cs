using RecruitmentsystemAPI.DTO.Auth;
using RecruitmentsystemAPI.Models;

public interface IAuthService
{
    Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO dto);
    string GenerateJwtToken(User user);
}
