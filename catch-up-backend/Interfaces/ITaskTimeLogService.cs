using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ITaskTimeLogService
    {
        public Task<TaskTimeLogDto> AddAsync(TaskTimeLogDto newTaskComment);
        public Task<TaskTimeLogDto> EditAsync(int id, TaskTimeLogDto newTaskComment);
        public Task<List<TaskTimeLogDto>> GetAllAsync();
        public Task<TaskTimeLogDto> GetByIdAsync(int id);
        public Task<(List<TaskTimeLogDto> timeLogs, int totalCount, int hours, int minutes)> GetByTaskIdAsync(int taskId, int page, int pageSize);
        public Task<bool> DeleteAsync(int id);
    }
}
