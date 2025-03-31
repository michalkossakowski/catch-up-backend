using catch_up_backend.Interfaces;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService, IRefreshTokenRepository refreshTokenRepository)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("GetByUserToken")]
        public async Task<IActionResult> GetByUserToken()
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(
                Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim()
            );
            var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == "nameid").Value);

            var notifications = await _notificationService.GetByUserId(userId);

            if (!notifications.Any())
                return NotFound(new { notifications = "No Notifications found" });
            return Ok(notifications);
        }

        [HttpGet]
        [Route("ReadNotifications")]
        public async Task<IActionResult> ReadNotifications()
        {
            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(
                    Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim()
                );
                var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == "nameid").Value);

                await _notificationService.ReadNotifications(userId);
                return Ok("Notifications read successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Notifications read error: {ex}");
            }
        }

        [HttpGet]
        [Route("HasUnreadNotifications")]
        public async Task<IActionResult> HasUnreadNotifications()
        {
            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(
                    Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim()
                );
                var userId = Guid.Parse(jwtToken.Claims.First(c => c.Type == "nameid").Value);

                var hasUnreadNotifications = await _notificationService.HasUnreadNotifications(userId);
                
                return Ok(hasUnreadNotifications);
            }
            catch (Exception ex)
            {
                return BadRequest($"Notifications read error: {ex}");
            }
        }
    }
}
