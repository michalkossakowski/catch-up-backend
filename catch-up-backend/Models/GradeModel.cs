using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class GradeModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("RatedId")]
        public Guid RatedId { get; set; }
        [ForeignKey("EvaluatorId")]
        public Guid EvaluatorId { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
        public StateEnum State { get; set; }
        public GradeModel(Guid ratedId, Guid evaluatorId, int value, string description)
        {
            this.RatedId = ratedId;
            this.EvaluatorId = evaluatorId;
            this.Value = value;
            this.Description = description;
            State = StateEnum.Active;
        }
    }
}
