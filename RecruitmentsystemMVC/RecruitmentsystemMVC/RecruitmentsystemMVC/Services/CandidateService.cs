using System.Text;
using System.Text.Json;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Services
{
    public class CandidateService : BaseService
    {
        public CandidateService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor) { }

        public async Task<bool> UploadResumeAsync(int candidateId, IFormFile resume)
        {
            AddToken();
            using var content = new MultipartFormDataContent();
            using var stream = resume.OpenReadStream();
            content.Add(new StreamContent(stream), "resume", resume.FileName);

            var response = await _httpClient.PostAsync($"{_baseUrl}candidates/{candidateId}/upload-resume", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<UserDTO>> GetCandidatesAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}users");
            List<UserDTO> candidates = new List<UserDTO>();
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var allUsers = JsonSerializer.Deserialize<List<UserDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UserDTO>();
                // Filter users who have the role 'Candidate'
                candidates = allUsers.Where(u => u.Role == "Candidate" || u.Role == "CANDIDATE").ToList();
            }
            
            return candidates;
        }

        public async Task<UserDTO> GetProfileAsync(int userId)
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDTO>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<UserDTO> GetMyProfileAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}candidates/me");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var candidateDto = JsonSerializer.Deserialize<UserDTO>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (candidateDto != null)
                {
                    candidateDto.Role = "Candidate";
                    candidateDto.IsActive = true;
                }
                return candidateDto;
            }
            return null;
        }

        public async Task<(bool Success, string Message)> CreateCandidateAsync(CreateUserDTO candidateDto)
        {
            AddToken();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var content = new StringContent(JsonSerializer.Serialize(candidateDto, options), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}users", content);
            var result = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Candidate added successfully" : result);
        }

        public async Task<(bool Success, string Message)> UpdateCandidateAsync(int id, UserDTO candidateDto)
        {
            AddToken();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var updateData = new
            {
                fullName = candidateDto.FullName,
                email = candidateDto.Email,
                phone = candidateDto.Phone,
                role = "Candidate",
                isActive = candidateDto.IsActive
            };
            var content = new StringContent(JsonSerializer.Serialize(updateData, options), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}users/{id}", content);
            var result = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Candidate updated successfully" : result);
        }

        public async Task<(bool Success, string Message)> DeleteCandidateAsync(int id)
        {
            AddToken();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}users/{id}");
            var message = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Candidate deleted successfully." : (string.IsNullOrEmpty(message) ? "Failed to delete candidate." : message));
        }

        public async Task<bool> ToggleCandidateStatusAsync(int id)
        {
            AddToken();
            var response = await _httpClient.PatchAsync($"{_baseUrl}users/{id}/status", null);
            return response.IsSuccessStatusCode;
        }
    }
}
