using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTOs.Interview
{
    public enum InterviewResult
    {
        Pending,
        Selected,
        Rejected
    }

    public class InterviewUpdateDTO
    {
        [Required]
        public int InterviewId { get; set; }

        public DateTime InterviewDate { get; set; }

        public string? Mode { get; set; }

        public int? RoundNo { get; set; }

        public string? Feedback { get; set; }

        [Required]
        public InterviewResult Result { get; set; }
    }
}
