using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class TaskTimeLogModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("TaskId")]
        public int TaskId { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public StateEnum State { get; set; }
        public TaskTimeLogModel(int taskId, int hours, int minutes, string description)
        {
            TaskId = taskId;
            Hours = hours;
            Minutes = minutes;
            Description = description;
            CreationDate = DateTime.Now;
            State = StateEnum.Active;
        }
        public TaskTimeLogModel()
        {
        }
    }
}
