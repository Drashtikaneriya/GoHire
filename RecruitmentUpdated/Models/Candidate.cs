using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentsystemAPI.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [MaxLength(200)]
        public string? LinkedInUrl { get; set; }

        [MaxLength(200)]
        public string? PortfolioUrl { get; set; }

        [MaxLength(500)]
        public string? DefaultResumePath { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
