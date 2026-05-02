using System.Text;
using System.Text.Json;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Services
{
    public class UserService : BaseService
    {
        public UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor) { }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}users");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<UserDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UserDTO>();
            }
            return new List<UserDTO>();
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}users/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDTO>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<(bool Success, string Message)> CreateUserAsync(CreateUserDTO userDto)
        {
            AddToken();
            var content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}users", content);
            var result = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "User created successfully" : result);
        }

        public async Task<(bool Success, string Message)> UpdateUserAsync(int id, UserDTO userDto)
        {
            AddToken();
            var updateData = new
            {
                fullName = userDto.FullName,
                email = userDto.Email,
                phone = userDto.Phone,
                roleId = userDto.RoleId,
                isActive = userDto.IsActive
            };
            var content = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}users/{id}", content);
            var result = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "User updated successfully" : result);
        }

        public async Task<(bool Success, string Message)> DeleteUserAsync(int id)
        {
            AddToken();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}users/{id}");
            var message = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "User deleted successfully." : (string.IsNullOrEmpty(message) ? "Failed to delete user." : message));
        }

        public async Task<bool> ToggleUserStatusAsync(int id)
        {
            AddToken();
            var response = await _httpClient.PatchAsync($"{_baseUrl}users/{id}/status", null);
            return response.IsSuccessStatusCode;
        }
    }
}
