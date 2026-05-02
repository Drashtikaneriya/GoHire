using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.InterviewRound
{
    public class InterviewRoundCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string RoundName { get; set; } = string.Empty;
    }
}
