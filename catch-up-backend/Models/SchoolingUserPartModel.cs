using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class SchoolingUserPartModel
    {
        [ForeignKey("SchoolingUserId")]
        public int SchoolingUserId { get; set; }
        [ForeignKey("SchoolingPartId")]
        public int SchoolingPartId { get; set; }

        public bool IsDone { get; set; }

        public SchoolingUserPartModel(int schoolingUserId,int schoolingPartId, bool isDone = false) {

            SchoolingUserId = schoolingUserId;
            SchoolingPartId = schoolingPartId;
            IsDone = isDone;
        }
    }
}
