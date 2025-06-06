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
        public int? IconId { get; set; }
        public int? Count { get; set; }
        public BadgeTypeCountEnum? CountType { get; set; }
        public StateEnum State { get; set; }
        public BadgeModel(string name, string description, int? iconId, int? count, BadgeTypeCountEnum? countType)
        {
            Name = name;
            Description = description;
            IconId = iconId;
            Count = count;
            CountType = countType;
            State = StateEnum.Active;
        }
    }
}
