using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTOs
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(150)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        // Optional: password update hoy to j moklo
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        public int RoleId { get; set; }
    }
}
