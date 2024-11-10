using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IMaterialService
    {
        public Task<MaterialDto> CreateMaterialAsync(MaterialDto materialDto);
        public Task AddFileAsync(int materialId, int fileId);
        public Task<MaterialDto> GetMaterialAsync(int materialID);
        public Task RemoveFileAsync(int materialId, int fileId);
        public Task<List<MaterialDto>> GetMaterialsAync();
        public Task<MaterialDto> GetFilesInMaterialAsync(int materialId);
        public Task DeleteAsync(int materialId);
        public Task EditAsync(int materialId, string name);

    }
}
