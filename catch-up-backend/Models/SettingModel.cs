using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace catch_up_backend.Models
{
    public class SettingModel
    {
        [Key]
        public string Name { get; set; }
        public bool Value { get; set; }

        public SettingModel(string name, bool value = false)
        {
            Name = name;
            Value = value;
        }
    }
}
