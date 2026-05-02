using System;
using System.Text.Json.Serialization;

namespace RecruitmentsystemMVC.Models.DTOs
{
    public class InterviewModel
    {
        [JsonPropertyName("applicationId")]
        public int ApplicationId { get; set; }

        [JsonPropertyName("roundId")]
        public int RoundId { get; set; }

        [JsonPropertyName("interviewerUserId")]
        public int InterviewerUserId { get; set; }

        [JsonPropertyName("interviewDate")]
        public DateTime InterviewDate { get; set; }

        [JsonPropertyName("interviewEnd")]
        public DateTime? InterviewEnd { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; } = string.Empty;

        // Suggested additional fields for better UI
        [JsonPropertyName("jobTitle")]
        public string JobTitle { get; set; } = string.Empty;

        [JsonPropertyName("interviewerName")]
        public string InterviewerName { get; set; } = string.Empty;

        [JsonPropertyName("roundName")]
        public string RoundName { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public string Result { get; set; } = "Pending";
    }
}
