using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemMVC.Models.DTOs
{
    public class JobDTO
    {
        [System.Text.Json.Serialization.JsonPropertyName("jobId")]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Requirements { get; set; } // Keep if still useful on frontend
        [Required]
        public string Location { get; set; }
        [Required]
        public string EmploymentType { get; set; }
        [Required]
        public decimal SalaryMin { get; set; }
        [Required]
        public decimal SalaryMax { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public int CreatedByUserId { get; set; }
    }

    public class CreateJobDTO
    {
        [Required(ErrorMessage = "Job title is required"), StringLength(100)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please provide a job description")]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string EmploymentType { get; set; }
        [Required]
        public decimal SalaryMin { get; set; }
        [Required]
        public decimal SalaryMax { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public DateTime ClosingDate { get; set; } = DateTime.Now.AddDays(30);
        public int CreatedByUserId { get; set; }
    }
}
