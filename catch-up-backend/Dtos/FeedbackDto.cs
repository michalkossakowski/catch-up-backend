using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Dtos
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public ResourceTypeEnum ResourceType { get; set; }
        public int ResourceId { get; set; }
        public string? ResourceName { get; set; }
        public string? UserName { get; set; }
        public DateTime createdDate { get; set; }
    }
}
