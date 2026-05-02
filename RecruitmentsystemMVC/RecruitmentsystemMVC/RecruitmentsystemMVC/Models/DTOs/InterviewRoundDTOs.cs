using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecruitmentsystemMVC.Models.DTOs
{
    public class InterviewRoundDTO
    {
        [JsonPropertyName("roundId")]
        public int Id { get; set; }
        
        [Required]
        [JsonPropertyName("roundName")]
        public string Name { get; set; }
    }

    public class CreateInterviewRoundDTO
    {
        [Required(ErrorMessage = "Round Name is required")]
        [MaxLength(100)]
        public string RoundName { get; set; }
    }
}
