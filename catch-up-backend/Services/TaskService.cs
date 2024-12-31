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
        private readonly IUserService _userService;

        public TaskService(CatchUpDbContext context, ITaskContentService contentService)
        {
            _context = context;
            _contentService = contentService;
        }
        public async Task<TaskDto> AddAsync(TaskDto newTask )
        {
            try
            {
                var task = new TaskModel(
                newTask.NewbieId,
                newTask.AssigningId,
                newTask.TaskContentId,
                newTask.RoadMapPointId,
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
        public async Task<bool> EditAsync(int taskId, TaskDto newTask)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) return false;
            try
            {
                task.RoadMapPointId = newTask.RoadMapPointId;
                task.Status = newTask.Status;
                task.Deadline = newTask.Deadline;
                task.SpendTime = newTask.SpendTime;
                task.Priority = newTask.Priority;
                task.Rate = newTask.Rate;
                task.AssigningId = newTask.AssigningId;
                task.NewbieId = newTask.NewbieId;
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Edit Task: " + ex);
            }
            return true;
        }
        public async Task<(TaskModel, TaskContentDto)> EditFullTaskAsync(int id, FullTask fullTask, Guid userId)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return (null,null);

            //var taskContent = await _context.TaskContents.FindAsync(task.TaskContentId);
            var taskContent = await _contentService.GetById(task.TaskContentId);
            if (taskContent == null) return (null, null);
            try
            {
                task.NewbieId = fullTask.NewbieId;
                task.AssigningId = fullTask.AssigningId;
                task.RoadMapPointId = fullTask.RoadMapPointId;
                task.Status = fullTask.Status;
                task.FinalizationDate = fullTask.FinalizationDate;
                task.Deadline = fullTask.Deadline;
                task.SpendTime = fullTask.SpendTime;
                task.Priority = fullTask.Priority;
                task.Rate = fullTask.Rate;

                if (fullTask.CategoryId != taskContent.CategoryId 
                    || fullTask.MaterialsId != taskContent.MaterialsId 
                    || fullTask.Title != taskContent.Title 
                    || fullTask.Description != taskContent.Description)
                {
                    var newTaskContent = new TaskContentDto(userId, fullTask.CategoryId, fullTask.MaterialsId, fullTask.Title,fullTask.Description);
                    newTaskContent = await _contentService.Add(newTaskContent);
                    if (newTaskContent == null) return (null, null);
                    task.TaskContentId = newTaskContent.Id;
                    taskContent = newTaskContent;
                }
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw new Exception("Error: Edit Task: " + ex);
            }
            return (task, taskContent);
        }
        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            return await _context.Tasks.Where(p=>p.State == StateEnum.Active).Select(task => new TaskDto(task)).ToListAsync();
        }

        public async Task<List<TaskDto>> GetAllTasksByNewbieIdAsync(Guid id)
        {
            if (_context.Users.Find(id) == null)
                return null;
            return await _context.Tasks.Where(task => task.NewbieId == id && task.State == StateEnum.Active)
                .Select(task => new TaskDto(task))
                .ToListAsync();
        }

        public async Task<List<TaskDto>> GetAllTaskByTaskContentIdAsync(int id)
        {
            if (_context.TaskContents.Find(id) == null)
                return null;
            return await _context.Tasks.Where(task => task.TaskContentId == id && task.State == StateEnum.Active)
                .Select(task => new TaskDto(task))
                .ToListAsync();
        }
        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.Where(task => task.Id == id)
                .Select(task => new TaskDto(task))
                .FirstOrDefaultAsync();
        }
        public async Task<List<FullTask>> GetAllFullTasksAsync()
        {
            return await _context.Tasks.Where(p => p.State == StateEnum.Active).Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task, taskContent)).ToListAsync();
        }

        public async Task<List<FullTask>> GetAllFullTasksByNewbieIdAsync(Guid id)
        {
            if (_context.Users.Find(id) == null)
                return null;
            return await _context.Tasks.Where(task => task.NewbieId == id && task.State == StateEnum.Active).Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task, taskContent)).ToListAsync();
        }

        public async Task<List<FullTask>> GetAllFullTaskByTaskContentIdAsync(int id)
        {
            if (_context.TaskContents.Find(id) == null)
                return null;
            return await _context.Tasks.Where(task => task.TaskContentId == id && task.State == StateEnum.Active).Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task, taskContent)).ToListAsync();
        }

        public async Task<List<FullTask>> GetAllFullTasksByAssigningIdAsync(Guid id)
        {
            if (_context.Users.Find(id) == null)
                return null;
            return await _context.Tasks.Where(p => p.State == StateEnum.Active && p.AssigningId == id).Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new { task, taskContent })
                .Select(joined => new FullTask(joined.task, joined.taskContent))
                .ToListAsync();
        }

        public async Task<FullTask> GetFullTaskByIdAsync(int id)
        {
            var result = await _context.Tasks
            .Join(
                _context.TaskContents,
                task => task.TaskContentId,
                taskContent => taskContent.Id,
                (task, taskContent) => new { Task = task, TaskContent = taskContent }
            )
            .FirstOrDefaultAsync(joined => joined.Task.Id == id); 

            if (result == null)
            {
                return null; 
            }

            return new FullTask(result.Task, result.TaskContent);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
                return false;
            task.State = StateEnum.Deleted;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return true;
        }
        
    }
}
