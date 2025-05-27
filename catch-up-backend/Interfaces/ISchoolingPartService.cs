using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ISchoolingPartService
    {
        public Task<List<SchoolingPartDto>> GetSchoolingParts(int schoolingId);
        public Task<List<MaterialDto>> GetMaterials(int schoolingId);
        public Task<List<SchoolingPartDto>> GetAllSchoolingParts();
        public Task<bool> AddMaterialToSchooling(int schoolingPartId, int materialId);
        public Task<bool> ArchiveSchoolingPart(int schoolingPartId);
        public Task<bool> DeleteSchoolingPart(int schoolingPartId);
        public Task<bool> ArchiveMaterialFromSchooling(int schoolingPartId, int materialId);
        public Task<bool> DeleteMaterialFromSchooling(int schoolingPartId, int materialId);
        public Task<bool> EditManySchoolingPart(List<SchoolingPartDto> schoolingPart);

        //Done
        public Task<List<SchoolingPartProgressBarDto>> GetUserSchoolingPartStatus(int schoolingId, Guid userId);
        public Task<List<SchoolingPartProgressBarDto>> GetSchoolingPartStatus(int schoolingId);
        public Task<SchoolingPartDto> GetSchoolingPart(int schoolingPartId, Guid userId);
        public Task<bool> ChangeUserSchoolingPartState(int schoolingUserId, int schoolingPartId, bool state);
        public Task<bool> EditSchoolingPart(SchoolingPartUpdateDto schoolingPart);
    }
}
