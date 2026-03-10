namespace RecruitmentsystemMVC.Models.DTOs
{
    public class ApplicationResponseDTO
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public string? JobTitle { get; set; }
        public int CandidateId { get; set; }
        public string? CandidateName { get; set; }
        public string Status { get; set; } = "Applied";
        public string? HRNotes { get; set; }
        public DateTime AppliedOn { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class ApplicationUpdateDTO
    {
        public int ApplicationId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? HRNotes { get; set; }
    }
}
