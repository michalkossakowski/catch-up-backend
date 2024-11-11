using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class SchoolingUserModel
    {
        [ForeignKey("NewbieId")]
        public Guid NewbieId { get; set; }
        [ForeignKey("SchoolingId")]
        public int SchoolingId { get; set; }

        public SchoolingUserModel(Guid newbieId, int schoolingId)
        {
            NewbieId = newbieId;
            SchoolingId = schoolingId;
        }
    }
}
