using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class PresetModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("CreatorId")]
        public Guid CreatorId { get; set; }
        public string Name { get; set; }

        public PresetModel(Guid creatorId, string name)
        {
            this.CreatorId = creatorId;
            this.Name = name;
        }
    }
}
