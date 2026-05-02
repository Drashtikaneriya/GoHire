using Microsoft.AspNetCore.Http;

namespace RecruitmentsystemAPI.DTO.Candidate
{
    public class CandidateCreateDTO
    {
        public string? LinkedInUrl { get; set; }
        public string? PortfolioUrl { get; set; }
        public IFormFile? DefaultResumeFile { get; set; }
    }
}
