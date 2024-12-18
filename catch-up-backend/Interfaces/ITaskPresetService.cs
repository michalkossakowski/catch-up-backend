using catch_up_backend.Dtos;
using catch_up_backend.Models;

namespace catch_up_backend.Services.Interfaces
{
    public interface ITaskPresetService
    {
        public Task<TaskPresetDto> Add(TaskPresetDto newTaskPreset);
        public Task<bool> Edit(int taskPresetId, TaskPresetDto newTaskPreset);
        public Task<bool> DeleteByPresetId(int presetId);
        public Task<bool> RemoveTaskFromPreset(int presetId, int taskContentId);
        public Task<bool> RemoveTaskFromAllPresets(int taskContentId);
        public Task<List<TaskPresetDto>> GetAll();
        public Task<List<TaskPresetDto>> GetById(int taskPresetId); 
        public Task<List<TaskPresetDto>> GetByTaskContent(int taskContentId); 
        public Task<List<TaskPresetDto>> GetByPresetId(int presetId);
    }
}