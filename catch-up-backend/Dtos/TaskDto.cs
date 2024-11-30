using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Dtos
{
    public class TaskDto
    {
        public int Id { get; set; }
        public Guid? NewbieId { get; set; }
        public int TaskContentId { get; set; }
        public int? RoadMapPointId { get; set; }
        public string Status { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public int Deadline { get; set; }
        public int SpendTime { get; set; }
        public int Priority { get; set; }
        public int? Rate { get; set; }
        public StateEnum State { get; set; }
    }
}
