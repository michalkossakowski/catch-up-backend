namespace catch_up_backend.Dtos
{
    public class FaqDto
    {
        public int Id { get; set; }
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public int? MaterialId { get; set; }
        public Guid CreatorId { get; set; }
    }
}
