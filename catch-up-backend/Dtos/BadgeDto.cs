namespace catch_up_backend.Dtos
{
    public class BadgeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? IconSource { get; set; }
        public int? Count { get; set; }
        public string? CountType { get; set; }
    }
}
