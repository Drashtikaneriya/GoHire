using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Company
{
    public class CompanyCreateDTO
    {
        [Required]
        [MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Industry { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(200)]
        public string? Website { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }
    }
}
