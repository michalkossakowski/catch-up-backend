using catch_up_backend.Helpers;
using catch_up_backend.Interfaces;
using catch_up_backend.Interfaces.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetByUserToken([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);

            var (notifications, totalCount) = await _notificationService.GetByUserId(userId, pageNumber, pageSize);

            if (!notifications.Any())
                return NotFound(new { notifications = "No Notifications found" });
            return Ok(new { notifications, totalCount });
        }

        [HttpGet]
        [Route("ReadNotifications")]
        public async Task<IActionResult> ReadNotifications()
        {
            try
            {
                var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);

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
                var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);

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
