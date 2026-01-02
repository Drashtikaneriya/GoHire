using System;

namespace RecruitmentsystemAPI.DTOs.Application
{
    public class ApplicationResponseDTO
    {
        public int ApplicationId { get; set; }

        public int JobId { get; set; }
        public string? JobTitle { get; set; }

        public int CandidateId { get; set; }
        public string? CandidateName { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? HRNotes { get; set; }

        public DateTime AppliedOn { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
