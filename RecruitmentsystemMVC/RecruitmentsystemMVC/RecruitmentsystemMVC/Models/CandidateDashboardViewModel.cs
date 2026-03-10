using System.Collections.Generic;
using RecruitmentsystemMVC.Models.DTOs;

namespace RecruitmentsystemMVC.Models
{
    public class CandidateDashboardViewModel
    {
        public int TotalJobs { get; set; }
        public int AppliedJobs { get; set; }
        public int UpcomingInterviews { get; set; }
        public List<JobPositionDTO> RecommendedJobs { get; set; }
        public List<ApplicationResponseDTO> RecentApplications { get; set; }
    }
}
