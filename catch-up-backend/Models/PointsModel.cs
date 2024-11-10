using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class PointsModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("MentorId")]
        public Guid MentorId { get; set; }
        public int Value { get; set; }

        public PointsModel(Guid mentorId)
        {
            this.MentorId = mentorId;
            this.Value = 0;
        }
    }
}
