using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.User
{
    public class UserCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? Phone { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
