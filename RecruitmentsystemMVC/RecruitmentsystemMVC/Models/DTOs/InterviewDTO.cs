using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemMVC.Models.DTOs
{
    public class InterviewCreateDTO
    {
        [Required]
        public int ApplicationId { get; set; }
        [Required]
        public int InterviewerId { get; set; }
        [Required]
        public DateTime InterviewDate { get; set; }
        public string Mode { get; set; } // Online, Offline
        public int RoundNo { get; set; }
        public string Feedback { get; set; }
        public string Result { get; set; } // Pending, Passed, Failed
    }

    public class InterviewDTO : InterviewCreateDTO
    {
        public int InterviewId { get; set; }
        public string CandidateName { get; set; }
        public string JobTitle { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
