using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ISchoolingPartService
    {
        public Task<List<SchoolingPartDto>> GetSchoolingParts(int schoolingId);
        public Task<List<MaterialDto>> GetMaterials(int schoolingId);
    }
}
