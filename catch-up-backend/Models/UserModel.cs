    using catch_up_backend.Enums;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Models
{
    public class UserModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
        public string? Position { get; set; }
        public int? AvatarId { get; set; }
        public Dictionary<BadgeTypeCountEnum, int>? Counters { get; set; }
        public StateEnum? State { get; set; }

        public UserModel(string name, string surname, string email, string password, string type, string? position = null)
        {
            this.Name = name ?? "";
            this.Surname = surname ?? "";
            this.Email = email;
            this.Password = password;
            this.Type = type;
            this.Position = position ?? string.Empty;
            this.AvatarId = null;
            State = StateEnum.Active;
            Counters = InitializeCounters();
        }

        public static Dictionary<BadgeTypeCountEnum, int> InitializeCounters()
        {
            return Enum.GetValues(typeof(BadgeTypeCountEnum))
                       .Cast<BadgeTypeCountEnum>()
                       .ToDictionary(enumValue => enumValue, enumValue => 0);
        }
    }
}
