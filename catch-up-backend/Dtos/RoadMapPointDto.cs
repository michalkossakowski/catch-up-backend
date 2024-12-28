using catch_up_backend.Enums;

namespace catch_up_backend.Dtos
{
    public class RoadMapPointDto
    {
        public int Id { get; set; }
        public int RoadMapId { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? Deadline { get; set; }
        public StatusEnum? Status { get; set; }
    }
}
