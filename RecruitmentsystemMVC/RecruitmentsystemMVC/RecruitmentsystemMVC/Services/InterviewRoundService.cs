using System.Text;
using System.Text.Json;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Services
{
    public class InterviewRoundService : BaseService
    {
        public InterviewRoundService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor) { }

        public async Task<List<InterviewRoundDTO>> GetInterviewRoundsAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}interview-rounds");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<InterviewRoundDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<InterviewRoundDTO>();
            }
            return new List<InterviewRoundDTO>();
        }

        public async Task<InterviewRoundDTO> GetInterviewRoundByIdAsync(int id)
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}interview-rounds/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InterviewRoundDTO>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<bool> CreateInterviewRoundAsync(CreateInterviewRoundDTO roundDto)
        {
            AddToken();
            var content = new StringContent(JsonSerializer.Serialize(roundDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}interview-rounds", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateInterviewRoundAsync(int id, CreateInterviewRoundDTO roundDto)
        {
            AddToken();
            var content = new StringContent(JsonSerializer.Serialize(roundDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}interview-rounds/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteInterviewRoundAsync(int id)
        {
            AddToken();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}interview-rounds/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
