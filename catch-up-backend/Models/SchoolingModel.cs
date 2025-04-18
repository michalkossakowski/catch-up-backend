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
        [ForeignKey("IconFileId")]
        public int? IconFileId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public int Priority { get; set; }
        public StateEnum State { get; set; }
        public SchoolingModel(Guid creatorId, int categoryId, string title, string shortDescription, string content, int priority, int? IconFileId)
        {
            CreatorId = creatorId;
            CategoryId = categoryId;
            Title = title;
            ShortDescription = shortDescription;
            Content = content;
            Priority = priority;
            if (IconFileId != null)
                this.IconFileId = IconFileId;
            else
                this.IconFileId = null;
            State = StateEnum.Active;
        }
    }
}
