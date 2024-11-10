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
        public FileModel(string name, string type, string source)
        {
            Name = name;
            Type = type;
            Source = source;
        }
    }
}
