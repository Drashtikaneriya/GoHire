using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Application
{
    public class ApplicationCreateDTO
    {
        [Required]
        public int JobId { get; set; }
        public IFormFile? ResumeFile { get; set; }
    }
}