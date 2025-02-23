using catch_up_backend.Interfaces;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using catch_up_backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public NotificationController(INotificationService notificationService, IRefreshTokenRepository refreshTokenRepository)
        {
            _notificationService = notificationService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        [HttpGet]
        [Route("GetByUserToken")]
        public async Task<IActionResult> GetByUserToken()
        {
            var token = Request.Query["access_token"].ToString();
            var userId = await _refreshTokenRepository.GetUserIdByRefreshToken(token);
            
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
                var token = Request.Query["access_token"].ToString();
                var userId = await _refreshTokenRepository.GetUserIdByRefreshToken(token);

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
                var token = Request.Query["access_token"].ToString();
                var userId = await _refreshTokenRepository.GetUserIdByRefreshToken(token);
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
