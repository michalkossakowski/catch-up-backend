using catch_up_backend.Dtos;
using catch_up_backend.Hubs;
using catch_up_backend.Interfaces;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace catch_up_backend.Services
{
    public class NotificationHubService : INotificationHubService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        public NotificationHubService(IRefreshTokenRepository refreshTokenRepository, IHubContext<NotificationHub> notificationHubContext)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _notificationHubContext = notificationHubContext;
        }

        public async Task SendNotification(Guid userId, NotificationDto notification)
        {
            await _notificationHubContext.Clients.Group(userId.ToString()).SendAsync("ReceiveNotification", notification);
        }
    }
}
