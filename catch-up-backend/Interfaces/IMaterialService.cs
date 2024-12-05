using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IMaterialService
    {
        public Task<MaterialDto> CreateMaterial(MaterialDto materialDto);
        public Task AddFile(int materialId, int fileId);
        public Task<MaterialDto> GetMaterial(int materialID);
        public Task RemoveFile(int materialId, int fileId);
        public Task<List<MaterialDto>> GetMaterials();
        public Task<MaterialDto> GetFilesInMaterial(int materialId);
        public Task Delete(int materialId);
        public Task Edit(int materialId, string name);
        public Task Archive(int materialId);

    }
}
