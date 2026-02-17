using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemMVC.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? Password { get; set; }

        [Required]
        public int RoleId { get; set; }

        public string? RoleName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
