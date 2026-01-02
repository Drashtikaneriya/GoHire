namespace RecruitmentsystemAPI.DTOs.Interview
{
    public class InterviewCreateDTO
    {
        public int ApplicationId { get; set; }

        public int InterviewerId { get; set; }

        public DateTime InterviewDate { get; set; }

        public string? Mode { get; set; }   // Online / Offline

        public int? RoundNo { get; set; }

        public string? Feedback { get; set; }
    }
}
