using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class SchoolingPartModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("SchoolingId")]
        public int SchoolingId { get; set; }
        [ForeignKey("MaterialsId")]
        public int? MaterialsId { get; set; } 
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public StateEnum State { get; set; }

        public SchoolingPartModel(int schoolingId, string title, string content, string shortDescription)
        {
            SchoolingId = schoolingId;
            Title = title;
            Content = content;
            State = StateEnum.Active;
            ShortDescription = shortDescription;
        }
    }
}
