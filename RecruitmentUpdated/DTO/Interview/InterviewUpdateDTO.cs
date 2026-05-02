using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Interview
{
    public class InterviewUpdateDTO
    {
        [Required]
        public int InterviewId { get; set; }

        public DateTime? InterviewDate { get; set; }

        public DateTime? InterviewEnd { get; set; }

        [MaxLength(50)]
        public string? Mode { get; set; }

        public string? Feedback { get; set; }

        [MaxLength(50)]
        public string? Result { get; set; }

        public int? UpdatedByUserId { get; set; }
    }
}
