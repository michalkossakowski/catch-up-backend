using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class FileModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public StateEnum State { get; set; }
        public DateTime DateOfUpload { get; set; }
        public Guid? Owner { get; set; }
        public long SizeInBytes { get; set; }
        public FileModel(string name, string type, string source, long sizeInBytes)
        {
            Name = name;
            Type = type;
            Source = source;
            State = StateEnum.Active;
            DateOfUpload = DateTime.Now;
            SizeInBytes = sizeInBytes;
        }
    }
}
