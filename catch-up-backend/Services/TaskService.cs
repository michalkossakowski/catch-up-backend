using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;

namespace catch_up_backend.Services
{
    public class TaskService : ITaskService
    {
        private readonly CatchUpDbContext _context;

        public TaskService(CatchUpDbContext contex)
        {
            _context = contex;
        }
        public async Task Add(TaskDto newTask, Guid newbieID, int taskContentId)
        {
            
            var task = new TaskModel(
                newbieID,
                taskContentId,
                newTask.RoadMapPointId,
                newTask.Status,
                newTask.Deadline,
                newTask.Priority
                );
            //Uncomment if you want to have valid response body
            //newTask.NewbieId = newbieID;
            //newTask.TaskContentId = taskContentId;
            //newTask.AssignmentDate = task.AssignmentDate;
            await _context.Tasks.AddAsync( task );
            await _context.SaveChangesAsync();
        }
    }
}
