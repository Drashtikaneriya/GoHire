using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTOs.Candidate
{
    public class CandidateCreateDTO
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? ResumePath { get; set; }


        public IFormFile? DocumentFile { get; set; } // For new file

    }
}
