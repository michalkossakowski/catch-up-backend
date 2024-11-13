using Azure;
using catch_up_backend.Dtos;
using catch_up_backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace catch_up_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService){
            _authService = authService;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var response = await _authService.Login(request);
            SetTokenCookies(response.AccessToken);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var response = await _authService.Register(request);
            SetTokenCookies(response.AccessToken);
            return Ok(response);
        }

        private void SetTokenCookies(string accessToken)
        {
            Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });
        }
    }
}
