using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentsystemAPI.Models
{
    public class JobPosition
    {
        [Key]
        public int JobId { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }

        [MaxLength(50)]
        public string? EmploymentType { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? SalaryMin { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? SalaryMax { get; set; }

        public DateTime? ClosingDate { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("CreatedByUser")]
        public int CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
