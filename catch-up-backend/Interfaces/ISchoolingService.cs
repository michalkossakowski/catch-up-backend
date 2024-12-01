using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface ISchoolingService
    {
        public Task<FullSchoolingDto> GetFullAsync(int schoolingId);
        public Task<FullSchoolingDto> CreateSchoolingAsync(SchoolingDto schoolingDto);
        public Task<List<FullSchoolingDto>> GetAllAsync();
        public Task<bool> DeleteAsync(int schoolingId);
    }
}
