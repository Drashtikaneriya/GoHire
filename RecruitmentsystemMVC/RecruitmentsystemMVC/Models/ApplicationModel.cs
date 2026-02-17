using System;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemMVC.Models
{
    public class ApplicationModel
    {
        // ===== COMMON =====
        public int ApplicationId { get; set; }

        // ===== APPLY =====
        [Required]
        public int JobId { get; set; }

        [Required]
        public int CandidateId { get; set; }

        // ===== DISPLAY =====
        public string? JobTitle { get; set; }
        public string? CandidateName { get; set; }

        // ===== UPDATE =====
        public string? Status { get; set; }
        public string? HRNotes { get; set; }

        public DateTime AppliedOn { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
