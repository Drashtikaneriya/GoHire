using System;

namespace RecruitmentsystemAPI.DTO.Application
{
    public class ApplicationResponseDTO
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int CandidateId { get; set; }
        public string CandidateFullName { get; set; } = string.Empty;
        public string? ResumePath { get; set; }
        public string Status { get; set; } = "Applied";
        public string? HRNotes { get; set; }
        public DateTime AppliedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
