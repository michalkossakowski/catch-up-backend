namespace catch_up_backend.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid OwnerId { get; set; }
        public List<Guid> ReceiverIds { get; set; } 

        public Event()
        {
            ReceiverIds = new List<Guid>();
        }
    }
}