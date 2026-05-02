using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Role
{
    public class RoleCreateDTO
    {
        [Required(ErrorMessage = "RoleName is required")]
        [MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;
    }
}
