using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using catch_up_backend.Enums;
using catch_up_backend.Dtos;

namespace catch_up_backend.Models
{
    public class TaskCommentModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("TaskId")]
        public int TaskId { get; set; }
        [ForeignKey("CreatorId")]
        public Guid CreatorId { get; set; }
        [ForeignKey("MaterialsId")]
        public int? MaterialId { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public StateEnum State { get; set; }

        public TaskCommentModel(int taskId, Guid creatorId, string content)
        {
            TaskId = taskId;
            CreatorId = creatorId;
            Content = content;
            CreationDate = DateTime.Now;
            State = StateEnum.Active;
        }
        public TaskCommentModel(TaskCommentDto commentDto)
        {
            TaskId = commentDto.TaskId;
            CreatorId = commentDto.CreatorId;
            MaterialId = commentDto.MaterialId;
            Content = commentDto.Content;
            CreationDate = commentDto.CreationDate;
            ModificationDate = commentDto.ModificationDate;
            State = StateEnum.Active;
        }
        public TaskCommentModel()
        {
        }
    }
}
