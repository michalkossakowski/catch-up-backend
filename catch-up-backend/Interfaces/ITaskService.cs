using catch_up_backend.Dtos;
using catch_up_backend.Enums;

namespace catch_up_backend.Interfaces
{
    public interface ITaskService
    {
        public Task<TaskDto> AddAsync(TaskDto newTask);
        public Task<bool> EditAsync(int taskId, TaskDto taskDto);
        public Task<(TaskDto, TaskContentDto)> EditFullTaskAsync(int taskId, FullTask fullTask, Guid userId);
        public Task<List<FullTask>> GetAllFullTasksAsync();
        public Task<FullTask> GetFullTaskByIdAsync(int id);
        public Task<List<FullTask>> GetAllFullTaskByTaskContentIdAsync(int taskContentId);
        public Task<List<FullTask>> GetAllFullTasksByNewbieIdAsync(Guid newbieId);
        public Task<List<FullTask>> GetAllFullTasksByAssigningIdAsync(Guid creatorId);
        public Task<List<TaskDto>> GetAllTasksAsync();
        public Task<TaskDto> GetTaskByIdAsync(int taskId);
        public Task<List<TaskDto>> GetAllTaskByTaskContentIdAsync(int taskContentId);
        public Task<List<TaskDto>> GetAllTasksByNewbieIdAsync(Guid newbieId);
        Task<List<FullTask>> GetAllFullTasksByRoadMapPointIdAsync(int roadMapPointId);
        public Task<bool> DeleteAsync(int taskId);

        //public Task AsignTask(int taskId,Guid newbieID);
        public Task<bool> SetStatusAsync(int taskId, StatusEnum status);
        public Task<TaskDto> AddTimeAsync(int taskId, double time);
        public Task<TaskDto> SetTimeAsync(int taskId, double time);
        public Task<TaskDto> SetRateAsync(int taskId, int rate);
    }
}
