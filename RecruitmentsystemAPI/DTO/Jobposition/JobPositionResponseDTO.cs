namespace RecruitmentsystemAPI.DTOs.JobPosition
{
    public class JobPositionResponseDTO
    {
        public int JobId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? Type { get; set; }

        public string? SalaryRange { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
