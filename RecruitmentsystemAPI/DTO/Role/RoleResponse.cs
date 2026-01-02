namespace RecruitmentsystemAPI.DTO.Role
{
    public class RoleResponse
    {

        public int RoleId { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
