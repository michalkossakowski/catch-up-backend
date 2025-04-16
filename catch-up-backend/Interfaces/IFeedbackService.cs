using catch_up_backend.Dtos;
using catch_up_backend.Enums;

namespace catch_up_backend.Interfaces
{
    public interface IFeedbackService
    {
        public Task<bool> AddAsync(FeedbackDto newFeedback);
        public Task<bool> EditAsync(int feedbackId, FeedbackDto newFeedback);
        public Task<bool> DeleteAsync(int feedbackId);
        public Task<FeedbackDto> GetByIdAsync(int feedbackId);
        public Task<List<FeedbackDto>> GetFeedbacksByResourceAsync(ResourceTypeEnum resourceType, int resourceId);
        public Task<List<FeedbackDto>> GetByTitleAsync(string title, Guid userId);
        public Task<List<FeedbackDto>> GetAllAsync(Guid userId);
        public Task<bool> ChangeDoneStatusAsync(int feedbackId);
    }
}
