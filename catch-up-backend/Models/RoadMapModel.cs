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
        [ForeignKey("CreatorId")]
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public StateEnum State { get; set; }
        public StatusEnum Status { get; set; }
        public decimal Progress { get; set; }
        public RoadMapModel(Guid newbieId, Guid creatorId, string title, string description)
        {
            this.NewbieId = newbieId;
            this.CreatorId = creatorId;
            this.Title = title;
            this.Description = description;
            this.AssignDate = DateTime.Now;
            State = StateEnum.Active;
            Status = StatusEnum.ToDo;
            Progress = 0;
        }
    }
}