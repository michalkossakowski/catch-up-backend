using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface INotificationHubService
    {
        Task SendNotification(Guid userId, NotificationDto notification);
    }
}
