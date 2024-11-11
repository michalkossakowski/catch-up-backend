using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class SchoolingUserModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("NewbieId")]
        public Guid NewbieId { get; set; }
        [ForeignKey("SchoolingId")]
        public int SchoolingId { get; set; }
        public StateEnum State { get; set; }
        public SchoolingUserModel(Guid newbieId, int schoolingId)
        {
            NewbieId = newbieId;
            SchoolingId = schoolingId;
        }
    }
}
