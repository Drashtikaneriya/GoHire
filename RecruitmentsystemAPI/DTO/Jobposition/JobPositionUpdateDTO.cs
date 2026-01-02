namespace RecruitmentsystemAPI.DTOs.JobPosition
{
    public class JobPositionUpdateDTO
    {
        public int JobId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? Type { get; set; }

        public string? SalaryRange { get; set; }
    }
}
