using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentsystemAPI.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        [ForeignKey("JobPosition")]
        public int JobId { get; set; }
        public JobPosition? JobPosition { get; set; }

        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public Candidate? Candidate { get; set; }

        [MaxLength(500)]
        public string? ResumePath { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Applied";

        public string? HRNotes { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
    }
}
