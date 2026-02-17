using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Services
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<ApiResponse> PostAsync<T>(string endpoint, T data);
        Task<ApiResponse> PutAsync<T>(string endpoint, T data);
        Task<ApiResponse> DeleteAsync(string endpoint);
    }
}
