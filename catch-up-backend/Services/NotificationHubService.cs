using catch_up_backend.Dtos;
using catch_up_backend.Hubs;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace catch_up_backend.Services
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        public NotificationHubService(IHubContext<NotificationHub> notificationHubContext)
        {
            _notificationHubContext = notificationHubContext;
        }

        public async Task SendNotification(Guid userId, NotificationDto notification)
        {
            await _notificationHubContext.Clients.Group(userId.ToString()).SendAsync("ReceiveNotification", notification);
        }
    }
}
