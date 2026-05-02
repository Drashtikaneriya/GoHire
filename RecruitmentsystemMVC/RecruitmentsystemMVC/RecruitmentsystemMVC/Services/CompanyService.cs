using System.Text;
using System.Text.Json;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Services
{
    public class CompanyService : BaseService
    {
        public CompanyService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor) { }

        public async Task<List<CompanyDTO>> GetCompaniesAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}companies");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<CompanyDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CompanyDTO>();
            }
            return new List<CompanyDTO>();
        }

        public async Task<CompanyDTO> GetCompanyByIdAsync(int id)
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}companies/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CompanyDTO>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<bool> CreateCompanyAsync(CreateCompanyDTO companyDto)
        {
            AddToken();
            var content = new StringContent(JsonSerializer.Serialize(companyDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}companies", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCompanyAsync(int id, CompanyDTO companyDto)
        {
            AddToken();
            var updateData = new
            {
                companyName = companyDto.CompanyName,
                industry = companyDto.Industry,
                email = companyDto.Email,
                phone = companyDto.Phone,
                website = companyDto.Website,
                address = companyDto.Address,
                isActive = companyDto.IsActive
            };
            var content = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}companies/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<(bool Success, string Message)> DeleteCompanyAsync(int id)
        {
            AddToken();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}companies/{id}");
            var message = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Company deleted successfully." : (string.IsNullOrEmpty(message) ? "Failed to delete company." : message));
        }
    }
}
