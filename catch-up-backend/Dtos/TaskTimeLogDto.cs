using catch_up_backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Dtos
{
    public class TaskTimeLogDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        public TaskTimeLogDto() { }
        public TaskTimeLogDto(TaskTimeLogModel taskTimeLog)
        {
            Id = taskTimeLog.Id;
            TaskId = taskTimeLog.TaskId;
            Hours = taskTimeLog.Hours;
            Minutes = taskTimeLog.Minutes;
            Description = taskTimeLog.Description;
            CreationDate = taskTimeLog.CreationDate;
            ModificationDate = taskTimeLog.ModificationDate;
        }
    }
}
