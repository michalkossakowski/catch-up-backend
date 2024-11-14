using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ITaskService
    {
        public Task Add(TaskDto newTask, Guid newbieID, int taskContentId);
        //public Task AsignTask(int taskId,Guid newbieID);
    }
}
