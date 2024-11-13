

using catch_up_backend.Dtos;

namespace catch_up_backend.Interfaces
{
    public interface IAuthService{
        Task<AuthResponseDto> Login(LoginRequestDto request);
        Task<AuthResponseDto> Register(RegisterRequestDto request);
        Task<AuthResponseDto> RefreshToken(string refreshToken);
    }
}
