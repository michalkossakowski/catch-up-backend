using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public StateEnum State { get; set; }
        public CategoryModel(string name)
        {
            Name = name;
        }
    }
}
