using catch_up_backend.Dtos;
using catch_up_backend.Models;
using catch_up_backend.Interfaces;
using catch_up_backend.Database;
using Microsoft.EntityFrameworkCore;
using catch_up_backend.Enums;
using catch_up_backend.Interfaces.RepositoryInterfaces;

namespace catch_up_backend.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly CatchUpDbContext _context;
        private readonly IFaqService _faqService;
        private readonly ITaskService _taskService;
        private readonly ISchoolingService _schoolingService;
        private readonly IUserRepository _userRepository;

        public FeedbackService(CatchUpDbContext context, ISchoolingService schoolingService, ITaskService taskService, IFaqService faqService, IUserRepository userRepository)
        {
            _context = context;
            _schoolingService = schoolingService;
            _taskService = taskService;
            _faqService = faqService;
            _userRepository = userRepository;
        }
        public async Task<bool> AddAsync(FeedbackDto newFeedback)
        {
            try
            {
                var feedback = new FeedbackModel(
                newFeedback.SenderId,
                newFeedback.ReceiverId,
                newFeedback.Title,
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
        public async Task<bool> EditAsync(int feedbackId, FeedbackDto newFeedback)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
                return false;
            try
            {
                feedback.SenderId = newFeedback.SenderId;
                feedback.ReceiverId = newFeedback.ReceiverId;
                feedback.Title = newFeedback.Title;
                feedback.Description = newFeedback.Description;
                feedback.ResourceType = newFeedback.ResourceType;
                feedback.ResourceId = newFeedback.ResourceId;
                feedback.IsResolved = newFeedback.IsResolved;
                _context.Feedbacks.Update(feedback);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Edit feedback:" + ex);
            }
            return true;
        }
        public async Task<bool> DeleteAsync(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
                return false;
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
        public async Task<FeedbackDto> GetByIdAsync(int feedbackId)
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
                    ResourceId = f.ResourceId,
                    IsResolved = f.IsResolved,
                    createdDate = f.createdDate
                }).FirstOrDefaultAsync();

            return feedback;
        }
        public async Task<List<FeedbackDto>> GetByTitleAsync(string title)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.State != StateEnum.Deleted && f.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();

            var feedbackDtos = new List<FeedbackDto>();

            foreach (var feedback in feedbacks)
            {
                string resourceName = await GetResourceNameAsync(feedback.ResourceType, feedback.ResourceId);
                var userReceive = await _userRepository.GetById(feedback.ReceiverId);
                var userSend = await _userRepository.GetById(feedback.SenderId);

                feedbackDtos.Add(new FeedbackDto
                {
                    Id = feedback.Id,
                    SenderId = feedback.SenderId,
                    ReceiverId = feedback.ReceiverId,
                    Title = feedback.Title,
                    Description = feedback.Description,
                    ResourceType = feedback.ResourceType,
                    ResourceId = feedback.ResourceId,
                    IsResolved = feedback.IsResolved,
                    ResourceName = resourceName,
                    UserSend = $"{userSend.Name} {userSend.Surname}",
                    UserReceive = $"{userReceive.Name} {userReceive.Surname}",
                    createdDate = feedback.createdDate
                });
            }

            return feedbackDtos;
        }
        public async Task<List<FeedbackDto>> GetAllAsync()
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.State != StateEnum.Deleted)
                .ToListAsync();

            var feedbackDtos = new List<FeedbackDto>();

            foreach (var feedback in feedbacks)
            {
                string resourceName = await GetResourceNameAsync(feedback.ResourceType, feedback.ResourceId);
                var userSend = await _userRepository.GetById(feedback.SenderId);
                var userReceive = await _userRepository.GetById(feedback.ReceiverId);

                feedbackDtos.Add(new FeedbackDto
                {
                    Id = feedback.Id,
                    SenderId = feedback.SenderId,
                    ReceiverId = feedback.ReceiverId,
                    Title = feedback.Title,
                    Description = feedback.Description,
                    ResourceType = feedback.ResourceType,
                    ResourceId = feedback.ResourceId,
                    IsResolved = feedback.IsResolved,
                    ResourceName = resourceName,
                    UserSend = $"{userSend.Name} {userSend.Surname}",
                    UserReceive = $"{userReceive.Name} {userReceive.Surname}",
                    createdDate = feedback.createdDate
                });
            }

            return feedbackDtos;
        }
        public async Task<List<FeedbackDto>> GetFeedbacksByResourceAsync(ResourceTypeEnum resourceType, int resourceId)
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
                    ResourceId = f.ResourceId,
                    IsResolved = f.IsResolved,
                    createdDate = f.createdDate
                })
                .ToListAsync();

            return feedbacks;
        }
        public async Task<bool> ChangeDoneStatusAsync(int feedbackId)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            if (feedback == null)
                return false;
            try
            {
                feedback.IsResolved = !feedback.IsResolved;
                _context.Feedbacks.Update(feedback);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Change feedback status:" + ex);
            }
            return true;
        }

        private async Task<string> GetResourceNameAsync(ResourceTypeEnum resourceType, int? resourceId)
        {
            if (!resourceId.HasValue)
                return "Removed";
            try
            {
                switch (resourceType)
                {
                    case ResourceTypeEnum.Faq:
                        Console.WriteLine("FaqCase");
                        var faq = await _faqService.GetByIdAsync((int)resourceId);
                        Console.WriteLine(faq == null ? $"Faq not found for ResourceId: {resourceId.Value}" : $"Faq found: {faq.Question}");
                        if (faq == null)
                        {
                            Console.WriteLine($"Faq not found for ResourceId: {resourceId.Value}");
                        }
                        return faq?.Question ?? "Removed";

                    case ResourceTypeEnum.Task:
                        Console.WriteLine("TaskCase");
                        var task = await _taskService.GetFullTaskByIdAsync((int)resourceId);
                        Console.WriteLine(task == null ? $"Task not found for ResourceId: {resourceId.Value}" : $"Task found: {task.Title}");
                        if (task == null)
                        {
                            Console.WriteLine($"Task not found for ResourceId: {resourceId.Value}");
                        }
                        return task?.Title ?? "Removed";

                    //case ResourceTypeEnum.Schooling:
                    //    Console.WriteLine(resourceId.Value);
                    //    var schooling = await _schoolingService.GetFull((int)resourceId);
                    //    Console.WriteLine(schooling == null ? $"Schooling not found for ResourceId: {resourceId.Value}" : $"Schooling found: {schooling?.Schooling?.Title}");
                    //    if (schooling == null)
                    //    {
                    //        Console.WriteLine($"Schooling not found for ResourceId: {resourceId.Value}");
                    //    }
                    //    return schooling?.Schooling?.Title ?? "Removed";

                    default:
                        return "Removed";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetResourceNameAsync: {ex.Message}");
                return "Removed";
            }
        }
    }
}
