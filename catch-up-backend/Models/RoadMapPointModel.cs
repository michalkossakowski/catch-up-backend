using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class RoadMapPointModel
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("RoadMap")]
        public int RoadmapId { get; set; }
        public string Name { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public int Deadline { get; set; }
        public string Status { get; set; }
        public StateEnum State { get; set; }
        public RoadMapPointModel(string id, int roadmapId, string name, int deadline, string status)
        {
            Id = id;
            RoadmapId = roadmapId;
            Name = name;
            AssignmentDate = DateTime.Now;
            Deadline = deadline;
            Status = status;
        }
    }
}
