using catch_up_backend.Enums;

namespace catch_up_backend.Dtos
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
        public string? Source { get; set; }
        public bool IsRead { get; set; }
    }
}
