using System.Text;
using System.Text.Json;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Services
{
    public class RoleService : BaseService
    {
        public RoleService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor) { }

        public async Task<List<RoleDTO>> GetRolesAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}Roles");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<RoleDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<RoleDTO>();
        }

        public async Task<bool> CreateRoleAsync(RoleDTO roleDto)
        {
            AddToken();
            var content = new StringContent(JsonSerializer.Serialize(roleDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}Roles", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateRoleAsync(int id, RoleDTO roleDto)
        {
            AddToken();
            var content = new StringContent(JsonSerializer.Serialize(roleDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}Roles/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            AddToken();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}Roles/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
