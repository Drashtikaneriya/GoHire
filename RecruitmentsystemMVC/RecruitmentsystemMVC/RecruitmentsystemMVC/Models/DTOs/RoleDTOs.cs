using System.Text.Json.Serialization;

namespace RecruitmentsystemMVC.Models.DTOs
{
    public class RoleDTO
    {
        [JsonPropertyName("roleId")]
        public int Id { get; set; }

        [JsonPropertyName("roleName")]
        public string Name { get; set; }
    }
}
