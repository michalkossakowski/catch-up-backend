using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ITaskCommentService
    {
        public Task<TaskCommentDto> AddAsync(TaskCommentDto newTaskComment);
        public Task<TaskCommentDto> EditAsync(int id, TaskCommentDto newTaskComment);
        public Task<List<TaskCommentDto>> GetAllAsync();
        public Task<TaskCommentDto> GetByIdAsync(int id);
        public Task<List<TaskCommentDto>> GetByTaskIdAsync(int taskId);
        public Task<bool> DeleteAsync(int id);
    }
}
