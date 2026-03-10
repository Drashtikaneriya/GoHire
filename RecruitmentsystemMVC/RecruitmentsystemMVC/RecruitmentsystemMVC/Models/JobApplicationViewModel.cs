using Microsoft.AspNetCore.Http;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Models
{
    public class JobApplicationViewModel
    {
        public int JobId { get; set; }
        public JobPositionDTO Job { get; set; }
        public IFormFile Resume { get; set; }
    }
}
