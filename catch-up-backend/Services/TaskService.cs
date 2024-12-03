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
        public async Task<List<TaskDto>> GetAllTasks()
        {
            return await _context.Tasks.Select(task => new TaskDto(task)).ToListAsync();
        }

        public async Task<List<TaskDto>> GetAllTasksByNewbieId(Guid id)
        {
            return await _context.Tasks.Where(task => task.NewbieId == id)
                .Select(task => new TaskDto(task))
                .ToListAsync();
        }

        public async Task<List<TaskDto>> GetAllTaskByTaskContentId(int id)
        {
            return await _context.Tasks.Where(task => task.TaskContentId == id)
                .Select(task => new TaskDto(task))
                .ToListAsync();
        }
        public async Task<TaskDto> GetTaskById(int id)
        {
            return await _context.Tasks.Where(task => task.Id == id)
                .Select(task => new TaskDto(task))
                .FirstOrDefaultAsync();
        }
        public async Task<List<FullTask>> GetAllFullTasks()
        {
            return await _context.Tasks.Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task, taskContent)).ToListAsync();
        }

        public async Task<List<FullTask>> GetAllFullTasksByNewbieId(Guid id)
        {
            return await _context.Tasks.Where(task => task.NewbieId == id).Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task, taskContent)).ToListAsync();
        }

        public async Task<List<FullTask>> GetAllFullTaskByTaskContentId(int id)
        {
            return await _context.Tasks.Where(task => task.TaskContentId == id).Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task, taskContent)).ToListAsync();
        }

        public async Task<List<FullTask>> GetAllFullTasksByCreatorId(Guid id)
        {
            return await _context.Tasks.Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new { task, taskContent })
                .Where(joined => joined.taskContent.CreatorId == id)
                .Select(joined => new FullTask(joined.task, joined.taskContent))
                .ToListAsync();
        }

        public async Task<FullTask> GetFullTaskById(int id)
        {
            var result = await _context.Tasks
            .Join(
                _context.TaskContents,
                task => task.TaskContentId,
                taskContent => taskContent.Id,
                (task, taskContent) => new { Task = task, TaskContent = taskContent } // Pobieramy dane w formacie anonimowym
            )
            .FirstOrDefaultAsync(joined => joined.Task.Id == id); // Warunek na id

            // Sprawdź, czy wynik istnieje, jeśli nie, zwróć null (lub rzuć wyjątek)
            if (result == null)
            {
                return null; // Możesz rzucić wyjątek, jeśli null jest nieakceptowalne
            }

            // Utwórz i zwróć FullTask w pamięci
            return new FullTask(result.Task, result.TaskContent);
        }

    }
}
