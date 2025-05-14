using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Dtos
{
    public class UserProfileDto
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        
        public string? Bio { get; set; }
        
        public string? Department { get; set; }
        public string? Location { get; set; }
        
        public string? Phone { get; set; }
        public string? TeamsUsername { get; set; }
        public string? SlackUsername { get; set; }
        
        public List<string>? Interests { get; set; }
        public List<string>? Languages { get; set; }
    }
} 