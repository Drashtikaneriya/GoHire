namespace RecruitmentsystemAPI.DTOs.Candidate
{
    public class CandidateUpdateDTO
    {
        public int CandidateId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? ResumePath { get; set; }
        // For updated file
        public IFormFile? documentFile { get; set; }
    }
}
