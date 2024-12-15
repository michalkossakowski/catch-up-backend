using catch_up_backend.Enums;

namespace catch_up_backend.Dtos
{
    public class TaskPresetDto
    {
        public int PresetId { get; set; }
        public int TaskContentId { get; set; }
        public StateEnum State { get; set; }
    }
} 