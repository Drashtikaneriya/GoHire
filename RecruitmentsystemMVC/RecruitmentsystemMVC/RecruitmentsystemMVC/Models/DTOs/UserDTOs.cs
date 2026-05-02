using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecruitmentsystemMVC.Models.DTOs
{
    public class UserDTO
    {
        [JsonPropertyName("userId")]
        public int Id { get; set; }
        
        [Required]
        public string? FullName { get; set; }
        
        [Required, EmailAddress]
        public string? Email { get; set; }
        
        public string? Phone { get; set; }
        
        [JsonPropertyName("roleName")]
        public string? Role { get; set; }
        
        [JsonPropertyName("roleId")]
        public int RoleId { get; set; }
        
        public bool IsActive { get; set; }
    }

    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }
        
        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required, MinLength(6)]
        public string Password { get; set; }
        
        public string Phone { get; set; }
        
        [Required]
        public int RoleId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
