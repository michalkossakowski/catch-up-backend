using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using catch_up_backend.Models;

namespace catch_up_backend.Dtos
{
    public class TaskCommentDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public Guid CreatorId { get; set; }
        public int? MaterialId { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        public TaskCommentDto() { }

        public TaskCommentDto(TaskCommentModel taskCommentModel) 
        {
            Id = taskCommentModel.Id;
            TaskId = taskCommentModel.TaskId;
            CreatorId = taskCommentModel.CreatorId;
            MaterialId = taskCommentModel.MaterialId;
            Content = taskCommentModel.Content;
            CreationDate = taskCommentModel.CreationDate;
            ModificationDate = taskCommentModel.ModificationDate;

        }
    }
}
