namespace catch_up_backend.Dtos
{
    public class FaqDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Answer { get; set; }
        public int? MaterialsId { get; set; }
    }
}
