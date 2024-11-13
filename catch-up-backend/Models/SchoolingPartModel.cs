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
        public string Name { get; set; }
        public string Content { get; set; }
        public StateEnum State { get; set; }
        public SchoolingPartModel(int schoolingId, string name, string content)
        {
            SchoolingId = schoolingId;
            Name = name;
            Content = content;
            State = StateEnum.Active;
        }
    }
}
