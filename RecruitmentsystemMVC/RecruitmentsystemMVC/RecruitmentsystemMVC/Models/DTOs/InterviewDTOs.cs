using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecruitmentsystemMVC.Models.DTOs
{
    public class InterviewDTO
    {
        [JsonPropertyName("interviewId")]
        public int Id { get; set; }

        [JsonPropertyName("applicationId")]
        public int ApplicationId { get; set; }

        [JsonPropertyName("candidateName")]
        public string CandidateName { get; set; }

        public int CandidateId { get; set; }

        [JsonPropertyName("jobTitle")]
        public string JobTitle { get; set; }

        [JsonPropertyName("interviewerUserId")]
        public int InterviewerUserId { get; set; }

        [JsonPropertyName("interviewerName")]
        public string InterviewerName { get; set; }

        [JsonPropertyName("interviewDate")]
        public DateTime InterviewDate { get; set; }

        [JsonPropertyName("interviewEnd")]
        public DateTime? InterviewEnd { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; }

        public string Status { get; set; }

        [JsonPropertyName("feedback")]
        public string Feedback { get; set; }

        [JsonPropertyName("result")]
        public string Result { get; set; }

        [JsonPropertyName("roundName")]
        public string RoundName { get; set; }

        [JsonPropertyName("roundId")]
        public int RoundId { get; set; }
    }

    public class ScheduleInterviewDTO
    {
        [Required]
        [JsonPropertyName("applicationId")]
        public int ApplicationId { get; set; }

        [Required]
        [JsonPropertyName("roundId")]
        public int RoundId { get; set; }

        [Required]
        [JsonPropertyName("interviewerUserId")]
        public int InterviewerUserId { get; set; }

        [Required]
        [JsonPropertyName("interviewDate")]
        public DateTime InterviewDate { get; set; }

        [JsonPropertyName("interviewEnd")]
        public DateTime? InterviewEnd { get; set; }

        [MaxLength(50)]
        [JsonPropertyName("mode")]
        public string? Mode { get; set; }
    }

    public class FeedbackDTO
    {
        [JsonPropertyName("feedback")]
        public string Feedback { get; set; }
        
        [JsonPropertyName("result")]
        public string Result { get; set; }
    }

    public class InterviewResultDTO
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }
    }
}
