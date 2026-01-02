using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Role
{
    public class RoleUpdateDTO
    {
        [Required(ErrorMessage = "RoleId is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "RoleName is required")]
        [MaxLength(50, ErrorMessage = "RoleName max 50 characters")]
        public string RoleName { get; set; } = string.Empty;
    }
}
