using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.DTO.Role
{
    public class RoleUpdateDTO
    {
        [Required(ErrorMessage = "RoleName is required")]
        [MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;
    }
}
