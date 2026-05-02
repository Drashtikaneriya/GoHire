using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Interview
{
    public class InterviewFeedbackDTO
    {
        [Required]
        public string Feedback { get; set; } = string.Empty;
    }

    public class InterviewResultDTO
    {
        [Required]
        [MaxLength(50)]
        public string Result { get; set; } = string.Empty;
    }
}