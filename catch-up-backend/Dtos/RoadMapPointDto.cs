using catch_up_backend.Enums;

namespace catch_up_backend.Dtos
{
    public class RoadMapPointDto
    {
        public int Id { get; set; }
        public int RoadMapId { get; set; }
        public string? Name { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public int? Deadline { get; set; }
        public StatusEnum? Status { get; set; }
    }
}
