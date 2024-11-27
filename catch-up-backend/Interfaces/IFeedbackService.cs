using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IFeedbackService
    {
        public Task<bool> Add(FeedbackDto newFeedback);
        public Task<bool> Edit(int feedbackId, FeedbackDto newFeedback);
        public Task<bool> Delete(int feedbackId);
        public Task<FeedbackDto> GetById(int feedbackId);
        public Task<FeedbackDto> GetBySenderId(Guid senderId);
        public Task<FeedbackDto> GetByReceiverId(Guid receiverId);
        public Task<FeedbackDto> GetByOrigin(string origin);
        public Task<List<FeedbackDto>> GetAll();
    }
}
