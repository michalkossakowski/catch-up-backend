using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace catch_up_backend.Models
{
    public class UserProfileModel
    {
        [Key]
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        public string Bio { get; set; } = string.Empty;
        
        public string Department { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        
        public string Phone { get; set; } = string.Empty;
        public string TeamsUsername { get; set; } = string.Empty;
        public string SlackUsername { get; set; } = string.Empty;
        
        public List<string> Interests { get; set; } = new List<string>();
        public List<string> Languages { get; set; } = new List<string>();
        
        public string InterestsJson
        {
            get => JsonSerializer.Serialize(Interests);
            set => Interests = string.IsNullOrEmpty(value) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(value) ?? new List<string>();
        }
        
        public string LanguagesJson
        {
            get => JsonSerializer.Serialize(Languages);
            set => Languages = string.IsNullOrEmpty(value) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(value) ?? new List<string>();
        }
        
        public virtual UserModel User { get; set; }
    }
} 