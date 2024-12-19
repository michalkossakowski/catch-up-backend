using catch_up_backend.Dtos;
using catch_up_backend.Models;

namespace catch_up_backend.Interfaces
{
    public interface ITaskService
    {
        public Task<TaskDto> Add(TaskDto newTask);
        public Task<bool> Edit(int taskId, TaskDto taskDto);
        public Task<bool> EditFullTask(int taskId, FullTask fullTask, Guid userId);
        public Task<List<FullTask>> GetAllFullTasks();
        public Task<FullTask> GetFullTaskById(int id);
        public Task<List<FullTask>> GetAllFullTaskByTaskContentId(int taskContentId);
        public Task<List<FullTask>> GetAllFullTasksByNewbieId(Guid newbieId);
        public Task<List<FullTask>> GetAllFullTasksByCreatorId(Guid creatorId);
        public Task<List<TaskDto>> GetAllTasks();
        public Task<TaskDto> GetTaskById(int taskId);
        public Task<List<TaskDto>> GetAllTaskByTaskContentId(int taskContentId);
        public Task<List<TaskDto>> GetAllTasksByNewbieId(Guid newbieId);
        public Task<bool> Delete(int taskId);

        //public Task AsignTask(int taskId,Guid newbieID);
    }
}
