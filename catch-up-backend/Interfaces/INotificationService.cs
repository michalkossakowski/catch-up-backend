using catch_up_backend.Dtos;
using catch_up_backend.Models;

namespace catch_up_backend.Interfaces
{
    public interface INotificationService
    {
        Task<(List<NotificationDto>,int totalCount)> GetByUserId(Guid userId, int pageNumber, int pageSize);
        Task ReadNotifications(Guid userId);
        Task<bool> HasUnreadNotifications(Guid userId);
        Task AddNotification(NotificationModel notification, List<Guid> receiverId);
        Task AddNotification(NotificationModel notification, Guid receiverId);
    }
}
