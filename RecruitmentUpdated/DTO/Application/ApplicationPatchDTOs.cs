using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Application
{
    public class ApplicationStatusDTO
    {
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        public string? HRNotes { get; set; }
    }

    public class HRNotesDTO
    {
        public string? HRNotes { get; set; }
    }
}