using System;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateId { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(300)]
        public string? ResumePath { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.Now;

     //   public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

      
    }
}
