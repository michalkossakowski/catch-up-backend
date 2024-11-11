using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class MaterialsSchoolingPartModel
    {
        [ForeignKey("MaterialsId")]
        public int MaterialsId { get; set; }
        [ForeignKey("SchoolingPartId")]
        public int SchoolingPartId { get; set; }
        public StateEnum State { get; set; }
        public MaterialsSchoolingPartModel(int materialsId, int schoolingPartId)
        {
            MaterialsId = materialsId;
            SchoolingPartId = schoolingPartId;
        }
    }
}
