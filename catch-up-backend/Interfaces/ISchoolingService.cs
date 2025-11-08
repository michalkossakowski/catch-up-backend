using catch_up_backend.Dtos;
using catch_up_backend.Response;

namespace catch_up_backend.Interfaces
{
    public interface ISchoolingService
    {
        Task<SchoolingDto> CreateSchoolingAsync(SchoolingDto schooling);
        public Task<SchoolingDto> GetById(int schoolingId);
        Task<PagedResponse<SchoolingDto>> GetSchoolingsAsync(SchoolingQueryParameters parameters);
        Task<bool> DeleteSchoolingAsync(int schoolingId);
        public Task<bool> EditSchoolingAsync(SchoolingDto schoolingDto);
    }
}
