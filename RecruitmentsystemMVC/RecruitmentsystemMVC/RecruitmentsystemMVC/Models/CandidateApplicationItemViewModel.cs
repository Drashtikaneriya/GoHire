using System;

namespace RecruitmentsystemMVC.Models
{
    public class CandidateApplicationItemViewModel
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string Status { get; set; }
        public DateTime AppliedOn { get; set; }
        public string CandidateName { get; set; }
        public string HRNotes { get; set; }
    }
}
