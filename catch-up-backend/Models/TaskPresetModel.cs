using catch_up_backend.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Models
{
    public class TaskPresetModel
    {
        [ForeignKey("PresetId")]
        public int PresetId { get; set; }
        [ForeignKey("TaskContentId")]
        public int TaskContentId { get; set; }
        public StateEnum State { get; set; }
        public TaskPresetModel(int presetId, int taskContentId)
        {
            PresetId = presetId;
            TaskContentId = taskContentId;
            State = StateEnum.Active;
        }
    }
}
