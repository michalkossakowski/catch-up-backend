using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class RoadMapModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("NewbieId")]
        public Guid NewbieId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public StatusEnum Status { get; set; }
        public StateEnum State { get; set; }
        public RoadMapModel(Guid newbieId, string name)
        {
            this.NewbieId = newbieId;
            this.Name = name;
            this.Status = StatusEnum.ToDo;
            this.StartDate = DateTime.Now;
            State = StateEnum.Active;
        }
    }
}
