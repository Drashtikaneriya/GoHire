using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTOs.Application
{
    public class ApplicationCreateDTO
    {
        [Required]
        public int JobId { get; set; }

        [Required]
        public int CandidateId { get; set; }

        // Optional – default handled in controller or DB
        [MaxLength(50)]
        public string? Status { get; set; }

        // Optional – usually empty at apply time
        public string? HRNotes { get; set; }
    }
}
