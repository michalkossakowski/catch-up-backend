using catch_up_backend.Enums;

namespace catch_up_backend.Dtos
{
    public class RoadMapDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Guid NewbieId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public bool? IsFinished { get; set; }
    }
}
