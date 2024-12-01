using catch_up_backend.Dtos;
using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;

namespace catch_up_backend.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly CatchUpDbContext _context;

        public FeedbackService(CatchUpDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Add(FeedbackDto newFeedback)
        {
            try
            {
                var feedback = new FeedbackModel(
                newFeedback.SenderId,
                newFeedback.ReceiverId,
                newFeedback.Title ?? "",
                newFeedback.Description ?? "",
                newFeedback.ResourceType,
                newFeedback.ResourceId);
                await _context.AddAsync(feedback);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Add feedback: " + ex);
            }
            return true;
        }
        public async Task<bool> Edit(int feedbackId, FeedbackDto newFeedback)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
            {
                return false;
            }
            try
            {
                feedback.SenderId = newFeedback.SenderId;
                feedback.ReceiverId = newFeedback.ReceiverId;
                feedback.Title = newFeedback.Title;
                feedback.Description = newFeedback.Description;
                feedback.ResourceType = newFeedback.ResourceType;
                feedback.ResourceId = newFeedback.ResourceId;
                _context.Feedbacks.Update(feedback);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Edit badge:" + ex);
            }
            return true;
        }
        public async Task<bool> Delete(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
            {
                return false;
            }
            try
            {
                feedback.State = StateEnum.Deleted;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Delete feedback:" + ex);
            }
            return true;
        }
        public async Task<FeedbackDto> GetById(int feedbackId)
        {
            var feedback = await _context.Feedbacks
                .Where(f => f.Id == feedbackId && f.State != StateEnum.Deleted)
                .Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    SenderId = f.SenderId,
                    ReceiverId = f.ReceiverId,
                    Title = f.Title,
                    Description = f.Description,
                    ResourceType = f.ResourceType,
                    ResourceId = f.ResourceId
                }).FirstOrDefaultAsync();

            return feedback;
        }
        public async Task<FeedbackDto> GetBySenderId(Guid SenderId)
        {
            var feedback = await _context.Feedbacks
                .Where(f => f.SenderId == SenderId && f.State != StateEnum.Deleted)
                .Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    SenderId = f.SenderId,
                    ReceiverId = f.ReceiverId,
                    Title = f.Title,
                    Description = f.Description,
                    ResourceType = f.ResourceType,
                    ResourceId = f.ResourceId
                }).FirstOrDefaultAsync();

            return feedback;
        }

        public async Task<FeedbackDto> GetByReceiverId(Guid ReceiverId)
        {
            var feedback = await _context.Feedbacks
                .Where(f => f.ReceiverId == ReceiverId && f.State != StateEnum.Deleted)
                .Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    SenderId = f.SenderId,
                    ReceiverId = f.ReceiverId,
                    Title = f.Title,
                    Description = f.Description,
                    ResourceType = f.ResourceType,
                    ResourceId = f.ResourceId
                }).FirstOrDefaultAsync();

            return feedback;
        }

        public async Task<List<FeedbackDto>> GetAll()
        {
            var feedback = await _context.Feedbacks
                .Where(f => f.State != StateEnum.Deleted)
                .Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    SenderId = f.SenderId,
                    ReceiverId = f.ReceiverId,
                    Title = f.Title,
                    Description = f.Description,
                    ResourceType = f.ResourceType,
                    ResourceId = f.ResourceId
                }).ToListAsync();

            return feedback;
        }

        public async Task<List<FeedbackDto>> GetFeedbacksByResource(ResourceTypeEnum resourceType, int resourceId)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.ResourceType == resourceType &&
                            f.ResourceId == resourceId &&
                            f.State != StateEnum.Deleted)
                .Select(f => new FeedbackDto
                {
                    Id = f.Id,
                    SenderId = f.SenderId,
                    ReceiverId = f.ReceiverId,
                    Title = f.Title,
                    Description = f.Description,
                    ResourceType = f.ResourceType,
                    ResourceId = f.ResourceId
                })
                .ToListAsync();

            return feedbacks;
        }
    }
}
