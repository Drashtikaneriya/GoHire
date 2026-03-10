using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTOs.Application
{
    public class ApplicationUpdateDTO
    {
        public int ApplicationId { get; set; }   // optional – used for mismatch check

        [MaxLength(50)]
        public string? Status { get; set; }

        public string? HRNotes { get; set; }

        public DateTime? ModifiedDate { get; set; } // set by server; client may send null
    }
}
