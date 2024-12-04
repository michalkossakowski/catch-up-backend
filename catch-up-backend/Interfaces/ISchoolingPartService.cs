using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ISchoolingPartService
    {
        public Task<List<SchoolingPartDto>> GetSchoolingParts(int schoolingId);
        public Task<List<MaterialDto>> GetMaterials(int schoolingId);
        public Task<List<SchoolingPartDto>> GetAllSchoolingParts();
        public Task AddMaterialToSchooling(int schoolingPartId, int materialId);
        public Task ArchiveSchoolingPart(int schoolingPartId);
        public Task DeleteSchoolingPart(int schoolingPartId);
        public Task ArchiveMaterialFromSchooling(int schoolingPartId,int materialId);
        public Task DeleteMaterialFromSchooling(int schoolingPartId, int materialId);

    }
}
