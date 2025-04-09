using catch_up_backend.Enums;

namespace catch_up_backend.Dtos
{
    public class RoadMapDto
    {
        public int Id { get; set; }
        public Guid NewbieId { get; set; }
        public Guid CreatorId { get; set; }
        public string? CreatorName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? AssignDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public StatusEnum? Status { get; set; }
        public decimal? Progress { get; set; }

    }
}
