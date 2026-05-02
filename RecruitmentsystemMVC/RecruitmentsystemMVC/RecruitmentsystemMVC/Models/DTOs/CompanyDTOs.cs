using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecruitmentsystemMVC.Models.DTOs
{
    public class CompanyDTO
    {
        [JsonPropertyName("companyId")]
        public int CompanyId { get; set; }
        
        [Required, StringLength(100)]
        [JsonPropertyName("companyName")]
        public string? CompanyName { get; set; }
        
        [Required]
        [JsonPropertyName("industry")]
        public string? Industry { get; set; }
        
        [Required, EmailAddress]
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        
        [JsonPropertyName("website")]
        public string? Website { get; set; }
        
        [JsonPropertyName("address")]
        public string? Address { get; set; }
        
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }

    public class CreateCompanyDTO
    {
        [Required(ErrorMessage = "Company Name is required")]
        public string? CompanyName { get; set; }
        [Required]
        public string? Industry { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
    }
}
