namespace RecruitmentsystemAPI.DTOs.Company
{
    public class CompanyResponseDTO
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string? Industry { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
