namespace RecruitmentsystemAPI.DTOs.Interview
{
    public class InterviewUpdateDTO
    {
        public int InterviewId { get; set; }

        public DateTime InterviewDate { get; set; }

        public string? Mode { get; set; }

        public int? RoundNo { get; set; }

        public string? Feedback { get; set; }

        public string? Result { get; set; }   // Pending / Selected / Rejected
    }
}
