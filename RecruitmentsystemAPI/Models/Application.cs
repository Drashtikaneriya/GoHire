using RecruitmentsystemAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Application
{
    [Key]
    public int ApplicationId { get; set; }

    [Required]
    public int JobId { get; set; }

    [Required]
    public int CandidateId { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Applied";

    public string? HRNotes { get; set; }

    public DateTime AppliedOn { get; set; } = DateTime.Now;

    public DateTime? ModifiedDate { get; set; }

    // 🔗 Navigation Properties (Optional but Recommended)
    [ForeignKey("JobId")]
    public JobPosition? JobPosition { get; set; }

    [ForeignKey("CandidateId")]
    public Candidate? Candidate { get; set; }

    public class ApplicationInsertDTO
    {
        [Required]
        public int JobId { get; set; }

        [Required]
        public int CandidateId { get; set; }

        // Optional – default will be "Applied" if not provided
        [MaxLength(50)]
        public string? Status { get; set; }

        // Optional – HR can add later
        public string? HRNotes { get; set; }
    }
}
