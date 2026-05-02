using Microsoft.AspNetCore.Http;

namespace RecruitmentsystemMVC.Helpers
{
    public class RoleHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetRole()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("UserRole") ?? string.Empty;
        }

        public bool IsAdmin()
        {
            var role = GetRole();
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsHR()
        {
            var role = GetRole();
            return string.Equals(role, "HR Manager", StringComparison.OrdinalIgnoreCase) || 
                   string.Equals(role, "HR_MANAGER", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(role, "HR", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsInterviewer()
        {
            var role = GetRole();
            return string.Equals(role, "Interviewer", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsCandidate()
        {
            var role = GetRole();
            return string.Equals(role, "Candidate", StringComparison.OrdinalIgnoreCase);
        }

        public bool CanManageInterviews()
        {
            return IsAdmin() || IsHR();
        }

        public bool CanManageInterviewRounds()
        {
            return IsAdmin();
        }
        
        public bool CanViewInterviewRounds()
        {
            return IsAdmin() || IsHR();
        }
    }
}
