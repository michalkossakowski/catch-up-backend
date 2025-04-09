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
        public RoadMapPointModel(int roadMapId, string name, DateTime? deadline = null)
        {
            RoadMapId = roadMapId;
            Name = name;
            Deadline = deadline;
            State = StateEnum.Active;
        }
    }
}
