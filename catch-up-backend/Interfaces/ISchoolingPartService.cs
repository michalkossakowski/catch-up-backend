using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ISchoolingPartService
    {
        public Task<SchoolingPartDto> GetSchoolingPart(int schoolingPartId);
        public Task<List<SchoolingPartDto>> GetSchoolingParts(int schoolingId);
        public Task<bool> DeleteSchoolingPart(int schoolingPartId);
        public Task<bool> EditSchoolingPart(SchoolingPartDto schoolingPart);
    }
}
