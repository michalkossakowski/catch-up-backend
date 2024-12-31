using catch_up_backend.Enums;
using catch_up_backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace catch_up_backend.Dtos
{
    public class FullTask
    {
        public int Id { get; set; }
        public Guid? NewbieId { get; set; }
        public Guid? AssigningId { get; set; }
        public int? MaterialsId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? RoadMapPointId { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public DateTime? Deadline { get; set; }
        public double SpendTime { get; set; }
        public int Priority { get; set; }
        public int? Rate { get; set; }
        public FullTask() { }
        public FullTask(TaskModel task, TaskContentModel taskContent)
        {
            Id = task.Id;
            NewbieId = task.NewbieId;
            AssigningId = task.AssigningId;
            CategoryId = taskContent.CategoryId;
            MaterialsId = taskContent.MaterialsId;
            Title = taskContent.Title;
            Description = taskContent.Description;
            RoadMapPointId = task.RoadMapPointId;
            Status = task.Status;
            AssignmentDate = task.AssignmentDate;
            FinalizationDate = task.FinalizationDate;
            Deadline = task.Deadline;
            SpendTime = task.SpendTime;
            Priority = task.Priority;
            Rate = task.Rate;
        }
        public FullTask(int id, Guid? newbieId, int? materialsId, int? categoryId, string title, string description, int? roadMapPointId, StatusEnum status, DateTime assignmentDate, DateTime? finalizationDate, DateTime? deadline, double spendTime, int priority, int? rate)
        {
            Id = id;
            NewbieId = newbieId;
            MaterialsId = materialsId;
            CategoryId = categoryId;
            Title = title;
            Description = description;
            RoadMapPointId = roadMapPointId;
            Status = status;
            AssignmentDate = assignmentDate;
            FinalizationDate = finalizationDate;
            Deadline = deadline;
            SpendTime = spendTime;
            Priority = priority;
            Rate = rate;
        }
    }
}
