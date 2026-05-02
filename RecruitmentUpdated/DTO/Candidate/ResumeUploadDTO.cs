using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Candidate
{
    public class ResumeUploadDTO
    {
        [Required]
        public IFormFile Resume { get; set; } = null!;
    }
}