using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemAPI.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime? Modified { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
