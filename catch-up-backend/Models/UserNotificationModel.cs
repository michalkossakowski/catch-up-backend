using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class UserNotificationModel
    {
        [ForeignKey("NotificationId")]
        public int NotificationId { get; set; }
        [ForeignKey("ReceiverId")]
        public Guid ReceiverId { get; set; }
        public StateEnum State { get; set; }
        public bool IsRead { get; set; }
        public UserNotificationModel(int notificationId, Guid receiverId)
        {
            NotificationId = notificationId;
            ReceiverId = receiverId;
            State = StateEnum.Active;
            IsRead = false;
        }
    }
}
