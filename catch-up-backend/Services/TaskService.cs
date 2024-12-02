using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace catch_up_backend.Services
{
    public class TaskService : ITaskService
    {
        private readonly CatchUpDbContext _context;

        public TaskService(CatchUpDbContext context)
        {
            _context = context;
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
        public async Task<List<FullTask>> GetAllTasks()
        {
            return await _context.Tasks.Join(
                _context.TaskContents,
                task => task.TaskContentId,taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task,taskContent)).ToListAsync();
        }
    }
}
