using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IMaterialService
    {
        public Task<MaterialDto> CreateMaterial(MaterialDto materialDto);
        public Task<bool> AddFile(int materialId, int fileId);
        public Task<MaterialDto> GetMaterial(int materialID);
        public Task<bool> RemoveFile(int materialId, int fileId);
        public Task<List<MaterialDto>> GetMaterials();
        public Task<MaterialDto> GetFilesInMaterial(int materialId);
        public Task<bool> Delete(int materialId);
        public Task<bool> Edit(int materialId, string name);
        public Task<bool> Archive(int materialId);

    }
}
