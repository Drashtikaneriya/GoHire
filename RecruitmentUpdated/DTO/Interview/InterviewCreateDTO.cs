using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Interview
{
    public class InterviewCreateDTO
    {
        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public int RoundId { get; set; }

        [Required]
        public int InterviewerUserId { get; set; }

        [Required]
        public DateTime InterviewDate { get; set; }

        public DateTime? InterviewEnd { get; set; }

        [MaxLength(50)]
        public string? Mode { get; set; }
    }
}
