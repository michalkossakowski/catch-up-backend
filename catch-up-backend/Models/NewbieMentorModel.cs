using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class NewbieMentorModel
    {
        [ForeignKey("NewbieId")]
        public Guid NewbieId { get; set; }
        [ForeignKey("MentorId")]
        public Guid MentorId { get; set; }
        public bool IsActive { get; set; }

        public NewbieMentorModel(Guid newbieId, Guid mentorId)
        {
            this.NewbieId = newbieId;
            this.MentorId = mentorId;
            this.IsActive = true;
        }
    }
}
