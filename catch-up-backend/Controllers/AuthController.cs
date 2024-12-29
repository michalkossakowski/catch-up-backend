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
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var response = await _authService.Login(request);
            return Ok(response);
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var response = await _authService.RefreshToken(refreshToken);
            return Ok(response);
        }
    }
}
