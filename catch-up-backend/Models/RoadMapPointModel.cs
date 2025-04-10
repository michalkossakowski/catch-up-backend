using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class RoadMapPointModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("RoadMapId")]
        public int RoadMapId { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? Deadline { get; set; }
        public StateEnum State { get; set; }
        public virtual ICollection<TaskModel> Tasks { get; set; }

        [NotMapped]
        public StatusEnum Status
        {
            get
            {
                var activeTasks = Tasks?.Where(t => t.State == StateEnum.Active).ToList() ?? new List<TaskModel>();
                var finishedTasks = activeTasks.Where(t => t.Status == StatusEnum.Done).ToList() ?? new List<TaskModel>();
                
                if (finishedTasks.Count == 0)
                {
                    return StatusEnum.ToDo;
                }
                if (activeTasks.All(t => t.Status == StatusEnum.Done))
                {
                    return StatusEnum.Done;
                }

                return StatusEnum.InProgress;
            }
        }

        public RoadMapPointModel(int roadMapId, string name, DateTime? deadline = null)
        {
            RoadMapId = roadMapId;
            Name = name;
            Deadline = deadline;
            State = StateEnum.Active;
        }
    }
}