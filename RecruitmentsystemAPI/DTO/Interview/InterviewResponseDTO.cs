namespace RecruitmentsystemAPI.DTOs.Interview
{
    public class InterviewResponseDTO
    {
        public int InterviewId { get; set; }

        public int ApplicationId { get; set; }

        public int InterviewerId { get; set; }

        public DateTime InterviewDate { get; set; }

        public string? Mode { get; set; }

        public int? RoundNo { get; set; }

        public string? Feedback { get; set; }

        public string? Result { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
