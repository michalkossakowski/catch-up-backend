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
        public virtual ICollection<RoadMapPointModel> RoadMapPoints { get; set; }

        [NotMapped]
        public StatusEnum Status
        {
            get
            {
                var activePoints = RoadMapPoints?.Where(rmp => rmp.State == StateEnum.Active).ToList() ?? new List<RoadMapPointModel>();
                var finishedPoints = activePoints.Where(rmp => rmp.Status == StatusEnum.Done).ToList() ?? new List<RoadMapPointModel>();
                
                if (finishedPoints.Count == 0)
                {
                    return StatusEnum.ToDo;
                }
                if (activePoints.All(rmp => rmp.Status == StatusEnum.Done))
                {
                    return StatusEnum.Done;
                }

                return StatusEnum.InProgress;
            }
        }

        [NotMapped]
        public decimal Progress
        {
            get
            {
                var activePoints = RoadMapPoints?.Where(rmp => rmp.State == StateEnum.Active).ToList();
                if (activePoints == null || !activePoints.Any())
                {
                    return 0;
                }
                var finishedPointsCount = activePoints.Count(rmp => rmp.Status == StatusEnum.Done);

                return Math.Round((decimal)finishedPointsCount / activePoints.Count * 100, 2);
            }
        }
        public RoadMapModel(Guid newbieId, Guid creatorId, string title, string description)
        {
            this.NewbieId = newbieId;
            this.CreatorId = creatorId;
            this.Title = title;
            this.Description = description;
            this.AssignDate = DateTime.Now;
            State = StateEnum.Active;
        }
    }
}