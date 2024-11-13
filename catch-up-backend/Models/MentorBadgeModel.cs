using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class MentorBadgeModel
    {
        [ForeignKey("MentorId")]
        public Guid MentorId { get; set; }
        [ForeignKey("BadgeId")]
        public int BadgeId { get; set; }
        public DateTime AchievedDate { get; set; }
        public StateEnum State { get; set; }
        public MentorBadgeModel(Guid mentorId, int badgeId)
        {
            MentorId = mentorId;
            BadgeId = badgeId;
            AchievedDate = DateTime.Now;
            State = StateEnum.Active;
        }
    }
}
