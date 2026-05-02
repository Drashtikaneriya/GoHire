using System.Text;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Services
{
    public class ApplicationService : BaseService
    {
        public ApplicationService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor) { }

        public async Task<List<ApplicationDTO>> GetApplicationsAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}applications");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ApplicationDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ApplicationDTO>();
            }
            return new List<ApplicationDTO>();
        }

        public async Task<List<ApplicationDTO>> GetMyApplicationsAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}applications/my");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ApplicationDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ApplicationDTO>();
            }
            return new List<ApplicationDTO>();
        }

        public async Task<ApplicationDTO> GetApplicationByIdAsync(int id)
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}applications/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApplicationDTO>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<(bool Success, string Message)> ApplyAsync(int jobId, int candidateId, IFormFile resumeFile)
        {
            AddToken();
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(jobId.ToString()), "jobId");
            content.Add(new StringContent(candidateId.ToString()), "candidateId");

            if (resumeFile != null && resumeFile.Length > 0)
            {
                var stream = resumeFile.OpenReadStream();
                var fileContent = new StreamContent(stream);
                content.Add(fileContent, "resumeFile", resumeFile.FileName);
            }

            var response = await _httpClient.PostAsync($"{_baseUrl}applications", content);
            var result = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                return (true, "Application submitted successfully.");
            }
            
            try 
            {
                var errorObj = JsonSerializer.Deserialize<JsonElement>(result);
                
                // Handle "errors" object (validation errors)
                if (errorObj.TryGetProperty("errors", out var errors))
                {
                    var errorMessages = new List<string>();
                    foreach (var prop in errors.EnumerateObject())
                    {
                        foreach (var m in prop.Value.EnumerateArray())
                        {
                            errorMessages.Add(m.GetString());
                        }
                    }
                    if (errorMessages.Any()) return (false, string.Join(" ", errorMessages));
                }

                // Handle "message" property
                if (errorObj.TryGetProperty("message", out var messageProp))
                {
                    return (false, messageProp.GetString() ?? "Failed to submit application.");
                }
            }
            catch {}

            return (false, string.IsNullOrEmpty(result) ? "Failed to submit application." : result);
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            AddToken();
            var response = await _httpClient.PatchAsync($"{_baseUrl}applications/{id}/status", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ShortlistAsync(int id)
        {
            AddToken();
            var response = await _httpClient.PatchAsync($"{_baseUrl}applications/{id}/shortlist", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RejectAsync(int id)
        {
            AddToken();
            var response = await _httpClient.PatchAsync($"{_baseUrl}applications/{id}/reject", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<(bool Success, string Message)> DeleteApplicationAsync(int id)
        {
            AddToken();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}applications/{id}");
            var message = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Application deleted successfully." : (string.IsNullOrEmpty(message) ? "Failed to delete application." : message));
        }
    }
}
