using catch_up_backend.Enums;
using catch_up_backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Dtos
{
    public class FullTask
    {
        public int Id { get; set; }
        public Guid? NewbieId { get; set; }
        public int? MaterialsId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public StateEnum State { get; set; }
        public int? RoadMapPointId { get; set; }
        public string Status { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public int Deadline { get; set; }
        public int SpendTime { get; set; }
        public int Priority { get; set; }
        public int? Rate { get; set; }
        public FullTask(TaskModel task, TaskContentModel taskContent)
        {
            Id = task.Id;
            NewbieId = task.NewbieId;
            CategoryId = taskContent.CategoryId;
            MaterialsId = taskContent.MaterialsId;
            Title = taskContent.Title;
            Description = taskContent.Description;
            State = task.State;
            RoadMapPointId = task.RoadMapPointId;
            Status = task.Status;
            AssignmentDate = task.AssignmentDate;
            FinalizationDate = task.FinalizationDate;
            Deadline = task.Deadline;
            SpendTime = task.SpendTime;
            Priority = task.Priority;
            Rate = task.Rate;
        }
    }
}
