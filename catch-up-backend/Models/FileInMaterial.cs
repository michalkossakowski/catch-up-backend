using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using catch_up_backend.Enums;

namespace catch_up_backend.Models
{
    public class FileInMaterial
    {
        [ForeignKey("MaterialId")]
        public int MaterialId { get; set; }
        [ForeignKey("SchoolingPartId")]
        public int FileId { get; set; }
        public StateEnum State { get; set; }
        public FileInMaterial(int materialId, int fileId)
        {
            MaterialId = materialId;
            FileId = fileId;
        }
    }
}
