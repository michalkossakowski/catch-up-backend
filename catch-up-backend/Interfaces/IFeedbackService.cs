using catch_up_backend.Dtos;
using catch_up_backend.Enums;

namespace catch_up_backend.Interfaces
{
    public interface IFeedbackService
    {
        public Task<bool> Add(FeedbackDto newFeedback);
        public Task<bool> Edit(int feedbackId, FeedbackDto newFeedback);
        public Task<bool> Delete(int feedbackId);
        public Task<FeedbackDto> GetById(int feedbackId);
        public Task<List<FeedbackDto>> GetBySenderId(Guid senderId);
        public Task<List<FeedbackDto>> GetByReceiverId(Guid receiverId);
        public Task<List<FeedbackDto>> GetFeedbacksByResource(ResourceTypeEnum resourceType, int resourceId);
        public Task<List<FeedbackDto>> GetAll();
    }
}
