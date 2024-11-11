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
        public bool IsFinished { get; set; }
        public StateEnum State { get; set; }
        public RoadMapModel(Guid newbieId)
        {
            this.NewbieId = newbieId;
            this.IsFinished = false;
        }
    }
}
