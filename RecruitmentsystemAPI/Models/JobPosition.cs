using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentsystemAPI.Models
{
    public class JobPosition
    {

        [Key]
        public int JobId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; }   // Full-time, Part-time, Remote

        [MaxLength(100)]
        public string? SalaryRange { get; set; }
        // 👇 FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
        [Required]
        [ForeignKey("User")]
        public int CreatedBy { get; set; }

        // Navigation property
        public User? User { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }
    }

}
