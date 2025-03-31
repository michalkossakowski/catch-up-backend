using Azure.Core;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;

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
            var accessToken = Context.GetHttpContext()?.Request?.Query["access_token"].ToString();
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == "nameid").Value);

            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}