using catch_up_backend.Dtos;
using catch_up_backend.Models;

namespace catch_up_backend.Interfaces
{
    public interface ITaskService
    {
        public Task Add(TaskDto newTask);
        public Task<List<FullTask>> GetAllFullTasks();
        public Task<FullTask> GetFullTaskById(int id);
        public Task<List<FullTask>> GetAllFullTaskByTaskContentId(int id);
        public Task<List<FullTask>> GetAllFullTasksByNewbieId(Guid id);
        public Task<List<FullTask>> GetAllFullTasksByCreatorId(Guid id);
        public Task<List<TaskDto>> GetAllTasks();
        public Task<TaskDto> GetTaskById(int id);
        public Task<List<TaskDto>> GetAllTaskByTaskContentId(int id);
        public Task<List<TaskDto>> GetAllTasksByNewbieId(Guid id);

        //public Task AsignTask(int taskId,Guid newbieID);
    }
}
