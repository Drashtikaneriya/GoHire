using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using RecruitmentsystemMVC.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace RecruitmentsystemMVC.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;

        public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = configuration.GetValue<string>("ApiBaseUrl");
        }

        private HttpClient GetClient()
        {
            var client = _httpClientFactory.CreateClient("RecruitmentAPI");
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var client = GetClient();
                // Ensure base URL ends with slash and endpoint doesn't start with one, or handle it gracefully
                var url = $"{_baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
                var response = await client.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (Exception)
            {
                // Log exception
            }
            return default;
        }

        public async Task<ApiResponse> PostAsync<T>(string endpoint, T data)
        {
            try
            {
                var client = GetClient();
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = $"{_baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
                
                var response = await client.PostAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse { IsSuccess = true };
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse { IsSuccess = false, Message = errorContent };
            }
            catch (Exception ex) 
            { 
                return new ApiResponse { IsSuccess = false, Message = "Connection Error: " + ex.Message }; 
            }
        }

        public async Task<ApiResponse> PutAsync<T>(string endpoint, T data)
        {
            try
            {
                var client = GetClient();
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var url = $"{_baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";

                var response = await client.PutAsync(url, content);
                
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse { IsSuccess = true };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse { IsSuccess = false, Message = errorContent };
            }
            catch (Exception ex) 
            { 
                return new ApiResponse { IsSuccess = false, Message = "Connection Error: " + ex.Message }; 
            }
        }

        public async Task<ApiResponse> DeleteAsync(string endpoint)
        {
            try
            {
                var client = GetClient();
                var url = $"{_baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
                var response = await client.DeleteAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse { IsSuccess = true };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse { IsSuccess = false, Message = errorContent };
            }
            catch (Exception ex)
            {
                return new ApiResponse { IsSuccess = false, Message = "Connection Error: " + ex.Message };
            }
        }
    }
}
