using catch_up_backend.Enums;

namespace catch_up_backend.Dtos
{
    public class PresetDto
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string Name { get; set; }
        public StateEnum State { get; set; }
    }
} 