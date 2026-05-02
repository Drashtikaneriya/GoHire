using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentsystemAPI.Models
{
    public class Interview
    {
        [Key]
        public int InterviewId { get; set; }

        [ForeignKey("Application")]
        public int ApplicationId { get; set; }
        public Application? Application { get; set; }

        [ForeignKey("Round")]
        public int RoundId { get; set; }
        public InterviewRound? Round { get; set; }

        [ForeignKey("Interviewer")]
        public int InterviewerUserId { get; set; }
        public User? Interviewer { get; set; }

        [Required]
        public DateTime InterviewDate { get; set; }

        public DateTime? InterviewEnd { get; set; }

        [MaxLength(50)]
        public string? Mode { get; set; }

        public string? Feedback { get; set; }

        [MaxLength(50)]
        public string? Result { get; set; }

        [ForeignKey("UpdatedByUser")]
        public int? UpdatedByUserId { get; set; }
        public User? UpdatedByUser { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }
    }
}
