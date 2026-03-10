using System.ComponentModel.DataAnnotations;

namespace RecruitmentsystemMVC.Models.DTOs
{
    public class JobPositionCreateDTO
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public string Location { get; set; }
        public string SalaryRange { get; set; } // Matches API Schema
        public string Type { get; set; } // JobType
    }

    public class JobPositionUpdateDTO : JobPositionCreateDTO
    {
        public int JobPositionId { get; set; }
    }
    
    public class JobPositionDTO
    {
         public int JobPositionId { get; set; }
         public string Title { get; set; }
         public string Description { get; set; }
         public int CompanyId { get; set; }
         public string CompanyName { get; set; }
         public string Location { get; set; }
         public string SalaryRange { get; set; }
         public string Type { get; set; }
         public int CreatedBy { get; set; }
         public DateTime CreatedDate { get; set; }
    }
}
