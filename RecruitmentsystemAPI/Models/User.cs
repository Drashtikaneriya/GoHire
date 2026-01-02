using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecruitmentsystemAPI.Models;
using RecruitmentsystemAPI.Models;
using YourProjectName.Models;

namespace RecruitmentsystemAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(150)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Password { get; set; } = string.Empty; 


        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }


       
        //  This is the FOREIGN KEY
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        // Navigation property
        public Role? Role { get; set; }

    

    }
}
