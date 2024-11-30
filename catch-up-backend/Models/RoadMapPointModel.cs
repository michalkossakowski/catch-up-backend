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
        public DateTime AssignmentDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public int Deadline { get; set; }
        public StatusEnum Status { get; set; }
        public StateEnum State { get; set; }
        public RoadMapPointModel(int roadMapId, string name, int deadline)
        {
            RoadMapId = roadMapId;
            Name = name;
            AssignmentDate = DateTime.Now;
            Deadline = deadline;
            Status = StatusEnum.ToDo;
            State = StateEnum.Active;
        }
    }
}
