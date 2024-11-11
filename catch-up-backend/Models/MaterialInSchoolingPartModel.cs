using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class MaterialInSchoolingPartModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("MaterialId")]
        public int MaterialId { get; set; }
        [ForeignKey("SchoolingPartId")]
        public int SchoolingPartId { get; set; }
        public StateEnum State { get; set; }
        public MaterialInSchoolingPartModel(int materialId, int schoolingPartId)
        {
            MaterialId = materialId;
            SchoolingPartId = schoolingPartId;
            State = StateEnum.Active;
        }
    }
}
