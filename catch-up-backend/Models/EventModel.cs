using catch_up_backend.Enums;

namespace catch_up_backend.Models
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid OwnerId { get; set; }
        public string TargetUserType { get; set; }
        public StateEnum State { get; set; }
        public EventModel()
        {
        }
    }
}