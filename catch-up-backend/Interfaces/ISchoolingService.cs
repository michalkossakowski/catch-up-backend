using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ISchoolingService
    {
        public Task<FullSchoolingDto> GetFull(int schoolingId);
        public Task<FullSchoolingDto> CreateSchooling(SchoolingDto schoolingDto);
        public Task<List<FullSchoolingDto>> GetAllFull();
        public Task Edit(FullSchoolingDto fullSchoolingDto);
        public Task AddSchoolingPart(SchoolingPartDto schoolingPart, int schoolingID);
        public Task DeleteSchooling(int schoolingId);
        public Task ArchiveSchooling(int schoolingId);
        public Task<List<FullSchoolingDto>> GetAllFull(Guid userId);
        public Task AddSchoolingToUser(Guid userId, int schoolingId);
        public Task<List<int>> GetUserSchoolingsID(Guid userId);
        public Task ArchiveUserSchooling(Guid userId, int schoolingId);
        public Task DeleteUserSchooling(Guid userId, int schoolingId);
    }
}
