using Microsoft.AspNetCore.Http;

namespace RecruitmentsystemAPI.DTO.Candidate
{
    public class CandidateUpdateDTO
    {
        public string? LinkedInUrl { get; set; }
        public string? PortfolioUrl { get; set; }
        public IFormFile? DefaultResumeFile { get; set; }
    }
}
