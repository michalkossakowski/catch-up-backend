using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Dtos
{
    public class MentorBadgeDto
    {
        public Guid MentorId { get; set; }
        public int BadgeId { get; set; }
        public DateTime AchievedDate { get; set; }
    }
}
