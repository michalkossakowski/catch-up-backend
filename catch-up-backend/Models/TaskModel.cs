using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class TaskModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("NewbieId")]
        public Guid? NewbieId { get; set; }
        [ForeignKey("TaskContentId")]
        public int TaskContentId { get; set; }
        [ForeignKey("RoadmapPointId")]
        public int? RoadMapPointId { get; set; }
        public string Status { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public int Deadline { get; set; }
        public int SpendTime { get; set; }
        public int Priority { get; set; }
        public int? Rate { get; set; }
        public StateEnum State { get; set; }
        public TaskModel(Guid? newbieId, int taskContentId, int? roadMapPointId, string status, int deadline, int priority)
        {
            NewbieId = newbieId;
            TaskContentId = taskContentId;
            RoadMapPointId = roadMapPointId;
            Status = status;
            AssignmentDate = DateTime.Now;
            Deadline = deadline;
            SpendTime = 0;
            Priority = priority;
            State = StateEnum.Active;
        }
    }
}
