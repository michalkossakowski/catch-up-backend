using catch_up_backend.Enums;
using System;
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
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public StateEnum State { get; set; }
        public NewbieMentorModel(Guid newbieId, Guid mentorId)
        {
            this.NewbieId = newbieId;
            this.MentorId = mentorId;
            State = StateEnum.Active;
            StartDate = DateTime.Now;
        }
    }
}
