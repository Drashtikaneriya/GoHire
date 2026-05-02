using System.Text;
using System.Text.Json;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Services
{
    public class JobService : BaseService
    {
        public JobService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor) { }

        public async Task<List<JobDTO>> GetJobsAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}jobs");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<JobDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<JobDTO>();
            }
            return new List<JobDTO>();
        }

        public async Task<JobDTO> GetJobByIdAsync(int id)
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}jobs/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<JobDTO>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<List<JobDTO>> GetActiveJobsAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}jobs/active");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<JobDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<JobDTO>();
            }
            return new List<JobDTO>();
        }

        public async Task<(bool Success, string Message)> CreateJobAsync(CreateJobDTO jobDto)
        {
            AddToken();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var content = new StringContent(JsonSerializer.Serialize(jobDto, options), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}jobs", content);
            var result = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Job created successfully" : result);
        }

        public async Task<(bool Success, string Message)> UpdateJobAsync(int id, JobDTO jobDto)
        {
            AddToken();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var updateData = new
            {
                jobId = id, // Added JobId as requested by API validation
                companyId = jobDto.CompanyId,
                title = jobDto.Title,
                description = jobDto.Description,
                location = jobDto.Location,
                employmentType = jobDto.EmploymentType,
                salaryMin = jobDto.SalaryMin,
                salaryMax = jobDto.SalaryMax,
                closingDate = jobDto.ClosingDate,
                createdByUserId = jobDto.CreatedByUserId,
                status = jobDto.Status
            };
            var content = new StringContent(JsonSerializer.Serialize(updateData, options), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}jobs/{id}", content);
            var result = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Job updated successfully" : result);
        }

        public async Task<(bool Success, string Message)> DeleteJobAsync(int id)
        {
            AddToken();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}jobs/{id}");
            var message = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Job deleted successfully." : (string.IsNullOrEmpty(message) ? "Failed to delete job." : message));
        }

        public async Task<bool> ToggleJobStatusAsync(int id)
        {
            AddToken();
            var response = await _httpClient.PatchAsync($"{_baseUrl}jobs/{id}/status", null);
            return response.IsSuccessStatusCode;
        }
    }
}
