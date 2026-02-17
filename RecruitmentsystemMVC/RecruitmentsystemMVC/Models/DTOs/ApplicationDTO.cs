namespace RecruitmentsystemMVC.Models.DTOs
{
    public class ApplicationDTO
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string ResumeUrl { get; set; }
        public string Status { get; set; } // Applied, Shortlisted, Rejected
        public string HRNotes { get; set; }
        public DateTime AppliedDate { get; set; }
    }

    public class ApplicationUpdateStatusDTO
    {
        public int ApplicationId { get; set; }
        public string Status { get; set; }
        public string HRNotes { get; set; }
    }
}
