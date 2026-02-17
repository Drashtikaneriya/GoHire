using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RecruitmentsystemMVC.Models
{
    public class JobPositionModel
    {
        [JsonProperty("JobId")]
        public int JobPositionId { get; set; }

        [Required]
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Description")]
        public string? Description { get; set; }

        [Required]
        [JsonProperty("CompanyId")]
        public int CompanyId { get; set; }

        [JsonProperty("CompanyName")]
        public string? CompanyName { get; set; }

        [JsonProperty("Location")]
        public string? Location { get; set; }

        [JsonProperty("SalaryRange")]
        public string? Salary { get; set; }

        [JsonProperty("Type")]
        public string? JobType { get; set; } // Full-time, Part-time, etc.

        [JsonProperty("CreatedBy")]
        public int CreatedBy { get; set; }
    }
}
