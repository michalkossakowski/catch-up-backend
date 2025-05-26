using catch_up_backend.Dtos;
using catch_up_backend.Response;

namespace catch_up_backend.Interfaces
{
    public interface ISchoolingService
    {
        public Task<SchoolingDto> GetById(int schoolingId);
        public Task<SchoolingDto> GetById(int schoolingId, Guid userId);
        Task<PagedResponse<SchoolingDto>> GetSchoolingsAsync(SchoolingQueryParameters parameters);
        Task<PagedResponse<SchoolingDto>> GetSubscribedSchoolingsAsync(SchoolingQueryParameters parameters, Guid userId);
        Task<PagedResponse<SchoolingDto>> GetOwnedSchoolingsAsync(SchoolingQueryParameters parameters, Guid userId);
        public Task<bool> EditSchooling(SchoolingDto schoolingDto);
    }
}
