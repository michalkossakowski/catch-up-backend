using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ISchoolingService
    {
        public Task<SchoolingDto> GetById(int schoolingId);
        public Task<SchoolingDto> GetById(int schoolingId, Guid userId);
        public Task<FullSchoolingDto?> CreateSchooling(SchoolingDto schoolingDto);
        public Task<List<FullSchoolingDto>> GetAllFull();
        public Task<bool> Edit(FullSchoolingDto fullSchoolingDto);
        public Task<bool> EditSchooling(SchoolingDto schoolingDto);
        public Task<SchoolingPartDto> CreateSchoolingPart(SchoolingPartDto schoolingPart, int schoolingID);
        public Task<bool> DeleteSchooling(int schoolingId);
        public Task<bool> ArchiveSchooling(int schoolingId);
        public Task<List<FullSchoolingDto>> GetAllFull(Guid userId);
        public Task<bool> AddSchoolingToUser(Guid userId, int schoolingId);
        public Task<List<int>> GetUserSchoolingsID(Guid userId);
        public Task<bool> ArchiveUserSchooling(Guid userId, int schoolingId);
        public Task<bool> DeleteUserSchooling(Guid userId, int schoolingId);
    }
}
