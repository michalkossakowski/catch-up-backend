using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class NotificationModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("SenderId")]
        public Guid SenderId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
        public string? LinkedContent { get; set; }
        public StateEnum State { get; set; }
        public NotificationModel(Guid senderId, string title, string message, string? linkedContent)
        {
            this.SenderId = senderId;
            this.Title = title;
            this.Message = message;
            this.SendDate = DateTime.Now;
            this.LinkedContent = linkedContent;
        }
    }
}
