using System.Text;
using System.Text.Json;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Services
{
    public class InterviewService : BaseService
    {
        public InterviewService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor) { }

        public async Task<List<InterviewDTO>> GetInterviewsAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}interviews");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<InterviewDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<InterviewDTO>();
            }
            return new List<InterviewDTO>();
        }

        public async Task<InterviewDTO> GetInterviewByIdAsync(int id)
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}interviews/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InterviewDTO>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<(bool Success, string Message)> DeleteInterviewAsync(int id)
        {
            AddToken();
            var response = await _httpClient.DeleteAsync($"{_baseUrl}interviews/{id}");
            var message = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? "Interview deleted successfully." : (string.IsNullOrEmpty(message) ? "Failed to delete interview." : message));
        }

        public async Task<List<InterviewDTO>> GetInterviewerInterviewsAsync(int userId)
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}interviews/interviewer/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<InterviewDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<InterviewDTO>();
            }
            return new List<InterviewDTO>();
        }

        public async Task<List<InterviewDTO>> GetCandidateInterviewsAsync(int candidateId)
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}interviews/candidate/{candidateId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<InterviewDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<InterviewDTO>();
            }
            return new List<InterviewDTO>();
        }

        public async Task<List<InterviewDTO>> GetInterviewsByApplicationAsync(int applicationId)
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}interviews/application/{applicationId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<InterviewDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<InterviewDTO>();
            }
            return new List<InterviewDTO>();
        }

        public async Task<List<InterviewDTO>> GetUpcomingInterviewsAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}interviews/upcoming");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<InterviewDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<InterviewDTO>();
        }

        public async Task<bool> ScheduleInterviewAsync(ScheduleInterviewDTO interviewDto)
        {
            AddToken();
            var content = new StringContent(JsonSerializer.Serialize(interviewDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}interviews", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddFeedbackAsync(int id, FeedbackDTO feedbackDto)
        {
            AddToken();
            var content = new StringContent(JsonSerializer.Serialize(feedbackDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{_baseUrl}interviews/{id}/feedback", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SubmitResultAsync(int id, string result)
        {
            AddToken();
            var body = new StringContent(
                JsonSerializer.Serialize(new { result }),
                System.Text.Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PatchAsync($"{_baseUrl}interviews/{id}/result", body);
            return response.IsSuccessStatusCode;
        }
        public async Task<List<InterviewDTO>> GetMyInterviewsAsync()
        {
            AddToken();
            var response = await _httpClient.GetAsync($"{_baseUrl}interviews/my");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<InterviewDTO>>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<InterviewDTO>();
            }
            return new List<InterviewDTO>();
        }
    }
}
