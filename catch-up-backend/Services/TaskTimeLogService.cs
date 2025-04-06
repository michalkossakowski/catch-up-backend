using catch_up_backend.Database;
using catch_up_backend.Dtos;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces;
using catch_up_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace catch_up_backend.Services
{
    public class TaskTimeLogService : ITaskTimeLogService
    {
        private readonly CatchUpDbContext _context;
        public TaskTimeLogService(CatchUpDbContext context)
        {
            _context = context;
        }
        public async Task<TaskTimeLogDto> AddAsync(TaskTimeLogDto newTimeLog)
        {
            try
            {
                newTimeLog.Hours = newTimeLog.Hours + (newTimeLog.Minutes / 60);
                newTimeLog.Minutes = newTimeLog.Minutes % 60;
                var timeLog = new TaskTimeLogModel(newTimeLog);
                timeLog.CreationDate = DateTime.Now;
                timeLog.ModificationDate = null;
                timeLog.State = StateEnum.Active;
                _context.TaskTimeLogs.Add(timeLog);
                _context.SaveChanges();
                newTimeLog = new TaskTimeLogDto(timeLog);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return newTimeLog;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var timeLog = await _context.TaskTimeLogs.FindAsync(id);
                if (timeLog == null)
                {
                    return false;
                }
                timeLog.State = StateEnum.Deleted;
                _context.TaskTimeLogs.Update(timeLog);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }

        public async Task<TaskTimeLogDto> EditAsync(int id, TaskTimeLogDto newTimeLog)
        {
            try
            {
                var timeLog = await _context.TaskTimeLogs.FindAsync(id);
                if (timeLog == null)
                {
                    return null;
                }
                timeLog.Description = newTimeLog.Description;
                timeLog.Hours = newTimeLog.Hours;
                timeLog.Minutes = newTimeLog.Minutes;
                timeLog.ModificationDate = DateTime.Now;
                _context.TaskTimeLogs.Update(timeLog);
                await _context.SaveChangesAsync();
                newTimeLog = new TaskTimeLogDto(timeLog);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return newTimeLog;
        }

        public async Task<List<TaskTimeLogDto>> GetAllAsync()
        {
            return await _context.TaskTimeLogs
                .Where(x => x.State == StateEnum.Active)
                .Select(x => new TaskTimeLogDto(x))
                .ToListAsync();
        }

        public async Task<TaskTimeLogDto> GetByIdAsync(int id)
        {
            return await _context.TaskTimeLogs
                .Where(x => x.State == StateEnum.Active && x.Id == id)
                .Select(x => new TaskTimeLogDto(x)).FirstOrDefaultAsync();  
        }

        public async Task<(List<TaskTimeLogDto> timeLogs, int totalCount, int hours, int minutes)> GetByTaskIdAsync(int taskId, int page, int pageSize)
        {
            var totalCount = await _context.TaskTimeLogs
                .Where(x => x.TaskId == taskId && x.State == StateEnum.Active)
                .CountAsync();
            var timeLogs = await _context.TaskTimeLogs
                .Where(x => x.TaskId == taskId && x.State == StateEnum.Active)
                .OrderByDescending(x => x.CreationDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TaskTimeLogDto(x))
                .ToListAsync();
            var hours = await _context.TaskTimeLogs
                .Where(x => x.TaskId == taskId && x.State == StateEnum.Active)
                .SumAsync(x => x.Hours ?? 0);
            var minutes = await _context.TaskTimeLogs
                .Where(x => x.TaskId == taskId && x.State == StateEnum.Active)
                .SumAsync(x => x.Minutes ?? 0);
            hours = hours + (minutes / 60);
            minutes = minutes % 60;
            return (timeLogs, totalCount, hours, minutes);
        }
    }
}
