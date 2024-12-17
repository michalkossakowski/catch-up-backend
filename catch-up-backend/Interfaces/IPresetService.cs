using catch_up_backend.Dtos;
using catch_up_backend.Models;

namespace catch_up_backend.Services.Interfaces
{
    public interface IPresetService
    {
        public Task<PresetDto> Add(PresetDto newPreset);
        public Task<PresetDto> Edit(PresetDto newPreset);
        public Task<bool> DeletePreset(int presetId);
        public Task<List<PresetDto>> GetAll();
        public Task<PresetDto> GetById(int presetId);
        public Task<List<PresetDto>> GetByCreatorId(Guid creatorId);
        public Task<List<PresetDto>> GetByName(string name);
        public Task<List<PresetDto>> GetByTaskContent(int taskContentId);
        public Task<List<PresetDto>> SearchPresets(string searchingString);


    }
}