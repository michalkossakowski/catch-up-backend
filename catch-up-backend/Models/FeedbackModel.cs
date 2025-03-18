using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class FeedbackModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("SenderId")]
        public Guid SenderId { get; set; }
        [ForeignKey("ReceiverId")]
        public Guid ReceiverId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ResourceTypeEnum ResourceType { get; set; }
        public int ResourceId { get; set; }
        public DateTime createdDate { get; set; }
        public bool IsResolved { get; set; }
        public StateEnum State { get; set; }
        public FeedbackModel(Guid senderId, Guid receiverId, string title, string description, ResourceTypeEnum resourceType, int resourceId)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Title = title;
            Description = description;
            ResourceType = resourceType;
            ResourceId = resourceId;
            IsResolved = false;
            createdDate = DateTime.Now;
            State = StateEnum.Active;
        }
    }
}
