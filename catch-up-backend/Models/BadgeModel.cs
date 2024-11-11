using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Models
{
    public class BadgeModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconSource { get; set; }

        public BadgeModel(string name, string description, string iconSource)
        {
            Name = name;
            Description = description;
            IconSource = iconSource;
        }
    }
}
