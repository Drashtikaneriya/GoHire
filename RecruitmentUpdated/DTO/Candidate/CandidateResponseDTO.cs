using System;

namespace RecruitmentsystemAPI.DTO.Candidate
{
    public class CandidateResponseDTO
    {
        public int CandidateId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? PortfolioUrl { get; set; }
        public string? DefaultResumePath { get; set; }
        public DateTime Created { get; set; }
    }
}
