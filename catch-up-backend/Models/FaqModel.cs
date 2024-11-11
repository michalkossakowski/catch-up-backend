using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class FaqModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }
        [ForeignKey("MaterialsId")]
        public int? MaterialsId { get; set; }
        public StateEnum State { get; set; }
        public FaqModel(string title, string answer, int? materialsId)
        {
            this.Title = title;
            this.Answer = answer;
            this.MaterialsId = materialsId;
            State = StateEnum.Active;
        }
    }
}
