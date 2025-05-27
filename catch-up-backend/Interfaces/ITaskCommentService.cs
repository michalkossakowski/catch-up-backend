using catch_up_backend.Dtos;
using catch_up_backend.Models;
using Microsoft.AspNetCore.JsonPatch;
namespace catch_up_backend.Interfaces
{
    public interface ITaskCommentService
    {
        public Task<TaskCommentDto> AddAsync(TaskCommentDto newTimeLog);
        public Task<TaskCommentDto> EditAsync(int id, TaskCommentDto newTimeLog);
        public Task<TaskCommentDto> PatchAsnc(int id, JsonPatchDocument<TaskCommentModel> patchDoc);
        public Task<List<TaskCommentDto>> GetAllAsync();
        public Task<TaskCommentDto> GetByIdAsync(int id);
        public Task<(List<TaskCommentDto> comments, int totalCount)> GetByTaskIdAsync(int taskId, int page, int pageSize);
        public Task<bool> DeleteAsync(int id);
    }
}
