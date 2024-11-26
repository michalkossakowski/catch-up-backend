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
        public required string Description { get; set; }
        public required string Origin { get; set; }
    }
}
