using catch_up_backend.Dtos;
using catch_up_backend.Models;

namespace catch_up_backend.Interfaces
{
    public interface ITaskContentService
    {
        public Task<bool> Add(TaskContentDto newTaskContent );
        public Task<bool> Edit(int taskContentId, TaskContentDto newTaskContent);
        public Task<bool> Delete(int taskContentId);
        public Task<List<TaskContentDto>> GetAll();
        public Task<TaskContentDto> GetById(int taskContentId);
        public Task<List<TaskContentDto>> GetByTitle(string title);
        public Task<List<TaskContentDto>> GetByCreatorId(Guid creatorId);
        public Task<List<TaskContentDto>> GetByCategoryId(int categoryId);
    }
}
