using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class RoadMapModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("NewbieId")]
        public Guid NewbieId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public bool IsFinished { get; set; }
        public StateEnum State { get; set; }
        public RoadMapModel(string name, Guid newbieId)
        {
            this.Name = name;
            this.NewbieId = newbieId;
            this.IsFinished = false;
            this.StartDate = DateTime.Now;
            State = StateEnum.Active;
        }
    }
}
