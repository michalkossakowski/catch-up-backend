namespace catch_up_backend.Dtos;

public class EventDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid OwnerId { get; set; }
    public string TargetUserType { get; set; }
}
