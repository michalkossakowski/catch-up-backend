using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Models
{
    public class MaterialsModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public StateEnum State { get; set; }
        public MaterialsModel(string name)
        {
            this.Name = name;
            State = StateEnum.Active;
        }
    }
}
