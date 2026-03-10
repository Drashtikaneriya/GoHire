using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemMVC.Models
{
    public class CompanyModel
    {
        public int CompanyId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Industry { get; set; }

        public string? Address { get; set; }

        public string? Website { get; set; }

        public int CreatedBy { get; set; }

        public bool IsActive { get; set; }
    }
}
