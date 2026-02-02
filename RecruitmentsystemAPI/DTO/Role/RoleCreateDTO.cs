using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Role
{
    public class RoleCreateDTO
    {
        [Required(ErrorMessage = "RoleName is required")]
        [MaxLength(50, ErrorMessage = "RoleName max 50 characters")]
        public string RoleName { get; set; } = string.Empty;
    }
}
    