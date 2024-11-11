using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class TaskContentModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CreatorId")]
        public Guid CreatorId { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        [ForeignKey("MaterialsId")]
        public int MaterialsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StateEnum State { get; set; }
        public TaskContentModel(Guid creatorId, int categoryId, int materialsId, string title, string description)
        {
            CreatorId = creatorId;
            CategoryId = categoryId;
            MaterialsId = materialsId;
            Title = title;
            Description = description;
            State = StateEnum.Active;
        }
    }
}
