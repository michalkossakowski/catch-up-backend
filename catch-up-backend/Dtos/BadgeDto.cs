using catch_up_backend.Enums;

namespace catch_up_backend.Dtos
{
    public class BadgeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? IconId { get; set; }
        public int? Count { get; set; }
        public BadgeTypeCountEnum? CountType { get; set; }
    }
}
