using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTOs
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(150)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "RoleId is required")]
        public int RoleId { get; set; }
    }
}
