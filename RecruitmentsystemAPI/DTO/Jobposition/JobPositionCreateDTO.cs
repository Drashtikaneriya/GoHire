namespace RecruitmentsystemAPI.DTOs.JobPosition
{
    public class JobPositionCreateDTO
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? Type { get; set; }   // Full-time, Part-time, Remote

        public string? SalaryRange { get; set; }

        public int CreatedBy { get; set; }   // UserId
    }
}
