using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Models
{
    public class FileInMaterial
    {
        [ForeignKey("MaterialId")]
        public int MaterialId { get; set; }
        [ForeignKey("FileId")]
        public int FileId { get; set; }

        public FileInMaterial(int materialId, int fileId)
        {
            MaterialId = materialId;
            FileId = fileId;
        }
    }
}
