using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class SchoolingModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CreatorId")]
        public Guid CreatorId { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public StateEnum State { get; set; }
        public SchoolingModel(Guid creatorId, int categoryId, string title, string description, int priority)
        {
            CreatorId = creatorId;
            CategoryId = categoryId;
            Title = title;
            Description = description;
            Priority = priority;
        }
    }
}
