using catch_up_backend.Interfaces.RepositoryInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace catch_up_backend.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public NotificationHub(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public override async Task OnConnectedAsync() 
        {
            var token = Context.GetHttpContext()?.Request?.Query["access_token"].ToString();

            if (!string.IsNullOrEmpty(token))
            {
                var userId = await _refreshTokenRepository.GetUserIdByRefreshToken(token);
                await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}