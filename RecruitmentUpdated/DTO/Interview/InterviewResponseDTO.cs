namespace RecruitmentsystemAPI.DTO.Interview
{
    public class InterviewResponseDTO
    {
        public int InterviewId { get; set; }
        public int ApplicationId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public int RoundId { get; set; }
        public string RoundName { get; set; } = string.Empty;
        public int InterviewerUserId { get; set; }
        public string InterviewerName { get; set; } = string.Empty;
        public DateTime InterviewDate { get; set; }
        public DateTime? InterviewEnd { get; set; }
        public string? Mode { get; set; }
        public string? Feedback { get; set; }
        public string? Result { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
