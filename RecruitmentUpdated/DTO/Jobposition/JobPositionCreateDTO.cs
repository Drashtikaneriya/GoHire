using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Jobposition
{
    public class JobPositionCreateDTO
    {
        [Required]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(150)]
        public string? Location { get; set; }

        [MaxLength(50)]
        public string? EmploymentType { get; set; }

        public decimal? SalaryMin { get; set; }

        public decimal? SalaryMax { get; set; }

        public DateTime? ClosingDate { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }
    }
}
