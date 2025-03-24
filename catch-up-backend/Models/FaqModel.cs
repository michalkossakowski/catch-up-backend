using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class FaqModel
    {
        [Key]
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        [ForeignKey("MaterialsId")]
        public int? MaterialId { get; set; }
        [ForeignKey("CreatorId")]
        public Guid CreatorId { get; set; }
        public StateEnum State { get; set; }
        public FaqModel(string question, string answer, int? materialId, Guid creatorId)
        {
            this.Question = question;
            this.Answer = answer;
            this.MaterialId = materialId;
            State = StateEnum.Active;
            CreatorId = creatorId;
        }
    }
}
