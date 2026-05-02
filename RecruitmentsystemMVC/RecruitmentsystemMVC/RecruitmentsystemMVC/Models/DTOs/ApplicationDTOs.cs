using System;
using System.Text.Json.Serialization;

namespace RecruitmentsystemMVC.Models.DTOs
{
    public class ApplicationDTO
    {
        [JsonPropertyName("applicationId")]
        public int Id { get; set; }

        [JsonPropertyName("jobId")]
        public int JobId { get; set; }

        [JsonPropertyName("jobTitle")]
        public string JobTitle { get; set; }

        [JsonPropertyName("candidateId")]
        public int CandidateId { get; set; }

        [JsonPropertyName("candidateFullName")]
        public string CandidateName { get; set; }

        // Not returned by API, kept for backward compatibility
        public string CandidateEmail { get; set; }

        [JsonPropertyName("resumePath")]
        public string ResumeUrl { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("hrNotes")]
        public string HrNotes { get; set; }

        [JsonPropertyName("appliedDate")]
        public DateTime AppliedDate { get; set; }

        [JsonPropertyName("modifiedDate")]
        public DateTime? ModifiedDate { get; set; }
    }

    public class CreateApplicationDTO
    {
        public int JobId { get; set; }
        public int CandidateId { get; set; }
        public string ResumeUrl { get; set; }
    }
}
