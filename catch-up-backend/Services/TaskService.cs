using catch_up_backend.Controllers;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace catch_up_backend.Services
{
    public class TaskService : ITaskService
    {
        private readonly CatchUpDbContext _context;
        private readonly ITaskContentService _contentService;

        public TaskService(CatchUpDbContext context, ITaskContentService contentService)
        {
            _context = context;
            _contentService = contentService;
        }
        public async Task<TaskDto> Add(TaskDto newTask )
        {
            try
            {
                var task = new TaskModel(
                newTask.NewbieId,
                newTask.TaskContentId,
                newTask.RoadMapPointId,
                newTask.Status,
                newTask.Deadline,
                newTask.Priority
                );
                await _context.Tasks.AddAsync(task);
                await _context.SaveChangesAsync();
                newTask.Id = task.Id;
            }
            catch(Exception ex)
            {
                throw new Exception("Error: Add taskContent: " + ex);
            }
            return newTask;

        }
        public async Task<bool> Edit(int taskId, TaskDto newTask)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;
            try
            {
                task.RoadMapPointId = newTask.RoadMapPointId;
                task.Status = newTask.Status ?? "";
                task.Deadline = newTask.Deadline;
                task.SpendTime = newTask.SpendTime;
                task.Priority = newTask.Priority;
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Edit Task: " + ex);
            }
            return true;
        }
        public async Task<bool> EditFullTask(int id, FullTask fullTask, Guid userId)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            var taskContent = await _context.TaskContents.FindAsync(task.TaskContentId);
            if (taskContent == null) return false;

            try
            {
                task.RoadMapPointId = fullTask.RoadMapPointId;
                task.Status = fullTask.Status ?? "";
                task.Deadline = fullTask.Deadline;
                task.SpendTime = fullTask.SpendTime;
                if (fullTask.CategoryId != taskContent.CategoryId 
                    || fullTask.MaterialsId != taskContent.MaterialsId 
                    || fullTask.Title != taskContent.Title 
                    || fullTask.Description != taskContent.Description)
                {
                    var newTaskContent = new TaskContentDto(userId, fullTask.CategoryId, fullTask.MaterialsId, fullTask.Title,fullTask.Description);
                    newTaskContent = await _contentService.Add(newTaskContent);
                    if (newTaskContent == null) return false;
                    task.TaskContentId = newTaskContent.Id;
                }
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw new Exception("Error: Edit Task: " + ex);
            }
            return true;
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
