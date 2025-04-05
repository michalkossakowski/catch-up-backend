using catch_up_backend.Dtos;
using catch_up_backend.Helpers;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IFirebaseService _firebaseService;

        public AuthController(IAuthService authService, IFirebaseService firebaseService)
        {
            _authService = authService;
            _firebaseService = firebaseService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var response = await _authService.Login(request);
            if (response == null)
                return NotFound(new { message = $"Invalid email or password" });
            return Ok(response);
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var response = await _authService.RefreshToken(refreshToken);
            return Ok(response);
        }

        [HttpPost("RegisterFirebaseToken")]
        public async Task<IActionResult> RegisterFirebaseToken([FromBody] RegisterFirebaseTokenRequest request)
        {
            try
            {
                var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);

                await _firebaseService.RegisterAsync(userId, request.FirebaseToken, request.DeviceName);
                return Ok("Firebase token sucessfully registered");
            }
            catch(Exception ex)
            {
                return BadRequest("Firebase token registeration error");
            }
        }

        [HttpPost("UnregisterFirebaseToken")]
        public async Task<IActionResult> UnregisterFirebaseToken([FromBody] string firebaseToken)
        {
            try
            {
                var userId = TokenHelper.GetUserIdFromTokenInRequest(Request);

                await _firebaseService.UnregisterAsync(userId, firebaseToken);
                return Ok("Firebase token sucessfully unregistered");
            }
            catch (Exception ex)
            {
                return BadRequest("Firebase token unregisteration error");
            }
        }
    }
}
