using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTOs.Application
{
    public class ApplicationUpdateDTO
    {
        [Required]
        public int ApplicationId { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        public string? HRNotes { get; set; }
    }
}
