using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.User
{
    public class UserUpdateDTO
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? Phone { get; set; }

        [Required]
        public int RoleId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
