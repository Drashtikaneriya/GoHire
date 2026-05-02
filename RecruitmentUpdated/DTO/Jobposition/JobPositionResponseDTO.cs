namespace RecruitmentsystemAPI.DTO.Jobposition
{
    public class JobPositionResponseDTO
    {
        public int JobId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? EmploymentType { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public DateTime? ClosingDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByFullName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
