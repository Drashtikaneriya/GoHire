using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.Models
{
    public class InterviewRound
    {
        [Key]
        public int RoundId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoundName { get; set; } = string.Empty;

        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
    }
}
