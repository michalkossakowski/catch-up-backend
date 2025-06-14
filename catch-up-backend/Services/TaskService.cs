using catch_up_backend.Controllers;
using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class TaskService : ITaskService
    {
        private readonly CatchUpDbContext _context;
        private readonly ITaskContentService _contentService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IRoadMapPointService _roadMapPointService;
        private readonly EmailController _emailController;

        public TaskService(
            CatchUpDbContext context,
            ITaskContentService contentService,
            IUserService userService,
            INotificationService notificationService,
            IRoadMapPointService roadMapPointService)
        {
            _context = context;
            _contentService = contentService;
            _userService = userService;
            _notificationService = notificationService;
            _roadMapPointService = roadMapPointService;
            _emailController = new EmailController();
        }
        public async Task<TaskDto> AddAsync(TaskDto newTask)
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
                newTask.AssignmentDate = task.AssignmentDate;

                await SendTaskAssignmentEmails(task);

                // Sending notification
                var taskContent = _context.TaskContents.FirstOrDefault(tc => tc.Id == newTask.TaskContentId);
                var sender = await _userService.GetById(newTask.AssigningId!.Value);

                var notification = new NotificationModel(
                    sender.Id,
                    "You have received a new Task !",
                    $"{sender.Name} {sender.Surname} assigned you a task: \"{taskContent!.Title}\"",
                    $"/tasks/{newTask.Id}"
                );

                await _notificationService.AddNotification(notification, newTask.NewbieId!.Value);
            }
            catch (Exception ex)
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
        public async Task<(TaskDto, TaskContentDto)> EditFullTaskAsync(int id, FullTask fullTask, Guid userId)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return (null, null);

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
                    var newTaskContent = new TaskContentDto(userId, fullTask.CategoryId, fullTask.MaterialsId, fullTask.Title, fullTask.Description);
                    newTaskContent = await _contentService.Add(newTaskContent);
                    if (newTaskContent == null) return (null, null);
                    task.TaskContentId = newTaskContent.Id;
                    taskContent = newTaskContent;
                }
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Edit Task: " + ex);
            }
            return (new TaskDto(task), taskContent);
        }
        public async Task<List<TaskDto>> GetAllTasksAsync()
        {
            return await _context.Tasks.Where(p => p.State == StateEnum.Active).Select(task => new TaskDto(task)).ToListAsync();
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
            var fullTasks = await _context.Tasks.Where(p => p.State == StateEnum.Active).Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task, taskContent)).ToListAsync();
            var users = await _userService.GetAll();
            foreach (var task in fullTasks)
            {
                if (task.NewbieId.HasValue && task.AssigningId.HasValue)
                {
                    var newbie = users.FirstOrDefault(users => users.Id == task.NewbieId);
                    var assigning = users.FirstOrDefault(users => users.Id == task.AssigningId);
                    task.NewbieName = newbie.Name + " " + newbie.Surname;
                    task.AssigningName = assigning.Name + " " + assigning.Surname;
                }

            }
            return fullTasks;
        }

        public async Task<List<FullTask>> GetAllFullTasksByNewbieIdAsync(Guid id)
        {
            if (_context.Users.Find(id) == null)
                return null;
            var fullTasks = await _context.Tasks.Where(task => task.NewbieId == id && task.State == StateEnum.Active).Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task, taskContent)).ToListAsync();
            var users = await _userService.GetAll();
            foreach (var task in fullTasks)
            {
                if (task.NewbieId.HasValue && task.AssigningId.HasValue)
                {
                    var newbie = users.FirstOrDefault(users => users.Id == task.NewbieId);
                    var assigning = users.FirstOrDefault(users => users.Id == task.AssigningId);
                    task.NewbieName = newbie.Name + " " + newbie.Surname;
                    task.AssigningName = assigning.Name + " " + assigning.Surname;
                }

            }
            return fullTasks;
        }

        public async Task<List<FullTask>> GetAllFullTaskByTaskContentIdAsync(int id)
        {
            if (_context.TaskContents.Find(id) == null)
                return null;
            var fullTasks = await _context.Tasks.Where(task => task.TaskContentId == id && task.State == StateEnum.Active).Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task, taskContent)).ToListAsync();
            var users = await _userService.GetAll();
            foreach (var task in fullTasks)
            {
                if (task.NewbieId.HasValue && task.AssigningId.HasValue)
                {
                    var newbie = users.FirstOrDefault(users => users.Id == task.NewbieId);
                    var assigning = users.FirstOrDefault(users => users.Id == task.AssigningId);
                    task.NewbieName = newbie.Name + " " + newbie.Surname;
                    task.AssigningName = assigning.Name + " " + assigning.Surname;
                }

            }
            return fullTasks;
        }

        public async Task<List<FullTask>> GetAllFullTasksByAssigningIdAsync(Guid id)
        {
            if (_context.Users.Find(id) == null)
                return null;
            var fullTasks = await _context.Tasks.Where(p => p.State == StateEnum.Active && p.AssigningId == id).Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new { task, taskContent })
                .Select(joined => new FullTask(joined.task, joined.taskContent))
                .ToListAsync();
            var users = await _userService.GetAll();
            foreach (var task in fullTasks)
            {
                if (task.NewbieId.HasValue && task.AssigningId.HasValue)
                {
                    var newbie = users.FirstOrDefault(users => users.Id == task.NewbieId);
                    var assigning = users.FirstOrDefault(users => users.Id == task.AssigningId);
                    task.NewbieName = newbie.Name + " " + newbie.Surname;
                    task.AssigningName = assigning.Name + " " + assigning.Surname;
                }

            }
            return fullTasks;
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

            var task = new FullTask(result.Task, result.TaskContent);
            if (task.NewbieId.HasValue && task.AssigningId.HasValue)
            {
                var newbie = await _userService.GetById(task.NewbieId.Value);
                var assigning = await _userService.GetById(task.AssigningId.Value);
                task.NewbieName = newbie.Name + " " + newbie.Surname;
                task.AssigningName = assigning.Name + " " + assigning.Surname;
            }
            return task;
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

        public async Task<bool> SetStatusAsync(int taskId, StatusEnum status)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return false;

            var previousStatus = task.Status;

            try
            {
                task.Status = status;
                if (status == StatusEnum.Done)
                    task.FinalizationDate = DateTime.Now;

                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();

                if (previousStatus != status)
                {
                    await SendStatusChangeEmails(task, previousStatus);
                }

                if(status == StatusEnum.Done && task.RoadMapPointId != null)
                {
                    await _roadMapPointService.UpdateRoadMapPointStatus((int)task.RoadMapPointId);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error: RoadMap SetStatus " + e);
            }
            return true;
        }

        private async Task SendTaskAssignmentEmails(TaskModel task)
        {
            try
            {
                var taskContent = await _context.TaskContents.FindAsync(task.TaskContentId);
                var sender = await _userService.GetById(task.AssigningId!.Value);
                var newbie = await _userService.GetById(task.NewbieId!.Value);

                // Sent to Newbie
                _ = Task.Run(() => _emailController.SendEmail(
                    newbie.Email,
                    "New Task Assigned",
                    $"Hello {newbie.Name} {newbie.Surname}!\n\n" +
                    $"You have been assigned a new task: \"{taskContent!.Title}\"\n\n" +
                    $"Deadline: {task.Deadline?.ToString("yyyy-MM-dd") ?? "Not specified"}\n" +
                    $"Priority: {task.Priority}\n\n" +
                    $"Description: {taskContent.Description}"
                ));

                // Sent to Mentor / Assigner
                _ = Task.Run(() => _emailController.SendEmail(
                    sender.Email,
                    "Task Assignment Confirmation",
                    $"Hello {sender.Name} {sender.Surname}!\n\n" +
                    $"You have successfully assigned a task to {newbie.Name} {newbie.Surname}:\n\n" +
                    $"Task: \"{taskContent.Title}\"\n" +
                    $"Deadline: {task.Deadline?.ToString("yyyy-MM-dd") ?? "Not specified"}\n" +
                    $"Priority: {task.Priority}"
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending task assignment emails: {ex.Message}");
            }
        }

        private async Task SendStatusChangeEmails(TaskModel task, StatusEnum previousStatus)
        {
            try
            {
                if (!task.NewbieId.HasValue || !task.AssigningId.HasValue)
                    return;

                var taskContent = await _context.TaskContents.FindAsync(task.TaskContentId);
                var newbie = await _userService.GetById(task.NewbieId.Value);
                var assigner = await _userService.GetById(task.AssigningId.Value);

                string previousStatusText = GetStatusDescription(previousStatus);
                string newStatusText = GetStatusDescription(task.Status);

                // Sent to Newbie
                _ = Task.Run(() => _emailController.SendEmail(
                    newbie.Email,
                    $"Task Status Updated: {taskContent.Title}",
                    $"Hello {newbie.Name} {newbie.Surname}!\n\n" +
                    $"The status of your task \"{taskContent.Title}\" has been updated " +
                    $"from {previousStatusText} to {newStatusText}."
                ));

                // Sent to Mentor / Assigner
                _ = Task.Run(() => _emailController.SendEmail(
                    assigner.Email,
                    $"Task Status Updated: {taskContent.Title}",
                    $"Hello {assigner.Name} {assigner.Surname}!\n\n" +
                    $"The status of the task \"{taskContent.Title}\" assigned to {newbie.Name} {newbie.Surname} " +
                    $"has been updated from {previousStatusText} to {newStatusText}."
                ));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending status change emails: {ex.Message}");
            }
        }

        private string GetStatusDescription(StatusEnum status)
        {
            return status switch
            {
                StatusEnum.ToDo => "To Do",
                StatusEnum.InProgress => "In Progress",
                StatusEnum.ToReview => "To Review",
                StatusEnum.ReOpen => "Reopened",
                StatusEnum.Done => "Done",
                _ => status.ToString()
            };
        }

        public async Task<TaskDto> AddTimeAsync(int taskId, double time)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return null;
            try
            {
                task.SpendTime += time;

                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: Task AddTime " + e);
            }
            return new TaskDto(task);
        }

        public async Task<TaskDto> SetTimeAsync(int taskId, double time)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return null;
            try
            {
                task.SpendTime = time;

                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: Task SetTime " + e);
            }
            return new TaskDto(task);
        }

        public async Task<TaskDto> SetRateAsync(int taskId, int rate)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                return null;
            try
            {
                task.Rate = rate;

                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error: Task SetTime " + e);
            }
            return new TaskDto(task);
        }


        public async Task<List<FullTask>> GetAllFullTasksByRoadMapPointIdAsync(int roadMapPointId)
        {
            var fullTasks = await _context.Tasks
                .Where(task => task.State == StateEnum.Active 
                    && task.RoadMapPointId == roadMapPointId)
                .Join(
                _context.TaskContents,
                task => task.TaskContentId, taskContent => taskContent.Id,
                (task, taskContent) => new FullTask(task, taskContent)).ToListAsync();

            var users = await _userService.GetAll();

            foreach (var task in fullTasks)
            {
                if (task.NewbieId.HasValue && task.AssigningId.HasValue)
                {
                    var newbie = users.FirstOrDefault(users => users.Id == task.NewbieId);
                    var assigning = users.FirstOrDefault(users => users.Id == task.AssigningId);
                    task.NewbieName = newbie.Name + " " + newbie.Surname;
                    task.AssigningName = assigning.Name + " " + assigning.Surname;
                }

            }

            return fullTasks;
        }
    }
}
