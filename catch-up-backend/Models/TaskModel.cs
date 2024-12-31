using catch_up_backend.Dtos;
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
        [ForeignKey("AssigningId")]
        public Guid? AssigningId { get; set; }
        [ForeignKey("TaskContentId")]
        public int TaskContentId { get; set; }
        [ForeignKey("RoadmapPointId")]
        public int? RoadMapPointId { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public DateTime? Deadline { get; set; }
        public double SpendTime { get; set; }
        public int Priority { get; set; }
        public int? Rate { get; set; }
        public StateEnum State { get; set; }
        public TaskModel() { }
        public TaskModel(Guid? newbieId, Guid? assigningID, int taskContentId, int? roadMapPointId, DateTime? deadline, int priority)
        {
            NewbieId = newbieId;
            AssigningId = assigningID;
            TaskContentId = taskContentId;
            RoadMapPointId = roadMapPointId;
            Status = StatusEnum.ToDo;
            AssignmentDate = DateTime.Now;
            FinalizationDate = null;
            Deadline = deadline;
            SpendTime = 0;
            Priority = priority;
            Rate = null;
            State = StateEnum.Active;
        }
    }
}
