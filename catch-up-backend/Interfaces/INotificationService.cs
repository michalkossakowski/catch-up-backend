using catch_up_backend.Dtos;
using catch_up_backend.Models;

namespace catch_up_backend.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetByUserId(Guid userId);
        Task ReadNotifications(Guid userId);
        Task<bool> HasUnreadNotifications(Guid userId);
        Task AddNotification(NotificationModel notification, List<Guid> receiverId);
        Task AddNotification(NotificationModel notification, Guid receiverId);
    }
}
